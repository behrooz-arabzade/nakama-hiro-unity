# CLAUDE.md

Guidance for Claude Code (claude.ai/code) when working in this repository.

## Project overview

`com.nakamahiro.client` is a Unity Package Manager (UPM) package that exposes typed C# wrappers for RPCs served by the sibling TypeScript project **`nakama-hiro`**. It sits on top of Heroic Labs' `com.heroiclabs.nakama-unity` (`NakamaRuntime`) and uses `com.unity.nuget.newtonsoft-json` (`Newtonsoft.Json`) to speak **snake_case** JSON on the wire while letting C# callers keep PascalCase.

This repo is **package-only**: there is no Unity project, no `Assets/`, no `ProjectSettings/`, no scenes, no tests. Code is consumed from a host Unity project (2022.3+) via git URL or local `file:` path. Compilation is only verified inside that host project.

## Server contract is the source of truth

The authoritative RPC catalog (ids and payload shapes) lives in the sibling repo `nakama-hiro`:

- Header comment at `nakama-hiro/src/main.ts` (currently ~lines 1–89 for the RPC surface, plus match-manager notes) describes every RPC id, request, and response.
- `nakama-hiro/src/features/<feature>/register*Rpcs.ts` files declare the registered id strings.

Both repos live side-by-side: `/Users/arabzb/Desktop/Projects/nakama-hiro` (server) and `/Users/arabzb/Desktop/Projects/nakama-hiro-unity` (this package). When a task involves adding or changing an RPC, always read the server repo first; never invent payloads.

## Directory layout

```
Runtime/
  NakamaHiroClient.cs              # Root facade (exposes per-feature sub-clients)
  NakamaHiro.Client.asmdef         # Engine-agnostic asmdef (noEngineReferences: true)
  Core/
    HiroJson.cs                    # SnakeCase + Ignore null/missing settings
    HiroRpcIds.cs                  # String constants for every public RPC
    HiroRpcInvoker.cs              # Wraps IClient.RpcAsync + JSON (de)serialization
    RewardDtos.cs                  # Shared reward DTOs reused across features
  Features/
    <Feature>Api.cs                # DTOs + <Feature>HiroClient per feature
  Unity/
    NakamaHiro.Client.Unity.asmdef # Unity-dependent asmdef (references NakamaHiro.Client)
    NakamaHiroCoordinator.cs       # MonoBehaviour: creates IClient + NakamaHiroClient
    INakamaSessionProvider.cs      # Game-supplied session factory
    NakamaSessionProviderBehaviour.cs
    DelegateNakamaSessionProvider.cs
    NakamaHiroFeatureSystemBase.cs # MonoBehaviour base that resolves the coordinator
    NakamaHiroUnityFeatureSystems.cs # Optional MonoBehaviour facades with *Completed events
    NakamaHiroSystemObserver.cs    # Disposable Subscribe helper
    NakamaHiroObservableState.cs   # Last-value cache + Changed event
package.json                       # UPM manifest (displayName, unity, dependencies)
README.md / DOCUMENTATION.md / AGENTS.md / CHANGELOG.md
```

## Assembly split (strict)

| Assembly | Purpose | Rules |
|----------|---------|-------|
| `NakamaHiro.Client` (`Runtime/NakamaHiro.Client.asmdef`) | Engine-agnostic client: `NakamaHiroClient`, per-feature `*HiroClient`, DTOs, `HiroRpcInvoker`, `HiroJson`, `HiroRpcIds` | `noEngineReferences: true`. **No `UnityEngine`, `MonoBehaviour`, `SerializeField`, coroutines, or Unity types.** References only `NakamaRuntime` and `Newtonsoft.Json`. Put new feature APIs and DTOs here. |
| `NakamaHiro.Client.Unity` (`Runtime/Unity/NakamaHiro.Client.Unity.asmdef`) | Unity glue: `NakamaHiroCoordinator`, session providers, `NakamaHiro*System` facades, observer/observable helpers | Unity-aware. References `NakamaHiro.Client` and `NakamaRuntime`. Only things that need `UnityEngine` live here. |

If a new type compiles cleanly without `UnityEngine`, it belongs under `Runtime/` or `Runtime/Core/` or `Runtime/Features/`, **not** under `Runtime/Unity/`.

## Coding conventions

