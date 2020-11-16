using System;
using System.Collections.Generic;
using System.Linq;
using strange.extensions.signal.impl;

namespace LevelEditor
{

    public class EditorBundleInfoData
    {
        private int _index;
        private string _version;
        private string _bundleName;
        private string _bundleDirectory;

        private int _enemyCount;
        private int _grassCount;
        private int _fishCount;
        private int _obstacleCount;
        private int _targetCount;
        
        public int Index
        {
            get => _index;
        }

        public string Version
        {
            get => _version;
            set => _version = value;
        }

        public string Bundlename
        {
            get => _bundleName;
            set => _bundleName = value;
        }

        public string BundleDirectory
        {
            get => _bundleDirectory;
            set => _bundleDirectory = value;
        }
        public int EnemyCount
        {
            get => _enemyCount;
            set => _enemyCount = value;
        }

        public int FishCount
        {
            get => _fishCount;
            set => _fishCount = value;
        }
        public int GrasCount
        {
            get => _grassCount;
            set => _grassCount = value;
        }
        
        public int ObstacleCount
        {
            get => _obstacleCount;
            set => _obstacleCount = value;
        }
        
        public int TargetCount
        {
            get => _targetCount;
            set => _targetCount = value;
        }

        public string BudnleFilename
        {
            get => _bundleName + ".txt";
        }

        public EditorBundleInfoData(int index, string version, string bundleName, string bundleDirectory, int enemyCount, int grassCount, int fishCount, int obstacleCount, int targetCount)
        {
            _index = index;
            _version = version;
            _bundleName = bundleName;
            _bundleDirectory = bundleDirectory;
            _enemyCount = enemyCount;
            _grassCount = grassCount;
            _fishCount = fishCount;
            _obstacleCount = obstacleCount;
            _targetCount = targetCount;
        }

        public EditorBundleInfoData(int index)
        {
            _index = index;
            _bundleName = string.Empty;
            _bundleDirectory = string.Empty;
        }
        
        public EditorBundleInfoData(EditorBundleInfoData data)
        {
            _index = data._index;
            _version = data._version;
            _bundleName = data._bundleName;
            _bundleDirectory = data._bundleDirectory;

        }
    }

    public interface IEditorLibraryBundleDataModel
    {
        EditorBundleInfoData AddBundleData(string version, string bundleName, string bundleDirectory, int enemyCount,
            int grassCount, int fishCount, int obstalceCount, int targetCount);
        EditorBundleInfoData AddEmptyBundleData();
        EditorBundleInfoData GetBundleData(int index);
        List<EditorBundleInfoData> GetAllBundleDataByDirectory(string directory);
        List<EditorBundleInfoData> GetAllBundleData {get; }

        EditorBundleInfoData GetSelectedBundleInfo();
        void SetSelectedBundleInfo(int index);
        void RemoveBundleInfoData(int index);
        
        
        //directories
        void UpdateDirectories();
        List<string> GetAllDirectories();
        void SetSelectedDirectory(int index);
        string GetSelectedDirectory();
        
        Signal DirectorySelectedSignal { get; }

    }

    public class EditorLibraryBudnleDataModel : IEditorLibraryBundleDataModel
    {
        private Dictionary<int, EditorBundleInfoData> _bundleById = new Dictionary<int, EditorBundleInfoData>();
        private List<string> _bundleDirectories = new List<string>();
        
        private int _bundleIndex;
        private int _selectedBundleIndex = -1;
        private int _selectedDirectory = -1;


        public Signal DirectorySelectedSignal { get; } = new Signal();

        public EditorBundleInfoData AddBundleData(string version, string bundleName, string bundleDirectory, int enemyCount, int grassCount, int fishCount, int obstalceCount, int targetCount)
        {
            var bundleInfo = new EditorBundleInfoData( _bundleIndex ,version, bundleName, bundleDirectory, enemyCount, grassCount, fishCount, obstalceCount, targetCount);
            _bundleById[_bundleIndex] = bundleInfo;
            _bundleIndex++;
            return bundleInfo;
        }
        

        public EditorBundleInfoData AddEmptyBundleData()
        {
            var bundleInfo = new EditorBundleInfoData(_bundleIndex);
            _bundleById[_bundleIndex] = bundleInfo;
            _bundleIndex++;
            return bundleInfo;
        }

        public EditorBundleInfoData GetBundleData(int index)
        {
            _bundleById.TryGetValue(index, out EditorBundleInfoData bundleInfo);
            return bundleInfo;
        }

        public List<EditorBundleInfoData> GetAllBundleDataByDirectory(string directory)
        {
            return _bundleById.Values.Where(b => b.BundleDirectory == directory).ToList();
        }

        public List<EditorBundleInfoData> GetAllBundleData
        {
            get => _bundleById.Values.ToList();
        }

        public EditorBundleInfoData GetSelectedBundleInfo()
        {
            return GetBundleData(_selectedBundleIndex);
        }

        public void SetSelectedBundleInfo(int index)
        {
            _selectedBundleIndex = index;
            DirectorySelectedSignal.Dispatch();
        }
        
        public void RemoveBundleInfoData(int index)
        {
            _bundleById.Remove(index);
            RefreshDirectories();
        }

        public List<string> GetAllDirectories()
        {
            return _bundleDirectories;
        }

        private void RefreshDirectories()
        {
            //add unique directories
            _bundleDirectories.Clear();
            foreach (var bundle in _bundleById)
            {
                var dir = bundle.Value.BundleDirectory;
                if (!_bundleDirectories.Contains(dir))
                {
                    _bundleDirectories.Add(dir);
                }
            }
        }

        public void UpdateDirectories()
        {
            RefreshDirectories();
        }

        public void SetSelectedDirectory(int index)
        {
            _selectedDirectory = index;
            DirectorySelectedSignal.Dispatch();
        }

        public string GetSelectedDirectory()
        {
            string dir = null;
            
            if (_selectedDirectory >= 0 && _selectedDirectory <_bundleDirectories.Count )
                dir = _bundleDirectories[_selectedDirectory];

            return dir;
        }
    }
}