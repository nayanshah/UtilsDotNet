namespace Utils.Gates
{
    public interface IGate<out T>
    {
        /// <summary>
        /// Description about the gate shown to the user when it's locked
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Checks if the gate is open
        /// </summary>
        /// <returns>Instance of <see cref="T"/></returns>
        T IsOpen();

        /// <summary>
        /// Action or cleanup to be performed after gate is opened
        /// </summary>
        /// <remarks><see cref="GateKeeper{T}"/> can skip Close based on it's settings</remarks>
        /// <returns>Instance of <see cref="T"/></returns>
        T Close();
    }
}