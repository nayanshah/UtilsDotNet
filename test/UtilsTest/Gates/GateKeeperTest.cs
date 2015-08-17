using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Gates;

namespace UtilsTest.Gates
{
    [TestClass]
    public class GateKeeperTest
    {
        /// <summary>
        /// List of mock gates for testing
        /// </summary>
        private IList<MockGate> MockGates { get; set; }

        /// <summary>
        /// Instance of <see cref="GateKeeper{T}"/>
        /// </summary>
        private GateKeeper<bool> GateKeeper { get; set; }

        [TestInitialize]
        public void Setup()
        {
            GateKeeper = new GateKeeper<bool>(v => v);
            MockGates = new List<MockGate>
            {
                new MockGate { CanClose = false },
                new MockGate(),
                new MockGate { CanOpen = false },
            };
        }

        [TestMethod]
        public void TestGateKeeper_TryPassThrough_AfterAllOpen()
        {
            IList<Tuple<bool, bool>> expected = new[]
            {
                new Tuple<bool, bool>(true, false),
                new Tuple<bool, bool>(true, false),
                new Tuple<bool, bool>(true, false),
            };

            CheckResults(expected);
        }

        [TestMethod]
        public void TestGateKeeper_TryPassThrough_Success()
        {
            GateKeeper = new GateKeeper<bool>(v => v);
            MockGates = new List<MockGate>
            {
                new MockGate(),
                new MockGate(),
                new MockGate(),
            };

            bool value;
            Assert.IsTrue(GateKeeper.TryPassThrough(MockGates, out value));
            Assert.IsTrue(value);

            foreach (MockGate gate in MockGates)
            {
                Assert.IsTrue(gate.OpenCalled);
                Assert.IsTrue(gate.CloseCalled);
            }
        }

        [TestMethod]
        public void TestGateKeeper_TryPassThrough_OpenedOnly()
        {
            IList<Tuple<bool, bool>> expected = new[]
            {
                new Tuple<bool, bool>(true, true),
                new Tuple<bool, bool>(true, true),
                new Tuple<bool, bool>(true, false),
            };

            GateKeeper.GateCloseBehavior = GateCloseOptions.OpenedOnly;

            CheckResults(expected);
        }

        [TestMethod]
        public void TestGateKeeper_TryPassThrough_OpenedOnly_NoRetreat()
        {
            IList<Tuple<bool, bool>> expected = new[]
            {
                new Tuple<bool, bool>(true, true),
                new Tuple<bool, bool>(true, false),
                new Tuple<bool, bool>(true, false),
            };

            GateKeeper.GateCloseBehavior = GateCloseOptions.OpenedOnly;
            GateKeeper.Retreat = false;

            CheckResults(expected);
        }

        [TestMethod]
        public void TestGateKeeper_TryPassThrough_Simultaneously()
        {
            IList<Tuple<bool, bool>> expected = new[]
            {
                new Tuple<bool, bool>(true, true),
                new Tuple<bool, bool>(false, false),
                new Tuple<bool, bool>(false, false),
            };

            GateKeeper.GateCloseBehavior = GateCloseOptions.Simultaneously;

            CheckResults(expected);
        }

        public void CheckResults(IList<Tuple<bool, bool>> expected)
        {
            bool value;
            Assert.IsFalse(GateKeeper.TryPassThrough(MockGates, out value));
            Assert.IsFalse(value);

            for (int i = 0; i < MockGates.Count; i++)
            {
                Assert.AreEqual(expected[i].Item1, MockGates[i].OpenCalled);
                Assert.AreEqual(expected[i].Item2, MockGates[i].CloseCalled);
            }
        }
    }
}