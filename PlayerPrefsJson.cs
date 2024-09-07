using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Rosynant.PlayerPrefsJson
{
    /// <summary>
    /// An alternative to the standard Unity PlayerPrefs class. It provides exactly the same methods,
    /// but instead of storing data in the system registry, all information is saved to a JSON file.
    /// It also provides a smooth transition from standard PlayerPrefs to the JSON version if necessary.
    /// </summary>
    public static class PlayerPrefsJson
    {
        private static string _directoryPath;
        private static string _fileName;
        private static JObject _playerPrefsJsonData;
        private static string _combinedPath;
        private static bool _saveOnQuit;

        /// <summary>
        /// Deletes PlayerPrefsJson data. By default deletes all standard PlayerPrefs as well.
        /// </summary>
        /// <param name="deletePlayerPrefs"></param>
        public static void DeleteAll(bool deletePlayerPrefs = true)
        {
            if (deletePlayerPrefs)
                PlayerPrefs.DeleteAll();

            _playerPrefsJsonData = new JObject();
        }

        /// <summary>
        /// Deletes the property whose name matches the key from the DirectoryPath/FilePath file.
        /// By default deletes the key in standard PlayerPrefs as well.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        public static void DeleteKey(string key, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access PlayerPrefsJson.");
                return;
            }

            if (deleteFromPlayerPrefs)
                PlayerPrefs.DeleteKey(key);

            _playerPrefsJsonData.Remove(key);
        }

        /// <summary>
        /// Retrieves the float value corresponding to the given key from PlayerPrefsJson.
        /// By default gets the value from standard PlayerPrefs, deletes it and writes it to PlayerPrefsJson,
        /// if cannot access the key in PlayerPrefsJson. Returns 0.0f if there is no valid value to return.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getFromPlayerPrefs"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <returns></returns>
        public static float GetFloat(string key, bool getFromPlayerPrefs = true, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access PlayerPrefsJson. Returning 0.0f.");
                return 0.0f;
            }

            try
            {
                JToken value = _playerPrefsJsonData.GetValue(key);
                float convertedValue = (float)(double)value;
                return convertedValue;
            }
            catch (Exception)
            {
                // Try to get value from standard PlayerPrefs
                if (getFromPlayerPrefs)
                    return GetFloatFromPlayerPrefs(key, deleteFromPlayerPrefs, 0.0f);

                Debug.LogWarning($"No float value at key '{key}' in PlayerPrefsJson. Returning 0.0f.");
                return 0.0f;
            }
        }

        /// <summary>
        /// Retrieves the float value corresponding to the given key from PlayerPrefsJson.
        /// By default gets the value from standard PlayerPrefs, deletes it and writes it to PlayerPrefsJson,
        /// if cannot access the key in the PlayerPrefsJson. Returns the given default value if there is no valid value to return.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="getFromPlayerPrefs"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <returns></returns>
        public static float GetFloat(string key, float defaultValue, bool getFromPlayerPrefs = true, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError($"Failed to access PlayerPrefsJson. Returning {defaultValue}.");
                return defaultValue;
            }

            try
            {
                JToken value = _playerPrefsJsonData.GetValue(key);
                float convertedValue = (float)(double)value;
                return convertedValue;
            }
            catch (Exception)
            {
                // Try to get value from standard PlayerPrefs
                if (getFromPlayerPrefs)
                    return GetFloatFromPlayerPrefs(key, deleteFromPlayerPrefs, defaultValue);

                Debug.LogWarning($"No float value at key '{key}' in PlayerPrefsJson. Returning {defaultValue}.");
                return defaultValue;
            }
        }

        /// <summary>
        /// Retrieves the int value corresponding to the given key from PlayerPrefsJson.
        /// By default gets the value from standard PlayerPrefs, deletes it and writes it to PlayerPrefsJson,
        /// if cannot access the key in PlayerPrefsJson. Returns 0 if there is no valid value to return.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getFromPlayerPrefs"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <returns></returns>
        public static int GetInt(string key, bool getFromPlayerPrefs = true, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access PlayerPrefsJson. Returning 0.");
                return 0;
            }

            try
            {
                JToken value = _playerPrefsJsonData.GetValue(key);
                int convertedValue = (int)value;
                return convertedValue;
            }
            catch (Exception)
            {
                // Try to get value from standard PlayerPrefs
                if (getFromPlayerPrefs)
                    return GetIntFromPlayerPrefs(key, deleteFromPlayerPrefs, 0);
                
                Debug.LogWarning($"No int value at key '{key}' in PlayerPrefsJson. Returning 0.");
                return 0;
            }
        }

        /// <summary>
        /// Retrieves the int value corresponding to the given key from PlayerPrefsJson.
        /// By default gets the value from standard PlayerPrefs, deletes it and writes it to PlayerPrefsJson,
        /// if cannot access the key in PlayerPrefsJson. Returns the given default value if there is no valid value to return.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="getFromPlayerPrefs"></param>throw new Exception();
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <returns></returns>
        public static int GetInt(string key, int defaultValue, bool getFromPlayerPrefs = true, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError($"Failed to access PlayerPrefsJson. Returning {defaultValue}.");
                return defaultValue;
            }

            try
            {
                JToken value = _playerPrefsJsonData.GetValue(key);
                int convertedValue = (int)value;
                return convertedValue;
            }
            catch (Exception)
            {
                // Try to get value from standard PlayerPrefs
                if (getFromPlayerPrefs)
                    return GetIntFromPlayerPrefs(key, deleteFromPlayerPrefs, defaultValue);
                
                Debug.LogWarning($"No int value at key '{key}' in PlayerPrefsJson. Returning {defaultValue}.");
                return defaultValue;
            }
        }

        /// <summary>
        /// Retrieves the string value corresponding to the given key from PlayerPrefsJson.
        /// By default gets the value from standard PlayerPrefs, deletes it and writes it to PlayerPrefsJson,
        /// if cannot access the key in PlayerPrefsJson. Returns an empty string if there is no valid value to return.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getFromPlayerPrefs"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <returns></returns>
        public static string GetString(string key, bool getFromPlayerPrefs = true, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access PlayerPrefsJson. Returning an empty string.");
                return "";
            }

            try
            {
                JToken value = _playerPrefsJsonData.GetValue(key);
                string convertedValue = (string)value;

                if (convertedValue.Equals(String.Empty))
                    Debug.LogWarning($"String value of key '{key}' in PlayerPrefsJson is an empty string.");
                
                return convertedValue;  
            }
            catch (Exception)
            {
                // Try to get value from standard PlayerPrefs
                if (getFromPlayerPrefs)
                    return GetStringFromPlayerPrefs(key, deleteFromPlayerPrefs, "");
                
                Debug.LogWarning($"No string value at key '{key}' in PlayerPrefsJson. Returning an empty string.");
                return "";
            }
        }

        /// <summary>
        /// Retrieves the string value corresponding to the given key from PlayerPrefsJson.
        /// By default gets the value from standard PlayerPrefs, deletes it and writes it to PlayerPrefsJson,
        /// if cannot access the key in PlayerPrefsJson. Returns the given default value if there is no valid value to return.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="getFromPlayerPrefs"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue, bool getFromPlayerPrefs = true, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError($"Failed to access PlayerPrefsJson. Returning {defaultValue}.");
                return defaultValue;
            }

            try
            {
                JToken value = _playerPrefsJsonData.GetValue(key);
                string convertedValue = (string)value;

                if (convertedValue.Equals(String.Empty))
                    Debug.LogWarning($"String value of key '{key}' in PlayerPrefsJson is an empty string.");

                return convertedValue;  
            }
            catch (Exception)
            {
                // Try to get value from standard PlayerPrefs
                if (getFromPlayerPrefs)
                    return GetStringFromPlayerPrefs(key, deleteFromPlayerPrefs, defaultValue);
                
                Debug.LogWarning($"No string value at key '{key}' in PlayerPrefsJson. Returning {defaultValue}.");
                return defaultValue;
            }
        }

        /// <summary>
        /// Checks whether a property of the given key exists in PlayerPrefsJson.
        /// By default checks the key in standard PlayerPrefs, deletes it and adds it to PlayerPrefsJson,
        /// if there is no key in PlayerPrefsJson.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getFromPlayerPrefs"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <returns></returns>
        public static bool HasKey(string key, bool getFromPlayerPrefs = true, bool deleteFromPlayerPrefs = true)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access PlayerPrefsJson.");
                return false;
            }

            if (_playerPrefsJsonData.Property(key) != null)
            {
                return true;
            }
            else
            {
                if (getFromPlayerPrefs)
                {
                    bool keyInPlayerPrefs = PlayerPrefs.HasKey(key);
                    if (deleteFromPlayerPrefs)
                    {
                        SetKey(key);
                        PlayerPrefs.DeleteKey(key);
                    }
                    return keyInPlayerPrefs;
                }
                return false;
            }
        }

        /// <summary>
        /// Saves the current PlayerPrefsJson data stored in memory to the actual JSON file.
        /// </summary>
        public static void Save()
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access PlayerPrefsJson.");
                return;
            }

            try
            {
                if (!Directory.Exists(_directoryPath))
                    Directory.CreateDirectory(_directoryPath);

                File.WriteAllText(_combinedPath, _playerPrefsJsonData.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError($"{ex.Message}. Failed to access the PlayerPrefsJson file at {_combinedPath}.");
            }
        }

        /// <summary>
        /// Sets the given float value at the given key in PlayerPrefsJson.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetFloat(string key, float value)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access the PlayerPrefsJson.");
                return;
            }

            if (_playerPrefsJsonData.Property(key) != null)
            {
                // Avoid overriding properties of different type
                if (_playerPrefsJsonData.Property(key).Value.Type != JTokenType.Float)
                {
                    Debug.LogWarning($"Trying to override property's '{key}' value '{_playerPrefsJsonData.Property(key).Value}' of type '{_playerPrefsJsonData.Property(key).Value.Type}' with a float.");
                    return;
                }
                _playerPrefsJsonData.Property(key).Value = value;
            }
            else
            {
                _playerPrefsJsonData.Add(key, value);
            }
        }

        /// <summary>
        /// Sets the given int value at the given key in PlayerPrefsJson.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetInt(string key, int value)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access the PlayerPrefsJson.");
                return;
            }

            if (_playerPrefsJsonData.Property(key) != null)
            {
                // Avoid overriding properties of different type
                if (_playerPrefsJsonData.Property(key).Value.Type != JTokenType.Integer)
                {
                    Debug.LogWarning($"Trying to override property's '{key}' value '{_playerPrefsJsonData.Property(key).Value}' of type '{_playerPrefsJsonData.Property(key).Value.Type}' with an int.");
                    return;
                }
                _playerPrefsJsonData.Property(key).Value = value;
            }
            else
            {
                _playerPrefsJsonData.Add(key, value);
            }
        }

        /// <summary>
        /// Sets the given string value at the given key in PlayerPrefsJson.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetString(string key, string value)
        {
            if (_playerPrefsJsonData == null)
            {
                Debug.LogError("Failed to access the PlayerPrefsJson.");
                return;
            }
            
            if (_playerPrefsJsonData.Property(key) != null)
            {
                // Avoid overriding properties of different type
                if (_playerPrefsJsonData.Property(key).Value.Type != JTokenType.String)
                {
                    Debug.LogWarning($"Trying to override property's '{key}' value '{_playerPrefsJsonData.Property(key).Value}' of type '{_playerPrefsJsonData.Property(key).Value.Type}' with a string.");
                    return;
                }
                _playerPrefsJsonData.Property(key).Value = value;
            }
            else
            {
                _playerPrefsJsonData.Add(key, value);
            }
        }

        /// <summary>
        /// Loads an existing PlayerPrefsJson file into memory or creates a new one if none exists at _directoryPath/_filePath.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadPlayerPrefsJson()
        {
            _directoryPath = PlayerPrefsJsonHelpers.DirectoryPath;
            _fileName = PlayerPrefsJsonHelpers.FileName;
            _saveOnQuit = PlayerPrefsJsonHelpers.SaveOnQuit;

            string path = PlayerPrefsJsonHelpers.DeterminePath(_directoryPath, _fileName);

            if (String.IsNullOrEmpty(path))
            {
                Debug.LogError($"Invalid PlayerPrefsJson path: '{path}'.");
                return;
            }

            if (!File.Exists(path))
            {
                _playerPrefsJsonData = new JObject();
                File.WriteAllText(path, _playerPrefsJsonData.ToString());
            }
            else
            {
                _playerPrefsJsonData = JObject.Parse(File.ReadAllText(path));
            }

            _combinedPath = path;

            // Save to JSON file when quitting the app
            if (_saveOnQuit)
                Application.quitting += SavePlayerPrefsJson;
        }

        /// <summary>
        /// Gets the float value at the given key from standard PlayerPrefs. Optionally, deletes it in PlayerPrefs
        /// and adds it to PlayerPrefsJson. Returns the default value if there is no float value at the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static float GetFloatFromPlayerPrefs(string key, bool deleteFromPlayerPrefs, float defaultValue)
        {
            // Try to get value from standard PlayerPrefs
            float playerPrefsValue = PlayerPrefs.GetFloat(key, float.MaxValue);
            
            // No value in standard PlayerPrefs
            if (playerPrefsValue == float.MaxValue)
            {
                Debug.LogWarning($"No float value at key '{key}' in standard PlayerPrefs. Returning {defaultValue}.");
                return defaultValue;
            }  
            else
            {
                // Delete value in standard PlayerPrefs
                if (deleteFromPlayerPrefs)
                {
                    SetFloat(key, playerPrefsValue);
                    PlayerPrefs.DeleteKey(key);
                }
                return playerPrefsValue;
            }   
        }

        /// <summary>
        /// Gets the int value at the given key from standard PlayerPrefs. Optionally, deletes it in PlayerPrefs
        /// and adds it to PlayerPrefsJson. Returns the default value if there is no int value at the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static int GetIntFromPlayerPrefs(string key, bool deleteFromPlayerPrefs, int defaultValue)
        {
            // Try to get value from standard PlayerPrefs
            int playerPrefsValue = PlayerPrefs.GetInt(key, int.MaxValue);
            
            // No value in standard PlayerPrefs
            if (playerPrefsValue == int.MaxValue)
            {
                Debug.LogWarning($"No int value at key '{key}' in standard PlayerPrefs. Returning {defaultValue}.");
                return defaultValue;
            }  
            else
            {
                // Delete value in standard PlayerPrefs
                if (deleteFromPlayerPrefs)
                {
                    SetInt(key, playerPrefsValue);
                    PlayerPrefs.DeleteKey(key);
                }
                return playerPrefsValue;
            }
        }

        /// <summary>
        /// Gets the string value at the given key from standard PlayerPrefs. Optionally, deletes it in PlayerPrefs
        /// and adds it to PlayerPrefsJson. Returns the default value if there is no string value at the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="deleteFromPlayerPrefs"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetStringFromPlayerPrefs(string key, bool deleteFromPlayerPrefs, string defaultValue)
        {
            // Try to get value from standard PlayerPrefs
            string playerPrefsValue = PlayerPrefs.GetString(key, "");
            
            // No value in standard PlayerPrefs
            if (String.IsNullOrEmpty(playerPrefsValue))
            {
                Debug.LogWarning($"No string value at key '{key}' in standard PlayerPrefs. Returning '{defaultValue}'.");
                return defaultValue;
            }  
            else
            {
                // Delete value in standard PlayerPrefs
                if (deleteFromPlayerPrefs)
                {
                    SetString(key, playerPrefsValue);
                    PlayerPrefs.DeleteKey(key);
                }
                return playerPrefsValue;
            }
        }

        /// <summary>
        /// Adds the key with its corresponding value from standard PlayerPrefs to PlayerPrefsJson.
        /// </summary>
        /// <param name="key"></param>
        private static void SetKey(string key)
        {
            // If float value
            float floatValue = PlayerPrefs.GetFloat(key, float.MaxValue);
            if (floatValue != float.MaxValue)
            {
                SetFloat(key, floatValue);
                return;
            }

            // If int value
            int intValue = PlayerPrefs.GetInt(key, int.MaxValue);
            if (intValue != int.MaxValue)
            {
                SetInt(key, intValue);
                return;
            }

            // If string value
            string stringValue = PlayerPrefs.GetString(key, "");
            if (!String.IsNullOrEmpty(stringValue))
                SetString(key, stringValue);
        }

        /// <summary>
        /// Saves the current PlayerPrefsJson data stored in memory to the actual JSON file and resets the data.
        /// </summary>
        private static void SavePlayerPrefsJson()
        {
            Save();
            _playerPrefsJsonData = null;
            _combinedPath = null;
        }
    }
}
