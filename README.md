# Nakama Hiro Client (Unity)

UPM package for calling **[nakama-hiro](https://github.com/behrooz-arabzade/nakama-hiro)** server RPCs from Unity. It builds on [Nakama Unity](https://github.com/heroiclabs/nakama-unity) (`com.heroiclabs.nakama-unity`) and Newtonsoft.Json with **snake_case** on the wire.

## RPC contract (source of truth)

The server owns the contract:

- **[nakama-hiro `src/main.ts`](https://github.com/behrooz-arabzade/nakama-hiro/blob/main/src/main.ts)** (header comment) — full RPC catalog and payload shapes  
- **`nakama-hiro` `src/features/*/register*Rpcs.ts`** — registered id strings  

This package mirrors public RPC ids in `HiroRpcIds` and typed clients under `NakamaHiroClient`.

For day-to-day work, a multi-root editor workspace with both GitHub clones keeps server and client changes aligned.

## Install

### From GitHub (recommended)

In your Unity project’s `Packages/manifest.json`, add:

```json
{
  "dependencies": {
    "com.nakamahiro.client": "https://github.com/behrooz-arabzade/nakama-hiro-unity.git"
  }
}
```

To pin a **branch or tag**, append `#branch-name` or `#v0.2.0` to the URL (same as Unity’s [git dependencies](https://docs.unity3d.com/Manual/upm-git.html)).

Alternatively: **Window → Package Manager → + → Add package from git URL…** and paste:

```
https://github.com/behrooz-arabzade/nakama-hiro-unity.git
```

If the package ever lives in a **subfolder** of the repo, use `?path=/Packages/YourPackage` on the URL (see [Nakama Unity install docs](https://heroiclabs.com/docs/nakama/client-libraries/unity/)).

### From a local clone (development)

If you have this repository on disk:

```json
"com.nakamahiro.client": "file:../../path/to/nakama-hiro-unity"
```

Adjust the path relative to your Unity project folder.

## Documentation

- **[DOCUMENTATION.md](DOCUMENTATION.md)** — setup, `NakamaHiroCoordinator`, session provider, feature systems, observers, JSON notes  
- **[CHANGELOG.md](CHANGELOG.md)** — version history  
- **[AGENTS.md](AGENTS.md)** — contributor contract (RPC alignment, admin RPC policy, assembly split)  

## Requirements

- Unity **2022.3** or newer (see `package.json`; aligned with current `com.heroiclabs.nakama-unity`).
