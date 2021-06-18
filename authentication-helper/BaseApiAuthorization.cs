using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;

namespace authentication_helper
{
/// <summary>
///     Base class for authenticated service which checks the incoming JWT token.
/// </summary>
public abstract class BaseApiAuthorization : IFunctionInvocationFilter
{
    private const string AuthenticationHeaderName = "Authorization";

    protected AuthenticationInfo Auth { get; private set; }

    public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
    {
        HttpRequest message = executingContext.Arguments.First().Value as HttpRequest;

        if (message == null || !message.Headers.ContainsKey(AuthenticationHeaderName))
        {
            return Task.FromException(new AuthenticationException("No Authorization header was present"));
        }

        try
        {
            Auth = new AuthenticationInfo(message);
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }

        if (!Auth.IsValid)
        {
            return Task.FromException(new KeyNotFoundException("No identity key was found in the claims."));
        }

        return Task.CompletedTask;
    }

    public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
}
