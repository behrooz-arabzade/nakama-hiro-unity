using System.Threading;
using System.Threading.Tasks;
using Nakama;

namespace NakamaHiro.Client.Unity
{
    /// <summary>
    /// Supplies an authenticated Nakama session (device login, refresh, etc.). Implemented by game code.
    /// </summary>
    public interface INakamaSessionProvider
    {
        Task<ISession> GetSessionAsync(CancellationToken cancellationToken = default);
    }
}
