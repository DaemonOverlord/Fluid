using System;

namespace Fluid
{
    [Flags]
    public enum AccessLevel : uint
    {
        /// <summary>
        /// Has no priveledges
        /// </summary>
        None = 1 << 0,

        /// <summary>
        /// Has owner priveledges
        /// </summary>
        Owner = 1 << 1,

        /// <summary>
        /// Has edit priveledges
        /// </summary>
        Edit = 1 << 2,

        /// <summary>
        /// Has kicking and command priveledges
        /// </summary>
        Guardian = 1 << 3
    }
}
