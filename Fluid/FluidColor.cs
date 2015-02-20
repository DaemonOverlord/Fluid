using System;
using System.Text;

namespace Fluid
{
    public class FluidColor
    {
        /// <summary>
        /// Gets or sets the red
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Gets a hex color segment
        /// </summary>
        /// <param name="b">The byte</param>
        /// <returns>The hex color segment</returns>
        public string GetHtmlSegment(byte b)
        {
            string htmlSeg = b.ToString("X");
            if (htmlSeg.Length == 1)
            {
                return '0' + htmlSeg;
            }

            return htmlSeg;
        }

        /// <summary>
        /// Converts the color into an html color string
        /// </summary>
        public string ToHtml()
        {
            StringBuilder html = new StringBuilder();

            html.Append('#');
            html.Append(GetHtmlSegment(R));
            html.Append(GetHtmlSegment(G));
            html.Append(GetHtmlSegment(B));
            return html.ToString();
        }

        /// <summary>
        /// Converts the color to argb
        /// </summary>
        public uint ToArgb()
        {
            return Convert.ToUInt32(((uint)R << 16 | (uint)G << 8 | (uint)B) & -1);
        }

        /// <summary>
        /// Tests if this is equal to an object
        /// </summary>
        /// <param name="obj">The object</param>
        public override bool Equals(object obj)
        {
            if (obj is FluidColor)
            {
                FluidColor color = (FluidColor)obj;
                return color.R == R && color.G == G && color.B == B;
            }

            return false;
        }

        /// <summary>
        /// Gets the Fluid color hashcode
        /// </summary>
        public override int GetHashCode()
        {
            uint argb = ToArgb();
            return argb.GetHashCode();
        }

        /// <summary>
        /// Gets the Fluid color debug message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("R: {0} G: {1} B: {2}", R, G, B);
        }

        /// <summary>
        /// Creates a new Fluid color
        /// </summary>
        /// <param name="r">The red value</param>
        /// <param name="g">The green value</param>
        /// <param name="b">The blue value</param>
        public FluidColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Creates a new Fluid color
        /// </summary>
        /// <param name="argb">Argb value</param>
        public FluidColor(uint argb)
        {
            long value = argb & -1;
            R = (byte)(value >> 16 & 255L);
            G = (byte)(value >> 8 & 255L);
            B = (byte)(value & 255L);
        }

        /// <summary>
        /// Creates a new Fluid color
        /// </summary>
        /// <param name="hexColor">The hexadecimal color string</param>
        public FluidColor(string hexColor)
        {
            if (hexColor == null || hexColor.Length == 0)
            {
                return;
            }

            if (hexColor[0] == '#' && hexColor.Length == 7)
            {
                R = Convert.ToByte(hexColor.Substring(1, 2), 16);
                G = Convert.ToByte(hexColor.Substring(3, 2), 16);
                B = Convert.ToByte(hexColor.Substring(5, 2), 16);
            }
        }
    }
}
