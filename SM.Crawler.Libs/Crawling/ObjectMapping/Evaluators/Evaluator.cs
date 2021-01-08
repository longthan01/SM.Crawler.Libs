using HtmlAgilityPack;
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
        public static object Evaluate(string html, IMapper mapper)
        {
            object result = Activator.CreateInstance(mapper.GetTargetType());
            EvaluateTo(result, html, mapper);
            return result;
        }

        public static void EvaluateTo(object target, string html, IMapper mapper)
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            EvaluateTo(target, document.DocumentNode, mapper);
        }
        public static void EvaluateTo(object target, HtmlNode root, IMapper mapper)
        {
            foreach (var ctx in mapper.GetEvaluationContext())
            {
                object value = ctx.MappingExpression.Map(new MappingContext()
                {
                    Container = root.SelectSingleNode(ctx.XpathRoot)
                });
                value = ctx.PostMapCallback == null ? value : ctx.PostMapCallback(value);
                ObjectUtils.SetValue(target, ctx.PropertyName, value);
            }
        }
        public static object Evaluate(HtmlNode root, IMapper mapper)
        {
            object result = Activator.CreateInstance(mapper.GetTargetType());
            EvaluateTo(result, root, mapper);
            return result;
        }
    }
}
