namespace Fluid
{
    public class LobbyProperties
    {
        /// <summary>
        /// Gets whether this is the first daily login
        /// </summary>
        public bool FirstDailyLogin { get; set; }

        /// <summary>
        /// Creates a new lobby properties
        /// </summary>
        /// <param name="firstDailyLogin">Is this is first daily login</param>
        public LobbyProperties(bool firstDailyLogin)
        {
            FirstDailyLogin = firstDailyLogin;
        }
    }
}
