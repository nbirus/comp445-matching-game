using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace LightsOut___Universal
{
    public class ColorConverter
    {
        public static Color ConvertFromString(string colorRgb)
        {
            switch (colorRgb)
            {
                case "#FFFFFFFF":
                    return Colors.White;
                case "#FF0000FF":
                    return Colors.Blue;
                default:
                    string alpha = colorRgb.Substring(1, 2);
                    string red = colorRgb.Substring(3, 2);
                    string green = colorRgb.Substring(5, 2);
                    string blue = colorRgb.Substring(7, 2);
                    return Color.FromArgb(Convert.ToByte(alpha, 16),
                        Convert.ToByte(red, 16), Convert.ToByte(green, 16), 
                        Convert.ToByte(blue, 16));
            }
        }
    }
}
