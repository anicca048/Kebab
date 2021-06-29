
using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Kebab
{
    // Class for config variables to be written to conf file (all value types should be easy to validate).
    public class ConfigVariables
    {
        public string flavor_text = (@"v" + Program.Version);
        public string update_check = @"false";
    }

    // Definitions of excepted values for config variables (all value types should be easy to validate).
    public class ConfigDefinitions
    {
        public readonly List<string> update_check = new List<string>() {@"false", @"true"};
    }

    public class Config
    {
        // List of config variables to be (de)serialized to and from json elements.
        public ConfigVariables CVars = new ConfigVariables();
        private ConfigDefinitions CVarDefs = new ConfigDefinitions();

        // Filename of config file save location.
        private string ConfigFileName;

        public Config(string fileName)
        {
            ConfigFileName = fileName;
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
                return false;
            }

            // Storage area for config if load.
            ConfigVariables savedConfig;

            // Attempt to parse file data as JSON data (any invalid variable names are discarded).
            try
            {
                savedConfig = JsonConvert.DeserializeObject<ConfigVariables>(jsonData);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                return false;
            }

            // Check if deserialization failed (such as in the case of an empty file).
            if (savedConfig == null)
                return false;

            // Apply any valid variable values found in conf file.
            UpdateConfig(savedConfig);

            // Inform caller the json config parsing succeeded.
            return true;
        }

        // Update varisables with new values if valid.
        private void UpdateConfig(ConfigVariables savedConfig)
        {
            // Apply banner message.
            if (!savedConfig.flavor_text.Equals(CVars.flavor_text))
                CVars.flavor_text = savedConfig.flavor_text;

            // Apply theme if valid.
            if (!savedConfig.update_check.Equals(CVars.update_check))
            {
                if (ValidateConfigVariable(savedConfig.update_check, CVarDefs.update_check))
                    CVars.update_check = savedConfig.update_check;
            }
        }

        // Write conf file.
        public bool SaveConfig()
        {
            // Serialize config vars to JSON object.
            string jsonData = JsonConvert.SerializeObject(CVars, Formatting.Indented);

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

        // Check if saved value matches a supprted value.
        private bool ValidateConfigVariable<T>(T cvar, List<T> defs)
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
    }
}
