namespace Utils.Gates
{
    public class MockGate : IGate<bool>
    {
        /// <summary>
        /// Mock instance of <see cref="IGate{T}"/> for unit testing
        /// </summary>
        public string Description
        {
            get { return string.Format("Mock gate [CanOpen: {0}; CanClose: {1}]", CanOpen, CanClose); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="MockGate"/>
        /// </summary>
        public MockGate()
        {
            CanOpen = true;
            CanClose = true;
        }

        /// <summary>
        /// Sets whether gate can be opened
        /// </summary>
        public bool CanOpen { get; set; }

        /// <summary>
        /// Sets whether gate can be closed
        /// </summary>
        public bool CanClose { get; set; }

        /// <summary>
        /// Whether <see cref="IsOpen"/> has been called
        /// </summary>
        public bool OpenCalled { get; private set; }

        /// <summary>
        /// Whether <see cref="Close"/> has been called
        /// </summary>
        public bool CloseCalled { get; private set; }

        /// <summary>
        /// Checks if the gate is open
        /// </summary>
        /// <returns><see cref="CanOpen"/></returns>
        public bool IsOpen()
        {
            OpenCalled = true;
            return CanOpen;
        }

        /// <summary>
        /// Action or cleanup to be performed after gate is opened
        /// </summary>
        /// <returns><see cref="CanClose"/></returns>
        public bool Close()
        {
            CloseCalled = true;
            return CanClose;
        }
    }
}