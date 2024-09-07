using UnityEditor;
using UnityEngine;

namespace Rosynant.PlayerPrefsJson
{
    /// <summary>
    /// An intermediary data container for custom PlayerPrefsJson settings.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerPrefsJsonSettings", menuName = "Rosynant/PlayerPrefsJson/PlayerPrefsJson Settings")]
    public class PlayerPrefsJsonSettings : ScriptableObject
    {
        private static PlayerPrefsJsonSettings _instance;
        private bool _persistentDataPath = true;
        private string _directoryPath = "Saves/Prefs";
        private string _fileName = "Prefs.json";
        private bool _saveOnQuit = true;

        public bool DefaultPersistentDataPath { get; } = true;
        public string DefaultDirectoryPath { get; } = "Saves/Prefs";
        public string DefaultFileName { get; } = "Prefs.json";
        public bool DefaultSaveOnQuit { get; } = true;

        public static PlayerPrefsJsonSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<PlayerPrefsJsonSettings>(path: "PlayerPrefsJsonSettings");
                
                return _instance;
            }
        }

        public bool PersistentDataPath
        {
            get { return _persistentDataPath; }
            set
            {
                _persistentDataPath = value;
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        public string DirectoryPath
        {
            get { return _directoryPath; }
            set
            {
                _directoryPath = value;
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        public bool SaveOnQuit
        {
            get { return _saveOnQuit; }
            set
            {
                _saveOnQuit = value;
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }
    }
}
