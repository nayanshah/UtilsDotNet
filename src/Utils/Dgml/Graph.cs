using System.IO;
using System.Text;
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
        /// List of links
        /// </summary>
        public Style[] Styles;

        /// <summary>
        /// Saves the graph to given file in dgml format
        /// </summary>
        /// <param name="filePath">File path</param>
        public void Save(string filePath)
        {
            File.WriteAllText(filePath, ToDgml());
        }

        /// <summary>
        /// Returns the dgml representation of the graph
        /// </summary>
        /// <returns>Graph as dgml string</returns>
        public string ToDgml()
        {
            XmlRootAttribute root = new XmlRootAttribute("DirectedGraph")
            {
                Namespace = "http://schemas.microsoft.com/vs/2009/dgml",
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Graph), root);
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = Encoding.UTF8,
            };

            StringWriter dgml = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(dgml, settings))
            {
                serializer.Serialize(xmlWriter, this);
            }

            return dgml.ToString();
        }
    }
}
