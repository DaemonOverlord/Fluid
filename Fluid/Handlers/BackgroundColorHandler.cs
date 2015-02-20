using Fluid.ServerEvents;
using PlayerIOClient;

namespace Fluid.Handlers
{
    public class BackgroundColorHandler : IMessageHandler
    {
        /// <summary>
        /// Gets the handled types
        /// </summary>
        public string[] HandleTypes
        {
            get { return new string[] { "backgroundColor" }; }
        }

        /// <summary>
        /// Processes the message
        /// </summary>
        /// <param name="connectionBase">The connection base</param>
        /// <param name="message">The playerio message</param>
        /// <param name="handled">Whether the message was already handled</param>
        public void Process(FluidConnectionBase connectionBase, Message message, bool handled)
        {
            uint argb = message.GetUInt(0);

            WorldConnection worldCon = (WorldConnection)connectionBase;
            FluidColor color = new FluidColor(argb);

            if (!handled)
            {
                worldCon.World.BackgroundColor = color;
            }

            BackgroundColorChangedEvent backgroundColorChangedEvent = new BackgroundColorChangedEvent()
            {
                Raw = message,
                Color = color
            };

            connectionBase.RaiseServerEvent<BackgroundColorChangedEvent>(backgroundColorChangedEvent);
        } 
    }
}
