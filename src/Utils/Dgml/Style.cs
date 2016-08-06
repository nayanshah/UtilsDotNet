using System.Xml.Serialization;

namespace Utils.Dgml
{
    public struct Style
    {
        [XmlAttribute]
        public string TargetType;

        public StyleCondition Condition;

        public StyleSetter Setter;

        /// <summary>
        /// Creates a new <see cref="Style"/> object
        /// </summary>
        /// <param name="category">Category id</param>
        /// <param name="color">Color for the category</param>
        public Style(string category, string color)
        {
            TargetType = "Node";
            Condition = new StyleCondition
            {
                Expression = string.Format("HasCategory('{0}')", category),
            };

            Setter = new StyleSetter
            {
                Value = color,
            };
        }

        public class StyleCondition
        {
            [XmlAttribute]
            public string Expression;
        }

        public class StyleSetter
        {
            [XmlAttribute]
            public string Property = "Background";

            [XmlAttribute]
            public string Value;
        }
    }
}
