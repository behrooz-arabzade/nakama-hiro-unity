# Nakama Hiro Client — documentation

Unity Package Manager (UPM) client for **nakama-hiro**: typed RPC wrappers and optional Unity `MonoBehaviour` facades on top of [Nakama Unity](https://github.com/heroiclabs/nakama-unity).

The **authoritative RPC contract** (payload shapes and id strings) lives in the server repository **[nakama-hiro](https://github.com/behrooz-arabzade/nakama-hiro)**: [`src/main.ts`](https://github.com/behrooz-arabzade/nakama-hiro/blob/main/src/main.ts) (header comment) and `src/features/*/register*Rpcs.ts`. This package mirrors those ids in `HiroRpcIds` and uses **snake_case** on the wire via Newtonsoft.Json.

---

## Getting started

### Prerequisites

- **Unity 2022.3** or newer (see `package.json`).
- A running **Nakama** instance with the **nakama-hiro** TypeScript module deployed and RPCs registered.
- Player authentication implemented in your game (device id, custom id, etc.) so you can obtain an `ISession`.

### Install the package

**From GitHub (recommended):** in your Unity project’s `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.nakamahiro.client": "https://github.com/behrooz-arabzade/nakama-hiro-unity.git"
  }
}
```

Pin a branch or tag with a URL suffix such as `#main` or `#v0.2.0`. Unity **Window → Package Manager → + → Add package from git URL…** accepts the same URL.

If the package ever lives in a **subfolder** of the repo, append `?path=/subfolder` (same pattern as [Nakama’s Unity install docs](https://heroiclabs.com/docs/nakama/client-libraries/unity/)).

**Local path (fork / offline dev):** point `com.nakamahiro.client` at a clone on disk, for example:

```json
"com.nakamahiro.client": "file:../../path/to/nakama-hiro-unity"
```

Adjust the path relative to your Unity project folder.

Dependencies are declared in `package.json` (`com.heroiclabs.nakama-unity`, `com.unity.nuget.newtonsoft-json`).

### What gets compiled

| Assembly | Purpose |
|----------|---------|
| **NakamaHiro.Client** | Engine-agnostic: `NakamaHiroClient`, DTOs, `HiroRpcInvoker`, `HiroJson`, `HiroRpcIds`. |
| **NakamaHiro.Client.Unity** | Unity integration: `NakamaHiroCoordinator`, `INakamaSessionProvider`, optional `NakamaHiro*System` facades, `NakamaHiroSystemObserver`, `NakamaHiroObservableState<T>`. |

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

### Observe system changes (vs Hiro Unity’s `SystemObserver`)

Heroic Labs’ [Observe System Changes](https://heroiclabs.com/docs/hiro/unity/getting-started/observing-system-changes/) doc targets the **commercial Hiro Unity SDK** (`SystemObserver`, `IObserver<T>`, in-client system state). This package is an **RPC client** for **nakama-hiro**: there is no shared Hiro system graph, but you get the same *practical* outcome—**UI updates when RPCs complete**—by subscribing to the `*Completed` events on **`NakamaHiro*System`** (or by handling `await` results from **`NakamaHiroClient`**).

**1. Subscribe to `*Completed` events** (after each successful call to the matching `*Async` method):

```csharp
using System;
using NakamaHiro.Client;
using NakamaHiro.Client.Unity;
using UnityEngine;

public sealed class WalletHud : MonoBehaviour
{
    [SerializeField] private NakamaHiroEconomySystem _economy;

    void OnEnable()
    {
        _economy.WalletGetCompleted += OnWallet;
    }

    void OnDisable()
    {
        _economy.WalletGetCompleted -= OnWallet;
    }

    void OnWallet(EconomyWalletGetResponse wallet)
    {
        // Refresh UI from wallet
    }

    public async void RefreshWallet()
    {
        await _economy.WalletGetAsync(destroyCancellationToken);
    }
}
```

**2. Disposable subscription with `NakamaHiroSystemObserver`** (same pattern, easier to pair with pooling or dynamic UI):

```csharp
using System;
using NakamaHiro.Client;
using NakamaHiro.Client.Unity;

IDisposable sub = NakamaHiroSystemObserver.Subscribe<EconomyWalletGetResponse>(
    h => economy.WalletGetCompleted += h,
    h => economy.WalletGetCompleted -= h,
    w => { /* update UI */ });

// later: sub.Dispose();
```

**3. Cached last value for binding** — **`NakamaHiroObservableState<T>`** stores the latest DTO and raises **`Changed`** whenever **`Set`** runs. Use **`MirrorFrom`** to tie it to a feature event, or call **`Set`** yourself after **`await`**:

```csharp
using NakamaHiro.Client;
using NakamaHiro.Client.Unity;

var walletState = new NakamaHiroObservableState<EconomyWalletGetResponse>();
using (walletState.MirrorFrom(
           h => economy.WalletGetCompleted += h,
           h => economy.WalletGetCompleted -= h))
{
    walletState.Changed += w => { /* bind to UI */ };
    await economy.WalletGetAsync(cancellationToken);
}
```

Facades do **not** auto-refresh: something in your game must call **`WalletGetAsync`** (or another `*Async`) when the player acts, on scene load, or on a timer.

**4. Cross-device or server-pushed freshness** — RPC completion events only run when **this client** invoked an RPC. If another session changes data, use **Nakama realtime** (socket notifications you emit from server code, or built-in notifications) and, in the handler, call the relevant **`NakamaHiro*System.*Async`** to refetch—or **poll** `*Async` on a **`MonoBehaviour`** interval. Wire that in your game assembly; this package stays transport-agnostic.

**Threading:** If you touch **UnityEngine** APIs from handlers, ensure callbacks run on the main thread (Nakama Unity often completes awaits on the main thread; verify for your integration).

### JSON: snake_case and DTOs

Serialization uses **HiroJson.DefaultSettings**: **SnakeCaseNamingStrategy** for property names, ignored nulls, and ignored missing members on deserialize. Use **PascalCase** in C# properties; the server still sees **snake_case** JSON.

Some responses expose flexible fragments as **Newtonsoft.Json.Linq.JObject** (for example achievement entries). For stable typing, map or deserialize those nodes in your game layer as needed.

### Adding game-specific RPCs (structured extension pattern)

The server project (`nakama-hiro`) uses a base/game split — `src/hiro/` is the upstream base tree and `src/game/` is the game-owned extension seam. This package mirrors that split: the base SDK (`NakamaHiro.Client` / `NakamaHiro.Client.Unity`) is never edited by game projects; all game-specific RPC wrappers live in the **host Unity project's** own assemblies.

#### Overview

Two SDK extension points enable this:

- **`NakamaHiroClient.Invoker`** — the `HiroRpcInvoker` shared by all 14 base clients. Pass this into your game feature clients so they inherit the same `IClient` and `HiroJson` settings automatically.
- **`NakamaHiroCoordinator.SetGameExtensions` / `GetGameExtensions<T>`** — an opaque slot where the coordinator holds your game's root extension object. The SDK has zero compile-time knowledge of your game types; the cast lives entirely in your game assembly.

#### Folder structure in your host Unity project

Mirror the server's `src/game/` structure under `Assets/Game/HiroExtensions/`:

```
Assets/Game/
  HiroExtensions/
    Game.HiroExtensions.asmdef           ← engine-agnostic (noEngineReferences: true)
    GameHiroRpcIds.cs                    ← game RPC id constants (mirrors HiroRpcIds)
    GameHiroClient.cs                    ← root facade wrapping NakamaHiroClient
    Features/
      DemoProfile/
        DemoProfileDtos.cs               ← request/response types
        DemoProfileHiroClient.cs         ← typed async methods
      <NextFeature>/
        ...
  HiroExtensions.Unity/
    Game.HiroExtensions.Unity.asmdef    ← Unity-aware
    GameHiroCoordinatorBootstrap.cs     ← wires GameHiroClient into coordinator (Awake)
    Features/
      DemoProfile/
        NakamaHiroDemoProfileSystem.cs  ← optional MonoBehaviour facade
```

#### Assembly definitions

**`Game.HiroExtensions.asmdef`** (engine-agnostic, mirrors `NakamaHiro.Client`):
```json
{
  "name": "Game.HiroExtensions",
  "rootNamespace": "Game.HiroExtensions",
  "references": ["NakamaHiro.Client", "NakamaRuntime", "Newtonsoft.Json"],
  "noEngineReferences": true,
  "autoReferenced": false
}
```

**`Game.HiroExtensions.Unity.asmdef`** (Unity-aware, mirrors `NakamaHiro.Client.Unity`):
```json
{
  "name": "Game.HiroExtensions.Unity",
  "rootNamespace": "Game.HiroExtensions.Unity",
  "references": ["NakamaHiro.Client", "NakamaHiro.Client.Unity", "Game.HiroExtensions", "NakamaRuntime"],
  "noEngineReferences": false,
  "autoReferenced": false
}
```

#### Worked example: `DemoProfile` feature

The server-side feature lives in `nakama-hiro/src/game/features/demoProfile/` and registers `demo_profile_get`, `demo_profile_update`, `demo_profile_add_xp`. Here is the full client-side mirror.

**`GameHiroRpcIds.cs`** — string constants matching the server exactly:
```csharp
namespace Game.HiroExtensions
{
    public static class GameHiroRpcIds
    {
        public const string DemoProfileGet    = "demo_profile_get";
        public const string DemoProfileUpdate = "demo_profile_update";
        public const string DemoProfileAddXp  = "demo_profile_add_xp";
    }
}
```

**`Features/DemoProfile/DemoProfileDtos.cs`** — PascalCase C# properties; snake_case on the wire via `HiroJson.DefaultSettings`:
```csharp
namespace Game.HiroExtensions
{
    public sealed class DemoProfileUpdateRequest
    {
        public string DisplayName { get; set; }
        public string AvatarId    { get; set; }
    }

    public sealed class DemoProfileAddXpRequest
    {
        public int Amount { get; set; }
    }

    public sealed class DemoProfileResponse
    {
        public string DisplayName { get; set; }
        public string AvatarId    { get; set; }
        public int    Level       { get; set; }
        public int    TotalXp     { get; set; }
    }
}
```

**`Features/DemoProfile/DemoProfileHiroClient.cs`** — same shape as every base `*HiroClient`:
```csharp
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using NakamaHiro.Client;

namespace Game.HiroExtensions
{
    public sealed class DemoProfileHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        public DemoProfileHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<DemoProfileResponse> GetAsync(
            ISession session, CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<DemoProfileResponse>(
                session, GameHiroRpcIds.DemoProfileGet, null, cancellationToken);

        public Task<DemoProfileResponse> UpdateAsync(
            ISession session, DemoProfileUpdateRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<DemoProfileResponse>(
                session, GameHiroRpcIds.DemoProfileUpdate, request, cancellationToken);

        public Task<DemoProfileResponse> AddXpAsync(
            ISession session, DemoProfileAddXpRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<DemoProfileResponse>(
                session, GameHiroRpcIds.DemoProfileAddXp, request, cancellationToken);
    }
}
```

**`GameHiroClient.cs`** — root game facade; wraps `NakamaHiroClient` (not inherits — it's `sealed`):
```csharp
using NakamaHiro.Client;

namespace Game.HiroExtensions
{
    public sealed class GameHiroClient
    {
        /// <summary>Base SDK client. Use for Achievements, Economy, Inventory, etc.</summary>
        public NakamaHiroClient Hiro      { get; }
        public DemoProfileHiroClient DemoProfile { get; }

        public GameHiroClient(NakamaHiroClient hiro)
        {
            Hiro        = hiro;
            DemoProfile = new DemoProfileHiroClient(hiro.Invoker);
        }
    }
}
```

**`GameHiroCoordinatorBootstrap.cs`** — the extension seam (analogue of `registerGameFeatures.ts` on the server). Add one per game, touch it only when adding a new feature client to `GameHiroClient`:
```csharp
using Game.HiroExtensions;
using NakamaHiro.Client.Unity;
using UnityEngine;

namespace Game.HiroExtensions.Unity
{
    /// <summary>
    /// Creates GameHiroClient and registers it with NakamaHiroCoordinator.
    /// [DefaultExecutionOrder(-50)] ensures this runs before any feature system
    /// that calls GetGameExtensions in its own Awake/Start.
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public sealed class GameHiroCoordinatorBootstrap : MonoBehaviour
    {
        [SerializeField] private NakamaHiroCoordinator _coordinator;

        private void Awake()
        {
            if (_coordinator == null)
                _coordinator = GetComponentInParent<NakamaHiroCoordinator>();
            _coordinator.SetGameExtensions(new GameHiroClient(_coordinator.Hiro));
        }
    }
}
```

**`Features/DemoProfile/NakamaHiroDemoProfileSystem.cs`** (optional) — MonoBehaviour facade with `*Completed` events, same pattern as the 14 base systems:
```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Game.HiroExtensions;
using NakamaHiro.Client.Unity;

namespace Game.HiroExtensions.Unity
{
    public sealed class NakamaHiroDemoProfileSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<DemoProfileResponse> GetCompleted;
        public event Action<DemoProfileResponse> UpdateCompleted;
        public event Action<DemoProfileResponse> AddXpCompleted;

        private DemoProfileHiroClient DemoProfile =>
            Coordinator.GetGameExtensions<GameHiroClient>().DemoProfile;

        public async Task<DemoProfileResponse> GetAsync(
            CancellationToken cancellationToken = default)
        {
            var r = await DemoProfile.GetAsync(
                await SessionAsync(cancellationToken), cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<DemoProfileResponse> UpdateAsync(
            DemoProfileUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await DemoProfile.UpdateAsync(
                await SessionAsync(cancellationToken), request, cancellationToken);
            UpdateCompleted?.Invoke(r);
            return r;
        }

        public async Task<DemoProfileResponse> AddXpAsync(
            DemoProfileAddXpRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await DemoProfile.AddXpAsync(
                await SessionAsync(cancellationToken), request, cancellationToken);
            AddXpCompleted?.Invoke(r);
            return r;
        }
    }
}
```

#### Scene wiring

1. Add **`GameHiroCoordinatorBootstrap`** to the same GameObject as (or a child of) `NakamaHiroCoordinator`.
2. Assign the coordinator reference, or leave it unset and let `GetComponentInParent` find it.
3. Optionally add **`NakamaHiroDemoProfileSystem`** as a sibling — it finds the coordinator the same way as any base system.

#### Adding a new feature — step-by-step guide

Assumes your server already registers the RPCs (e.g. `npc_shop_get`, `npc_shop_buy`). No SDK files change; no bootstrap file changes.

**Step 1 — Add the RPC id constants**

Open `GameHiroRpcIds.cs` and append your new ids:

```csharp
public static class GameHiroRpcIds
{
    // existing ids...

    public const string NpcShopGet = "npc_shop_get";
    public const string NpcShopBuy = "npc_shop_buy";
}
```

**Step 2 — Create the DTOs**

Create `Assets/Game/HiroExtensions/Features/NpcShop/NpcShopDtos.cs`. PascalCase in C#; snake_case on the wire is handled automatically by `HiroJson.DefaultSettings`. Match field names to what the server returns.

```csharp
using System.Collections.Generic;

namespace Game.HiroExtensions
{
    public sealed class NpcShopGetRequest
    {
        public string ShopId { get; set; }
    }

    public sealed class NpcShopBuyRequest
    {
        public string ShopId   { get; set; }
        public string ItemId   { get; set; }
        public int    Quantity { get; set; }
    }

    public sealed class NpcShopItem
    {
        public string ItemId      { get; set; }
        public string DisplayName { get; set; }
        public int    Price       { get; set; }
        public int    Stock       { get; set; }
    }

    public sealed class NpcShopGetResponse
    {
        public string            ShopId { get; set; }
        public List<NpcShopItem> Items  { get; set; }
    }

    public sealed class NpcShopBuyResponse
    {
        public bool Success            { get; set; }
        public int  RemainingCurrency  { get; set; }
    }
}
```

**Step 3 — Create the feature client**

Create `Assets/Game/HiroExtensions/Features/NpcShop/NpcShopHiroClient.cs`. Same shape as every base `*HiroClient` — holds a `HiroRpcInvoker`, exposes typed async methods:

```csharp
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using NakamaHiro.Client;

namespace Game.HiroExtensions
{
    public sealed class NpcShopHiroClient
    {
        private readonly HiroRpcInvoker _rpc;

        public NpcShopHiroClient(HiroRpcInvoker rpc) => _rpc = rpc;

        public Task<NpcShopGetResponse> GetAsync(
            ISession session,
            NpcShopGetRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<NpcShopGetResponse>(
                session, GameHiroRpcIds.NpcShopGet, request, cancellationToken);

        public Task<NpcShopBuyResponse> BuyAsync(
            ISession session,
            NpcShopBuyRequest request,
            CancellationToken cancellationToken = default) =>
            _rpc.CallAsync<NpcShopBuyResponse>(
                session, GameHiroRpcIds.NpcShopBuy, request, cancellationToken);
    }
}
```

**Step 4 — Register it in `GameHiroClient`**

Open `Assets/Game/HiroExtensions/GameHiroClient.cs` and add one property and one constructor line:

```csharp
public sealed class GameHiroClient
{
    public NakamaHiroClient      Hiro        { get; }
    public DemoProfileHiroClient DemoProfile { get; }
    public NpcShopHiroClient     NpcShop     { get; }   // ← add

    public GameHiroClient(NakamaHiroClient hiro)
    {
        Hiro        = hiro;
        DemoProfile = new DemoProfileHiroClient(hiro.Invoker);
        NpcShop     = new NpcShopHiroClient(hiro.Invoker);  // ← add
    }
}
```

`GameHiroCoordinatorBootstrap` needs **no change**.

**Step 5 (optional) — Create the MonoBehaviour facade**

Only needed if you want scene-based wiring and `*Completed` events. Create `Assets/Game/HiroExtensions.Unity/Features/NpcShop/NakamaHiroNpcShopSystem.cs`:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Game.HiroExtensions;
using NakamaHiro.Client.Unity;

namespace Game.HiroExtensions.Unity
{
    public sealed class NakamaHiroNpcShopSystem : NakamaHiroFeatureSystemBase
    {
        public event Action<NpcShopGetResponse> GetCompleted;
        public event Action<NpcShopBuyResponse> BuyCompleted;

        private NpcShopHiroClient NpcShop =>
            Coordinator.GetGameExtensions<GameHiroClient>().NpcShop;

        public async Task<NpcShopGetResponse> GetAsync(
            NpcShopGetRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await NpcShop.GetAsync(
                await SessionAsync(cancellationToken), request, cancellationToken);
            GetCompleted?.Invoke(r);
            return r;
        }

        public async Task<NpcShopBuyResponse> BuyAsync(
            NpcShopBuyRequest request,
            CancellationToken cancellationToken = default)
        {
            var r = await NpcShop.BuyAsync(
                await SessionAsync(cancellationToken), request, cancellationToken);
            BuyCompleted?.Invoke(r);
            return r;
        }
    }
}
```

**Files changed or created**

| Action | File |
|--------|------|
| Edit | `GameHiroRpcIds.cs` — new id constants |
| Edit | `GameHiroClient.cs` — 1 new property + 1 constructor line |
| Create | `Features/NpcShop/NpcShopDtos.cs` |
| Create | `Features/NpcShop/NpcShopHiroClient.cs` |
| Create (optional) | `Features/NpcShop/NakamaHiroNpcShopSystem.cs` |

#### Calling game features from game code

Three call styles are available simultaneously; choose the abstraction level that fits:

**Direct (no MonoBehaviour facade needed):**
```csharp
var game = coordinator.GetGameExtensions<GameHiroClient>();
var profile = await game.DemoProfile.GetAsync(session);
```

**Through the optional MonoBehaviour facade (scene-based wiring):**
```csharp
[SerializeField] private NakamaHiroDemoProfileSystem _demoProfile;

void OnEnable()  => _demoProfile.GetCompleted += OnProfileLoaded;
void OnDisable() => _demoProfile.GetCompleted -= OnProfileLoaded;
```

**Disposable subscription (same as base systems):**
```csharp
using NakamaHiro.Client.Unity;

IDisposable sub = NakamaHiroSystemObserver.Subscribe<DemoProfileResponse>(
    h => demoProfileSystem.GetCompleted += h,
    h => demoProfileSystem.GetCompleted -= h,
    r => { /* update UI */ });
// later: sub.Dispose();
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
- **Request/response shapes:** [nakama-hiro `src/main.ts`](https://github.com/behrooz-arabzade/nakama-hiro/blob/main/src/main.ts) header comment and Hiro concepts docs linked there.

---

## Related files in this repo

- **README.md** — short overview, install snippet, requirements.
- **CHANGELOG.md** — version history.
- **AGENTS.md** — contributor notes (contract, casing, assembly split).
- **Runtime/Unity/NakamaHiroUnityFeatureSystems.cs** — `NakamaHiro*System` facades and `*Completed` events.
- **Runtime/Unity/NakamaHiroSystemObserver.cs** — disposable `Subscribe` helper for those events.
- **Runtime/Unity/NakamaHiroObservableState.cs** — last-value cache and `Changed` for UI binding.

For day-to-day integration work, keep the **[nakama-hiro](https://github.com/behrooz-arabzade/nakama-hiro)** server repository open alongside this package so RPC and config changes stay aligned.
