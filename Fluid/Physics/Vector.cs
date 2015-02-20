namespace Fluid.Physics
{
    public class Vector
    {
        /// <summary>
        /// Gets or sets the x component
        /// </summary>
        public double X { get; internal set; }

        /// <summary>
        /// Gets or sets the y component
        /// </summary>
        public double Y { get; internal set; }

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
