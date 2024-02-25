namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

internal sealed record RefreshTokenRequest(string AccessToken, string RefreshToken);