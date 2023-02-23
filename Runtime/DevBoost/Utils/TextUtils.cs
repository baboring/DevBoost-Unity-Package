using System;

/// <summary>
/// Class to collect any utilities that act on strings and other text objects (chars, etc).
/// </summary>
public static class TextUtils
{

	#region General Utility

	/// <summary>
	/// Determine if this string starts with any of an array of characters.
	/// </summary>
	/// <param name="checkString">The string to check.</param>
	/// <param name="checkCharacters">The characters to check against.</param>
	/// <returns>True if the string starts with any of the characters on the list; false otherwise.</returns>
	public static bool StartsWithAny(this string checkString, params char[] checkCharacters)
	{
		foreach (char checkChar in checkCharacters)
		{
			if (checkString.StartsWith(checkChar.ToString()))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Simple XOR encryption scheme to use for save files and other on device stuff to prevent casual cheating.
	/// </summary>
	/// <param name="text">The text to encrypt or decrypt.</param>
	/// <param name="encodeChar">Char to use as the encoding key.</param>
	/// <returns>Encrypted or decrypted text.</returns>
	public static string XOREncryptDecrypt(string text, char encodeChar)
	{
		System.Text.StringBuilder inputSB = new System.Text.StringBuilder(text);
		System.Text.StringBuilder outputSB = new System.Text.StringBuilder(inputSB.Length);
		char temp = default(char);
		for (int i = 0; i < inputSB.Length; ++i)
		{
			temp = inputSB[i];
			temp = (char)(inputSB[i] ^ encodeChar);
			outputSB.Append(temp);
		}

		return outputSB.ToString();
	}

    /// <summary>
    /// Extract Param by key
    /// </summary>
    /// <param name="szString"></param>
    /// <param name="key"></param>
    /// <param name="splitter"></param>
    /// <returns></returns>
    public static string[] ExtractParams(string szString, string key, char splitter = '|')
    {
        int found = szString?.IndexOf(key) ?? -1;
        if (found < 0)
            return new string[0];

        return szString.Substring(szString.IndexOf(key)).Split('|');
    }

	/// <summary>
    /// Get a human readable file size string
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
	public static string GetReadableFileSize(long bytes)
    {
		string result = bytes.ToString();
		if (bytes < 1024)
        {
			return result + " bytes";
        }
		bytes = bytes / 1024;
		if (bytes < 1024)
        {
			return bytes.ToString() + " KB";
        }
		bytes = bytes / 1024;
		if (bytes < 1024)
        {
			return bytes.ToString() + " MB";
        }
		bytes = bytes / 1024;

		return bytes.ToString() + " GB";
    }

    #endregion

}
