using System;
using UnityEditor;
using UnityEngine;
public class EventSubscriber
{
    public EventSubscriber(IEventBus eventBus, Action<EventArgs> callback)
    {
        eventBus.Subscribe(callback);
    }
}
