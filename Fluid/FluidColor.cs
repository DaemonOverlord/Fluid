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
        /// Gets the brightness of the color
        /// </summary>
        /// <returns>The brightness of the color</returns>
        public float GetBrightness()
        {
            float num = (float)this.R / 255f;
            float num2 = (float)this.G / 255f;
            float num3 = (float)this.B / 255f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
            {
                num4 = num2;
            }
            if (num3 > num4)
            {
                num4 = num3;
            }
            if (num2 < num5)
            {
                num5 = num2;
            }
            if (num3 < num5)
            {
                num5 = num3;
            }
            return (num4 + num5) / 2f;
        }

        /// <summary>
        /// Gets the hue of the color
        /// </summary>
        /// <returns>The hue of the color</returns>
        public float GetHue()
        {
            if (this.R == this.G && this.G == this.B)
            {
                return 0f;
            }
            float num = (float)this.R / 255f;
            float num2 = (float)this.G / 255f;
            float num3 = (float)this.B / 255f;
            float num4 = 0f;
            float num5 = num;
            float num6 = num;
            if (num2 > num5)
            {
                num5 = num2;
            }
            if (num3 > num5)
            {
                num5 = num3;
            }
            if (num2 < num6)
            {
                num6 = num2;
            }
            if (num3 < num6)
            {
                num6 = num3;
            }
            float num7 = num5 - num6;
            if (num == num5)
            {
                num4 = (num2 - num3) / num7;
            }
            else
            {
                if (num2 == num5)
                {
                    num4 = 2f + (num3 - num) / num7;
                }
                else
                {
                    if (num3 == num5)
                    {
                        num4 = 4f + (num - num2) / num7;
                    }
                }
            }
            num4 *= 60f;
            if (num4 < 0f)
            {
                num4 += 360f;
            }
            return num4;
        }

        /// <summary>
        /// Gets the saturation of the color
        /// </summary>
        /// <returns>The saturation of the color</returns>
        public float GetSaturation()
        {
            float num = (float)this.R / 255f;
            float num2 = (float)this.G / 255f;
            float num3 = (float)this.B / 255f;
            float result = 0f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
            {
                num4 = num2;
            }
            if (num3 > num4)
            {
                num4 = num3;
            }
            if (num2 < num5)
            {
                num5 = num2;
            }
            if (num3 < num5)
            {
                num5 = num3;
            }
            if (num4 != num5)
            {
                float num6 = (num4 + num5) / 2f;
                if ((double)num6 <= 0.5)
                {
                    result = (num4 - num5) / (num4 + num5);
                }
                else
                {
                    result = (num4 - num5) / (2f - num4 - num5);
                }
            }
            return result;
        }

        /// <summary>
        /// Linearly interpolates fits the color with another color
        /// </summary>
        /// <param name="color">The color</param>
        /// <param name="amount">The amount; a rate or time between 0 and 1</param>
        public void Lerp(FluidColor color, double amount)
        {
            R = (byte)(R + (R - color.R) * amount);
            G = (byte)(G + (G - color.G) * amount);
            B = (byte)(B + (B - color.B) * amount);
        }

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
