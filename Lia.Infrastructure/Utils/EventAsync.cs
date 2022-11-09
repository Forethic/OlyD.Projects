using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Lia.Utils
{
    public static class EventAsync
    {
        private sealed class EventHandlerTaskSource<TEventArgs>
        {
            private readonly TaskCompletionSource<object> _taskCompletionSource;
            private readonly Action<EventHandler<TEventArgs>> _removeEventHandler;

            public Task<object> Task => _taskCompletionSource.Task;

            public EventHandlerTaskSource(Action<EventHandler<TEventArgs>> addEventHandler, Action<EventHandler<TEventArgs>> removeEventHandler, Action beginAction = null)
            {
                if (addEventHandler == null) { throw new ArgumentNullException(nameof(addEventHandler)); }
                if (removeEventHandler == null) { throw new ArgumentNullException(nameof(removeEventHandler)); }

                _taskCompletionSource = new TaskCompletionSource<object>();
                _removeEventHandler = removeEventHandler;
                addEventHandler(EventCompleted);
                beginAction?.Invoke();
            }

            private void EventCompleted(object sender, TEventArgs e)
            {
                _removeEventHandler(EventCompleted);
                _taskCompletionSource.SetResult(e);
            }
        }

        private sealed class RoutedEventHandlerTaskSource
        {
            private readonly TaskCompletionSource<RoutedEventArgs> _taskCompletionSource;
            private readonly Action<RoutedEventHandler> _removeEventHandler;

            public Task<RoutedEventArgs> Task => _taskCompletionSource.Task;

            public RoutedEventHandlerTaskSource(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler, Action beginAction = null)
            {
                if (addEventHandler == null) { throw new ArgumentNullException(nameof(addEventHandler)); }
                if (removeEventHandler == null) { throw new ArgumentNullException(nameof(removeEventHandler)); }

                _taskCompletionSource = new TaskCompletionSource<RoutedEventArgs>();
                _removeEventHandler = removeEventHandler;
                addEventHandler(EventCompleted);
                beginAction?.Invoke();
            }

            private void EventCompleted(object sender, RoutedEventArgs e)
            {
                _removeEventHandler(EventCompleted);
                _taskCompletionSource.SetResult(e);
            }
        }

        public static Task<object> FromEvent<T>(Action<EventHandler<T>> addEventHandler, Action<EventHandler<T>> removeEventHanlder, Action beginAction = null)
        {
            return new EventHandlerTaskSource<T>(addEventHandler, removeEventHanlder, beginAction).Task;
        }

        public static Task<RoutedEventArgs> FromRoutedEvent(Action<RoutedEventHandler> addEventHandler, Action<RoutedEventHandler> removeEventHandler, Action beginAction = null)
        {
            return new RoutedEventHandlerTaskSource(addEventHandler, removeEventHandler, beginAction).Task;
        }
    }
}