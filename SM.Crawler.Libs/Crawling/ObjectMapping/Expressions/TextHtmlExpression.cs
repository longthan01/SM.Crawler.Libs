using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Libs.Crawling.ObjectMapping;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Expressions
{
    public class TextHtmlExpression : ExpressionBase
    {
        public override object Map(MappingContext context)
        {
            var nodes = context.Container.SelectSingleNode(Expression);
            return nodes?.InnerHtml;
        }
    }

}
