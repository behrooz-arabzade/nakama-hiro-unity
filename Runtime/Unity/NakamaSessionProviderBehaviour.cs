using System.Threading;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace NakamaHiro.Client.Unity
{
    /// <summary>
    /// Optional base class for a MonoBehaviour that exposes <see cref="INakamaSessionProvider"/>.
    /// </summary>
    public abstract class NakamaSessionProviderBehaviour : MonoBehaviour, INakamaSessionProvider
    {
        public abstract Task<ISession> GetSessionAsync(CancellationToken cancellationToken = default);
    }
}
