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
