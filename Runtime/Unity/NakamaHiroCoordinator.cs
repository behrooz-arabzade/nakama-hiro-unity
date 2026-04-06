using System;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using NakamaHiro.Client;
using UnityEngine;

namespace NakamaHiro.Client.Unity
{
    /// <summary>
    /// Holds the Nakama <see cref="IClient"/> and <see cref="NakamaHiroClient"/> for Hiro-style RPCs.
    /// Server contract: sibling repo <c>nakama-hiro/src/main.ts</c> and <c>register*Rpcs.ts</c>.
    /// </summary>
    public class NakamaHiroCoordinator : MonoBehaviour
    {
        [SerializeField] private string _scheme = "http";
        [SerializeField] private string _host = "127.0.0.1";
        [SerializeField] private int _port = 7350;
        [SerializeField] private string _serverKey = "defaultkey";
        [Tooltip("MonoBehaviour implementing INakamaSessionProvider")]
        [SerializeField] private MonoBehaviour _sessionProviderBehaviour;

        private IClient _nakamaClient;
        private NakamaHiroClient _hiro;
        private INakamaSessionProvider _sessionProvider;

        /// <summary>Built-in Nakama client (HTTP).</summary>
        public IClient Nakama => _nakamaClient;

        /// <summary>Typed RPC facade for nakama-hiro.</summary>
        public NakamaHiroClient Hiro => _hiro;

        /// <summary>Override client creation (e.g. tests). Call before <see cref="Awake"/>.</summary>
        public void ConfigureClient(IClient client, NakamaHiroClient hiroClient)
        {
            _nakamaClient = client ?? throw new ArgumentNullException(nameof(client));
            _hiro = hiroClient ?? new NakamaHiroClient(_nakamaClient);
        }

        /// <summary>Wire session provider from code.</summary>
        public void ConfigureSessionProvider(INakamaSessionProvider provider) =>
            _sessionProvider = provider ?? throw new ArgumentNullException(nameof(provider));

        protected virtual void Awake()
        {
            if (_nakamaClient == null)
                _nakamaClient = new Client(_scheme, _host, _port, _serverKey, UnityWebRequestAdapter.Instance);

            if (_hiro == null)
                _hiro = new NakamaHiroClient(_nakamaClient);

            if (_sessionProvider == null && _sessionProviderBehaviour != null)
                _sessionProvider = _sessionProviderBehaviour as INakamaSessionProvider;
        }

        public Task<ISession> GetSessionAsync(CancellationToken cancellationToken = default)
        {
            if (_sessionProvider == null)
            {
                throw new InvalidOperationException(
                    "Assign a MonoBehaviour implementing INakamaSessionProvider or call ConfigureSessionProvider.");
            }

            return _sessionProvider.GetSessionAsync(cancellationToken);
        }
    }
}
