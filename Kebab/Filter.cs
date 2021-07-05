
using System;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using ShimDotNet;

namespace Kebab
{
    // List of arguments used by filter.
    public enum Argument
    {
        TCP,
        UDP,
        HOST,
        PORT,
        ISO,
        ASN,
        NULL
    }

    public enum Modifyer
    {
        SRC,
        DST,
        NONE
    }

    // Basic filter class for holding one of many "anded" conditions.
    public class Filter
    {
        // Used to evaluate alpha only string.
        static public readonly string nonAlphaRegex = @"[^a-zA-Z]";
        // Used to evaluate alpha and hyphen string such as ISO.
        static public readonly string nonAlphaAndDashRegex = @"[^a-zA-Z-]";
        // Used to evaluate numeric only strings such as ports.
        static public readonly string nonNumericRegex = @"[^0-9]";
        // Used to evaluate numeric and period only string such as ip addrs.
        static public readonly string nonNumericAndDotRegex = @"[^0-9\.]";
        // Used to evaluate alphanumeric strings that may have seperators such as ASN.
        static public readonly string nonAlphaNumericAndSeparatorsRegex = @"[^0-9a-zA-Z-_]";

        // The main argument (filter operation).
        private readonly Argument _argument;
        public Argument Argument { get { return _argument; } }
        // Modifies the argument (src, dst, etc..).
        private readonly Modifyer _modifyer;
        public Modifyer Modifyer { get { return _modifyer; } }
        // The main value that will be paired with the argument to filter out connections.
        private readonly string _value;
        public string Value { get { return _value; } }
        // A secondary value, usually representing an upper range in a range based value.
        private readonly string _secondaryValue;
        public string SecondaryValue { get { return _secondaryValue; } }

        // Basic constructor, not usually called without using TryParse().
        public Filter(Argument argument, Modifyer modifyer, string value, string secondaryValue)
        {
            _argument = argument;
            _modifyer = modifyer;
            _value = value;
            _secondaryValue = secondaryValue;
        }

        // Used to ensure that a valid filter is constructed.
        static public bool TryParse(Argument argument, Modifyer modifyer, string value, out Filter filter)
        {
            // Set default value of filter incase parsing fails.
            filter = new Filter(Argument.NULL, Modifyer.NONE, string.Empty, string.Empty);
            // Create empty potential secondaryValue (may be used by some arguments).
            string secondaryValue = String.Empty;

            // Don't allow modifyers on arguments that don't use them.
            if (!(argument == Argument.HOST || argument == Argument.PORT)
                && modifyer != Modifyer.NONE)
                return false;

            // Validate host argument.
            if (argument == Argument.HOST)
            {
                // Check for an IP range.
                if (value.Split('-').Length == 2)
                {
                    // Split string based on '-' delimiter (represents a range).
                    secondaryValue = value.Split('-')[1];
                    value = value.Split('-')[0];

                    // Check IP against regex before TryParse.
                    if (Regex.Match(secondaryValue, Filter.nonNumericAndDotRegex).Length > 0)
                        return false;

                    // Make sure secondary IP is valid.
                    if (!IPAddress.TryParse(secondaryValue, out IPAddress _))
                        return false;
                }

                // Check IP against regex before TryParse.
                if (Regex.Match(value, Filter.nonNumericAndDotRegex).Length > 0)
                    return false;

                // Make sure primary IP is valid.
                if (!IPAddress.TryParse(value, out IPAddress _))
                    return false;

                // Make sure range is sane.
                if (secondaryValue != string.Empty && ConnectionAddress.IPV4ToUint32(IPAddress.Parse(value))
                                                      >= ConnectionAddress.IPV4ToUint32(IPAddress.Parse(secondaryValue)))
                    return false;
            }
            // Validate port argument.
            else if (argument == Argument.PORT)
            {
                // Check for a port range.
                if (value.Split('-').Length == 2)
                {
                    // Split string based on '-' delimiter (represents a range).
                    secondaryValue = value.Split('-')[1];
                    value = value.Split('-')[0];

                    // Check Port against regex before TryParse.
                    if (Regex.Match(secondaryValue, Filter.nonNumericRegex).Length > 0)
                        return false;

                    // Make sure we have a valid secondary port value.
                    if (!UInt16.TryParse(secondaryValue, out _))
                        return false;
                }

                // Check Port against regex before TryParse.
                if (Regex.Match(value, Filter.nonNumericRegex).Length > 0)
                    return false;

                // Make sure we have a valid primary port value.
                if (!UInt16.TryParse(value, out _))
                    return false;

                // Make sure range is sane.
                if (secondaryValue != string.Empty && UInt16.Parse(value) >= UInt16.Parse(secondaryValue))
                    return false;
            }
            else if (argument == Argument.ISO)
            {
                // ISO code should have at 2 or 5 chars.
                if (!(value.Length == 2 || value.Length == 5))
                    return false;

                // Check string against regex before.
                if (Regex.Match(value, Filter.nonAlphaAndDashRegex).Length > 0)
                    return false;
            }
            else if (argument == Argument.ASN)
            {
                // Check string against regex before.
                if (Regex.Match(value, Filter.nonAlphaNumericAndSeparatorsRegex).Length > 0)
                    return false;
            }

            // Create new filter on successful parse.
            filter = new Filter(argument, modifyer, value, secondaryValue);
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
            "iso",
            "asn"
        };

