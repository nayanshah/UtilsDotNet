using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Dgml;

namespace UtilsTest.Dgml
{
    [TestClass]
    public class GraphTest
    {
        //[TestMethod]
        public void TestSave_EmptyDgml()
        {
            Graph graph = new Graph();
            string actualDgml = graph.ToDgml();

            Assert.AreEqual(EmptyDgml, actualDgml);
        }

        [TestMethod]
        public void TestSave_ValidDgml()
        {
            Node first = new Node("1", "first");
            Node second = new Node("2", "second");
            Link link = new Link("1", "2", "connects");

            Graph graph = new Graph()
            {
                Nodes = new[] { first, second },
                Links = new[] { link },
            };

            string actualDgml = graph.ToDgml();

            Assert.AreEqual(ValidDgml, actualDgml);
        }

        [TestMethod]
        public void TestSave_StyledDgml()
        {
            Node first = new Node("1", "first", "category");
            Node second = new Node("2", "second");
            Link link = new Link("1", "2", "connects");
            Style style = new Style("category", "#FF000000");

            Graph graph = new Graph()
            {
                Nodes = new[] { first, second },
                Links = new[] { link },
                Styles = new[] { style },
            };

            string actualDgml = graph.ToDgml();

            Assert.AreEqual(StyledDgml, actualDgml);
        }

        private const string EmptyDgml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<DirectedGraph xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/vs/2009/dgml"" />";

        private const string ValidDgml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<DirectedGraph xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/vs/2009/dgml"">
  <Nodes>
    <Node Id=""1"" Label=""first"" />
    <Node Id=""2"" Label=""second"" />
  </Nodes>
  <Links>
    <Link Source=""1"" Target=""2"" Label=""connects"" />
  </Links>
</DirectedGraph>";

        private const string StyledDgml =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<DirectedGraph xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/vs/2009/dgml"">
  <Nodes>
    <Node Id=""1"" Label=""first"" Category=""category"" />
    <Node Id=""2"" Label=""second"" />
  </Nodes>
  <Links>
    <Link Source=""1"" Target=""2"" Label=""connects"" />
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
