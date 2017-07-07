using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Dgml;
using Utils.Dgml.Extensions;
using Utils.Graph.Extensions;

namespace UtilsTest.Dgml
{
    [TestClass]
    public class DirectedGraphCreatorTest
    {
        public class CustomNode
        {
            public string Name { get; set; }

            public ICollection<CustomNode> InputNodes { get; set; }

            public CustomNode()
            {
                InputNodes = new List<CustomNode>();
            }
        }

        [TestMethod]
        public void TestCreateDirectedGraph()
        {

            IList<CustomNode> nodes = new List<CustomNode>
            {
                new CustomNode { Name = "First", },
                new CustomNode { Name = "Second" },
                new CustomNode { Name = "Third" },
            };

            nodes[0].InputNodes = new[] { nodes[1], nodes[2] };


            DirectedGraphNodeCreator<CustomNode> creator = node => new DirectedGraphNode { Id = node.Name };
            NodeLinksResolver<CustomNode> resolver = node => node.InputNodes;

            DirectedGraph graph = nodes.ToGraph(creator, resolver, incomingLinks: true);
            Assert.IsNotNull(graph.ToDgml());
        }
    }
}