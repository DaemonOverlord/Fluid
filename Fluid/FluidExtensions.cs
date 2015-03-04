using Fluid.Blocks;

namespace Fluid
{
    public static class FluidExtensions
    {
        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this int i)
        {
            return (BlockID)i;
        }

        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this short i)
        {
            return (BlockID)i;
        }

        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this uint i)
        {
            return (BlockID)i;
        }

        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this long i)
        {
            return (BlockID)i;
        }

        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this byte i)
        {
            return (BlockID)i;
        }

        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this sbyte i)
        {
            return (BlockID)i;
        }

        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this ulong i)
        {
            return (BlockID)i;
        }

        /// <summary>
        /// Converts the number to a block id
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>The block id</returns>
        public static BlockID ToID(this ushort i)
        {
            return (BlockID)i;
        }
    }
}
