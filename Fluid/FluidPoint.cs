using Fluid.Physics;

namespace Fluid
{
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
        /// Converts this point into a vector
        /// </summary>
        /// <returns>The vector</returns>
        public Vector ToVector()
        {
            return new Vector(X, Y);
        }

        /// <summary>
        /// Gets the point debug message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
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
