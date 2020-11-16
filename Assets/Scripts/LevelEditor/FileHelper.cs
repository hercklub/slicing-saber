

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileHelpers
{
  public static string GetEscapedURLForFilePath(string filePath)
  {
    return "file:///" + UnityWebRequest.EscapeURL(filePath);
  }

  public static string GetUniqueDirectoryNameByAppendingNumber(string dirName)
  {
    int num = 0;
    string path;
    for (path = dirName; Directory.Exists(path); path = dirName + " " + (object) num)
      ++num;
    return path;
  }

  public static string[] GetFilePaths(string directoryPath, HashSet<string> extensions)
  {
    if (!Directory.Exists(directoryPath))
      return (string[]) null;
    string[] files = Directory.GetFiles(directoryPath);
    List<string> stringList = new List<string>();
    foreach (string path in files)
    {
      string str = Path.GetExtension(path).Replace(".", "");
      if (extensions.Contains(str))
        stringList.Add(path);
    }
    return stringList.ToArray();
  }

  public static string[] GetFileNamesFromFilePaths(string[] filePaths)
  {
    List<string> stringList = new List<string>();
    foreach (string filePath in filePaths)
      stringList.Add(Path.GetFileName(filePath));
    return stringList.ToArray();
  }

  public static void SaveToJSONFile(object obj, string filePath, string tempFilePath, string backupFilePath)
  {
    try
    {
      string json = JsonUtility.ToJson(obj);
      File.WriteAllText(filePath, json);
        

      // if (File.Exists(filePath))
      // {z
      //   File.WriteAllText(tempFilePath, json);
      //   File.Replace(tempFilePath, filePath, backupFilePath);|
      // }
      // else
      //   File.WriteAllText(filePath, json);
    }
    catch
    {
    }
  }

  public static T LoadFromJSONFile<T>(string filePath, string backupFilePath = null) where T : class
  {
    T obj = default (T);
    if (File.Exists(filePath))
    {
      try
      {
        obj = JsonUtility.FromJson<T>(File.ReadAllText(filePath));
      }
      catch
      {
        obj = default (T);
      }
    }
    if ((object) obj == null && backupFilePath != null)
    {
      if (File.Exists(backupFilePath))
      {
        try
        {
          obj = JsonUtility.FromJson<T>(File.ReadAllText(backupFilePath));
        }
        catch
        {
          obj = default (T);
        }
      }
    }
    return obj;
  }

  public static string LoadJSONFile(string filePath, string backupFilePath = null)
  {
    string str = (string) null;
    if (File.Exists(filePath))
    {
      try
      {
        str = File.ReadAllText(filePath);
      }
      catch
      {
        str = (string) null;
      }
    }
    if (str == null && backupFilePath != null)
    {
      if (File.Exists(backupFilePath))
      {
        try
        {
          str = File.ReadAllText(backupFilePath);
        }
        catch
        {
          str = (string) null;
        }
      }
    }
    return str;
  }
}
