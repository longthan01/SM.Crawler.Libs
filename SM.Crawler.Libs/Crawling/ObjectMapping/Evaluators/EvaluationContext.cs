using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Libs.Crawling.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators
{
    public class EvaluationContext
    {
        public string PropertyName { get; set; }
        public IMappingExpression MappingExpression { get; set; }
        public string XpathRoot { get; set; }
        public Func<object, object> PostMapCallback { get; set; }
    }
}
