using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Serialization {

	/// <summary>
	/// This class works on data serialized and deserialized by miniJSON.
	/// </summary>
	public static class SerializationUtils {

		#region Constants

		/// <summary>
		/// Character to split a CSV string with.
		/// </summary>
		public static readonly char[] CSV_SPLIT_CHAR = { ',' };

		/// <summary>
		/// Serialization key for the X vector value.
		/// </summary>
		private const string VECTOR_X = "x";

		/// <summary>
		/// Serialization key for the y vector value.
		/// </summary>
		private const string VECTOR_Y = "y";

		/// <summary>
		/// Serialization key for the z vector value.
		/// </summary>
		private const string VECTOR_Z = "z";

		/// <summary>
		/// Serialization key for the w vector value.
		/// </summary>
		private const string VECTOR_W = "w";

		/// <summary>
		/// Serializtion key for a transform's postiion value.
		/// </summary>
		private const string TRANSFORM_POSITION = "pos";

		/// <summary>
		/// Serialization key for a transform's rotation value.
		/// Rotations will be serialized as Euler angles for easier human readability.
		/// </summary>
		private const string TRANSFORM_ROTATION = "rot";

		/// <summary>
		/// Serialization key for a transform scale.
		/// </summary>
		private const string TRANSFORM_SCALE = "scale";

		/// <summary>
		/// Formatter for use with the colour serialization utils.
		/// </summary>
		private const string COLOUR_SERIALIZATION_FORMATTER = "#{0}";

		#endregion

		#region Dictionary Extensions

		/// <summary>
		/// Tries to get a component from a given dictionary 
		/// </summary>
		/// <param name="dictionary">Dictionary that this function is operating on.</param>
		/// <param name="key">The key to check for a value.</param>
		/// <param name="fallback">Fallback value for if a key is not found.</param>
		/// <typeparam name="T">The type of the value to parse to.</typeparam>
		/// <returns>An object containing the given value or the default for that object type.</returns>
		public static T GetValueForKey<T>(this IDictionary dictionary, object key, T fallback = default(T)) {
			if (dictionary.Contains(key)) {
				return SerializationUtils.DeserializeValue<T>(dictionary[key]);
			} else {
				return fallback;
			}
		}

		/// <summary>
		/// Function that will try and parse out value from a given dictionary.
		/// It will assert if the value is not found.
		/// </summary>
		/// <param name="dictionary">Dictionary that this function is operating on.</param>
		/// <param name="key">The key to check for a value.</param>
		/// <typeparam name="T">The type of the value to parse to.</typeparam>
		/// <returns>An object containing the given value or the default for that object type.</returns>
		public static T AssertValueForKey<T>(this IDictionary dictionary, object key) {
			if (dictionary.Contains(key)) {
				return SerializationUtils.DeserializeValue<T>(dictionary[key]);
			} else {
				Debug.AssertFormat(false, "No value for key {0}", key.ToString());
				return default(T);
			}
		}

		#endregion

		#region Serialization Helpers

		/// <summary>
		/// Serializes an IEnumerable system of ISerializable objects into a List<Dictionary<string, object>>.
		/// Slightly less efficient than the standard IList serializer for list implementations.
		/// </summary>
		/// <param name="enumerableList">The enumerable object to serialize.</param>
		/// <param name="count">Helper value to prime the dictionary length.</param>
		/// <typeparam name="T">ISerialiable implementing type.</typeparam>
		/// <returns>A list of dictionaries containing the serialized versions of the ISerializable objects in the provided enumerator.</returns>
		public static List<Dictionary<string, object>> SerializeObjectEnumerable<T>(IEnumerable<T> enumerableList, int count = 0) where T : ISerializable {
			List<Dictionary<string, object>> serializedObject = new List<Dictionary<string, object>>(count);
			foreach(T item in enumerableList) {
				serializedObject.Add(item.Serialize());
			}

			return serializedObject;
		}

		/// <summary>
		/// Serialized a list of ISerializable objects into a list of dictionary objects.
		/// </summary>
		/// <param name="list">List of objects to serialize.</param>
		/// <typeparam name="T">The type of ISerializable derived object in the list.</typeparam>
		/// <returns>A list of serialized objects suitable for MiniJSON.</returns>
		public static List<Dictionary<string, object>> SerializeObjectList<T>(IList<T> list) where T : ISerializable {
			List<Dictionary<string, object>> serializedObjects = new List<Dictionary<string, object>>(list.Count);
			for(int i = 0; i < list.Count; ++i) {
				serializedObjects.Add(list[i].Serialize());
			}

			return serializedObjects;
		}

		/// <summary>
		/// Deserializes a list of objects into 
		/// </summary>
		/// <param name="serializedObjects">List of serialized objects to deserialize into a list of objects.</param>
		/// <typeparam name="T">ISerialized derived type to create.</typeparam>
		/// <returns>A list of created ISerialized derived objects.</returns>
		public static List<T> DeserializeObjectList<T>(Dictionary<string, object> dict, string listKey) where T : ISerializable, new() {
			List<Dictionary<string, object>> serializedObjects = SerializationUtils.DeserializeList<Dictionary<string, object>>(dict, listKey);
			List<T> objectList = new List<T>(serializedObjects.Count);
			for (int i = 0; i < serializedObjects.Count; ++i) {
				objectList.Add(SerializationUtils.CreateSerializedObject<T>(serializedObjects[i]));
			}

			return objectList;
		}

		/// <summary>
		/// Deserializes a list of the given type from the specified dictionary. 
		/// If the key is not in the dictionary it will return an empty list.
		/// </summary>
		/// <returns>The list of the given type that was deserialized.</returns>
		/// <param name="dict">Dictionary to get the value from.</param>
		/// <param name="key">Key of the value to deserialize.</param>
		/// <typeparam name="T">The type of the desired list.</typeparam>
		public static List<T> DeserializeList<T>(Dictionary<string, object> dict, string key) {

			if (dict[key] is List<T>)
			{
				return dict[key] as List<T>;
			}

			List<object> objectList = dict.GetValueForKey<List<object>>(key);
			if (objectList != null) {
				return ConvertObjectList<T>(objectList);
			}

			Debug.LogErrorFormat("Deserialization of list with key {0} failed.", key);
			return new List<T>();
		}

		/// <summary>
		/// Deserializes a list of objects into the provided type.
		/// </summary>
		/// <param name="objectList">The serialized object list.</param>
		/// <typeparam name="T">The type to convert the list.</typeparam>
		/// <returns>The list converted to the correct type.</returns>
		public static List<T> ConvertObjectList<T>(IList<object> objectList)
		{
			List<T> deserializedList = new List<T>(objectList.Count);
			
			for (int i = 0; i < objectList.Count; i++) {
				deserializedList.Add(SerializationUtils.DeserializeValue<T>(objectList[i]));
			}
			
			return deserializedList;
		}

		/// <summary>
		/// Deserializes a dictionary of the given key and value types from the specified dictionary. 
		/// If the key is not in the dictionary it will return an empty dictionary.
		/// </summary>
		/// <returns>The dictionary of the given key and value types that was deserialized.</returns>
		/// <param name="dict">Dictionary to get the value from.</param>
		/// <param name="key">Key of the value to deserialize.</param>
		/// <typeparam name="K">The type of the key of the desired dictionary.</typeparam>
		/// <typeparam name="V">The type of the value of the desired dictionary.</typeparam>
		public static Dictionary<K, V> DeserializeDictionary<K, V>(Dictionary<string, object> dict, string key) {
			Dictionary<string, object> objectDict = dict.GetValueForKey<Dictionary<string, object>>(key);

			if (objectDict != null) {
				Dictionary<K, V> deserializedDict = new Dictionary<K, V>(objectDict.Count);
				foreach (KeyValuePair<string, object> kvp in objectDict) {
					K typedKey = SerializationUtils.DeserializeValue<K>(kvp.Key);
					V typedValue = SerializationUtils.DeserializeValue<V>(kvp.Value);
						
					deserializedDict[typedKey] = typedValue;
				}

				return deserializedDict;
			}

			Debug.LogErrorFormat("Deserialzation of dictionary with key {0} failed.", key);
			// If deserialization fails return an empty dictionary.
			return new Dictionary<K, V>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parentDictionary"></param>
		/// <param name="objectKey">The key of the object in </param>
		/// <typeparam name="T">The type of object to create.</typeparam>
		/// <returns>A new object reference created from the </returns>
		public static T CreateSerializedObject<T>(Dictionary<string, object> parentDictionary, string objectKey) where T : ISerializable, new() {
			Dictionary<string, object> serializedObject = DeserializeDictionary<string, object>(parentDictionary, objectKey);
			return CreateSerializedObject<T>(serializedObject);
		}

		#endregion

		#region List Serialization Helpers

		/// <summary>
		/// Tries to get a value from a list deserialized to a specific value type through the standard serializtion tools.
		/// </summary>
		/// <param name="list">List to take the value from.</param>
		/// <param name="index">Index of the requested value in the list.</param>
		/// <typeparam name="T">Type that is request for the deserialized value.</typeparam>
		/// <returns>The value at the indicated index cast to the requested value.</returns>
		public static T GetValue<T>(this IList<object> list, int index, T fallback = default(T))
		{
			T returnValue = fallback;

			if (list.HasIndex(index) && list[index] != null) 
			{
				returnValue = SerializationUtils.DeserializeValue<T>(list[index]);
			}
			
			return returnValue;
		}

		/// <summary>
		/// Checks if a list has a non-null value in it.
		/// </summary>
		/// <param name="list">The list to check the value in.</param>
		/// <param name="index">The index to check.</param>
		/// <returns>True if there is a value at the specified index, false if the value is outside the range of the list or it is a null value.</returns>
		public static bool HasValue(this IList<object> list, int index)
		{
			bool hasValue = false;

			hasValue = list.HasIndex(index) && list[index] != null && !IsEmptyCell(list[index]);

			return hasValue;
		}

		/// <summary>
		/// Check if a value is an empty spreadsheet cell from a serialized list format.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>True if the cell matches the profile of an empty database cell (is an empty string value); False otherwise.</returns>
		public static bool IsEmptyCell(object value)
		{
			return (value is string && string.IsNullOrEmpty((string)value));
		}

		#endregion

		#region Serialization Helpers

		/// <summary>
		/// Helper function for directly creating new ISerialized objects.
		/// </summary>
		/// <param name="serializedISerialized">The serialized values to parse into the new ISerialized object.</param>
		/// <typeparam name="T">The object type to create.</typeparam>
		/// <returns>The newly create ISerialized value.</returns>
		public static T CreateSerializedObject<T>(Dictionary<string, object> serializedISerialized) where T : ISerializable, new() {
			T value = new T();
			value.Deserialize(serializedISerialized);
			return value;
		}

		/// <summary>
		/// Deserializes a given value into the specified type.
		/// </summary>
		/// <param name="value">The object to deserialize.</param>
		/// <typeparam name="T">The type to deserialize the value as.</typeparam>
		/// <returns>The value cast to the correct type.</returns>
		public static T DeserializeValue<T>(object value)
		{
			System.Type objectType = typeof(T);
			if (value is T)
			{
				return (T)value;
			}
			else
			{
				if (objectType.IsEnum)
				{
					return (T)System.Enum.Parse(objectType, value.ToString(), true);
				}
				else
				{
					return (T)System.Convert.ChangeType(value, objectType, System.Globalization.CultureInfo.InvariantCulture);
				}
			}
		}

		#endregion

		#region Vector Serialization

		/// <summary>
		/// Serialize a Vector2 into a dictionary that can be written to disc.
		/// </summary>
		/// <param name="vector">Vector 2 to serialize.</param>
		/// <returns>A serialized vector2.</returns>
		public static Dictionary<string, object> SerializeVector2(Vector2 vector) {
			Dictionary<string, object> serializedVector = new Dictionary<string, object>();
			
			serializedVector[VECTOR_X] = vector.x;
			serializedVector[VECTOR_Y] = vector.y;

			return serializedVector;
		}

		/// <summary>
		/// Deserializes a Vector2.
		/// </summary>
		/// <param name="serializedVector">The dictionary containing the serialized vector.</param>
		/// <returns></returns>
		public static Vector2 DeserializeVector2(Dictionary<string, object> dict, string vectorKey) {
			Dictionary<string, object> serializedVector = DeserializeDictionary<string, object>(dict, vectorKey);
			Vector2 vector = new Vector2 (
				serializedVector.GetValueForKey<float>(VECTOR_X),
				serializedVector.GetValueForKey<float>(VECTOR_Y));

			return vector;
		}

		/// <summary>
		/// Serializes a vector3.
		/// </summary>
		/// <param name="vector">The vector to serialize.</param>
		/// <returns>A dictionary containing the vector 3 values.</returns>
		public static Dictionary<string, object> SerializeVector3(Vector3 vector) {
			Dictionary<string, object> serializedVector = new Dictionary<string, object>();
			
			serializedVector[VECTOR_X] = vector.x;
			serializedVector[VECTOR_Y] = vector.y;
			serializedVector[VECTOR_Z] = vector.z;

			return serializedVector;
		}

		/// <summary>
		/// Deserialize a Vector3.
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="vectorKey">The key for the vector.</param>
		/// <returns>A dictionary containing vector 3 values.</returns>
		public static Vector3 DeserializeVector3(Dictionary<string, object> dict, string vectorKey) {
			Dictionary<string, object> serializedVector = DeserializeDictionary<string, object>(dict, vectorKey);
			return new Vector3 (
				serializedVector.GetValueForKey<float>(VECTOR_X),
				serializedVector.GetValueForKey<float>(VECTOR_Y),
				serializedVector.GetValueForKey<float>(VECTOR_Z));
		}

		/// <summary>
		/// Serializes a vector 4 value.
		/// </summary>
		/// <param name="vector">The vector to serialize.</param>
		/// <returns>A dictionary containing a serialized vector4.</returns>
		public static Dictionary<string, object> SerializeVector4(Vector4 vector) {
			Dictionary<string, object> serializedVector = new Dictionary<string, object>();
			
			serializedVector[VECTOR_X] = vector.x;
			serializedVector[VECTOR_Y] = vector.y;
			serializedVector[VECTOR_Z] = vector.z;
			serializedVector[VECTOR_W] = vector.w;

			return serializedVector;
		}

		/// <summary>
		/// Deserialize a vector4.
		/// </summary>
		/// <param name="dict">Dictionary containing the serialized values.</param>
		/// <param name="vectorKey">The key for the vector4.</param>
		/// <returns>A Vector4 associated with the </returns>
		public static Vector4 DeserializeVector4(Dictionary<string, object> dict, string vectorKey) {
			Dictionary<string, object> serializedVector = DeserializeDictionary<string, object>(dict, vectorKey);
			return new Vector4 (
				serializedVector.GetValueForKey<float>(VECTOR_X),
				serializedVector.GetValueForKey<float>(VECTOR_Y),
				serializedVector.GetValueForKey<float>(VECTOR_Z),
				serializedVector.GetValueForKey<float>(VECTOR_W));
		}

		#endregion

		#region Transform Serialization

		/// <summary>
		/// Serializes a transform into a dictionary that can be serialized.
		/// </summary>
		/// <param name="transform">The transform to serialize.</param>
		/// <returns>A dictionary containing serialized transform information.</returns>
		public static Dictionary<string, object> SerializeTransform(Transform transform) {
			Dictionary<string, object> serializedTransform = new Dictionary<string, object>();

			serializedTransform[TRANSFORM_POSITION] = SerializeVector3(transform.localPosition);
			serializedTransform[TRANSFORM_ROTATION] = SerializeVector3(transform.localRotation.eulerAngles);
			serializedTransform[TRANSFORM_SCALE] = SerializeVector3(transform.localScale);

			return serializedTransform;
		}

		/// <summary>
		/// Deserializes a serialized transform dictionary into a transform.
		/// </summary>
		/// <param name="dict">Serialized transform to load into it.</param>
		/// <param name="transform">The transform to load the values into.</param>
		public static void DeserializeTransform(Dictionary<string, object> dict, Transform transform, string serializedTransformKey) {
			Dictionary<string, object> serializedTransform = DeserializeDictionary<string, object>(dict, serializedTransformKey);

			transform.localPosition = DeserializeVector3(serializedTransform, TRANSFORM_POSITION);
			transform.localRotation = Quaternion.Euler(DeserializeVector3(serializedTransform, TRANSFORM_ROTATION));
			
			transform.localScale = DeserializeVector3(serializedTransform, TRANSFORM_SCALE);
		}

		#endregion

		#region Colours

		/// <summary>
		/// Serializes a colour into a format as required by the ColourUtility.
		/// </summary>
		/// <param name="colourToSerialize">The colour to serialize into a string.</param>
		/// <returns>The colour serialized into a string that can be used by ColourUtility.</returns>
		public static string SerializeColour(Color colourToSerialize) {
			return string.Format(COLOUR_SERIALIZATION_FORMATTER, ColorUtility.ToHtmlStringRGBA(colourToSerialize));
		}

		/// <summary>
		/// Deserializes an RGB colour
		/// </summary>
		/// <param name="rgbString">A string formatted as required by ColourUtility helper class.</param>
		/// <returns>The deserialized colour.</returns>
		public static Color DeserializeRGBColour(string rgbString) {
			Color deserializedColour = Color.white;

			if (!ColorUtility.TryParseHtmlString(rgbString, out deserializedColour)) {
				Debug.LogError("Cannot parse colour string: " + rgbString);
			}

			return deserializedColour;
		}

		#endregion

		#region CSV

		/// <summary>
		/// This function will parse an RFC4180 compliance CSV file.
		/// </summary>
		/// <param name="csvText">The text comprising the whole of the CSV file you wish to parse.</param>
		/// <param name="csvSeparator">The separator to use if it is not a comma.</param>
		/// <returns>A list, containing the parsed data for the CSV file, with each entry representing one line of CSV data, delimited by a line break.</returns>
		public static List<List<string>> ParseCSV(string csvText, char csvSeparator = ',')
		{
			// The current position of the pointer in the CSV text.
			int pointerPos = 0;
			List<List<string>> csv = new List<List<string>>();

			// While the pointer position is less than the text length we want to read the next CSV line and parse it into it's tokens.
			while (pointerPos < csvText.Length)
			{
				List<string> csvLine = null;
				// Starting from the current pointer position read the next line of CSV text.
				csvLine = ParseCSVLine(ref csvText, ref pointerPos, csvSeparator);
				
				if (ListUtils.IsNullOrEmpty(csvLine))
				{
					break;
				}

				csv.Add(csvLine);
			}

			return csv;
		}

		/// <summary>
		/// This function will parse a single valid line of CSV text into it's constituent tokens.
		/// </summary>
		/// <param name="csvText">The text to derive the line from.</param>
		/// <param name="lineStart">The start index of the current line.</param>
		/// <param name="csvSeparator">The separate value that we are using for tokens in this CSV format.</param>
		/// <returns>A list containing the CSV tokens that are a part of this line.</returns>
		private static List<string> ParseCSVLine(ref string csvText, ref int currentCharIndex, char csvSeparator = ',')
		{
			List<string> parsedLine = new List<string>();

			int textLength = csvText.Length;
			int currentTokenStart = currentCharIndex;

			// Flag describing if the parser is currently inside a pair of quotes delimiting a 
			bool insideWordQuote = false;

			while (currentCharIndex < textLength)
			{
				char currentChar = csvText[currentCharIndex];

				// If we are inside a word quote most special characters are allowed (commas, newlines and double quotes).
				// We want to check that first so later we can be sure all special characters are actually special.
				if (insideWordQuote)
				{
					// If we're currently inside a word quote first check if this is the ending quote.
					if (currentChar == '\"')
					{
						// If the text ends or the next character is not a quotation mark that means that this character is the end of the word quote.
						if (currentCharIndex + 1 >= textLength || csvText[currentCharIndex + 1] != '\"')
						{
							// The next character is not a quotation mark indicating that the double quote mark is signaling the end of a word.
							insideWordQuote = false;
						}
						else if (currentCharIndex + 2 < textLength && csvText[currentCharIndex + 2] == '\"')
						{
							// Since this got past the first double quotation mark check that means that this is a triple quotation mark symbol. 
							// Excel exports use this to distinquish fields with quotaion marks inside of them.
							insideWordQuote = false;
							currentCharIndex += 2;
						}
						else
						{
							// This implies that the character is a double quote inside of a word quote.
							// This valid so we just advance the read pointer to look for the next token.
							++currentCharIndex;
						}
					}
				}
				else if (currentChar == '\n' || currentChar == csvSeparator)
				{
					// We have reached the end of the current token so we should parse it.
					// The parse function will advance the current token start position.
					ParseCSVToken(ref parsedLine, ref csvText, currentCharIndex, ref currentTokenStart);

					if (currentChar == '\n')
					{
						++currentCharIndex;
						// An unenclosed newline signals the end of this line so we can break out of the line reading loop.
						break;
					}
				}
				else if (currentChar == '\"')
				{
					// We are starting a new possible world quote.
					insideWordQuote = true;
				}

				// Advance the index pointer.
				++currentCharIndex;
			}

			// If there is an unparsed token parse it.
			if (currentCharIndex > currentTokenStart)
			{
				ParseCSVToken(ref parsedLine, ref csvText, currentCharIndex, ref currentTokenStart);
			}


			return parsedLine;
		}

		/// <summary>
		/// This will run through the provided 
		/// </summary>
		/// <param name="currentTokenList">The tokens that have already been parsed into this line. The current token will be added to this list.</param>
		/// <param name="csvText">The text to parse the token from.</param>
		/// <param name="tokenStart">The start index of the token.</param>
		/// <param name="tokenEnd">The token end in the </param>
		private static void ParseCSVToken(ref List<string> currentTokenList, ref string csvText, int tokenEnd, ref int currentTokenStart)
		{
			string tokenText = csvText.Substring(currentTokenStart, tokenEnd - currentTokenStart);
			// Set the token start index.
			currentTokenStart = tokenEnd + 1;

			// Replace double quotes with single quotes as defined in RFC 4180.
			tokenText = tokenText.Replace("\"\"", "\"");

			// If the token is wrapped in quotation marks at either end remove them.
			if (tokenText.Length > 1 && tokenText[0] == '\"' && tokenText[tokenText.Length - 1] == '\"')
			{
				tokenText = tokenText.Substring(1, tokenText.Length - 2);
			}

			currentTokenList.Add(tokenText);
		}

		/// <summary>
		/// Simple function that will parse a single valid CSV string.
		/// </summary>
		/// <param name="csvMasterString">Master comma separated value string.</param>
		/// <returns>The split string.</returns>
		public static string[] ParseSimpleCSVLine(string csvMasterString)
		{
			int startIndex = 0;
			return ParseCSVLine(ref csvMasterString, ref startIndex).ToArray();
		}

		/// <summary>
		/// Writes a list of string data to a string formatted to be compatible with RFC 4180.
		/// </summary>
		/// <param name="dataToWrite">A list containing a list of strings, each representing one line of a CSV document.</param>
		/// <returns>A string formatted to match RFC 4180's definition of a CSV listing of the provided strings.</returns>
		public static string WriteCSV<T>(IList<T> dataToWrite) where T : IList<string>
		{
			System.Text.StringBuilder csvDoc = new System.Text.StringBuilder();

			for (int i = 0; i < dataToWrite.Count; ++i)
			{
				csvDoc.Append(WriteCSVLine(dataToWrite[i]));

				if (i < dataToWrite.Count - 1)
				{
					csvDoc.Append('\n');
				}
			}

			return csvDoc.ToString();
		}

		/// <summary>
		/// Writes a single line of RFC 4180 compatible CSV data.
		/// </summary>
		/// <param name="dataForLine">List of strings to turn into CSV tokens.</param>
		/// <param name="csvSeparator">Character to use for separating tokens.</param>
		/// <returns>A string contining the data passed in serialized into tokens separated by the CSV separator.</returns>
		public static string WriteCSVLine(IList<string> dataForLine, char csvSeparator = ',')
		{
			System.Text.StringBuilder csvLine = new System.Text.StringBuilder();
			for (int i = 0; i < dataForLine.Count; ++i)
			{
				string token = dataForLine[i];

				// If the token contains newlines, commas or double quotes it needs to be wrapped in double quotes.
				if (token.Contains("\r") || token.Contains("\n") || token.Contains(csvSeparator.ToString()) || token.Contains("\""))
				{
					// Replace any double quotes with double, double quotes and wrap the whole token.
					token = token.Replace("\"", "\"\"");
					csvLine.AppendFormat("\"{0}\"", token);
				}
				else
				{
					csvLine.Append(token);
				}

				if (i < dataForLine.Count - 1)
				{
					csvLine.Append(csvSeparator);
				}
			}

			return csvLine.ToString();
		}

		#endregion

	}
}
