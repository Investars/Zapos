using System.IO;

using NUnit.Framework;

using Zapos.Common.Styles;
using Zapos.Constructors.Razor.Parsers;

namespace Zapos.Constructors.Razor.Tests.CssParserTests
{
    [TestFixture]
    public class CssParserTests
    {
        [Test]
        public void TestCssParcer()
        {
            var content = File.ReadAllText(Path.Combine("Content", "SimpleStyles.css"));

            var parser = new CssParser();
            var classes = parser.ParceCss(content);

            BaseStyle temp;

            Assert.NotNull(classes);
            Assert.IsTrue(classes.TryGetValue("head", out temp));
            Assert.IsTrue(classes.TryGetValue("odd", out temp));
            Assert.IsTrue(classes.TryGetValue("value", out temp));
            Assert.IsTrue(classes.TryGetValue("name", out temp));
        }
    }
}