using System.Security.Claims;

namespace VyazmaTech.Prachka.Application.Dto.Identity;

public record PrincipalDto(ClaimsPrincipal? Principal);