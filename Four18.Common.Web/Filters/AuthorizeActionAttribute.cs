using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Four18.Common.Interfaces;
using Four18.Common.Logging;

namespace Four18.Common.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class AuthorizeActionAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private ILogger<AuthorizeActionAttribute>? _logger;
    private IClaimProvider? _claimProvider;
        
    public object ActionId { get; }

    public AuthorizeActionAttribute(object actionId)
    {
        ActionId = actionId;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _logger = context.HttpContext.RequestServices.GetService<ILogger<AuthorizeActionAttribute>>();
            
        _logger!.LogInformation(LoggingHelper.GetLogClassMethodWithParams("ActionId"),
            nameof(AuthorizeActionAttribute), nameof(OnAuthorization), ActionId);

        int actionId;
        if (ActionId is Enum)
        {
            // ReSharper disable once PossibleInvalidCastException
            actionId = (int)ActionId;
        }
        else if (!int.TryParse(ActionId.ToString(), out actionId))
        {
            context.Result = GetForbidden("unable to parse actionId");
        }

        _logger!.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(actionId)),
            nameof(AuthorizeActionAttribute), nameof(OnAuthorization), actionId);

        _claimProvider = context.HttpContext.RequestServices.GetService<IClaimProvider>();
        var isAuthorized = _claimProvider?.ActionExists(actionId) ?? false;

        if (!isAuthorized)
        {
            context.Result = GetForbidden("actionId not found in privileges");
        }
    }

    private StatusCodeResult GetForbidden(string reason)
    {
        _logger!.LogInformation(LoggingHelper.GetLogClassMethodWithParams(nameof(reason)),
            nameof(AuthorizeActionAttribute), nameof(GetForbidden), reason);

        return new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
    }
}