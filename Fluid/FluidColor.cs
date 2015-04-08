using Fluid.Room;
using System;
using System.Diagnostics;
using System.Text;

namespace Fluid
{
    [DebuggerDisplay("A = {A}, R = {R}, G = {G}, B = {B}")]
    public class FluidColor
    {
        /// <summary>
        /// Gets or sets the red value
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green value
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue value
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Gets or sets the alpha value
        /// </summary>
        public byte A { get; set; }

        private double PivotRgb(double n)
        {
            return (n > 0.04045 ? Math.Pow((n + 0.055) / 1.055, 2.4) : n / 12.92) * 100.0;
        }

        private double PivotXyz(double n)
        {
            return n > 0.008856 ? Math.Pow(n, 1 / 3.0) : (903.3 * n + 16) / 116;
        }

        /// <summary>
        /// Gets the color's L*A*B* values
        /// </summary>
        public double[] GetLabValues()
        {
            double rgbx = PivotRgb(R / 255.0);
            double rgby = PivotRgb(G / 255.0);
            double rgbz = PivotRgb(B / 255.0);

            double labx = PivotXyz(rgbx / 95.047);
            double laby = PivotXyz(rgby / 100.0);
            double labz = PivotXyz(rgbz / 108.883);

            return new double[] { labx, laby, labz };
        }

        private static double Distance(double a, double b)
        {
            return (a - b) * (a - b);
        }

        /// <summary>
        /// Implements the CIE76 method of delta-e: http://en.wikipedia.org/wiki/Color_difference#CIE76
        /// </summary>
        public double CIE76(FluidColor color)
        {
            double[] labA = GetLabValues();
            double[] labB = color.GetLabValues();

            double deltaE2 = Distance(labA[0], labB[0]) +
                             Distance(labA[1], labB[1]) +
                             Distance(labA[2], labB[1]);

            return Math.Sqrt(deltaE2);
        }

        /// <summary>
        /// Implements the Cie94 method of delta-e: http://en.wikipedia.org/wiki/Color_difference#CIE94
        /// </summary>
        public double CIE94(FluidColor color)
        {
            double[] labA = GetLabValues();
            double[] labB = color.GetLabValues();

            double deltaL = labA[0] - labB[0];
            double deltaA = labA[1] - labB[1];
            double deltaB = labA[2] - labB[2];

            double c1 = Math.Sqrt(labA[1] * labA[1] + labA[2] * labA[2]);
            double c2 = Math.Sqrt(labB[1] * labB[1] + labB[2] * labB[2]);
            double deltaC = c1 - c2;

            double deltaH = deltaA * deltaA + deltaB * deltaB - deltaC * deltaC;
            deltaH = deltaH < 0 ? 0 : Math.Sqrt(deltaH);

            const double sl = 1.0;
            const double kc = 1.0;
            const double kh = 1.0;

            var sc = 1.0 + 0.045 * c1;
            var sh = 1.0 + 0.015 * c1;

            double deltaLKlsl = deltaL / (1 * sl);
            double deltaCkcsc = deltaC / (kc * sc);
            double deltaHkhsh = deltaH / (kh * sh);
            double i = deltaLKlsl * deltaLKlsl + deltaCkcsc * deltaCkcsc + deltaHkhsh * deltaHkhsh;
            return i < 0 ? 0 : Math.Sqrt(i);
        }

