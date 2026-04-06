using System;
using System.Threading;
using System.Threading.Tasks;
using Nakama;

namespace NakamaHiro.Client.Unity
{
    /// <summary>
    /// Wraps a delegate for tests or non-Unity wiring.
    /// </summary>
    public sealed class DelegateNakamaSessionProvider : INakamaSessionProvider
    {
        private readonly Func<CancellationToken, Task<ISession>> _factory;

        public DelegateNakamaSessionProvider(Func<CancellationToken, Task<ISession>> factory) =>
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

        public Task<ISession> GetSessionAsync(CancellationToken cancellationToken = default) =>
            _factory(cancellationToken);
    }
}
