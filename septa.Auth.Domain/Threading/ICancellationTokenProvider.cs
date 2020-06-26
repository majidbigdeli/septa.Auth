using Microsoft.AspNetCore.Http;
using System.Threading;

namespace septa.Auth.Domain.Threading
{
    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }
    }

    public class HttpContextCancellationTokenProvider : ICancellationTokenProvider
    {
        public CancellationToken Token => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }

    public class NullCancellationTokenProvider : ICancellationTokenProvider
    {
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        public CancellationToken Token { get; } = CancellationToken.None;

        private NullCancellationTokenProvider()
        {

        }
    }

}