        // Values that match the modifyers to the user supplied string.
        static private readonly string[] ModifyerTokens =
        {
            "src",
            "dst"
        };

        // Holds list of user supplied filter conditions.
        private readonly List<Filter> Filters;

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
            // Possible modifyer.
            Modifyer modifyer = Modifyer.NONE;

            foreach (string token in tokens)
            {
                // Check if current token could be an argument modifyer.
                if (modifyer == Modifyer.NONE && argument == Argument.NULL)
                {
                    // Attempt to match a modifyer to the token.
                    for (uint i = 0; i < ModifyerTokens.Length; i++)
                    {
                        if (ModifyerTokens[i].Equals(token))
                        {
                            modifyer = (Modifyer)i;
                            break;
                        }
                    }

                    // If a valid modifyer was found, continue to next token (should be an argument).
                    if (modifyer != Modifyer.NONE)
                        continue;
                }

                // Check if the current token should be an argument.
                if (argument == Argument.NULL)
                {
                    // Attempt to match an argument to the token.
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
                        // Make sure regular argument value combo is valid.
                        if (!Filter.TryParse(argument, modifyer, String.Empty, out Filter tmpFilter))
                            return false;

                        // Add valid filter to list.
                        tmpFilters.Add(tmpFilter);

                        // Reset modifyer and argument / flags.
                        modifyer = Modifyer.NONE;
                        argument = Argument.NULL;
                    }

                    // Contine to next loop so that arg value will be processed.
                    continue;
                }

                // Make sure regular argument value combo is valid.
                if (!Filter.TryParse(argument, modifyer, token, out Filter tmpfilter))
                    return false;

                // Add valid filter to list.
                tmpFilters.Add(tmpfilter);

                // Reset modifyer and argument / flags.
                modifyer = Modifyer.NONE;
                argument = Argument.NULL;
            }

