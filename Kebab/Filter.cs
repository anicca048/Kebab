
namespace Kebab
{
    // Used to filter out non-matching connections from a display area.
    class DisplayFilter
    {
        public bool TryParse(string filterStr)
        {
            if (!filterStr.Equals("valid syntax"))
                return false;

            return true;
        }

        public bool IsMatch(Connection conn)
        {
            return false;
        }

        // Clears internal state creating an empty filter.
        public void Clear()
        {

        }
    }
}
