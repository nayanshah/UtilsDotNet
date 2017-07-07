using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Dgml;

namespace UtilsTest.Dgml
{
    [TestClass]
    public class DirectedGraphTest
    {
        [TestMethod]
        public void TestSave_ValidDgml()
        {
            DirectedGraphNode first = new DirectedGraphNode { Id = "1", Label = "first" };
            DirectedGraphNode second = new DirectedGraphNode { Id = "2", Label = "second" };
            DirectedGraphLink link = new DirectedGraphLink { Source = "1", Target = "2", Label = "connects" };

            DirectedGraph graph = new DirectedGraph()
            {
                Nodes = new[] { first, second },
                Links = new[] { link },
            };

            string actualDgml = graph.ToDgml();

            MatchDgml(ValidDgml, actualDgml);
        }

        [TestMethod]
        public void TestSave_StyledDgml()
        {
            DirectedGraphNode first = new DirectedGraphNode { Id = "1", Label = "first", Category1 = "category", };
            DirectedGraphNode second = new DirectedGraphNode { Id = "2", Label = "second" };
            DirectedGraphLink link = new DirectedGraphLink { Source = "1", Target = "2", Label = "connects" };
            DirectedGraphStyle style = DgmlHelper.CreateCategoryBackgroundStyle("category", "#FF000000");

            DirectedGraph graph = new DirectedGraph
            {
                Nodes = new[] { first, second },
                Links = new[] { link },
                Styles = new[] { style },
            };

            string actualDgml = graph.ToDgml();

            MatchDgml(StyledDgml, actualDgml);
        }

        private void MatchDgml(string expected, string actual)
        {
            Assert.AreEqual(
                Regex.Replace(expected, @"\r|\n", ""),
                Regex.Replace(actual, @"\r|\n", "")
            );
        }

        private const string ValidDgml =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<DirectedGraph xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/vs/2009/dgml"">
  <Nodes>
    <Node Id=""1"" Label=""first"" />
    <Node Id=""2"" Label=""second"" />
  </Nodes>
  <Links>
    <Link Label=""connects"" Source=""1"" Target=""2"" />
  </Links>
</DirectedGraph>";

        private const string StyledDgml =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<DirectedGraph xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/vs/2009/dgml"">
  <Nodes>
    <Node Id=""1"" Category=""category"" Label=""first"" />
    <Node Id=""2"" Label=""second"" />
  </Nodes>
  <Links>
    <Link Label=""connects"" Source=""1"" Target=""2"" />
  </Links>
  <Styles>
    <Style TargetType=""Node"">
      <Condition Expression=""HasCategory('category')"" />
      <Setter Property=""Background"" Value=""#FF000000"" />
    </Style>
  </Styles>
</DirectedGraph>";

    }
}