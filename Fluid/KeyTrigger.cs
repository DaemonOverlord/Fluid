using Fluid.Room;

namespace Fluid
{
    public class KeyTrigger
    {
        /// <summary>
        /// Gets or sets the key
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// Creates a key trigger
        /// </summary>
        /// <param name="key">The key pressed</param>
        public KeyTrigger(Key key)
        {
            Key = key;
        }
    }
}
