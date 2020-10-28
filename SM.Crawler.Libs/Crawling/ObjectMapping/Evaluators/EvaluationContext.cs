using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Libs.Crawling.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators
{
    public class EvaluationContext
    {
        public IMappingExpression MappingExpression { get; set; }
        public string XpathRoot { get; set; }
    }
}
