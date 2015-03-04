using System.Diagnostics;

namespace Fluid
{
    [DebuggerDisplay("WorldID = {WorldID}")]
    public class WorldReference
    {
        private FluidClient m_Client;

        /// <summary>
        /// Gets the world id
        /// </summary>
        public string WorldID { get; private set; }

        /// <summary>
        /// Loads the world
        /// </summary>
        /// <returns>The world if valid; otherwise null</returns>
        public World Load()
        {
            return m_Client.LoadWorld(WorldID);
        }

        /// <summary>
        /// Creates a new world reference
        /// </summary>
        /// <param name="client">The Fluid client</param>
        /// <param name="worldId">The world id</param>
        public WorldReference(FluidClient client, string worldId)
        {
            this.m_Client = client;
            WorldID = worldId;
        }
    }
}
