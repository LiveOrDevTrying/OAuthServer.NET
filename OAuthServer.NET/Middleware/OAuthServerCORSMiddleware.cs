using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OAuthServer.NET.Core.Models.Exceptions;
using OAuthServer.NET.Services;
using System;
using System.Threading.Tasks;

namespace OAuthServer.NET.Middleware
{
    public class OAuthServerCORSMiddleware
    {
        protected readonly RequestDelegate _next;
        protected readonly ICorsService _corsService;
        protected readonly ICorsPolicyProvider _corsPolicyProvider;
        private readonly Func<object, Task> OnResponseStartingDelegate = OnResponseStarting; 
        private ILogger Logger { get; }

        public OAuthServerCORSMiddleware(
            ILoggerFactory loggerFactory,
            RequestDelegate next,
            ICorsService corsService,
            ICorsPolicyProvider policyProvider)
            : this(loggerFactory, next, corsService, policyProvider, policyName: null)
        {
        }

        public OAuthServerCORSMiddleware(
            ILoggerFactory loggerFactory,
            RequestDelegate next,
            ICorsService corsService,
            ICorsPolicyProvider policyProvider,
            string policyName)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _corsService = corsService ?? throw new ArgumentNullException(nameof(corsService));
            _corsPolicyProvider = policyProvider ?? throw new ArgumentNullException(nameof(policyProvider));
            Logger = loggerFactory.CreateLogger<CorsMiddleware>();
        }

        public OAuthServerCORSMiddleware(
            ILoggerFactory loggerFactory,
            RequestDelegate next,
            ICorsService corsService,
            CorsPolicy policy)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _corsService = corsService ?? throw new ArgumentNullException(nameof(corsService));
            Logger = loggerFactory.CreateLogger<CorsMiddleware>();
        }
        public async Task InvokeAsync(HttpContext context, OAuthServerDAL dal)
        {
            if (context.Request.Headers.ContainsKey(CorsConstants.Origin))
            {
                // This custom middleware is going to first pull the client_id from the request
                // and verify that the client is allowing cors origins
                var bearerToken = context.Request.Headers["Authorization"];

                string client_id, client_secret;
                if (!string.IsNullOrWhiteSpace(bearerToken))
                {
                    var credentials = bearerToken.DecodeCredentials();
                    client_id = credentials.Item1;
                    client_secret = credentials.Item2;
                }
                else
                {
                    client_id = context.Request.Query["client_id"].ToString();
                    client_secret = context.Request.Query["client_secret"].ToString();
                }

                var corsPolicy = await _corsPolicyProvider?.GetPolicyAsync(context, string.Empty);

                var client = await dal.GetClientAsync(client_id);

                if (client != null && client.ValidateCORS)
                {
                    var corsOrigins = await dal.GetClientCORSOriginsAsync(client.Id);

                    foreach (var corsOrigin in corsOrigins)
                    {
                        corsPolicy.Origins.Add(corsOrigin.OriginURI);
                    }

                    var corsResult = _corsService.EvaluatePolicy(context, corsPolicy);

                    if (!corsPolicy.IsOriginAllowed(context.Request.Headers[CorsConstants.Origin]))
                    {
                        throw new AppException("Invalid CORS configuration");
                    }

                    if (corsResult.IsPreflightRequest)
                    {
                        CorsService.ApplyResult(corsResult, context.Response);

                        // Since there is a policy which was identified,
                        // always respond to preflight requests.
                        context.Response.StatusCode = StatusCodes.Status204NoContent;
                    }
                    else
                    {
                        context.Response.OnStarting(OnResponseStartingDelegate, Tuple.Create(this, context, corsResult));
                        await _next(context);
                    }
                    return;
                }
            }

            // Todo: In production, should return Error because all requests should have an origin
            await _next(context);
        }
        private static Task OnResponseStarting(object state)
        {
            var (middleware, context, result) = (Tuple<OAuthServerCORSMiddleware, HttpContext, CorsResult>)state;
            try
            {
                middleware.CorsService.ApplyResult(result, context.Response);
            }
            catch
            {
            }
            return Task.CompletedTask;
        }
        public ICorsService CorsService
        {
            get
            {
                return _corsService;
            }
        }
    }
}
