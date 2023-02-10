using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// EnumUtils is a series of extenstion functions that make working with enumerations easier.
/// </summary>
public static class EnumUtils {

	/// <summary>
	/// Gets the total number of values in an enumeration.
	/// </summary>
	/// <returns>The number of elements in the requested enum.</returns>
	/// <typeparam name="T">The Enum type to get the length of.</typeparam>
	public static int GetLength<T>() {
		return System.Enum.GetValues(typeof(T)).Length;
	}

	/// <summary>
	/// Parse the specified string value into an enum.
	/// </summary>
	/// <param name="value">Value to parse.</param>
	/// <param name="ignoreCase">If set to <c>true</c> ignore case.</param>
	/// <typeparam name="T">The enum type to return.</typeparam>
	public static T Parse<T>(string value, bool ignoreCase = false) {
		return (T)System.Enum.Parse(typeof(T), value, ignoreCase);
	}

	/// <summary>
	/// Gets all of the elements of an enum as an array.
	/// </summary>
	/// <typeparam name="T">The type of enum to get the elements of.</typeparam>
	/// <returns>An array containing all the elements of an array.</returns>
	public static T[] GetElementArray<T>() {
		return (T[])System.Enum.GetValues(typeof(T));
	}

	/// <summary>
	/// Gets a random entry of an enum.
	/// </summary>
	/// <returns>A random enum entry.</returns>
	/// <typeparam name="T">The enum.</typeparam>
	public static T RandomElement<T>() {
		return RandomElement<T>(GetLength<T>());
	}

	/// <summary>
	/// Gets a random entry of an enum.
	/// </summary>
	/// <returns>A random enum entry at index less than max.</returns>
	/// <param name="max">Maximum (exclusive.)</param>
	/// <typeparam name="T">The enum.</typeparam>
	public static T RandomElement<T>(int max) {
		return RandomElement<T>(0, max);
	}

	/// <summary>
	/// Gets a random entry of an enum between min (inclusive) and max (exclusive).
	/// </summary>
	/// <param name="min">Minimum (inclusive).</param>
	/// <param name="max">Maximum (exclusive).</param>
	/// <typeparam name="T">The enum type to select a random value from.</typeparam>
	public static T RandomElement<T>(int min, int max) {
		System.Type t = typeof(T);
		int length = System.Enum.GetValues(t).Length;
		Debug.Assert(min >= 0 && max >= min && min < length && max <= length, "Random min/max out of enum bounds.");

		int index = UnityEngine.Random.Range(min, max);
		string name = System.Enum.GetName(t, index);
		return Parse<T>(name, false);
	}

	/// <summary>
	/// Determines whether one or more bit fields are set in the current instance.
	/// thisInstance And flag = flag 
	/// </summary>
	/// <param name="flag">An enumeration value.</param>
	/// <returns>true if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, false.</returns>
	public static bool HasFlag(this Enum thisInstance, Enum flag) {
		long setBits = Convert.ToInt64(flag);
		return (Convert.ToInt64(thisInstance) & setBits) == setBits;
	}

}
