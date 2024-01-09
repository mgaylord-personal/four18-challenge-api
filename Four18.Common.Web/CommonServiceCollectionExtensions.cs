using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using Four18.Common.Interfaces;
using Four18.Common.Security;
//using Four18.Common.Tenant.Context;

namespace Four18.Common.Web;

public static class CommonServiceCollectionExtensions
{
    public static void RegisterCommonServices(this IServiceCollection serviceCollection)
    {
        // allow access to HttpContext and TenantCache
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // allow access to an IPrincipal configured as User from the context
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        serviceCollection.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        serviceCollection.AddScoped<IClaimProvider, ClaimProvider>();
        //serviceCollection.AddScoped<ITenantContextProvider, TenantContextProvider>();
    }
}
