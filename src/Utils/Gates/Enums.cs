namespace Utils.Gates
{
    public enum GateCloseOptions
    {
        /// <summary>
        /// Close gates only if all of them are open
        /// </summary>
        AfterAllOpen = 0,

        /// <summary>
        /// Simultaneously open and then close gates before moving forward
        /// </summary>
        Simultaneously,

        /// <summary>
        /// Close the opened gates only, i.e. all before first locked one
        /// </summary>
        OpenedOnly,
    }
}