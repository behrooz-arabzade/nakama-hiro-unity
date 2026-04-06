using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nakama;

namespace NakamaHiro.Client
{
    public sealed class EnergySpendGrantRequest
    {
        public Dictionary<string, double> Amounts { get; set; }
    }

    public sealed class EnergyListEntry
    {
        public double Current { get; set; }
        public double MaxCount { get; set; }
        public double MaxOverfill { get; set; }
        public double HardCap { get; set; }
        public double RefillCount { get; set; }
        public double RefillTimeSec { get; set; }
        public double? NextRefillTimeSec { get; set; }
        public bool IsInfinite { get; set; }
    }

    public sealed class EnergyListResponse
    {
        public double CurrentTimeSec { get; set; }
        public Dictionary<string, EnergyListEntry> Energies { get; set; }
    }

    public sealed class EnergyHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        internal EnergyHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<EnergyListResponse> GetAsync(ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EnergyListResponse>(session, HiroRpcIds.EnergyGet, null, cancellationToken);

        public Task<EnergyListResponse> SpendAsync(
            ISession session,
            EnergySpendGrantRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EnergyListResponse>(session, HiroRpcIds.EnergySpend, request, cancellationToken);

        public Task<EnergyListResponse> GrantAsync(
            ISession session,
            EnergySpendGrantRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<EnergyListResponse>(session, HiroRpcIds.EnergyGrant, request, cancellationToken);
    }
}
