# Nakama Hiro Client — documentation

Unity Package Manager (UPM) client for **nakama-hiro**: typed RPC wrappers and optional Unity `MonoBehaviour` facades on top of [Nakama Unity](https://github.com/heroiclabs/nakama-unity).

The **authoritative RPC contract** (payload shapes and id strings) lives in the sibling server repository: `nakama-hiro/src/main.ts` (header comment) and `nakama-hiro/src/features/*/register*Rpcs.ts`. This package mirrors those ids in `HiroRpcIds` and uses **snake_case** on the wire via Newtonsoft.Json.

---

## Getting started

### Prerequisites

- **Unity 2022.3** or newer (see `package.json`).
- A running **Nakama** instance with the **nakama-hiro** TypeScript module deployed and RPCs registered.
- Player authentication implemented in your game (device id, custom id, etc.) so you can obtain an `ISession`.

### Install the package

**Local path (development):** in your Unity project’s `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nakamahiro.client": "file:../../path/to/nakama-hiro-unity"
  }
}
```

Adjust the path relative to your Unity project folder.

**From Git:** Unity **Window → Package Manager → + → Add package from git URL…**  
If the package is in a subfolder of the repo, append `?path=/subfolder` (same pattern as [Nakama’s Unity install docs](https://heroiclabs.com/docs/nakama/client-libraries/unity/)).

Dependencies are declared in `package.json` (`com.heroiclabs.nakama-unity`, `com.unity.nuget.newtonsoft-json`).

### What gets compiled

| Assembly | Purpose |
|----------|---------|
| **NakamaHiro.Client** | Engine-agnostic: `NakamaHiroClient`, DTOs, `HiroRpcInvoker`, `HiroJson`, `HiroRpcIds`. |
| **NakamaHiro.Client.Unity** | Unity integration: `NakamaHiroCoordinator`, `INakamaSessionProvider`, optional `NakamaHiro*System` facades. |

Reference **NakamaHiro.Client** from non-Unity assemblies if you share client code; use **NakamaHiro.Client.Unity** only where `UnityEngine` is available.

### Minimal scene setup

1. Add an empty GameObject with **NakamaHiroCoordinator**.
2. Set **Scheme**, **Host**, **Port**, and **Server Key** to match your Nakama API settings.
3. Add a **MonoBehaviour** that implements **INakamaSessionProvider** (see below) and assign it to the coordinator’s session provider field **or** call `ConfigureSessionProvider` from code before `GetSessionAsync` is used.
4. Optionally add **NakamaHiro*System** components (e.g. `NakamaHiroEconomySystem`) as children or siblings; they resolve `NakamaHiroCoordinator` via parent or serialized reference.

---

## How-tos and examples

### Call Hiro RPCs from plain C# (no Unity types)

Use **Nakama**’s `IClient` plus **NakamaHiroClient**. Every feature is a nested client (e.g. `Hiro.Economy`, `Hiro.Challenges`).

```csharp
using System.Threading.Tasks;
using Nakama;
using NakamaHiro.Client;

public static class HiroExample
{
    public static async Task<EconomyWalletGetResponse> FetchWalletAsync(
        IClient client,
        ISession session)
    {
        var hiro = new NakamaHiroClient(client);
        return await hiro.Economy.WalletGetAsync(session);
    }
}
```

Pass a **cancellation token** into overloads that accept `CancellationToken` for cooperative cancellation.

### Wire authentication with `INakamaSessionProvider`

The coordinator does **not** log users in by itself. You supply a session by implementing **INakamaSessionProvider** (or subclass **NakamaSessionProviderBehaviour**).

Example: device id authentication using the same coordinator’s Nakama client:

```csharp
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using NakamaHiro.Client.Unity;
using UnityEngine;

public sealed class DeviceIdSessionProvider : NakamaSessionProviderBehaviour
{
    [SerializeField] private NakamaHiroCoordinator _coordinator;

    public override async Task<ISession> GetSessionAsync(CancellationToken cancellationToken = default)
    {
        var client = _coordinator.Nakama;
        var id = SystemInfo.deviceUniqueIdentifier;
        return await client.AuthenticateDeviceAsync(id, create: true, cancellationToken: cancellationToken);
    }
}
```

Assign this component to **NakamaHiroCoordinator**’s session provider reference, or call `coordinator.ConfigureSessionProvider(myProvider)` from `Awake`/`Start` in bootstrap code.

### Use optional `MonoBehaviour` feature systems

Types such as **NakamaHiroEconomySystem**, **NakamaHiroChallengesSystem**, and **NakamaHiroInventorySystem** wrap `NakamaHiroClient` and call `Coordinator.GetSessionAsync()` for you. They expose **events** (e.g. `WalletGetCompleted`) and **async methods** that return the same DTOs as the core client.

```csharp
// From a UI button or other MonoBehaviour:
var economy = GetComponent<NakamaHiroEconomySystem>();
var wallet = await economy.WalletGetAsync(cancellationToken);
```

Ensure a **NakamaHiroCoordinator** is assigned or found in parents (**NakamaHiroFeatureSystemBase** uses `GetComponentInParent` when the field is unset).

### JSON: snake_case and DTOs

Serialization uses **HiroJson.DefaultSettings**: **SnakeCaseNamingStrategy** for property names, ignored nulls, and ignored missing members on deserialize. Use **PascalCase** in C# properties; the server still sees **snake_case** JSON.

Some responses expose flexible fragments as **Newtonsoft.Json.Linq.JObject** (for example achievement entries). For stable typing, map or deserialize those nodes in your game layer as needed.

### Custom RPCs or raw JSON

If you add new RPCs on the server before this package ships wrappers:

- Add the id to **HiroRpcIds** (keep in sync with `register*Rpcs.ts`).
- Use **HiroRpcInvoker** with the same `JsonSerializerSettings` as `NakamaHiroClient`, or call **CallRawAsync** when you already have a JSON string.

```csharp
var invoker = new HiroRpcInvoker(client); // uses HiroJson.DefaultSettings
var response = await invoker.CallAsync<MyResponseDto>(session, "my_new_rpc", new { foo = 1 });
```

### Tests and dependency injection

- **NakamaHiroCoordinator.ConfigureClient(IClient, NakamaHiroClient)** — inject a mock `IClient` or prebuilt `NakamaHiroClient` before `Awake` (e.g. from a test harness).
- **DelegateNakamaSessionProvider** — supply `Func<CancellationToken, Task<ISession>>` without a `MonoBehaviour`.

```csharp
var coordinator = go.AddComponent<NakamaHiroCoordinator>();
coordinator.ConfigureSessionProvider(new DelegateNakamaSessionProvider(
    ct => Task.FromResult(fakeSession)));
```

### Discovering methods and RPC ids

- **C# API:** explore **NakamaHiroClient** properties (`Achievements`, `Inventory`, `Economy`, `Auctions`, `Progression`, `Stats`, `Streaks`, `Leaderboards`, `EventLeaderboards`, `Energy`, `Challenges`, `Unlockables`).
- **RPC string ids:** **HiroRpcIds** (`achievements_get`, `economy_wallet_get`, …).
- **Request/response shapes:** `nakama-hiro/src/main.ts` comment block (lines 1–89) and Hiro concepts docs linked there.

---

## Related files in this repo

- **README.md** — short overview, install snippet, requirements.
- **CHANGELOG.md** — version history.
- **AGENTS.md** — contributor notes (contract, casing, assembly split).

For day-to-day integration work, keep the **nakama-hiro** server repo open alongside this package so RPC and config changes stay aligned.
