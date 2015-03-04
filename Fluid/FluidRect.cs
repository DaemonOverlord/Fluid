using System;
using System.Diagnostics;

namespace Fluid
{
    [DebuggerDisplay("X = {X}, Y = {Y}, Width = {Width}, Height = {Height}")]
    public class FluidRect
    {
        /// <summary>
        /// Gets or sets the x coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets the center of the rectangle
        /// </summary>
        /// <returns>The center point</returns>
        public FluidPoint Center()
        {
            return new FluidPoint(X + (int)Math.Round(Width / 2.0), Y + (int)Math.Round(Height / 2.0));
        }

        /// <summary>
        /// Checks if this rectangle intersects with another rectangle
        /// </summary>
        /// <param name="rect">The rectangle to check</param>
        /// <returns>True is they intersect; otherwise false</returns>
        public bool Intersects(FluidRect rect)
        {
            return (X >= rect.X && X <= rect.X + rect.Width) || (Y >= rect.Y && Y <= rect.Y + rect.Height);
        }

        /// <summary>
        /// Checks if a point is within this rectangle
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>True if the point is within this rectangle</returns>
        public bool Contains(FluidPoint point)
        {
            return point.X >= X && point.X <= X + Width && point.Y >= Y && point.Y <= Y + Height;
        }

        /// <summary>
        /// Creates a new rectangle
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public FluidRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new rectangle
        /// </summary>
        /// <param name="p">The location</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public FluidRect(FluidPoint p, int width, int height)
        {
            X = p.X;
            Y = p.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new rectangle
        /// </summary>
        /// <param name="p">The top-left location</param>
        /// <param name="e">The bottom-right location</param>
        public FluidRect(FluidPoint p, FluidPoint e)
        {
            X = Math.Min(p.X, e.X);
            Y = Math.Min(p.Y, e.Y);
            Width = Math.Max(p.X, e.X) - X;
            Height = Math.Max(p.Y, e.Y) - Y;
        }
    }
}
