using System;
using System.IO;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Rosynant.PlayerPrefsJson.Editor
{
    /// <summary>
    /// Handles the behaviour of the editor tool that allows to apply custom PlayerPrefsJson settings.
    /// </summary>
    public sealed class PlayerPrefsJsonTool : EditorWindow
    {
        public bool PersistentDataPath;
        public string DirectoryPath;
        public string FileName;
        public bool SaveOnQuit;

        private bool _isDirectoryPathInvalid = false;
        private bool _isFilNameInvalid = false;

        [MenuItem("Tools/Rosynant/PlayerPrefsJson/PlayerPrefsJson Tool")]
        public static void ShowWindow()
        {
            PlayerPrefsJsonTool playerPrefsJsonSettingsWindow = GetWindow<PlayerPrefsJsonTool>("PlayerPrefsJson Tool");
            playerPrefsJsonSettingsWindow.minSize = new Vector2(350, 230);

            playerPrefsJsonSettingsWindow.PersistentDataPath = PlayerPrefsJsonSettings.Instance.PersistentDataPath;
            playerPrefsJsonSettingsWindow.DirectoryPath = PlayerPrefsJsonSettings.Instance.DirectoryPath;
            playerPrefsJsonSettingsWindow.FileName = PlayerPrefsJsonSettings.Instance.FileName;
            playerPrefsJsonSettingsWindow.SaveOnQuit = PlayerPrefsJsonSettings.Instance.SaveOnQuit;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            PersistentDataPath = EditorGUILayout.Toggle(new GUIContent("Persistent data path", "If true, the directory for PlayerPrefsJson will be placed at the Unity's default persistent data directory."), PersistentDataPath);
            DirectoryPath = EditorGUILayout.TextField(new GUIContent("Directory path", "Directory path to store the PlayerPrefsJson file at."), DirectoryPath);
            FileName = EditorGUILayout.TextField(new GUIContent("File name", "Name of the JSON file to store the PlayerPrefsJson in."), FileName);
            SaveOnQuit = EditorGUILayout.Toggle(new GUIContent("Save on quit", "Whether to automatically save PlayerPrefsJson data to file on quitting the application.\nMay behave differently depending on the system. For details see: https://docs.unity3d.com/ScriptReference/Application-quitting.html).\nThe recommended way to save PlayerPrefsJson data to file is to manually call the Save() method somewhere in your application."), SaveOnQuit);
        
            EditorGUILayout.Space();

            GUIStyle warningStyle = new GUIStyle(GUI.skin.textArea)
            {
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("Applying new settings or reverting to default\nwill delete all currently stored PlayerPrefsJson!", style: warningStyle, GUILayout.MaxHeight(40));
            
            EditorGUILayout.Space();

            if (GUILayout.Button(new GUIContent("Apply settings")))
                SaveSettings();
            
            if (GUILayout.Button(new GUIContent("Revert settings to default")))
                RevertSettings();
            
            if (!PlayerPrefsJsonHelpers.ValidateDirectoryOrFileName(DirectoryPath))
            {
                EditorGUILayout.HelpBox($"Invalid directory path: {DirectoryPath}", MessageType.Warning);
                _isDirectoryPathInvalid = true;
            }
            else
            {
                _isDirectoryPathInvalid = false;
            }
                
            if (!PlayerPrefsJsonHelpers.ValidateDirectoryOrFileName(FileName, true))
            {
                EditorGUILayout.HelpBox($"Invalid file name: {FileName}", MessageType.Warning);
                _isFilNameInvalid = true;
            }
            else
            {
                _isFilNameInvalid = false;
            }
        }

        /// <summary>
        /// Saves the currently chosen settings if valid.
        /// </summary>
        private void SaveSettings()
        {   
            if (_isDirectoryPathInvalid || _isFilNameInvalid)
                return;

            if (!PlayerPrefsJsonHelpers.ValidateJsonFileName(FileName))
                FileName = $"{FileName}.json";

            try
            {
                // Delete PlayerPrefsJson at the previous directory
                Directory.Delete(PlayerPrefsJsonHelpers.DirectoryPath, true);
            }
            catch (Exception)
            {
                Debug.LogWarning($"Could not delete previous PlayerPrefsJson directory at {PlayerPrefsJsonHelpers.DirectoryPath}.");
            }
            finally
            {
                PlayerPrefsJsonSettings.Instance.PersistentDataPath = PersistentDataPath;
                PlayerPrefsJsonSettings.Instance.DirectoryPath = DirectoryPath;
                PlayerPrefsJsonSettings.Instance.FileName = FileName;
                PlayerPrefsJsonSettings.Instance.SaveOnQuit = SaveOnQuit;
            }
        }

        /// <summary>
        /// Reverts settings to default values from PlayerPrefsJsonSettings.
        /// </summary>
        private void RevertSettings()
        {
            PlayerPrefsJsonSettings.Instance.PersistentDataPath = PlayerPrefsJsonSettings.Instance.DefaultPersistentDataPath;
            PlayerPrefsJsonSettings.Instance.DirectoryPath = PlayerPrefsJsonSettings.Instance.DefaultDirectoryPath;
            PlayerPrefsJsonSettings.Instance.FileName = PlayerPrefsJsonSettings.Instance.DefaultFileName;
            PlayerPrefsJsonSettings.Instance.SaveOnQuit = PlayerPrefsJsonSettings.Instance.DefaultSaveOnQuit;

            PersistentDataPath = PlayerPrefsJsonSettings.Instance.DefaultPersistentDataPath;
            DirectoryPath = PlayerPrefsJsonSettings.Instance.DefaultDirectoryPath;
            FileName = PlayerPrefsJsonSettings.Instance.DefaultFileName;
            SaveOnQuit = PlayerPrefsJsonSettings.Instance.DefaultSaveOnQuit;

            SaveSettings();
        }
    }
}
#endif
