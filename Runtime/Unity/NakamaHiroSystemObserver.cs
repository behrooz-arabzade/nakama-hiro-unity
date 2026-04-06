using System;
using System.Threading;

namespace NakamaHiro.Client.Unity
{
    /// <summary>
    /// Subscribe to <c>event Action&lt;T&gt;</c> style callbacks from <see cref="NakamaHiroFeatureSystemBase"/>
    /// derivatives (e.g. <see cref="NakamaHiroEconomySystem.WalletGetCompleted"/>). Disposing unsubscribes.
    /// </summary>
    public static class NakamaHiroSystemObserver
    {
        /// <param name="add">Adds the handler, e.g. <c>h => economy.WalletGetCompleted += h</c>.</param>
        /// <param name="remove">Removes the handler, e.g. <c>h => economy.WalletGetCompleted -= h</c>.</param>
        /// <param name="onNext">Invoked whenever the source event fires.</param>
        public static IDisposable Subscribe<T>(
            Action<Action<T>> add,
            Action<Action<T>> remove,
            Action<T> onNext)
        {
            if (add == null) throw new ArgumentNullException(nameof(add));
            if (remove == null) throw new ArgumentNullException(nameof(remove));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            void Handler(T value) => onNext(value);
            add(Handler);
            return new Subscription<T>(remove, Handler);
        }

        private sealed class Subscription<T> : IDisposable
        {
            private Action<Action<T>> _remove;
            private readonly Action<T> _handler;

            public Subscription(Action<Action<T>> remove, Action<T> handler)
            {
                _remove = remove;
                _handler = handler;
            }

            public void Dispose()
            {
                var r = Interlocked.Exchange(ref _remove, null);
                r?.Invoke(_handler);
            }
        }
    }
}
