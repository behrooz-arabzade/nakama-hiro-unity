# Agent / contributor notes

- **Server contract:** Implement RPC names and JSON payloads to match the sibling **nakama-hiro** repo. Primary spec: `nakama-hiro/src/main.ts` lines 1–89 (comment block). Registration ids: `src/features/*/register*Rpcs.ts`.
- **Casing:** Server uses **snake_case** in JSON; mirror that in C# DTOs (e.g. `System.Text.Json` snake_case naming or Newtonsoft settings).
- **Stack:** This package depends on **com.heroiclabs.nakama-unity**; reference assembly **NakamaRuntime** from `NakamaHiro.Client.asmdef`. Call RPCs via `IClient.RpcAsync(ISession, string id, string payload, ...)`.
- **Scope:** Keep this package free of game-specific code; only reusable client surface and types.
