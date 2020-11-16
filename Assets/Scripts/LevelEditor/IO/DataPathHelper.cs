using System.IO;
using Common;
using UnityEngine;

public class DataPathHelper
{
    private static string _customLevelsDirectoryPath;
    private static string _levelInfoPath;
    private static string _levelDataPath;
    
    private static string _bundleInfoPath;
    private static string _bundleDataPath;
    public const string CustomLevelsDirectoryName = "CustomLevels";
    public static string LevelInfoPath
    {
        get
        {
            if (_levelInfoPath == null)
                _levelInfoPath = Application.dataPath + "/Resources/" + ResourcePath.LEVEL_ROOT + ResourcePath.LEVEL_INFO;
            return _levelInfoPath;
        }
    }
    public static string LevelDataPath
    {
        get
        {
            if (_levelDataPath == null)
                _levelDataPath = Application.dataPath + "/Resources/" + ResourcePath.LEVEL_ROOT + ResourcePath.LEVEL_DATA;
            return _levelDataPath;
        }
    }
    
    
    public static string BundleInfoPath
    {
        get
        {
            if (_bundleInfoPath == null)
                _bundleInfoPath = Application.dataPath + "/Resources/" + ResourcePath.BUNDLES_ROOT + ResourcePath.BUNDLES_INFO;
            return _bundleInfoPath;
        }
    }
    
    public static string BundleDataPath
    {
        get
        {
            if (_bundleDataPath == null)
                _bundleDataPath = Application.dataPath + "/Resources/" + ResourcePath.BUNDLES_ROOT + ResourcePath.BUNDLES_DATA;
            return _bundleDataPath;
        }
    }

    

    public static string GetDefaultNameForCustomLevel(string songName, string songAuthorName, string levelAuthorName)
    {
        return songAuthorName + " - " + songName + " (" + levelAuthorName + ")";
    }
}