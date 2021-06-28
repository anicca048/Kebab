
using ShimDotNet;
using System;
using System.Collections.Generic;
using System.Net;

namespace Kebab
{
    // List of arguments used by filter.
    public enum Argument
    {
        TCP,
        UDP,
        HOST,
        PORT,
        NULL
    }

    // Basic filter class for holding one of many "anded" conditions.
    public class Filter
    {
        private Argument _argument;
        public Argument Argument { get { return _argument; } }
        private string _value;
        public string Value { get { return _value; } }
        private string _secondaryValue;
        public string SecondaryValue { get { return _secondaryValue; } }

        public Filter(Argument argument, string value, string secondaryValue)
        {
            _argument = argument;
            _value = value;
            _secondaryValue = secondaryValue;
        }

        // Used to ensure that a valid filter is constructed.
        static public bool TryParse(Argument argument, string value, out Filter filter)
        {
            // Set default value of filter incase parsing fails.
            filter = new Filter(Argument.NULL, string.Empty, string.Empty);
            // Create empty potential secondaryValue (may be used by some arguments).
            string secondaryValue = String.Empty;

            // Validate host argument.
            if (argument == Argument.HOST)
            {
                // Check for an IP range.
                if (value.Split('-').Length == 2)
                {
                    secondaryValue = value.Split('-')[1];
                    value = value.Split('-')[0];

                    if (!IPAddress.TryParse(secondaryValue, out IPAddress _))
                        return false;
                }

                if (!IPAddress.TryParse(value, out IPAddress _))
                    return false;

                // Make sure range is sane.
                if (secondaryValue != string.Empty && ConnectionAddress.IPV4ToUint32(IPAddress.Parse(value))
                                                      >= ConnectionAddress.IPV4ToUint32(IPAddress.Parse(secondaryValue)))
                    return false;
            }
            // Validate port argument.
            if (argument == Argument.PORT)
            {
                // Check for a port range.
                if (value.Split('-').Length == 2)
                {
                    secondaryValue = value.Split('-')[1];
                    value = value.Split('-')[0];

                    if (!UInt16.TryParse(secondaryValue, out UInt16 _))
                        return false;
                }

                if (!UInt16.TryParse(value, out UInt16 _))
                    return false;

                // Make sure range is sane.
                if (secondaryValue != string.Empty && UInt16.Parse(value) >= UInt16.Parse(secondaryValue))
                    return false;
            }

            // Create new filter on successful parse.
            filter = new Filter(argument, value, secondaryValue);
            return true;
        }
    }

    // Used to filter out non-matching connections from a display area.
    public class DisplayFilter
    {
        // Values that match the arguments to the user supplied string.
        static private readonly string[] ArgumentTokens =
        {
            "tcp",
            "udp",
            "host",
            "port",
        };

        // Holds list of user supplied filter conditions.
        private List<Filter> Filters;

        public DisplayFilter(List<Filter> filters)
        {
            Filters = filters;
        }

        // Attempts to create a valid filter from a string, and retunrs a success indicator.
        static public bool TryParse(string filter, out DisplayFilter displayFilter)
        {
            // Set default value of displayFilter incase parsing fails.
            displayFilter = new DisplayFilter(new List<Filter>());

            // Create a temporary list of filters which will be used if parsing is successful.
            List<Filter> tmpFilters = new List<Filter>();

            // Split string into tokens using a space separator.
            string[] tokens = filter.ToLower().Split(' ');

            // Argument being processed, acts as flag.
            Argument argument = Argument.NULL;

            foreach (string token in tokens)
            {
                // If an argument is not active then the current token must be an argument.
                if (argument == Argument.NULL)
                {
                    // Match the argument to the token.
                    for (uint i = 0; i < ArgumentTokens.Length; i++)
                    {
                        if (ArgumentTokens[i].Equals(token))
                        {
                            argument = (Argument)i;
                            break;
                        }
                    }

                    // No argument found when one is needed means we have an invalid string.
                    if (argument == Argument.NULL)
                        return false;

                    // Arguments that require no follow up value must be immidiately added.
                    if (argument == Argument.TCP || argument == Argument.UDP)
                    {
                        tmpFilters.Add(new Filter(argument, string.Empty, string.Empty));
                        argument = Argument.NULL;
                    }

                    // Contine to next loop so that arg value will be processed.
                    continue;
                }

                // Create new filter from tokens.
                Filter tmpFilter;

                // Make sure regular argument value combo is valid.
                if (!Filter.TryParse(argument, token, out tmpFilter))
                    return false;

                // Add valid filter to list.
                tmpFilters.Add(tmpFilter);

                // Reset argument / flag.
                argument = Argument.NULL;
            }

            // If the last argument never recieved a value, then we have an invalid string.
            if (argument != Argument.NULL)
                return false;

            // Create Display filter and indicate successful result.
            displayFilter = new DisplayFilter(tmpFilters);
            return true;
        }

        // Checks if a connection matches the filter.
        public bool IsMatch(Connection conn)
        {
            // If we have no filters, nothing can match.
            if (!(Filters.Count > 0))
                return false;

            // Check all active filters against the connection.
            foreach (Filter filter in Filters)
            {
                if (filter.Argument == Argument.TCP && conn.Type.Protocol != L4_PROTOCOL.TCP)
                    return false;
                else if (filter.Argument == Argument.UDP && conn.Type.Protocol != L4_PROTOCOL.UDP)
                    return false;
                else if (filter.Argument == Argument.HOST)
                {
                    if (filter.SecondaryValue != String.Empty)
                    {
                        UInt32 lowerRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.Value));
                        UInt32 upperRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.SecondaryValue));

                        if (!(conn.Source.AddressValue() >= lowerRange && conn.Source.AddressValue() <= upperRange)
                            && !(conn.Destination.AddressValue() >= lowerRange && conn.Destination.AddressValue() <= upperRange))
                            return false;
                    }
                    else if (!conn.Source.Address.ToString().Equals(filter.Value)
                             && !conn.Destination.Address.ToString().Equals(filter.Value))
                        return false;
                }
                else if (filter.Argument == Argument.PORT)
                {
                    // Handle port range filter
                    if (filter.SecondaryValue != String.Empty)
                    {
                        UInt16 lowerRange = UInt16.Parse(filter.Value);
                        UInt16 upperRange = UInt16.Parse(filter.SecondaryValue);

                        if (!(conn.SrcPort >= lowerRange && conn.SrcPort <= upperRange)
                            && !(conn.DstPort >= lowerRange && conn.DstPort <= upperRange))
                            return false;
                    }
                    else if (!conn.SrcPort.ToString().Equals(filter.Value) && !conn.DstPort.ToString().Equals(filter.Value))
                        return false;
                }
            }

            // No non matches, so must have a match.
            return true;
        }
    }
}
