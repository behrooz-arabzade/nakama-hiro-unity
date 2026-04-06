# Agent / contributor notes

- **Server contract:** Implement RPC names and JSON payloads to match the sibling **nakama-hiro** repo. Primary spec: `nakama-hiro/src/main.ts` lines 1–89 (comment block). Registration ids: `src/features/*/register*Rpcs.ts`.
- **Casing:** Server uses **snake_case** in JSON; mirror that in C# DTOs (e.g. `System.Text.Json` snake_case naming or Newtonsoft settings).
- **Stack:** Depends on **com.heroiclabs.nakama-unity** (`NakamaRuntime`) and **com.unity.nuget.newtonsoft-json** (`Newtonsoft.Json`) for snake_case RPC JSON.
  - **NakamaHiro.Client** (`Runtime/NakamaHiro.Client.asmdef`): engine-agnostic `NakamaHiroClient` + DTOs (`noEngineReferences: true`).
  - **NakamaHiro.Client.Unity** (`Runtime/Unity/NakamaHiro.Client.Unity.asmdef`): `NakamaHiroCoordinator`, session provider hook, optional feature `MonoBehaviour` facades with `*Completed` events, plus `NakamaHiroSystemObserver` / `NakamaHiroObservableState<T>` for disposable subscriptions and last-value caches (see `DOCUMENTATION.md`).
- **RPCs:** Prefer `NakamaHiroClient` (wraps `IClient.RpcAsync`); ids are `HiroRpcIds` / sibling `register*Rpcs.ts`.
- **Scope:** Keep this package free of game-specific code; only reusable client surface and types.
