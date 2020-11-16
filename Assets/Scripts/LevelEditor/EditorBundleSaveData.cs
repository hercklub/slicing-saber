using System;
using UnityEngine;

namespace LevelEditor
{
    [Serializable]
    public class EditorBundleSaveData
    {
        private const string CURRENT_VERSION = "0.0.3";
        [SerializeField] private string _version;
        [SerializeField] private string _bundleName;
        [SerializeField] private string _bundleDirectory;

        [SerializeField]private int _enemyCount;
        [SerializeField]private int _grassCount;
        [SerializeField]private int _fishCount;
        [SerializeField]private int _obstacleCount;
        [SerializeField]private int _targetCount;
        
        public string Version
        {
            get => _version;
        }

        public string BundleName
        {
            get => _bundleName;
        }
        
        public string BundleDirectoryName
        {
            get => _bundleDirectory;
        }
        
        public int EnemyCount
        {
            get => _enemyCount;
        }
        public int GrassCount
        {
            get => _grassCount;
        }
        public int FishCount
        {
            get => _fishCount;
        }
        
        public int ObstacleCount
        {
            get => _obstacleCount;
        }
        public int TargetCount
        {
            get => _targetCount;
        }
        public bool HasAllData
        {
            get
            {
                return this._version != null && this._bundleName != null  && BundleDirectoryName != null;
            }
        }
        
        
        public EditorBundleSaveData(EditorBundleInfoData data)
        {
            _version = CURRENT_VERSION;
            _bundleName = data.Bundlename;
            _bundleDirectory = data.BundleDirectory;
            
            _enemyCount = data.EnemyCount;
            _fishCount = data.FishCount;
            _grassCount = data.GrasCount;
            _obstacleCount = data.ObstacleCount;
            _targetCount = data.TargetCount;
        }
        
        public static EditorBundleSaveData DeserializeFromJSONString(string stringData)
        {
            EditorBundleSaveData bundleInfoSaveData = (EditorBundleSaveData) null;
            
            try
            {
                bundleInfoSaveData = JsonUtility.FromJson<EditorBundleSaveData>(stringData);
            }
            catch
            {
                Debug.Log("Cant read BUNDLE INFO !");
            }
            
            return bundleInfoSaveData;
        }
        public string SerializeToJSONString()
        {
            return JsonUtility.ToJson((object) this);
        }
    }
}