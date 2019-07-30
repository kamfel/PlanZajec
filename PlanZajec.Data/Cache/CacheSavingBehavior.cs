namespace PlanZajec.Data
{
    /// <summary>
    /// Describes how the cache should save to file
    /// </summary>
    public enum CacheSavingBehavior
    {
        /// <summary>
        /// Should not save to file
        /// </summary>
        None,

        /// <summary>
        /// Should save upon every modification
        /// </summary>
        Modification,

        /// <summary>
        /// Should save to file in regular intervals
        /// </summary>
        Interval,

        /// <summary>
        /// Should save to file upon closure
        /// </summary>
        Closure
    }
}