- **Namespaces:** DTOs and feature clients are `namespace NakamaHiro.Client`. Unity types are `namespace NakamaHiro.Client.Unity`.
- **JSON:** Always serialize through `HiroJson.DefaultSettings` (snake_case, ignore null, ignore missing members). `HiroRpcInvoker` already wires this. Do not hand-roll `JsonConvert.SerializeObject` without those settings.
- **Casing:** C# properties stay PascalCase; the SnakeCase naming strategy handles the wire format (e.g. `ItemId` ↔ `item_id`). Do **not** add `[JsonProperty("snake_case_name")]` unless a name cannot be derived mechanically.
- **DTOs:** Plain `public sealed class` with get/set auto-properties. Use `Dictionary<string, double>` for currency/item bags; `List<T>` for arrays; `Newtonsoft.Json.Linq.JObject` for deliberately-flexible payload fragments (e.g. achievement `additional_properties`).
- **Feature clients:** Each `<Feature>HiroClient` holds a `HiroRpcInvoker`, has an `internal` constructor, and exposes `Task<TResponse> XxxAsync(ISession session, TRequest request = null, CancellationToken cancellationToken = default)` methods that call `_rpc.CallAsync<TResponse>(session, HiroRpcIds.XxxId, request, cancellationToken)`.
- **Cancellation:** Every async method accepts `CancellationToken cancellationToken = default` as the **last** parameter and forwards it to `CallAsync`.
- **Public API surface:** `NakamaHiroClient` exposes each feature client as a public property. Keep the list alphabetical by feature intent (see existing order in `Runtime/NakamaHiroClient.cs`). Pluralize the same way the server file/folder does (`Auctions`, `Progression`, `Unlockables`, etc.).

## How to add a new RPC wrapper

1. Read the server side first:
   - Find the id in `nakama-hiro/src/features/<feature>/register*Rpcs.ts`.
   - Find the payload shapes in the `nakama-hiro/src/main.ts` header comment (or the handler in `nakama-hiro/src/features/<feature>/`).
2. If it's **admin-only** (e.g. restricted to operator user ids via env config like `MATCH_MANAGER_ADMIN_USER_IDS` or `SERVER_LB_ADMIN_USER_IDS`), **stop**. Do not wrap it here (see policy below).
3. Add the id constant in `Runtime/Core/HiroRpcIds.cs`, grouped with its siblings, matching the exact server string.
4. Add or extend the DTOs and the `<Feature>HiroClient` method in `Runtime/Features/<Feature>Api.cs`. PascalCase properties; snake_case stays at the wire layer.
5. If the feature client is new, register it as a public property on `NakamaHiroClient` (`Runtime/NakamaHiroClient.cs`) and instantiate it in the constructor.
6. If a `NakamaHiro<Feature>System` facade exists in `Runtime/Unity/NakamaHiroUnityFeatureSystems.cs`, add a matching `*Async` method and `*Completed` event there (same pattern as existing entries). If creating a new system, inherit from `NakamaHiroFeatureSystemBase` so it resolves the coordinator via parent or serialized reference.
7. Update `CHANGELOG.md` under `## Unreleased`.

## Admin-only RPC policy (hard rule)

The server restricts some RPCs to operator/admin user ids (configured through env vars). Examples include `match_manager_admin_*`, `server_lb_admin_*`, and anything gated by `MATCH_MANAGER_ADMIN_USER_IDS` / `SERVER_LB_ADMIN_USER_IDS`. Those RPCs must **not** appear in:

- `HiroRpcIds`
- any `NakamaHiroClient` feature surface
- any `NakamaHiro*System` facade

They belong in Nakama Console, server-side scripts, or a separate admin tool — not in the shipped game client. Public, non-admin counterparts from the same feature (e.g. `server_lb_regions_list`) are fine to expose.

## Game-specific extension pattern

The server project (`nakama-hiro`) uses a base/game split: `src/hiro/` is the upstream base tree (never edited per-clone) and `src/game/` is the game-owned extension seam. This package mirrors that split: the SDK is never edited by game projects; all game-specific RPC wrappers live in the **host Unity project's** own assemblies.

### Two SDK extension points

- **`NakamaHiroClient.Invoker`** — the shared `HiroRpcInvoker`. Pass to game `*HiroClient` constructors so they inherit the same `IClient` and `HiroJson` settings.
- **`NakamaHiroCoordinator.SetGameExtensions` / `GetGameExtensions<T>`** — opaque slot for the game's root extensions object. The SDK has zero compile-time knowledge of game types; the cast lives entirely in game code.

### What game developers create in their host Unity project

Mirror `nakama-hiro/src/game/` under `Assets/Game/HiroExtensions/`:

