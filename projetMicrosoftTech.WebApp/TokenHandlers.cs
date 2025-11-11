using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

public class TokenHandlers : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenHandlers(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new Exception("HttpContext not available");
        }

        var accessToken = await _httpContextAccessor.HttpContext
            .GetTokenAsync("access_token");

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}