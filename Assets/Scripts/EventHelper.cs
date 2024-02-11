using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHelper
{
    private static Dictionary<string, Action> events = new Dictionary<string, Action>();

    public static void Subscribe(string eventName, Action listener)
    {
        if (!events.ContainsKey(eventName))
        {
            events[eventName] = listener;
        }
        else
        {
            events[eventName] += listener;
        }
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] -= listener;
            if (events[eventName] == null)
            {
                events.Remove(eventName);
            }
        }
    }

    public static void Raise(string eventName)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName]?.Invoke();
        }
    }
}