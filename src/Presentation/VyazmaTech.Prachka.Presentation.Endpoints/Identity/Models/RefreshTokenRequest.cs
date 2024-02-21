namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.Models;

internal sealed record RefreshTokenRequest(string AccessToken, string RefreshToken);