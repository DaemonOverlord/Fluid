using System;
namespace Fluid.Physics
{
    public class Vector
    {


        /// <summary>
        /// Gets or sets the x component
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y component
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Adds x and y components to a vector
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <returns>The resulting vector</returns>
        public Vector Add(double x, double y)
        {
            return new Vector(X + x, Y + y);
        }

        /// <summary>
        /// Adds a vector
        /// </summary>
        /// <param name="v">The vector</param>
        /// <returns>The resulting vector</returns>
        public Vector Add(Vector v)
        {
            return new Vector(v.X + X, v.Y + Y);
        }

        /// <summary>
        /// Subtracts x and y components from a vector
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <returns>The resulting vecotr</returns>
        public Vector Subtract(double x, double y)
        {
            return new Vector(X - x, Y - y);
        }

        /// <summary>
        /// Subtracts a vector
        /// </summary>
        /// <param name="v">The vector the subtract</param>
        /// <returns>The resulting vecotr</returns>
        public Vector Subtract(Vector v)
        {
            return new Vector(X - v.X, Y - v.Y);
        }

        /// <summary>
        /// Distance to another vector
        /// </summary>
        /// <param name="v">The vecotr</param>
        /// <returns>The distance</returns>
        public double DistanceTo(Vector v)
        {
            return Math.Sqrt(((X - v.X) * (X - v.X)) + ((Y - v.Y) * (Y - v.Y)));
        }

        /// <summary>
        /// Converts the vector into a point
        /// </summary>
        /// <returns>A truncated point</returns>
        public FluidPoint ToPoint()
        {
            return new FluidPoint((int)Math.Round(X), (int)Math.Round(Y));
        }
 
        /// <summary>
        /// Inverts the vector
        /// </summary>
        public void Invert()
        {
            X = -X;
            Y = -Y;
        }

        /// <summary>
        /// Tests if the X and Y components are zero
        /// </summary>
        public bool IsZero()
        {
            return X == 0 && Y == 0;
        }

        /// <summary>
        /// Tests if this vector is equal to another vector
        /// </summary>
        /// <param name="v">The vector to check</param>
        /// <returns>True if equal; otherwise false</returns>
        public bool Equals(Vector v)
        {
            return v.X == X && v.Y == Y;
        }

        /// <summary>
        /// Gets the vector debug message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        /// <summary>
        /// Creates a new vector
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
