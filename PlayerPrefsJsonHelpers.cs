using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Rosynant.PlayerPrefsJson
{
    /// <summary>
    /// Provides helper methods for PlayerPrefsJson.
    /// </summary>
    public static class PlayerPrefsJsonHelpers
    {
        public static string DirectoryPath { get; private set; } = Path.Combine(Application.persistentDataPath, "Saves", "Prefs");
        public static string FileName { get; private set; } = "Prefs.json";
        public static bool SaveOnQuit { get; private set; } = true;

        private static readonly string _strRegex = "[<>:\"|?*]";

        /// <summary>
        /// Determines the PlayerPrefsJson file path. Returns an empty string if path cannot be determined.
        /// </summary>
        /// <returns></returns>
        public static string DeterminePath(string directoryPath, string fileName)
        {
            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{ex.Message}. Could not create directory for PlayerPrefsJson file at '{directoryPath}'.");
                    return "";
                }
            }

            if (!ValidateJsonFileName(fileName))
            {
                Debug.LogError($"Invalid PlayerPrefsJson file name at '{fileName}'.");
                return "";
            }
            
            return Path.Combine(directoryPath, fileName);
        }

        /// <summary>
        /// Validates the supplied path checking for the presence of forbidden characters for directory and file names,
        /// as per https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file, except "/" and "\" (it's a path),
        /// unless validating a file name (it's not a path).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateDirectoryOrFileName(string name, bool isFileName = false)
        {
            if (String.IsNullOrWhiteSpace(name))
                return false;
            
            if (isFileName)
            {
                if (name.Contains("/") || name.Contains("\\"))
                    return false;
            }

            Regex regex = new Regex(_strRegex, RegexOptions.IgnoreCase);
            return !regex.IsMatch(name);
        }

        /// <summary>
        /// Validates the supplied name of a JSON file, checking for the correct file extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ValidateJsonFileName(string fileName)
        {
            if (fileName.Length <= 5)
                return false;

            string fileExtension = fileName.Substring(fileName.Length - 5);
            return fileExtension.Equals(".json") ? true : false;
        }

        /// <summary>
        /// Retrieves PlayerPrefsjson settings from the settings ScriptableObject if it contains valid data.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void GetPlayerPrefsJsonSettings()
        {
            try
            {
                if (String.IsNullOrEmpty(PlayerPrefsJsonSettings.Instance.DirectoryPath) || String.IsNullOrEmpty(PlayerPrefsJsonSettings.Instance.FileName))
                    return;

                if (PlayerPrefsJsonSettings.Instance.PersistentDataPath)
                    DirectoryPath = Path.Combine(Application.persistentDataPath, PlayerPrefsJsonSettings.Instance.DirectoryPath);
                else
                    DirectoryPath = PlayerPrefsJsonSettings.Instance.DirectoryPath;

                FileName = PlayerPrefsJsonSettings.Instance.FileName;
                SaveOnQuit = PlayerPrefsJsonSettings.Instance.SaveOnQuit;
            }
            catch (Exception)
            {
                Debug.LogWarning("Unable to access the PlayerPrefsJsonSettings ScriptableObject. Using default settings.");
            }
        }
    }
}
