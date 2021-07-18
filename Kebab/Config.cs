
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Kebab
{
    // Class for config variables to be written to conf file (all value types should be easy to validate).
    public class ConfigVariables
    {
        public string update_check = @"false";
        public string flavor_text = (@"v" + Program.Version);
        public List<string[]> tag_list = new List<string[]>();
    }

    // Definitions of excepted values for config variables (all value types should be easy to validate).
    public class ConfigDefinitions
    {
        public readonly string[] update_check = { @"false", @"true" };
    }

    // Limits on excepted values.
    public class ConfigBounds
    {
        public readonly uint[] flavor_text = { 0, 160 };
        public readonly uint[] tag_list = { 1, 80 };
    }

    public class Config
    {
        // Flag to decide if the config file should be overwritten after load.
        public bool SaveOnLoad = false;
        // Flag for checking if there are pending config changes.
        private bool _changesPending = false;
        public bool ChangesPending { get { return _changesPending;  } }
        // Flag to indicate that a json-valid, but incorrect variable was found.
        private bool InvalidCVarFound = false;

        // List of config variables to be (de)serialized to and from json elements.
        private readonly ConfigVariables _cvars = new ConfigVariables();
        public ConfigVariables CVars { get { return _cvars; } }
        private readonly ConfigDefinitions CVarDefs = new ConfigDefinitions();
        private readonly ConfigBounds CVarBounds = new ConfigBounds();
        
        // Filename of config file save location.
        private readonly string ConfigFileName;

        public Config(string fileName)
        {
            ConfigFileName = fileName;
        }

        // Allow caller to mark pended config changes as handled.
        public void ClearPendingChanges()
        {
            if (_changesPending)
                _changesPending = false;
        }

        // Load conf file.
        public bool LoadConfig()
        {
            // Open file stream to config file if not already open.
            string jsonData;

            try
            {
                // Inform caller of failed read operation.
                jsonData = File.ReadAllText(ConfigFileName);
            }
            catch
            {
                // Check if we need to overwrite file.
                if (SaveOnLoad)
                    SaveConfig();

                return false;
            }

            // Storage area for config loaded from file.
            ConfigVariables loadedConfig;

            // Attempt to parse file data as JSON data (any invalid variable names are discarded).
            try
            {
                loadedConfig = JsonConvert.DeserializeObject<ConfigVariables>(jsonData);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                // Check if we need to overwrite file.
                if (SaveOnLoad)
                    SaveConfig();

                return false;
            }

            // Check if deserialization failed (such as in the case of an empty file).
            if (loadedConfig == null)
            {
                // Check if we need to overwrite file.
                if (SaveOnLoad)
                    SaveConfig();

                return false;
            }

            // Apply any valid variable values found in conf file.
            UpdateConfig(loadedConfig);

            // Check if we need to overwrite file.
            if ((_changesPending || InvalidCVarFound) && SaveOnLoad)
                SaveConfig();

            // Inform caller the json config parsing succeeded.
            return true;
        }

        // Update varisables with new values if valid.
        private void UpdateConfig(ConfigVariables loadedConfig)
        {
            // Save banner message.
            if (!loadedConfig.flavor_text.Equals(_cvars.flavor_text))
            {
                // Check for variable validity.
                if (CVarBoundsCheck((uint)loadedConfig.flavor_text.Length, CVarBounds.flavor_text))
                {
                    _cvars.flavor_text = loadedConfig.flavor_text;

                    // Trip pending changes flag.
                    _changesPending = true;
                }
                else
                    // Trip invalid var flag.
                    InvalidCVarFound = true;
            }

            // Enable auto update.
            if (!loadedConfig.update_check.Equals(_cvars.update_check))
            {
                // Check for variable validity.
                if (CVarValidation(loadedConfig.update_check, CVarDefs.update_check))
                {
                    // Write validated change to config var.
                    _cvars.update_check = loadedConfig.update_check;

                    // Trip pending changes flag.
                    _changesPending = true;
                }
                else
                    // Trip invalid var flag.
                    InvalidCVarFound = true;
            }

            // Load connection tag list.
            for (int i = 0; i < loadedConfig.tag_list.Count; i++)
            {
                // Check for variable validity.
                if (IPAddress.TryParse(loadedConfig.tag_list[i][0], out _)
                    && CVarBoundsCheck(loadedConfig.tag_list[i][1], CVarBounds.tag_list))
                {
                    if (i < _cvars.tag_list.Count)
                    {
                        // Overwrite existing entry.
                        _cvars.tag_list[i][0] = loadedConfig.tag_list[i][0];
                        _cvars.tag_list[i][1] = loadedConfig.tag_list[i][1];

                        // Trip pending changes flag.
                        _changesPending = true;
                    }
                    else
                        _cvars.tag_list.Add(loadedConfig.tag_list[i]);
                }
                else
                    // Trip invalid var flag.
                    InvalidCVarFound = true;
            }
        }

        // Write conf file.
        public bool SaveConfig()
        {
            // Serialize config vars to JSON object.
            string jsonData = JsonConvert.SerializeObject(_cvars, Formatting.Indented);

            // Write JSON object to config file (complete overwrite of file) or return false on fail.
            try
            {
                File.WriteAllText(ConfigFileName, jsonData);
            }
            catch
            {
                // Inform caller of failed write operation.
                return false;
            }

            return true;
        }

        // Check if saved value matches a supprted value definition.
        private bool CVarValidation<T>(T cvar, T[] defs)
        where T : IEquatable<T>
        {
            // Loop through all valid options and return true if one matches.
            foreach (T def in defs)
            {
                if (def.Equals(cvar))
                    return true;
            }

            // Return false if no match is found.
            return false;
        }

        // Check if a saved value matches a lower and upper bound limit.
        private bool CVarBoundsCheck<T>(T cvar, T[] limits)
        where T : IComparable<T>
        {
            if (limits[0].CompareTo(cvar) <= 0 && limits[1].CompareTo(cvar) >= 0)
                return true;

            return false;
        }

        // String bounds check overload.
        private bool CVarBoundsCheck(string cvar, uint[] limits)
        {
            if (cvar.Length >= limits[0] && cvar.Length <= limits[1])
                return true;

            return false;
        }
    }
}
