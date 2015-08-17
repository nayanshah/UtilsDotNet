using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Logging;

namespace Utils.Gates
{
    public class GateKeeper<T>
    {
        /// <summary>
        /// Success function for given type
        /// </summary>
        private Func<T, bool> IsSuccess { get; set; }

        /// <summary>
        /// Close gates in reverse order, default: True
        /// </summary>
        public bool Retreat { get; set; }

        /// <summary>
        /// Controls gate closing behaviour
        /// </summary>
        public GateCloseOptions GateCloseBehavior { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="GateKeeper{T}"/>
        /// </summary>
        /// <param name="successFunc">Success function for given type of value</param>
        public GateKeeper(Func<T, bool> successFunc)
        {
            IsSuccess = successFunc;
            Retreat = true;
            GateCloseBehavior = GateCloseOptions.AfterAllOpen;
        }

        /// <summary>
        /// Checks if each gate is open and then closes them
        /// </summary>
        /// <param name="gates">List of gates</param>
        /// <param name="returnValue">Return value</param>
        /// <returns>True if passage was successful</returns>
        public bool TryPassThrough(IEnumerable<IGate<T>> gates, out T returnValue)
        {
            returnValue = default(T);
            bool success = true;

            IList<IGate<T>> openedGates = new List<IGate<T>>();
            // Check if all gates are open
            foreach (IGate<T> gate in gates)
            {
                returnValue = gate.IsOpen();
                if (!IsSuccess(returnValue))
                {
                    LogHelper.LogError(gate.Description);
                    success = false;
                    break;
                }

                openedGates.Add(gate);
                if (GateCloseBehavior == GateCloseOptions.Simultaneously)
                {
                    if (!CloseGate(gate, ref returnValue))
                    {
                        return false;
                    }
                }
            }

            if (!success && GateCloseBehavior == GateCloseOptions.AfterAllOpen)
            {
                return false;
            }

            IEnumerable<IGate<T>> gatesToClose = (Retreat ? openedGates.Reverse() : openedGates);
            foreach (IGate<T> gate in gatesToClose)
            {
                if (!CloseGate(gate, ref returnValue))
                {
                    return false;
                }
            }

            return success;
        }

        /// <summary>
        /// Closes the gate and updates return value on failure
        /// </summary>
        /// <param name="gate">Gate to close</param>
        /// <param name="returnValue">Return value</param>
        /// <returns>True if closed successfully</returns>
        private bool CloseGate(IGate<T> gate, ref T returnValue)
        {
            T value = gate.Close();
            if (!IsSuccess(value))
            {
                // Makes sure return value has the last occured failure
                returnValue = value;
                LogHelper.LogError("Failed to close gate '{0}'", gate.GetType().Name);
                return false;
            }
            return true;
        }
    }
}