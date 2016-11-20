using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Dgml;
using Utils.Dgml.Extensions;
using Utils.Graph.Extensions;

namespace UtilsTest.Dgml
{
    [TestClass]
    public class GraphCreatorTest
    {
        [TestMethod]
        public void TestToGraph_ValidGraph()
        {
            IDictionary<string, ICollection<string>> inputGraph = new Dictionary<string, ICollection<string>>
            {
                { "node-1", new string[] { "node-2", "node-3", } },
                { "node-2", new string[] { "node-4", } },
                { "node-3", new string[] { "node-4", "node-5" } },
                { "node-4", new string[] { } },
                { "node-5", new string[] { } },
            };

            Graph graph = CreateGraph(inputGraph);

            Assert.IsNotNull(graph);
            MatchGraph(inputGraph, graph);
        }

        [TestMethod]
        public void TestToGraph_ExternalLinks_Ignored()
        {
            IDictionary<string, ICollection<string>> inputGraph = new Dictionary<string, ICollection<string>>
            {
                { "node-1", new string[] { "node-2", "node-3", } },
                { "node-2", new string[] { } },
            };

            Graph graph = CreateGraph(inputGraph);

            IDictionary<string, ICollection<string>> expectedGraph = new Dictionary<string, ICollection<string>>
            {
                { "node-1", new string[] { "node-2", } },
                { "node-2", new string[] { } },
            };

            Assert.IsNotNull(graph);
            MatchGraph(expectedGraph, graph);
        }

        [TestMethod]
        public void TestToGraph_IncomingLinks()
        {
            IDictionary<string, ICollection<string>> inputGraph = new Dictionary<string, ICollection<string>>
            {
                { "node-1", new string[] { "node-2", } },
                { "node-2", new string[] { } },
            };

            Graph graph = CreateGraph(inputGraph, true);

            IDictionary<string, ICollection<string>> expectedGraph = new Dictionary<string, ICollection<string>>
            {
                { "node-1", new string[] { } },
                { "node-2", new string[] { "node-1", } },
            };

            Assert.IsNotNull(graph);
            MatchGraph(expectedGraph, graph);
        }

        private Graph CreateGraph(IDictionary<string, ICollection<string>> inputGraph, bool incomingLinks = false)
        {
            IList<CustomNode> nodes = inputGraph.Keys.Select(node => new CustomNode { Name = node }).ToList();

            NodeLinksResolver<CustomNode> resolver =
                node => inputGraph[node.Name]
                            .Select(
                               link => nodes.SingleOrDefault(n => link == n.Name) ?? new CustomNode { Name = link });

            NodeCreator<CustomNode> creator = node => new Node(node.Name);

            return nodes.ToGraph(creator, resolver, incomingLinks);
        }

        private void MatchGraph(IDictionary<string, ICollection<string>> expectedGraph, Graph actualGraph)
        {
            Assert.AreEqual(expectedGraph.Keys.Count, actualGraph.Nodes.Length);
            Assert.AreEqual(expectedGraph.Values.SelectMany(v => v).Count(), actualGraph.Links.Length);

            foreach (string nodeName in expectedGraph.Keys)
            {
                Assert.IsNotNull(actualGraph.Nodes.Single(n => n.Id == nodeName));
                foreach (string link in expectedGraph[nodeName])
                {
                    Assert.IsNotNull(actualGraph.Links.Single(node => node.Source == nodeName && node.Target == link));
                }
            }
        }

        private class CustomNode
        {
            public string Name;
        }
    }
}
