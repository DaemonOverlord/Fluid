namespace Fluid
{
    public enum PotionState
    {
        /// <summary>
        /// The potion is active
        /// </summary>
        Active,
        
        /// <summary>
        /// The potion is suspended due to the world changing their policy
        /// </summary>
        Suspended,

        /// <summary>
        /// The potion is inactive
        /// </summary>
        Inactive
    }
}
