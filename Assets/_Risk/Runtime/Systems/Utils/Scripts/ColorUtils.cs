using UnityEngine;

namespace Risk.Runtime.Utils
{
    /// <summary>
    /// Provides utility methods for working with colors.
    /// </summary>
    public static class ColorUtils
    {
        public static Color FromColorName(string colorName)
        {
            colorName = colorName.ToLower();
            return colorName switch
            {
                "red" => Color.red,
                "green" => Color.green,
                "blue" => Color.blue,
                "yellow" => Color.yellow,
                "purple" => Color.magenta,
                "black" => Color.black,
                _ => Color.white
            };
        }
    }
}
