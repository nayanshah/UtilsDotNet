using System.Xml;
using System.Xml.Serialization;

namespace Utils.Dgml
{
    public struct Graph
    {
        /// <summary>
        /// List of nodes
        /// </summary>
        public Node[] Nodes;

        /// <summary>
        /// List of links
        /// </summary>
        public Link[] Links;

        /// <summary>
        /// Saves the graph to given file in dgml format
        /// </summary>
        /// <param name="filePath">File path</param>
        public void Save(string filePath)
        {
            XmlRootAttribute root = new XmlRootAttribute("DirectedGraph")
            {
                Namespace = "http://schemas.microsoft.com/vs/2009/dgml"
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Graph), root);
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(filePath, settings))
            {
                serializer.Serialize(xmlWriter, this);
            }
        }
    }
}
