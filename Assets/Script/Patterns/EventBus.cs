using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : IEventBus
{
    public static IEventBus openMenuEvent = new EventBus();
    public static IEventBus openEndMenuEvent = new EventBus();
    public static IEventBus resetGameEvent = new EventBus();
    public static IEventBus enablePlayerHUDEvent = new EventBus();
    public static IEventBus DisableUIInputsEvent = new EventBus();
    public static IEventBus openGarageEvent = new EventBus();
    public static IEventBus switchEventSystemEvent = new EventBus();

    public static IEventBus healthChangeEvent = new EventBus();
    public static IEventBus boltChangeEvent = new EventBus();
    public static IEventBus ammoChangeEvent = new EventBus();
    public static IEventBus startCooldownEvent = new EventBus();
    public static IEventBus killChangeEvent = new EventBus();

    private readonly Dictionary<Type, List<object>> subscribers = new Dictionary<Type, List<object>>();

    public static void ResetEventSubscriber(ref IEventBus eventBus)
    {
        eventBus = new EventBus();
    }
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
