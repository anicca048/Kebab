﻿
using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Kebab
{
    // Class for config variables to be written to conf file (all value types should be easy to validate).
    public class ConfVars
    {
        public string banner_message = @"Connection Oriented Packet Sniffer";
        public string theme = @"light";
    }

    // Definitions of excepted values for config variables (all value types should be easy to validate).
    public class ConfVarDefinitions
    {
        public readonly List<string> theme = new List<string>() {@"light", @"dark"};
    }

    public class Config
    {
        // List of config variables to be (de)serialized to and from json elements.
        public ConfVars Vars = new ConfVars();
        private ConfVarDefinitions VarDefs = new ConfVarDefinitions();

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
            ConfVars savedConfig;

            // Attempt to parse file data as JSON data (any invalid variable names are discarded).
            try
            {
                savedConfig = JsonConvert.DeserializeObject<ConfVars>(jsonData);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                return false;
            }

            // Apply any valid variable values found in conf file.
            UpdateConfig(savedConfig);

            return true;
        }

        // Update varisables with new values if valid.
        private void UpdateConfig(ConfVars savedConfig)
        {
            // Apply banner message.
            if (!savedConfig.banner_message.Equals(Vars.banner_message))
                Vars.banner_message = savedConfig.banner_message;

            // Apply theme if valid.
            if (!savedConfig.theme.Equals(Vars.theme))
            {
                if (ValidateConfigVariable(savedConfig.theme, VarDefs.theme))
                    Vars.theme = savedConfig.theme;
            }
        }

        // Write conf file.
        public bool SaveConfig()
        {
            // Serialize config vars to JSON object.
            string jsonData = JsonConvert.SerializeObject(Vars, Formatting.Indented);

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
        private bool ValidateConfigVariable<T>(T value, List<T> valueDefinitions)
        where T : IEquatable<T>
        {
            // Loop through all valid options and return true if one matches.
            foreach (T definition in valueDefinitions)
            {
                if (definition.Equals(value))
                    return true;
            }

            // Return false if no match is found.
            return false;
        }
    }
}