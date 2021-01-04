using SM.Crawler.Libs.Crawling.ObjectMapping.Base;

namespace SM.Libs.Crawling.ObjectMapping
{
    public class TextExpression : ExpressionBase
    {
        public TextExpression()
        {
        }

        public override object Map(MappingContext context)
        {
            var nodes = context.Container.SelectSingleNode(Expression);
            return nodes?.InnerText;
        }
    }

}
