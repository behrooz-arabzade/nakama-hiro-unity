using Nakama;

namespace NakamaHiro.Client
{
    /// <summary>
    /// Scaffold type so the assembly compiles and references Nakama Unity (<c>NakamaRuntime</c>).
    /// Replace with <c>RpcClient</c> and feature facades in a later version.
    /// </summary>
    internal static class NakamaHiroClientAssembly
    {
        internal static System.Type NakamaClientType => typeof(Client);
    }
}
