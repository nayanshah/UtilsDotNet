using System.Xml.Serialization;

namespace Utils.Dgml
{
    public struct Category
    {
        [XmlAttribute]
        public string Id;

        [XmlAttribute]
        public string Background;

        [XmlAttribute]
        public string Stroke;

        [XmlAttribute]
        public string BasedOn;

        /// <summary>
        /// Creates a new <see cref="Category"/> object
        /// </summary>
        /// <param name="id">Category id</param>
        /// <param name="background">Background for given category</param>
        /// <param name="Stoke">Color for the link</param>
        /// <param name="basedOn">Inherit properties from given category</param>
        public Category(string id, string background = null, string stroke = null, string basedOn = null)
        {
            Id = id;
            Background = background;
            Stroke = stroke;
            BasedOn = basedOn;
        }
    }
}
