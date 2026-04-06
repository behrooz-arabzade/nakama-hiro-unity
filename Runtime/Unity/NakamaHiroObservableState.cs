using System;

namespace NakamaHiro.Client.Unity
{
    /// <summary>
    /// Keeps the last value from an RPC (or any source) and raises <see cref="Changed"/> when <see cref="Set"/> runs.
    /// Pair with <see cref="NakamaHiroSystemObserver"/> or <see cref="MirrorFrom"/> for UI-friendly caches.
    /// </summary>
    public sealed class NakamaHiroObservableState<T>
    {
        private T _value;
        private bool _hasValue;

        public bool HasValue => _hasValue;

        public T Value =>
            _hasValue
                ? _value
                : throw new InvalidOperationException("No value has been set yet.");

        public event Action<T> Changed;

        public void Set(T value)
        {
            _value = value;
            _hasValue = true;
            Changed?.Invoke(value);
        }

        public void Clear()
        {
            _hasValue = false;
            _value = default;
        }

        /// <summary>
        /// Subscribes so each notification calls <see cref="Set"/> (and thus <see cref="Changed"/>).
        /// Dispose the returned object to unsubscribe.
        /// </summary>
        public IDisposable MirrorFrom(Action<Action<T>> add, Action<Action<T>> remove) =>
            NakamaHiroSystemObserver.Subscribe(add, remove, Set);
    }
}
