using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Libs.Crawling.ObjectMapping;
using SM.Libs.Utils;
using System;
using System.Collections.Generic;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Expressions
{
    public class ObjectExpression : ExpressionBase, IMappingExpression
    {
        private readonly Dictionary<string, IMappingExpression> _propertyExpressions;
        private readonly Type _targetType;

        public ObjectExpression(Type targetType, Dictionary<string, IMappingExpression> propertyExpressions)
        {
            _targetType = targetType;
            _propertyExpressions = propertyExpressions;
        }
        public override object Map(MappingContext Context)
        {
            object result = Activator.CreateInstance(_targetType);
            var rootNode = Context.Container.SelectSingleNode(Expression);
            foreach (var exp in _propertyExpressions)
            {
                var propertyName = exp.Key;
                var expression = exp.Value;
                var value = expression.Map(new MappingContext()
                {
                    Container = rootNode
                });
                ObjectUtils.SetValue(result, propertyName, value);
            }

            return result;
        }
    }

}
