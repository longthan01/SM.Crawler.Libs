using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Libs.Crawling.ObjectMapping;
using SM.Libs.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators
{
    public class Evaluator
    {
        public static T Evaluate<T>(string html, IMapper mapper) where T : class, new()
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            EvaluateTo(result, html, mapper);
            return result;
        }

        public static void EvaluateTo<T>(T target, string html, IMapper mapper) where T : class, new()
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            foreach (var ctx in mapper.GetMappingExpressions())
            {
                Func<IMapper, EvaluationContext> mappingDelegate = ctx.Value;
                EvaluationContext evaluationContext = mappingDelegate(mapper);
                object value = evaluationContext.MappingExpression.Map(new MappingContext()
                {
                    Container = document.DocumentNode.SelectSingleNode(evaluationContext.XpathRoot)
                });
                ObjectUtils.SetValue(target, ctx.Key, value);
            }
        }
    }
}
