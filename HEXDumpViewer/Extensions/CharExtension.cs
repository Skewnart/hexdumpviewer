using System.Linq;

namespace HEXDumpViewer.Extensions
{
    public static class CharExtension
    {
        private static char[] SPECIALCHARACTERS = new char[] { '\a', '\r', '\n', '\b', '\t', '\v', '\f' };

        public static char RemoveSpecialCharacters(this char car)
        {
            return SPECIALCHARACTERS.ToList().Contains(car) ? '?' : car;
        }
    }
}
