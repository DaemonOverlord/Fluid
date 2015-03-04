using Fluid.Physics;
using System;
using System.Diagnostics;

namespace Fluid
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public class FluidPoint
    {
        /// <summary>
        /// Gets the x component
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets the y component
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets the distance to another point
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns>The distance</returns>
        public double DistanceTo(FluidPoint p)
        {
            return Math.Sqrt(((X - p.X) * (X - p.X)) + ((Y - p.Y) * (Y - p.Y)));
        }

        /// <summary>
        /// Converts this point into a vector
        /// </summary>
        /// <returns>The vector</returns>
        public Vector ToVector()
        {
            return new Vector(X, Y);
        }

        /// <summary>
        /// Checks if this point is equal to another point
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns>True if equal; otherwise false</returns>
        public bool Equals(FluidPoint p)
        {
            return X == p.X && Y == p.Y;
        }

        /// <summary>
        /// Creates a new point
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        public FluidPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
