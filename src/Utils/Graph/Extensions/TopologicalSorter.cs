using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Graph.Extensions
{
    public delegate IEnumerable<T> NodeLinksResolver<T>(T node);

    public static class TopologicalSorterDfs
    {
        public static IEnumerable<T> TopologicalSort<T>(this ICollection<T> list, NodeLinksResolver<T> linkResolver)
        {
            return list.TopologicalSortReverse(linkResolver).Reverse();
        }

        public static IEnumerable<T> TopologicalSortReverse<T>(this ICollection<T> list, NodeLinksResolver<T> linkResolver)
        {
            TopologicalSortState<T> state = new TopologicalSortState<T>
            {
                Unmarked = new HashSet<T>(list),
                GetLinks = linkResolver,
            };

            while (state.Unmarked.Count > 0)
            {
                Visit(state.Unmarked.First(), state);
            }

            return state.SortedNodes;
        }

        internal static void Visit<T>(T node, TopologicalSortState<T> state)
        {
            if (state.TemporaryMarked.Contains(node))
            {
                throw new InvalidOperationException("Not a DAG");
            }

            if (!state.Unmarked.Contains(node))
            {
                return;
            }

            state.TemporaryMarked.Add(node);
            foreach (T link in state.GetLinks(node))
            {
                Visit(link, state);
            }

            state.SortedNodes.Add(node);
            state.TemporaryMarked.Remove(node);
            state.Unmarked.Remove(node);
        }
    }

    internal class TopologicalSortState<T>
    {
        public IList<T> SortedNodes { get; set; }

        public ISet<T> Unmarked { get; set; }

        public ISet<T> TemporaryMarked { get; set; }

        public NodeLinksResolver<T> GetLinks { get; set; }

        public TopologicalSortState()
        {
            SortedNodes = new List<T>();
            TemporaryMarked = new HashSet<T>();
        }
    }
}