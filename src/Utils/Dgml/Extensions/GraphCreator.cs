using System.Collections.Generic;
using System.Linq;
using Utils.Graph.Extensions;

namespace Utils.Dgml.Extensions
{
    public delegate Node NodeCreator<T>(T node);

    public static class GraphCreator
    {
        /// <summary>
        /// Creates a graph from given list of nodes. Links to nodes outside the collection are ignored
        /// </summary>
        /// <typeparam name="T">Custom node type</typeparam>
        /// <param name="list">List of custom nodes</param>
        /// <param name="nodeCreator">Delegate for creating <see cref="Node"/></param>
        /// <param name="linkResolver">Delegate for finding links for given node</param>
        /// <returns><see cref="Graph"/> object</returns>
        public static Graph ToGraph<T>(this IEnumerable<T> list, NodeCreator<T> nodeCreator, NodeLinksResolver<T> linkResolver)
        {
            IDictionary<T, Node> nodeCache = list.ToDictionary(node => node, node => nodeCreator(node));

            IEnumerable<Link> links =
                nodeCache.Keys.SelectMany(source =>
                    linkResolver(source)
                        .Where(nodeCache.ContainsKey)
                        .Select(target => new Link(nodeCache[source].Id, nodeCache[target].Id)));

            return new Graph
            {
                Nodes = nodeCache.Values.ToArray(),
                Links = links.ToArray(),
            };
        }
    }
}