            // If the last argument never recieved a value, then we have an invalid string.
            if (!(modifyer == Modifyer.NONE && argument == Argument.NULL))
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
                // Drop through each possible filter type and run the matching argument test.
                if (filter.Argument == Argument.TCP && conn.Type.Protocol != L4_PROTOCOL.TCP)
                    return false;
                else if (filter.Argument == Argument.UDP && conn.Type.Protocol != L4_PROTOCOL.UDP)
                    return false;
                else if (filter.Argument == Argument.HOST)
                {
                    // Handle ANY matching host.
                    if (filter.Modifyer == Modifyer.NONE)
                    {
                        // Handle range based values.
                        if (filter.SecondaryValue != String.Empty)
                        {
                            UInt32 lowerRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.Value));
                            UInt32 upperRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.SecondaryValue));

                            if (!(conn.SrcHost.AddressValue() >= lowerRange && conn.SrcHost.AddressValue() <= upperRange)
                                && !(conn.DstHost.AddressValue() >= lowerRange && conn.DstHost.AddressValue() <= upperRange))
                                return false;
                        }
                        // Handle single value.
                        else if (!conn.SrcHost.Address.ToString().Equals(filter.Value)
                                 && !conn.DstHost.Address.ToString().Equals(filter.Value))
                            return false;
                    }
                    // Handle SRC matching host.
                    else if (filter.Modifyer == Modifyer.SRC)
                    {
                        // Handle range based values.
                        if (filter.SecondaryValue != String.Empty)
                        {
                            UInt32 lowerRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.Value));
                            UInt32 upperRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.SecondaryValue));

                            if (!(conn.SrcHost.AddressValue() >= lowerRange && conn.SrcHost.AddressValue() <= upperRange))
                                return false;
                        }
                        // Handle single value.
                        else if (!conn.SrcHost.Address.ToString().Equals(filter.Value))
                            return false;
                    }
                    // Handle DST matching host.
                    else if (filter.Modifyer == Modifyer.DST)
                    {
                        // Handle range based values.
                        if (filter.SecondaryValue != String.Empty)
                        {
                            UInt32 lowerRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.Value));
                            UInt32 upperRange = ConnectionAddress.IPV4ToUint32(IPAddress.Parse(filter.SecondaryValue));

                            if (!(conn.DstHost.AddressValue() >= lowerRange && conn.DstHost.AddressValue() <= upperRange))
                                return false;
                        }
                        // Handle single value.
                        else if (!conn.DstHost.Address.ToString().Equals(filter.Value))
                            return false;
                    }
                }
                else if (filter.Argument == Argument.PORT)
                {
                    // Handle ANY matching port.
                    if (filter.Modifyer == Modifyer.NONE)
                    {
                        // Handle range based values.
                        if (filter.SecondaryValue != String.Empty)
                        {
                            UInt16 lowerRange = UInt16.Parse(filter.Value);
                            UInt16 upperRange = UInt16.Parse(filter.SecondaryValue);

                            if (!(conn.SrcPort >= lowerRange && conn.SrcPort <= upperRange)
                                && !(conn.DstPort >= lowerRange && conn.DstPort <= upperRange))
                                return false;
                        }
                        // Handle single value.
                        else if (!conn.SrcPort.ToString().Equals(filter.Value) && !conn.DstPort.ToString().Equals(filter.Value))
                            return false;
                    }
                    // Handle SRC matching port.
                    else if (filter.Modifyer == Modifyer.SRC)
                    {
                        // Handle range based values.
                        if (filter.SecondaryValue != String.Empty)
                        {
                            UInt16 lowerRange = UInt16.Parse(filter.Value);
                            UInt16 upperRange = UInt16.Parse(filter.SecondaryValue);

                            if (!(conn.SrcPort >= lowerRange && conn.SrcPort <= upperRange))
                                return false;
                        }
                        // Handle single value.
                        else if (!conn.SrcPort.ToString().Equals(filter.Value))
                            return false;
                    }
                    // Handle DST matching port.
                    else if (filter.Modifyer == Modifyer.DST)
                    {
                        // Handle range based values.
                        if (filter.SecondaryValue != String.Empty)
                        {
                            UInt16 lowerRange = UInt16.Parse(filter.Value);
                            UInt16 upperRange = UInt16.Parse(filter.SecondaryValue);

                            if (!(conn.DstPort >= lowerRange && conn.DstPort <= upperRange))
                                return false;
                        }
                        // Handle single value.
                        else if (!conn.DstPort.ToString().Equals(filter.Value))
                            return false;
                    }
                }
                else if (filter.Argument == Argument.ISO)
                {
                    if (!(Regex.Match(conn.DstGeo.ToString().ToLower(), filter.Value).Length > 0))
                        return false;
                }
                else if (filter.Argument == Argument.ASN)
                {
                    if (!(Regex.Match(conn.DstASNOrg.ToLower(), filter.Value).Length > 0))
                        return false;
                }
            }

            // No non-matches were found with active filters, so we must have a match.
            return true;
        }
    }
}
