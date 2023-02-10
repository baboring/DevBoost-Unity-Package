using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class encapsulates utility functions that operate on IList objects.
/// </summary>
public static class ListUtils  {

	/// <summary>
	/// Does a fisher-yates shuffle on this list.null Randomizing the order of the elements.
	/// The shuffle proceeds from the end of the list down to the start.
	/// </summary>
	/// <param name="operatingList">The list of elements to shuffle.</param>
	/// <typeparam name="T">Type of list to shuffle.</typeparam>
	public static void Shuffle<T>(this IList<T> operatingList) {
		T temp = default(T);
		int j = 0;
		for (int i = operatingList.Count - 1; i > 0; i--) {
			j = Random.Range(0, i);
			temp = operatingList[j];
			operatingList[j] = operatingList[i];
			operatingList[i] = temp;
		}
	}

	/// <summary>
	/// Gets a random element from this list.
	/// </summary>
	/// <param name="operatingList">List to draw element from.</param>
	/// <typeparam name="T">Type of list.</typeparam>
	/// <returns>A random element from this list.</returns>
	public static T RandomElement<T>(this IList<T> operatingList) {
		if (operatingList.IsNullOrEmpty()) {
			throw new System.IndexOutOfRangeException("Cannot get random element of empty list.");
		}

		return operatingList[UnityEngine.Random.Range(0, operatingList.Count)];
	}

	/// <summary>
	/// Gets the first element from this list.
	/// </summary>
	/// <param name="operatingList">List to draw element from.</param>
	/// <typeparam name="T">Type of list.</typeparam>
	/// <returns>The first item from this list.</returns>
	public static T First<T>(this IList<T> operatingList) {
		if (operatingList.IsNullOrEmpty()) {
			throw new System.IndexOutOfRangeException("Cannot get the first element of an empty list.");
		}
		return operatingList[0];
	}
	
	/// <summary>
	/// Gets the last element from this list.
	/// </summary>
	/// <param name="operatingList">List to draw element from.</param>
	/// <typeparam name="T">Type of list.</typeparam>
	/// <returns>The last item from this list.</returns>
	public static T Last<T>(this IList<T> operatingList) {
		if (operatingList.IsNullOrEmpty()) {
			throw new System.IndexOutOfRangeException("Cannot get the last element of an empty list.");
		}
		return operatingList[operatingList.Count - 1];
	}

	/// <summary>
	/// Gets whether or not the list is null or empty.
	/// </summary>
	/// <param name="operatingList">The list to check.</param>
	/// <typeparam name="T">Type of list.</typeparam>
	/// <returns>True if the list is null or empty and False otherwise.</returns>
	public static bool IsNullOrEmpty<T>(this IList<T> operatingList) {
		if (operatingList == null || operatingList.Count == 0) {
			return true;
		}

		return false;
	}

	/// <summary>
	/// Checks if a list has a particular index.
	/// </summary>
	/// <param name="list">The list to check.</param>
	/// <param name="index">Index value to check if it exists in this list.</param>
	/// <typeparam name="T">Type of list.</typeparam>
	/// <returns>True if the list has the requested index, false otherwise.</returns>
	public static bool HasIndex<T>(this IList<T> list, int index)
	{
		return index >= 0 && index < list.Count;
	}
}
