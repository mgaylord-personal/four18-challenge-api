using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Four18.Common.Logging;

namespace Four18.Common.Web.Middleware;

public static class PipelineDebug
{
    /// <summary>
    /// Middleware Debug point which can be inserted in the pipeline in multiple locations
    /// </summary>
    /// <param name="app"></param>
    /// <param name="logger"></param>
    /// <param name="position"></param>
    public static void PipelineCheckPoint(this IApplicationBuilder app, ILogger logger, int position = 1)
    {
        var message = "Middleware Initialization";
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(x => x.GetName().ToString().ToLower(CultureInfo.CurrentCulture).Contains("webapi"));

        logger.LogInformation(
            LoggingHelper.GetLogClassMethodWithParams(
                nameof(message),
                nameof(assembly)),
            nameof(PipelineDebug),
            nameof(PipelineCheckPoint),
            message,
            assembly);

        app.Use(async (context, next) =>
        {
            message = "Before Invoke";
            logger.LogInformation(
                LoggingHelper.GetLogClassMethodWithParams(
                    nameof(message),
                    nameof(assembly),
                    nameof(context.Request.Protocol),
                    nameof(context.Request.Host),
                    nameof(context.Request.Method),
                    nameof(context.Request.Path),
                    nameof(position)),
                nameof(PipelineDebug),
                nameof(PipelineCheckPoint),
                message,
                assembly,
                context.Request.Protocol,
                context.Request.Host,
                context.Request.Method,
                context.Request.Path,
                position);


            await next.Invoke();

            message = "After Invoke";
            var cPrincipal = context.User;

            var claims = cPrincipal?.Claims
                .OrderBy(x => x.Type)
                .Select(x =>$"{x.Type.Substring(Math.Max(0, x.Type.LastIndexOfAny(new[] {'/', '.'}) + 1))}=={x.Value.ToString()}");

            logger.LogInformation(
                LoggingHelper.GetLogClassMethodWithParams(
                    nameof(message),
                    nameof(assembly),
                    nameof(context.Request.Protocol),
                    nameof(context.Request.Host),
                    nameof(context.Request.Method),
                    nameof(context.Request.Path),
                    nameof(position),
                    nameof(claims)),
                nameof(PipelineDebug),
                nameof(PipelineCheckPoint),
                message,
                assembly,
                context.Request.Protocol,
                context.Request.Host,
                context.Request.Method,
                context.Request.Path,
                position,
                claims);
        });
    }
}