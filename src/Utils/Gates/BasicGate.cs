using System;

namespace Utils.Gates
{
    public class BasicGate : IGate<bool>
    {
        /// <summary>
        /// Description about the gate shown to the user when it's locked
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Method to open the gate
        /// </summary>
        public Func<bool> OnOpen { private get; set; }

        /// <summary>
        /// Method to close the gate
        /// </summary>
        public Func<bool> OnClose { private get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BasicGate"/>
        /// </summary>
        public BasicGate()
        {
            OnOpen = () => true;
            OnClose = () => true;
            Description = "Basic gate";
        }

        /// <summary>
        /// Checks if the gate is open
        /// </summary>
        /// <returns>True if opened successfully</returns>
        public bool IsOpen()
        {
            return OnOpen();
        }

        /// <summary>
        /// Action or cleanup to be performed after gate is opened
        /// </summary>
        /// <returns>True if closed successfully</returns>
        public bool Close()
        {
            return OnClose();
        }
    }
}