using Fluid.Blocks;

namespace Fluid
{
    public class KeyPress : KeyTrigger
    {
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Creates new key press
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="userId">The user id</param>
        public KeyPress(Key key, int userId) : base(key)
        {
            UserId = userId;
        }
    }
}
