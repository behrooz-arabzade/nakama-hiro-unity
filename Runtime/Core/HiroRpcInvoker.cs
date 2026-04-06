using System.Threading;
using System.Threading.Tasks;
using Nakama;
using Newtonsoft.Json;

namespace NakamaHiro.Client
{
    /// <summary>
    /// Serializes requests and deserializes RPC payloads via <see cref="IClient.RpcAsync"/>.
    /// </summary>
    public sealed class HiroRpcInvoker
    {
        private readonly IClient _client;
        private readonly JsonSerializerSettings _json;

        public HiroRpcInvoker(IClient client, JsonSerializerSettings json = null)
        {
            _client = client;
            _json = json ?? HiroJson.DefaultSettings;
        }

        public JsonSerializerSettings JsonSettings => _json;

        public async Task<TResponse> CallAsync<TResponse>(
            ISession session,
            string rpcId,
            object requestBody,
            CancellationToken cancellationToken = default)
        {
            var payload = requestBody == null ? "{}" : JsonConvert.SerializeObject(requestBody, _json);
            var apiRpc = await _client.RpcAsync(session, rpcId, payload, null, cancellationToken);
            var responsePayload = string.IsNullOrEmpty(apiRpc.Payload) ? "{}" : apiRpc.Payload;
            return JsonConvert.DeserializeObject<TResponse>(responsePayload, _json);
        }

        public async Task CallAsync(
            ISession session,
            string rpcId,
            object requestBody,
            CancellationToken cancellationToken = default)
        {
            var payload = requestBody == null ? "{}" : JsonConvert.SerializeObject(requestBody, _json);
            await _client.RpcAsync(session, rpcId, payload, null, cancellationToken);
        }

        public async Task<TResponse> CallRawAsync<TResponse>(
            ISession session,
            string rpcId,
            string jsonPayload,
            CancellationToken cancellationToken = default)
        {
            var payload = string.IsNullOrEmpty(jsonPayload) ? "{}" : jsonPayload;
            var apiRpc = await _client.RpcAsync(session, rpcId, payload, null, cancellationToken);
            var responsePayload = string.IsNullOrEmpty(apiRpc.Payload) ? "{}" : apiRpc.Payload;
            return JsonConvert.DeserializeObject<TResponse>(responsePayload, _json);
        }
    }
}
