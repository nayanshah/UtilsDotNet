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
            string file = Path.GetTempFileName();
            graph.Save(file);

            Assert.IsTrue(File.Exists(file));
            Assert.AreEqual(EmptyDgml, File.ReadAllText(file));

            File.Delete(file);
        }

        //[TestMethod]
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

            string file = Path.GetTempFileName();
            graph.Save(file);

            Assert.IsTrue(File.Exists(file));
            Assert.AreEqual(ValidDgml, File.ReadAllText(file));

            File.Delete(file);
        }

        private const string EmptyDgml =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<DirectedGraph xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/vs/2009/dgml"" />";

        private const string ValidDgml =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<DirectedGraph xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/vs/2009/dgml"">
  <Nodes>
    <Node Id=""1"" Label=""first"" />
    <Node Id=""2"" Label=""second"" />
  </Nodes>
  <Links>
    <Link Source=""1"" Target=""2"" Label=""connects"" />
  </Links>
</DirectedGraph>";

    }
}
