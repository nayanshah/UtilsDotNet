namespace Utils.Dgml
{
    public class DgmlHelper
    {
        public static DirectedGraphStyle CreateCategoryBackgroundStyle(string category, string background)
        {
            return new DirectedGraphStyle
            {
                Condition = new DirectedGraphStyleCondition { Expression = $"HasCategory('{category}')" },
                Setter = new[] { new DirectedGraphStyleSetter { Property = PropertyType.Background, Value = background }, }
            };
        }
    }
}