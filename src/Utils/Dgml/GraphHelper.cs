using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Dgml
{
    public static class GraphHelper
    {
        /// <summary>
        /// Set of known nodes
        /// </summary>
        private static HashSet<Node> Nodes { get; set; }

        /// <summary>
        /// Set of known links
        /// </summary>
        private static HashSet<Link> Links { get; set; }

        /// <summary>
        /// Adds a new node
        /// </summary>
        /// <param name="node">Instance of <see cref="Node"/></param>
        public static void AddNode(Node node)
        {
            if (Nodes == null)
            {
                throw new InvalidOperationException("GraphHelper.Initialize() must be called first.");
            }

            Nodes.Add(node);
        }

        /// <summary>
        /// Adds a new link
        /// </summary>
        /// <param name="link">Instance of <see cref="Link"/></param>
        public static void AddLink(Link link)
        {
            if (Links == null)
            {
                throw new InvalidOperationException("GraphHelper.Initialize() must be called first.");
            }

            Links.Add(link);
        }

        /// <summary>
        /// Saves the dgml graph to given file path
        /// </summary>
        /// <param name="filePath"></param>
        public static void Save(string filePath)
        {
            Graph g = new Graph()
            {
                Nodes = Nodes.ToArray(),
                Links = Links.ToArray(),
            };

            g.Save(filePath);
        }

        /// <summary>
        /// Initialized the <see cref="GraphHelper"/>
        /// </summary>
        public static void Initialize()
        {
            Nodes = new HashSet<Node>();
            Links = new HashSet<Link>();
        }
    }
}
