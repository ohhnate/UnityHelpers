// Handler.cs - A generic singleton handler class for handling a specific type of component.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Handler implementation provides a singleton MonoBehaviour component that can be used as a
// handler for a specific type of component. It allows easy access to the component without having to
// search for it every time it's needed. Other handlers can derive from this class to become singletons
// for their own specific component types.
// No accreditation is required but it would be highly appreciated <3

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Singleton MonoBehaviour component that can be used as a handler for a specific type of component.
/// </summary>
/// <typeparam name="T">Type of the component to be handled by this singleton.</typeparam>
public class Handler<T> : MonoBehaviour where T : Component
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object LockObject = new();
    private static readonly Dictionary<string, T> Instances = new();

    /// <summary>
    /// Gets the singleton instance of the component.
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (LockObject)
            {
                string fullName = typeof(T).FullName;
                if (Instances.TryGetValue(fullName, out T instance))
                {
                    return instance;
                }
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject go = new();
                    instance = go.AddComponent<T>();
                    go.name = instance.GetType()?.FullName;
                }
                Instances[typeof(T).FullName] = instance;
                return instance;
            }
        }
    }

    /// <summary>
    /// Initializes the instance of the component, if it has not already been initialized.
    /// </summary>
    protected virtual void Awake()
    {
        lock (LockObject)
        {
            string fullName = typeof(T).FullName;
            if (fullName != null && !Instances.ContainsKey(fullName))
            {
                Instances[fullName] = this as T;
            }
        }
    }

    /// <summary>
    /// Removes a singleton instance of the component.
    /// </summary>
    /// <param name="instance">The singleton instance to remove.</param>
    public static void RemoveInstance(T instance)
    {
        lock (LockObject)
        {
            if (!Instances.ContainsValue(instance)) return;
        
            string key = Instances.FirstOrDefault(x => x.Value == instance).Key;
            Instances.Remove(key);
        }
    }

    /// <summary>
    /// Clears all singleton instances of the component.
    /// </summary>
    public static void ClearInstances()
    {
        lock (LockObject)
        {
            Instances.Clear();
        }
    }

    /// <summary>
    /// Returns true if an instance of the component exists.
    /// </summary>
    /// <returns>True if an instance of the component exists, false otherwise.</returns>
    public static bool HasInstance()
    {
        lock (LockObject)
        {
            string fullName = typeof(T).FullName;
            return fullName != null && Instances.ContainsKey(fullName);
        }
    }

    /// <summary>
    /// Returns the number of singleton instances of the component.
    /// </summary>
    /// <returns>The number of singleton instances of the component.</returns>
    public static int Count()
    {
        lock (LockObject)
        {
            return Instances.Count;
        }
    }
}