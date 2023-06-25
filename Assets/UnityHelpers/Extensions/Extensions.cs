// Extensions.cs - A collection of extension methods for different primitives and generics.
// Version 1.0.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// This class contains various extension methods to complement built-in C# and Unity types.
// No accreditation is required but it would be highly appreciated <3

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

internal static class Extensions
{
    #region String Extensions

    /// <summary>
    /// Concatenates a collection of strings into a single string, with the specified delimiter between each element.
    /// </summary>
    /// <param name="strings">The collection of strings to concatenate.</param>
    /// <param name="delimiter">The string to use as a delimiter between each element in the result.</param>
    /// <returns>A new string that is the concatenation of all the input strings, separated by the specified delimiter.</returns>
    public static string Join(this IEnumerable<string> strings, string delimiter)
    {
        return string.Join(delimiter, strings);
    }
    
    /// <summary>
    /// Splits a camelCase string into separate words.
    /// </summary>
    /// <param name="input">The camelCase string to split.</param>
    /// <returns>A string array containing the individual words in the camelCase string.</returns>
    public static string SplitCamelCase(this string input)
    {
        return Regex.Replace(input, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
    }
    
    /// <summary>
    /// Truncates the string to the specified length.
    /// </summary>
    public static string Truncate(this string str, int maxLength)
    {
        return str.Length <= maxLength ? str : str[..maxLength];
    }
    
    /// <summary>
    /// Converts the string to title case.
    /// </summary>
    public static string ToTitleCase(this string str)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
    }
    
    /// <summary>
    /// Converts the first character of the string to uppercase and leaves the rest of the string unchanged.
    /// </summary>
    public static string Capitalize(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }
        return char.ToUpper(str[0]) + str[1..];
    }

    /// <summary>
    /// Returns the string with all whitespace removed.
    /// </summary>
    public static string RemoveWhitespace(this string str)
    {
        return new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    #endregion

    #region List & Array Extensions
    
    /// <summary>
    /// Returns a random element from the list.
    /// </summary>
    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }
        int index = Random.Range(0, list.Count);
        return list[index];
    }
    
    /// <summary>
    /// Removes the first occurrence of the specified item from the list.
    /// </summary>
    public static void RemoveFirst<T>(this List<T> list, T item)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }
        int index = list.IndexOf(item);
        if (index >= 0)
        {
            list.RemoveAt(index);
        }
    }
    
    /// <summary>
    /// Removes a random element from the list and returns it.
    /// </summary>
    public static T RemoveRandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }
        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

    /// <summary>
    /// Rotates a list by a given number of positions to the right. Items that fall off the end are moved to the beginning of the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to rotate.</param>
    /// <param name="offset">The number of positions to rotate the list by. A positive value rotates right, and a negative value rotates left.</param>
    public static void Rotate<T>(this IList<T> list, int offset)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }
        int n = list.Count;
        offset %= n;
        if (offset < 0) {
            offset += n;
        }
        for (int i = 0; i < offset; i++) {
            T value = list[0];
            for (int j = 1; j < n; j++) {
                list[j - 1] = list[j];
            }
            list[n - 1] = value;
        }
    }
    
    /// <summary>
    /// Shuffle the elements in a collection using the Fisher-Yates algorithm. This method shuffles the elements in-place.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="list">The collection to shuffle.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    #endregion

    #region int Extensions

    /// <summary>
    /// Maps a value from one range to another range.
    /// </summary>
    public static float Map(this int value, int fromLow, int fromHigh, float toLow, float toHigh)
    {
        return (float)(value - fromLow) / (fromHigh - fromLow) * (toHigh - toLow) + toLow;
    }
    
    /// <summary>
    /// Clamps the value to the given range.
    /// </summary>
    public static int Clamp(this int value, int min, int max)
    {
        return value < min ? min : (value > max ? max : value);
    }

    #endregion

    #region float Extensions
    
    /// <summary>
    /// Remaps a value from one range to another range.
    /// </summary>
    public static float Map(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) / (fromHigh - fromLow) * (toHigh - toLow) + toLow;
    }

    /// <summary>
    /// Clamps the value to the given range.
    /// </summary>
    public static float Clamp(this float value, float min, float max)
    {
        return value < min ? min : (value > max ? max : value);
    }

    #endregion

    #region Boolean Extensions

    /// <summary>
    /// Converts a boolean value to an integer where true is represented as 1 and false as 0.
    /// </summary>
    /// <param name="b">The boolean value to convert.</param>
    /// <returns>The integer representation of the boolean value.</returns>
    public static int ToInt(this bool b) 
    {
        return b ? 1 : 0;
    }

    /// <summary>
    /// Toggles the value of a boolean variable between true and false.
    /// </summary>
    /// <param name="b">The boolean variable to toggle.</param>
    public static void Toggle(this ref bool b) 
    {
        b = !b;
    }

    /// <summary>
    /// Performs a logical AND operation between two boolean values.
    /// </summary>
    /// <param name="b">The first boolean value.</param>
    /// <param name="other">The second boolean value to perform the operation with.</param>
    /// <returns>True if both boolean values are true; otherwise, false.</returns>
    public static bool And(this bool b, bool other) 
    {
        return b && other;
    }

    /// <summary>
    /// Performs a logical XOR (exclusive or) operation between two boolean values.
    /// </summary>
    /// <param name="b">The first boolean value.</param>
    /// <param name="other">The second boolean value to perform the operation with.</param>
    /// <returns>True if only one of the boolean values is true; otherwise, false.</returns>
    public static bool Xor(this bool b, bool other) 
    {
        return b ^ other;
    }

    #endregion

    #region Color Extensions

    /// <summary>
    /// Returns a new color with the alpha value set to the specified value.
    /// </summary>
    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    #endregion
    
    #region UI Extensions

    /// <summary>
    /// Sets the alpha value of the image without changing its color.
    /// </summary>
    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
    
    /// <summary>
    /// Sets the color of the Image or RawImage component with the given RGB values and optional alpha value.
    /// </summary>
    /// <param name="graphic">The Image or RawImage component to set the color for.</param>
    /// <param name="color"> new color of the image.</param>
    public static void SetColor(this Graphic graphic, Color color)
    {
        graphic.color = color;
    }

    #endregion
}