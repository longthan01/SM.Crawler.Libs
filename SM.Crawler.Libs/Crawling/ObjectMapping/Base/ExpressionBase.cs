using SM.Libs.Crawling.ObjectMapping;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Base
{
    public abstract class ExpressionBase : IMappingExpression
    {
        private string _expression;
        public string Expression
        {
            get
            {
                if (string.IsNullOrEmpty(_expression))
                {
                    return ".";
                }

                return _expression;
            }
            set => _expression = value;
        }

        public abstract object Map(MappingContext context);
    }
}
