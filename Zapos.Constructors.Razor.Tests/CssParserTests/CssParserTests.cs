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
        [DeploymentItem(@".\Content", "Content")]
        public void TestCssParcer()
        {
            var parser = new CssParser();
            var data = new StreamReader(@"Content\SimpleStyles.css").ReadToEnd();
            var classes = parser.ParceCss(data);

            BaseStyle temp;

            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.TryGetValue("head", out temp));
            Assert.IsTrue(classes.TryGetValue("odd", out temp));
            Assert.IsTrue(classes.TryGetValue("value", out temp));
            Assert.IsTrue(classes.TryGetValue("name", out temp));
        }
    }
}