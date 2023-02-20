// Tools.cs - A static class with useful generic helper methods for a Unity project.
// Version 1.0.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// This Tools class contains various generic static methods that can be useful in a Unity project.
// No accreditation is required but it would be highly appreciated <3

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

internal static class Tools
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    /// <summary>
    /// Caches wait for seconds so you do not have to create new on Coroutines.
    /// Reduces trash for the garbage collector
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }
    
    private static readonly Dictionary<Func<bool>, WaitUntil> WaitUntilDictionary = new();

    /// <summary>
    /// Caches wait until conditions so you do not have to create new on Coroutines.
    /// Reduces trash for the garbage collector
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static WaitUntil GetWaitUntil(Func<bool> condition)
    {
        if (WaitUntilDictionary.TryGetValue(condition, out WaitUntil wait)) return wait;

        WaitUntilDictionary[condition] = new WaitUntil(condition);
        return WaitUntilDictionary[condition];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    /// <summary>
    /// Returns true if mouse pointer is currently hovering the UI
    /// </summary>
    /// <returns></returns>
    public static bool PointerIsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    /// <summary>
    /// Returns the world position of the mouse on a specified camera plane.
    /// </summary>
    /// <param name="camera">The camera to project the mouse position onto.</param>
    /// <returns>The world position of the mouse.</returns>
    public static Vector3 GetMouseWorldPosition(Camera camera)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -camera.transform.position.z;
        return camera.ScreenToWorldPoint(mousePosition);
    }

    /// <summary>
    /// Destroys all the children of a given object
    /// </summary>
    /// <param name="t"></param>
    public static void DeleteAllChildren(this Transform t)
    {
        foreach(Transform child in t)
        {
            Object.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Returns a random element from a dictionary of elements with associated weights.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="weightedValues">A dictionary of elements with associated weights.</param>
    /// <returns>A random element based on the provided weights.</returns>
    public static T GetRandomWeightedElement<T>(Dictionary<T, int> weightedValues)
    {
        int totalWeight = weightedValues.Sum(pair => pair.Value);
        int randomValue = Random.Range(0, totalWeight);
        foreach (KeyValuePair<T, int> pair in weightedValues)
        {
            randomValue -= pair.Value;
            if (randomValue < 0)
            {
                return pair.Key;
            }
        }
        return default;
    }

    public static Color LerpColor(Color from, Color to, float duration)
    {
        duration = Mathf.Clamp01(duration);
        return new Color(
            Mathf.Lerp(from.r, to.r, duration),
            Mathf.Lerp(from.g, to.g, duration),
            Mathf.Lerp(from.b, to.b, duration),
            Mathf.Lerp(from.a, to.a, duration)
        );
    }

    /// <summary>
    /// Checks quaternions for approximate equality
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static bool Approximately(Quaternion q1, Quaternion q2, float threshold = 0.1f)
    {
        return 1 - Mathf.Abs(Quaternion.Dot(q1, q2)) < threshold;
    }

    /// <summary>
    /// Checks vector3 for approximate equality
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static bool Approximately(Vector3 v1, Vector3 v2, float threshold = 0.1f)
    {
        return 1 - Mathf.Abs(Vector3.Dot(v1, v2)) < threshold;
    }

    /// <summary>
    /// Give the absolute value of a vector3
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 Vector3Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    /// <summary>
    /// Give the absolute value of a Quaternion
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    public static Quaternion QuaternionAbs(Quaternion q)
    {
        return new Quaternion(Mathf.Abs(q.x), Mathf.Abs(q.y), Mathf.Abs(q.z), Mathf.Abs(q.w));
    }
}