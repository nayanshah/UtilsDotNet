using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Utils.Dgml
{
    public partial class DirectedGraph
    {
        /// <summary>
        /// Saves the graph to given file in dgml format
        /// </summary>
        /// <param name="filePath">File path</param>
        public void Save(string filePath)
        {
            using (TextWriter fileWriter = new StreamWriter(File.OpenWrite(filePath)))
            {
                Serialize(fileWriter);
            }
        }

        public string ToDgml()
        {
            using (StringWriter dgml = new Graph.Utf8StringWriter())
            {
                Serialize(dgml);
                return dgml.ToString();
            }
        }

        public void Serialize(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DirectedGraph));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                serializer.Serialize(xmlWriter, this);
            }
        }
    }
}