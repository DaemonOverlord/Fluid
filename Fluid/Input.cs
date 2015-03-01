using System;
namespace Fluid
{
    [Flags]
    public enum Input : uint
    {
        /// <summary>
        /// No input
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Holding left
        /// </summary>
        HoldLeft = 1 << 0,

        /// <summary>
        /// Holding up
        /// </summary>
        HoldUp = 1 << 1,

        /// <summary>
        /// Holding right
        /// </summary>
        HoldRight = 1 << 2,

        /// <summary>
        /// Holding down
        /// </summary>
        HoldDown = 1 << 3,

        /// <summary>
        /// Holding space
        /// </summary>
        HoldSpace = 1 << 4,

        /// <summary>
        /// Releasing left
        /// </summary>
        ReleaseLeft = 1 << 5,

        /// <summary>
        /// Releasing up
        /// </summary>
        ReleaseUp = 1 << 6,

        /// <summary>
        /// Releasing right
        /// </summary>
        ReleaseRight = 1 << 7,

        /// <summary>
        /// Releasing down
        /// </summary>
        ReleaseDown = 1 << 8,

        /// <summary>
        /// Releasing space
        /// </summary>
        ReleaseSpace = 1 << 9

    }
}
