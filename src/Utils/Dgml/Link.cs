using System.Xml.Serialization;

namespace Utils.Dgml
{
    public struct Link
    {
        /// <summary>
        /// Id for source node
        /// </summary>
        [XmlAttribute]
        public string Source;

        /// <summary>
        /// Id for target node
        /// </summary>
        [XmlAttribute]
        public string Target;

        /// <summary>
        /// Label for the link
        /// </summary>
        [XmlAttribute]
        public string Label;

        /// <summary>
        /// Creates a new <see cref="Link"/>
        /// </summary>
        /// <param name="source">Source node</param>
        /// <param name="target">Target node</param>
        /// <param name="label">Label for link</param>
        public Link(string source, string target, string label = null)
        {
            Source = source;
            Target = target;
            Label = label ?? string.Empty;
        }
    }
}
