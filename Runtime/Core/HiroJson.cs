using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NakamaHiro.Client
{
    /// <summary>
    /// JSON settings for Hiro RPC payloads (snake_case on the wire).
    /// </summary>
    public static class HiroJson
    {
        public static JsonSerializerSettings DefaultSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(
                    processDictionaryKeys: true,
                    overrideSpecifiedNames: false
                )
            },
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }
}
