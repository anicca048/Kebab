
using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Kebab
{
    // Class for config variables to be written to conf file (should all be strings for input verification).
    public class ConfVars
    {
        public string banner_message = @"Connection Oriented Packet Sniffer";
        public string theme = @"light";
    }
    
    public class Config
    {
        // List of config variables to be (de)serialized to and from json elements.
        public ConfVars Vars = new ConfVars();
        
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

            // Parse file data as JSON data (any invalid variable names are discarded).
            ConfVars savedConfig = JsonConvert.DeserializeObject<ConfVars>(jsonData);

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
            if (!savedConfig.theme.Equals(Vars.banner_message))
            {
                if (savedConfig.theme.Equals("light"))
                    Vars.theme = "light";
                else if (savedConfig.theme.Equals("dark"))
                    Vars.theme = "dark";
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
    }
}
