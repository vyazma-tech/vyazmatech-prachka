using Microsoft.AspNetCore.Authorization;
using VyazmaTech.Prachka.Presentation.Authorization.Contracts;

namespace VyazmaTech.Prachka.Presentation.Authorization.Tools;

internal sealed class FeatureAuthorizationHandler : AuthorizationHandler<AuthorizeFeatureRequirement>
{
    private readonly IFeatureService _service;

    public FeatureAuthorizationHandler(IFeatureService service)
    {
        _service = service;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AuthorizeFeatureRequirement requirement)
    {
        if (_service.HasFeature(context.User, requirement.Scope, requirement.Feature))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}