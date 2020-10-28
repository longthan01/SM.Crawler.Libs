using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Libs.Crawling.ObjectMapping;
using System;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Expressions
{
    public class ArrayExpression : ExpressionBase
    {
        public Type TargetType { get; }
        public IMappingExpression ElementExpression { get; private set; }
        public ArrayExpression(Type targetType, IMappingExpression elementExpression)
        {
            TargetType = targetType;
            ElementExpression = elementExpression;
        }
        public override object Map(MappingContext context)
        {
            var nodes = context.Container.SelectNodes(Expression);
            Array array = Array.CreateInstance(TargetType, nodes.Count);
            for (int i = 0; i < nodes.Count; i++)
            {
                var value = ElementExpression.Map(new MappingContext()
                {
                    Container = nodes[i]
                });
                array.SetValue(Convert.ChangeType(value, TargetType), i);
            }
            return array;
        }
    }

}
