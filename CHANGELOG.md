# Changelog

## Unreleased

- Add `MatchManagerHiroClient` (`match_manager_get`, `match_manager_mine`) and `NakamaHiroMatchManagerSystem` Unity facade.
- Add `NakamaHiroSystemObserver.Subscribe` and `NakamaHiroObservableState<T>` for disposable subscriptions and last-value UI caches; document observing RPC-driven updates and realtime/polling in `DOCUMENTATION.md`.

## 0.1.0

- Initial scaffold: UPM `package.json`, `Runtime` assembly referencing Nakama Unity (`NakamaRuntime`), placeholder type, documentation only. No RPC client API yet.
