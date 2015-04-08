using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Fluid
{
    public class BlockImage
    {
        private int m_Stride;
        private byte[] m_Data;
        private FluidColor[,] m_ColorMap;

        private Image image;

        /// <summary>
        /// Gets the block image's width
        /// </summary>
        public int Width { get { return m_ColorMap.GetLength(0); } }

        /// <summary>
        /// Gets the block image's height
        /// </summary>
        public int Height { get { return m_ColorMap.GetLength(1); } }

        /// <summary>
        /// Gets the color values of the image
        /// </summary>
        public FluidColor[,] Values { get { return m_ColorMap; } }

        /// <summary>
        /// Gets the image as a System.Drawing.Image
        /// </summary>
        public Image Image { get { return image; } }

        private void Read()
        {
            m_ColorMap = new FluidColor[image.Width, image.Height];
            using (Bitmap bitmap = new Bitmap(image))
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

                m_Stride = Math.Abs(bitmapData.Stride);
                int size = m_Stride * bitmapData.Height;

                m_Data = new byte[size];
                Marshal.Copy(bitmapData.Scan0, m_Data, 0, size);

                for (int y = bitmap.Height - 1; y >= 0; y--)
                {
                    byte[] rowArray = new byte[bitmapData.Stride + 1];
                    Array.Copy(m_Data, y * bitmapData.Stride, rowArray, 0, bitmapData.Stride);

                    for (int x = bitmap.Width - 1; x >= 0; x--)
                    {
                        int i = (y * bitmapData.Stride) + (x * 4);

                        byte b = m_Data[i];
                        byte g = m_Data[i + 1];
                        byte r = m_Data[i + 2];
                        byte a = m_Data[i + 3];

                        m_ColorMap[x, y] = new FluidColor(a, r, g, b);
                    }
                }

                bitmap.UnlockBits(bitmapData);
            }
        }

        /// <summary>
        /// Checks if the x and y location is a valid pixel
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>True if value; otherwise false</returns>
        public bool ValidPixel(int x, int y)
        {
            return x >= 0 && y >= 0 && x < m_ColorMap.GetLength(0) && y < m_ColorMap.GetLength(1);
        }

        /// <summary>
        /// Gets a pixel at a x and y coordinate
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>The color of the block at the coordinate</returns>
        public FluidColor this[int x, int y]
        {
            get { return GetPixel(x, y); }
        }

        /// <summary>
        /// Gets a pixel at a x and y coordinate
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>The color of the block at the coordinate</returns>
        public FluidColor GetPixel(int x, int y)
        {
            if (!ValidPixel(x, y))
                return null;

            return m_ColorMap[x, y];
        }

        public void Dispose()
        {
            image.Dispose();
        }

        public BlockImage(Image image)
        {
            this.image = image;
            Read();
        }

        public BlockImage(string fileName)
        {
            this.image = Image.FromFile(fileName);
            Read();
        }

        public BlockImage(FluidColor[,] map)
        {
            m_ColorMap = map;
        }
    }
}
