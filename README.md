# Nakama Hiro Client (Unity)

UPM package for calling the **nakama-hiro** server RPCs from Unity. It builds on [Nakama Unity](https://github.com/heroiclabs/nakama-unity) (`com.heroiclabs.nakama-unity`).

**Status:** scaffold only (`0.1.0`). RPC wrappers and DTOs are not implemented yet.

## RPC contract (source of truth)

RPC ids and JSON request/response shapes are documented in the server repo:

- **Sibling server repo** `nakama-hiro/src/main.ts` — header comment (lines 1–89): full RPC catalog
- `nakama-hiro/src/features/*/register*Rpcs.ts` — registered id strings

Use **snake_case** JSON field names to match the server.

Develop side by side: open a **multi-root** Cursor/VS Code workspace with both `nakama-hiro` and `nakama-hiro-unity`.

## Install

### From disk (local dev)

In your Unity project `Packages/manifest.json`, add:

```json
"com.nakamahiro.client": "file:../../path/to/nakama-hiro-unity"
```

Adjust the path relative to your Unity project folder.

### From Git (after you publish this repo)

**Window → Package Manager → + → Add package from git URL…**

```
https://github.com/<org>/<repo>.git
```

If the package lives in a subfolder, use Unity’s `?path=` syntax (same as Nakama’s [install docs](https://heroiclabs.com/docs/nakama/client-libraries/unity/)).

## Requirements

- Unity **2022.3** or newer (aligned with current `com.heroiclabs.nakama-unity`).

## Changelog

See [CHANGELOG.md](CHANGELOG.md).