```
Assets/Game/
  HiroExtensions/
    Game.HiroExtensions.asmdef           ← noEngineReferences: true, refs NakamaHiro.Client
    GameHiroRpcIds.cs                    ← game RPC id constants (namespace Game.HiroExtensions)
    GameHiroClient.cs                    ← wraps NakamaHiroClient; holds all game feature clients
    Features/<Feature>/
      <Feature>Dtos.cs
      <Feature>HiroClient.cs             ← takes HiroRpcInvoker, same shape as base *HiroClient
  HiroExtensions.Unity/
    Game.HiroExtensions.Unity.asmdef    ← refs NakamaHiro.Client + NakamaHiro.Client.Unity + Game.HiroExtensions
    GameHiroCoordinatorBootstrap.cs     ← [DefaultExecutionOrder(-50)] Awake → SetGameExtensions
    Features/<Feature>/
      NakamaHiro<Feature>System.cs      ← optional; inherits NakamaHiroFeatureSystemBase
```

### Key conventions for game extension code

- **Game feature client constructors are `public`** (not `internal`); game assemblies aren't the SDK.
- **`GameHiroClient` wraps `NakamaHiroClient`** with a `public NakamaHiroClient Hiro` property — do not try to inherit (`sealed`).
- **`GameHiroCoordinatorBootstrap`** is the single seam file. Add a new feature by adding its client to `GameHiroClient`'s constructor; the bootstrap needs no change.
- **Game `NakamaHiro*System` facades** get their feature client via `Coordinator.GetGameExtensions<GameHiroClient>().<Feature>` — the same property chain every other call site uses.
- **Namespace**: `Game.HiroExtensions` (engine-agnostic tier), `Game.HiroExtensions.Unity` (Unity tier). Never `NakamaHiro.*` — that namespace is SDK-only.

### What NOT to do

- Do **not** edit any file under `Runtime/` for game-specific RPCs. `HiroRpcIds`, base feature clients, and the 14 `NakamaHiro*System` facades are upstream SDK; game ids and DTOs live in the host project.
- Do **not** subclass `NakamaHiroClient` or add game feature properties to it.

See `DOCUMENTATION.md` → "Adding game-specific RPCs (structured extension pattern)" for the complete worked example with all file contents.



Consumers add it to their Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nakamahiro.client": "https://github.com/behrooz-arabzade/nakama-hiro-unity.git"
  }
}
```

…or use a local `file:` path during development. There is **no build step in this repo** — Unity compiles it when the host project opens. When verifying a change, open the package in a Unity 2022.3+ project that also has `com.heroiclabs.nakama-unity` and `com.unity.nuget.newtonsoft-json` installed.

### Minimum runtime wiring (for reference when writing samples/docs)

1. `NakamaHiroCoordinator` MonoBehaviour on a scene object, with scheme/host/port/server-key fields set.
2. A component implementing `INakamaSessionProvider` (often via `NakamaSessionProviderBehaviour`) assigned to the coordinator.
3. Optional `NakamaHiro<Feature>System` components as children or siblings; they find the coordinator through `GetComponentInParent`.
4. Game code awaits `coordinator.Hiro.<Feature>.<Method>Async(session, ...)` or the facade's `*Async`, and subscribes to `*Completed` events (or uses `NakamaHiroSystemObserver` / `NakamaHiroObservableState<T>` for disposable subscriptions and last-value caches).

## Dependencies (from `package.json`)

- Unity **2022.3**+
- `com.heroiclabs.nakama-unity` pinned to the `v3.21.1` tag (git URL with `?path=/Packages/Nakama`)
- `com.unity.nuget.newtonsoft-json` `3.2.1`

Only bump these intentionally; they gate the assembly references (`NakamaRuntime`, `Newtonsoft.Json`) that both asmdefs rely on.

## Checks before finishing a change

- `Runtime/NakamaHiro.Client.asmdef` stays `noEngineReferences: true`; nothing under `Runtime/` (outside `Runtime/Unity/`) imports `UnityEngine`.
- Every new RPC id matches the exact server string and lives in `HiroRpcIds`.
- DTOs use PascalCase properties and rely on `HiroJson.DefaultSettings` (no ad-hoc `JsonProperty` attributes unless unavoidable).
- Each new async method ends with `CancellationToken cancellationToken = default` and forwards it.
- `CHANGELOG.md` reflects the change.
- No admin-only RPC leaked into `HiroRpcIds`, `NakamaHiroClient`, or `NakamaHiro*System`.

## Related files

- `AGENTS.md` — contributor/agent contract (concise version of the rules above).
- `DOCUMENTATION.md` — consumer-facing how-tos, examples, and observation patterns.
- `README.md` — install snippet and elevator pitch.
- `CHANGELOG.md` — release notes.
- Sibling server repo `nakama-hiro` — authoritative RPC contract.
