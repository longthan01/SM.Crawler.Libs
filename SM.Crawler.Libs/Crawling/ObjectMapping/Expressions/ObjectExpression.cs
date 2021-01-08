using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators;
using SM.Libs.Crawling.ObjectMapping;
using SM.Libs.Utils;
using System;
using System.Collections.Generic;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Expressions
{
    public class ObjectExpression : ExpressionBase
    {
        private readonly IMapper _mapper;

        public ObjectExpression(IMapper mapper)
        {
            _mapper = mapper;
        }
        public override object Map(MappingContext context)
        {
            var rootNode = context.Container.SelectSingleNode(Expression);
            return Evaluator.Evaluate(rootNode, _mapper);
        }
    }

}
