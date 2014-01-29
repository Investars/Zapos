using System.Collections.Generic;
using System.Linq;

using ExCSS;
using Zapos.Common.Styles;

namespace Zapos.Constructors.Razor.Parsers
{
    public class CssParser
    {
        public static IDictionary<string, BaseStyle> ParceCss(string content)
        {
            var parser = new StylesheetParser();

            var styles = parser.Parse(content);
            var rules = styles.RuleSets.ToArray();

            var result = rules.ToDictionary(
                rule => rule.Selectors.First().SimpleSelectors.First().Class,
                rule => new StyleFactory(rule.Declarations).Style);

            return result;
        }
    }
}
