using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : IEventBus
{
    public static IEventBus openMenuEvent = new EventBus();
    public static IEventBus openEndMenuEvent = new EventBus();
    public static IEventBus resetGameEvent = new EventBus();
    public static IEventBus enablePlayerHUD = new EventBus();
    public static IEventBus disableUiInputs = new EventBus();

    private readonly Dictionary<Type, List<object>> subscribers = new Dictionary<Type, List<object>>();

    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!subscribers.TryGetValue(eventType, out var handlers))
        {
            handlers = new List<object>();
            subscribers[eventType] = handlers;
        }

        handlers.Add(handler);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (subscribers.TryGetValue(eventType, out var handlers))
        {
            handlers.Remove(handler);
        }
    }

    public void Publish<TEvent>(TEvent eventArgs)
    {
        Type eventType = typeof(TEvent);

        if (subscribers.TryGetValue(eventType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                ((Action<TEvent>)handler)(eventArgs);
            }
        }
    }
}
