using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Zapos.Common.Styles;
using Zapos.Constructors.Razor.Parsers;

namespace Zapos.Constructors.Razor.Tests.CssParserTests
{
    [TestClass]
    public class CssParserTests
    {
        [TestMethod]
        public void TestCssParcer()
        {
            var content = File.ReadAllText(Path.Combine("Content", "SimpleStyles.css"));

            var parser = new CssParser();
            var classes = parser.ParceCss(content);

            BaseStyle temp;

            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.TryGetValue("head", out temp));
            Assert.IsTrue(classes.TryGetValue("odd", out temp));
            Assert.IsTrue(classes.TryGetValue("value", out temp));
            Assert.IsTrue(classes.TryGetValue("name", out temp));
        }
    }
}