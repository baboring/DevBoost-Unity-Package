using System;
using System.IO;

/// <summary>
/// Class to collect any utilities that act on files.
/// </summary>
public static class FileUtils
{

    /// <summary>
    /// Save json object to the file
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="text"></param>
    public static void SaveTextFile(string fullPath, string text)
    {
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        StreamWriter writer = File.CreateText(fullPath);

        writer.WriteLine(text);
        writer.Close();
    }

}
