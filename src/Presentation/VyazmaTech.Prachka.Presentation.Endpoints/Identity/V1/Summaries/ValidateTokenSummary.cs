using FastEndpoints;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class ValidateTokenSummary : Summary<ValidateTokenEndpoint>
{
    public ValidateTokenSummary()
    {
        Summary = "validates access token for a user";
        Response<PrincipalDto>();
    }
}