        private double DegToRad(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// Implements the DE2000 method of delta-e: http://en.wikipedia.org/wiki/Color_difference#CIEDE2000
        /// Correct implementation provided courtesy of Jonathan Hofinger, jaytar42
        /// </summary>
        public double CIEDE2000(FluidColor color)
        {
            //Set weighting factors to 1
            double k_L = 1.0d;
            double k_C = 1.0d;
            double k_H = 1.0d;

            //Change Color Space to L*a*b:
            double[] lab1 = GetLabValues();
            double[] lab2 = color.GetLabValues();

            //Calculate Cprime1, Cprime2, Cabbar
            double c_star_1_ab = Math.Sqrt(lab1[1] * lab1[1] + lab1[2] * lab1[2]);
            double c_star_2_ab = Math.Sqrt(lab2[1] * lab2[1] + lab2[2] * lab2[2]);
            double c_star_average_ab = (c_star_1_ab + c_star_2_ab) / 2;

            double c_star_average_ab_pot7 = c_star_average_ab * c_star_average_ab * c_star_average_ab;
            c_star_average_ab_pot7 *= c_star_average_ab_pot7 * c_star_average_ab;

            double G = 0.5d * (1 - Math.Sqrt(c_star_average_ab_pot7 / (c_star_average_ab_pot7 + 6103515625))); //25^7
            double a1_prime = (1 + G) * lab1[1];
            double a2_prime = (1 + G) * lab2[1];

            double C_prime_1 = Math.Sqrt(a1_prime * a1_prime + lab1[2] * lab1[2]);
            double C_prime_2 = Math.Sqrt(a2_prime * a2_prime + lab2[2] * lab2[2]);
            //Angles in Degree.
            double h_prime_1 = ((Math.Atan2(lab1[2], a1_prime) * 180d / Math.PI) + 360) % 360d;
            double h_prime_2 = ((Math.Atan2(lab2[2], a2_prime) * 180d / Math.PI) + 360) % 360d;

            double delta_L_prime = lab2[0] - lab1[0];
            double delta_C_prime = C_prime_2 - C_prime_1;
                 
            double h_bar = Math.Abs(h_prime_1 - h_prime_2);
            double delta_h_prime;
            if (C_prime_1 * C_prime_2 == 0) delta_h_prime = 0;
            else
            {
                if (h_bar <= 180d)
                {
                    delta_h_prime = h_prime_2 - h_prime_1;
                }
                else if (h_bar > 180d && h_prime_2 <= h_prime_1)
                {
                    delta_h_prime = h_prime_2 - h_prime_1 + 360.0;
                }
                else
                {
                    delta_h_prime = h_prime_2 - h_prime_1 - 360.0;
                }
            }
            double delta_H_prime = 2 * Math.Sqrt(C_prime_1 * C_prime_2) * Math.Sin(delta_h_prime * Math.PI / 360d);

            // Calculate CIEDE2000
            double L_prime_average = (lab1[0] + lab2[0]) / 2d;
            double C_prime_average = (C_prime_1 + C_prime_2) / 2d;

            //Calculate h_prime_average

            double h_prime_average;
            if (C_prime_1 * C_prime_2 == 0) h_prime_average = 0;
            else
            {
                if (h_bar <= 180d)
                {
                    h_prime_average = (h_prime_1 + h_prime_2) / 2;
                }
                else if (h_bar > 180d && (h_prime_1 + h_prime_2) < 360d)
                {
                    h_prime_average = (h_prime_1 + h_prime_2 + 360d) / 2;
                }
                else
                {
                    h_prime_average = (h_prime_1 + h_prime_2 - 360d) / 2;
                }
            }
            double L_prime_average_minus_50_square = (L_prime_average - 50);
            L_prime_average_minus_50_square *= L_prime_average_minus_50_square;

            double S_L = 1 + ((.015d * L_prime_average_minus_50_square) / Math.Sqrt(20 + L_prime_average_minus_50_square));
            double S_C = 1 + .045d * C_prime_average;
            double T = 1
                - .17 * Math.Cos(DegToRad(h_prime_average - 30))
                + .24 * Math.Cos(DegToRad(h_prime_average * 2))
                + .32 * Math.Cos(DegToRad(h_prime_average * 3 + 6))
                - .2 * Math.Cos(DegToRad(h_prime_average * 4 - 63));
            double S_H = 1 + .015 * T * C_prime_average;
            double h_prime_average_minus_275_div_25_square = (h_prime_average - 275) / (25);
            h_prime_average_minus_275_div_25_square *= h_prime_average_minus_275_div_25_square;
            double delta_theta = 30 * Math.Exp(-h_prime_average_minus_275_div_25_square);

            double C_prime_average_pot_7 = C_prime_average * C_prime_average * C_prime_average;
            C_prime_average_pot_7 *= C_prime_average_pot_7 * C_prime_average;
            double R_C = 2 * Math.Sqrt(C_prime_average_pot_7 / (C_prime_average_pot_7 + 6103515625));

            double R_T = -Math.Sin(DegToRad(2 * delta_theta)) * R_C;

            double delta_L_prime_div_k_L_S_L = delta_L_prime / (S_L * k_L);
            double delta_C_prime_div_k_C_S_C = delta_C_prime / (S_C * k_C);
            double delta_H_prime_div_k_H_S_H = delta_H_prime / (S_H * k_H);

            double CIEDE2000 = Math.Sqrt(
                delta_L_prime_div_k_L_S_L * delta_L_prime_div_k_L_S_L
                + delta_C_prime_div_k_C_S_C * delta_C_prime_div_k_C_S_C
                + delta_H_prime_div_k_H_S_H * delta_H_prime_div_k_H_S_H
                + R_T * delta_C_prime_div_k_C_S_C * delta_H_prime_div_k_H_S_H
                );

            return CIEDE2000;
        }

        private static double DistanceDivided(double a, double dividend)
        {
            var adiv = a / dividend;
            return adiv * adiv;
        }
        /// <summary>
        /// Implements the CMC l:c (1984) method of delta-e: http://en.wikipedia.org/wiki/Color_difference#CMC_l:c_.281984.29
        /// </summary>
        public double Cmc(FluidColor color)
        {
            double[] aLab = GetLabValues();
            double[] bLab = color.GetLabValues();

            var deltaL = aLab[0] - bLab[0];
            var h = Math.Atan2(aLab[2], aLab[1]);

            var c1 = Math.Sqrt(aLab[1] * aLab[1] + aLab[2] * aLab[2]);
            var c2 = Math.Sqrt(bLab[1] * bLab[1] + bLab[2] * bLab[2]);
            var deltaC = c1 - c2;

            var deltaH = Math.Sqrt(
                (aLab[1] - bLab[1]) * (aLab[1] - bLab[1]) +
                (aLab[2] - bLab[2]) * (aLab[2] - bLab[2]) -
                deltaC * deltaC);

            var c1_4 = c1 * c1;
            c1_4 *= c1_4;
            var t = 164 <= h || h >= 345
                        ? .56 + Math.Abs(.2 * Math.Cos(h + 168.0))
                        : .36 + Math.Abs(.4 * Math.Cos(h + 35.0));
            var f = Math.Sqrt(c1_4 / (c1_4 + 1900.0));

            var sL = aLab[0] < 16 ? .511 : (.040975 * aLab[0]) / (1.0 + .01765 * aLab[0]);
            var sC = (.0638 * c1) / (1 + .0131 * c1) + .638;
            var sH = sC * (f * t + 1 - f);

            var differences = DistanceDivided(deltaL, 2.0 * sL) +
                              DistanceDivided(deltaC, 1.0 * sC) +
                              DistanceDivided(deltaH, sH);

            return Math.Sqrt(differences);
        }

        /// <summary>
        /// Compares this color to other colors and calculates the most similiar color
        /// </summary>
        /// <param name="method">The method for comparision</param>
        /// <param name="colors">The list of colors</param>
        /// <returns>The most similiar color</returns>
        public T Compare<T>(ComparisonMethod method, Func<T, FluidColor> selector, params T[] values)
        {
            double lowestDelta = double.MaxValue;
            T mostSimiliar = default(T);

            for (int i = 0; i < values.Length; i++)
            {
                double deltaE = double.MaxValue;
                FluidColor value = selector(values[i]);

                if (value == null)
                {
                    continue;
                }

                switch (method)
                {
                    case ComparisonMethod.Cie76:
                        deltaE = CIE76(value);
                        break;
                    case ComparisonMethod.Cie94:
                        deltaE = CIE94(value);
                        break;
                    case ComparisonMethod.CieDE2000:
                        deltaE = CIEDE2000(value);
                        break;
                    case ComparisonMethod.Cmc:
                        deltaE = Cmc(value);
                        break;
                }

                if (deltaE < lowestDelta)
                {
                    lowestDelta = deltaE;
                    mostSimiliar = values[i];
                }
            }

            return mostSimiliar;
        }

        /// <summary>
        /// Compares this color to other colors and calculates the most similiar color
        /// </summary>
        /// <param name="method">The method for comparision</param>
        /// <param name="colors">The list of colors</param>
        /// <returns>The most similiar color</returns>
        public FluidColor Compare(ComparisonMethod method, params FluidColor[] colors)
        {
            double lowestDelta = double.MaxValue;
            FluidColor mostSimiliar = null;

            for (int i = 0; i < colors.Length; i++)
            {
                double deltaE = double.MaxValue;
                switch (method)
                {
                    case ComparisonMethod.Cie76:
                        deltaE = CIE76(colors[i]);
                        break;
                    case ComparisonMethod.Cie94:
                        deltaE = CIE94(colors[i]);
                        break;
                    case ComparisonMethod.CieDE2000:
                        deltaE = CIEDE2000(colors[i]);
                        break;
                    case ComparisonMethod.Cmc:
                        deltaE = Cmc(colors[i]);
                        break;
                }

                if (deltaE < lowestDelta)
                {
                    lowestDelta = deltaE;
                    mostSimiliar = colors[i];
                }
            }

            return mostSimiliar;
        }

        /// <summary>
        /// Gets the inverse of the color
        /// </summary>
        /// <returns></returns>
        public FluidColor Invert()
        {
            return new FluidColor(~ToArgb());
        }

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
        /// Linearly interpolates the color with another color
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
            return Convert.ToUInt32(((uint)A << 24 | (uint)R << 16 | (uint)G << 8 | (uint)B) & -1);
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
        /// Creates a new Fluid color
        /// </summary>
        /// <param name="r">The red value</param>
        /// <param name="g">The green value</param>
        /// <param name="b">The blue value</param>
        public FluidColor(byte r, byte g, byte b)
            : this(255, r, g, b)
        {

        }

        /// <summary>
        /// Creates a new Fluid color
        /// </summary>
        /// <param name="a">The alpha value</param>
        /// <param name="r">The red value</param>
        /// <param name="g">The green value</param>
        /// <param name="b">The blue value</param>
        public FluidColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Creates a new Fluid color
        /// </summary>
        /// <param name="argb">Argb value</param>
        public FluidColor(long argb)
        {
            long value = argb & -1;
            A = (byte)(value >> 24 & 255L);
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
                A = 255;
                R = Convert.ToByte(hexColor.Substring(1, 2), 16);
                G = Convert.ToByte(hexColor.Substring(3, 2), 16);
                B = Convert.ToByte(hexColor.Substring(5, 2), 16);
            }
        }
    }
}
