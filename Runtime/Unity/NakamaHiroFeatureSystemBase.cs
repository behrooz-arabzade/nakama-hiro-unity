using System;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using NakamaHiro.Client;
using UnityEngine;

namespace NakamaHiro.Client.Unity
{
    /// <summary>
    /// Base for MonoBehaviour facades that call <see cref="NakamaHiroClient"/> through a <see cref="NakamaHiroCoordinator"/>.
    /// Place feature systems under the same hierarchy as the coordinator or assign <see cref="_coordinator"/> explicitly.
    /// </summary>
    public abstract class NakamaHiroFeatureSystemBase : MonoBehaviour
    {
        [SerializeField] private NakamaHiroCoordinator _coordinator;

        protected NakamaHiroCoordinator Coordinator
        {
            get
            {
                if (_coordinator == null)
                    _coordinator = GetComponentInParent<NakamaHiroCoordinator>();
                return _coordinator;
            }
        }

        protected NakamaHiroClient Hiro =>
            Coordinator != null
                ? Coordinator.Hiro
                : throw new InvalidOperationException(
                    "Assign NakamaHiroCoordinator on this system or parent it under a coordinator.");

        protected Task<ISession> SessionAsync(CancellationToken cancellationToken = default) =>
            Coordinator.GetSessionAsync(cancellationToken);
    }
}
