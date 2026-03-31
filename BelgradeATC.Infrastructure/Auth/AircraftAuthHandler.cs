using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using BelgradeATC.Core.Entities;
using BelgradeATC.Core.Enums;
using BelgradeATC.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BelgradeATC.Infrastructure.Auth;

public class AircraftAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IAircraftRepository aircraftRepository)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "AircraftPublicKey";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 1. Extracting call sign from route
        var callSign = Request.RouteValues["call_sign"]?.ToString();
        if (string.IsNullOrEmpty(callSign))
            return AuthenticateResult.Fail("Missing call sign in route.");

        // 2. Read the three required headers
        if (!Request.Headers.TryGetValue("X-Public-Key", out var publicKeyHeader))
            return AuthenticateResult.Fail("Missing X-Public-Key header.");

        if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
            return AuthenticateResult.Fail("Missing X-Signature header.");

        if (!Request.Headers.TryGetValue("X-Timestamp", out var timestampHeader))
            return AuthenticateResult.Fail("Missing X-Timestamp header.");

        // 3. Validate timestamp — reject requests older than 5 minutes (replay protection)
        if (!long.TryParse(timestampHeader, out var unixTimestamp))
            return AuthenticateResult.Fail("Invalid timestamp format.");

        var requestTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        if (Math.Abs((DateTimeOffset.UtcNow - requestTime).TotalMinutes) > 5)
            return AuthenticateResult.Fail("Request timestamp is expired.");

        // 4. Verify RSA signature
        //    The aircraft signed: "{callSign}:{unixTimestamp}" with its private key.
        //    We verify that using the public key it sent.
        var publicKeyPem = publicKeyHeader.ToString();
        var signatureBytes = Convert.FromBase64String(signatureHeader.ToString());
        var message = $"{callSign}:{unixTimestamp}";

        var result = VerifySignature(publicKeyPem, message, signatureBytes);
        if (!result.IsValid)
            return AuthenticateResult.Fail(result.Error ?? "Signature verification failed.");

        // 5. Look up aircraft by call sign in the database
        var aircraft = await aircraftRepository.GetByCallSignAsync(callSign);

        if (aircraft is null)
        {
            // First contact — register this aircraft with its public key
            aircraft = new Aircraft
            {
                CallSign = callSign,
                PublicKey = publicKeyPem,
                State = AircraftState.Parked,
                LastSeen = DateTime.UtcNow
            };
            await aircraftRepository.AddAsync(aircraft);
            await aircraftRepository.SaveChangesAsync();
        }
        else if (aircraft.PublicKey != publicKeyPem)
        {
            // Known aircraft but different key — reject
            return AuthenticateResult.Fail("Public key does not match registered key for this call sign.");
        }

        // 6. Build an authenticated principal with the call sign as the identity
        var claims = new[] { new Claim(ClaimTypes.Name, callSign) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private static (bool IsValid, string? Error) VerifySignature(string publicKeyPem, string message, byte[] signature)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(publicKeyPem))
                return (false, "Public key is missing");

            if (string.IsNullOrWhiteSpace(message))
                return (false, "Message is empty");

            if (signature == null || signature.Length == 0)
                return (false, "Signature is empty");

            using var rsa = RSA.Create();

            try
            {
                rsa.ImportFromPem(publicKeyPem);
            }
            catch (Exception ex)
            {
                return (false, $"Invalid public key format: {ex.Message}");
            }

            var messageBytes = Encoding.UTF8.GetBytes(message);

            var isValid = rsa.VerifyData(
                messageBytes,
                signature,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );

            if (!isValid)
                return (false, "Signature verification failed");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"Unexpected error: {ex.Message}");
        }
    }
}
