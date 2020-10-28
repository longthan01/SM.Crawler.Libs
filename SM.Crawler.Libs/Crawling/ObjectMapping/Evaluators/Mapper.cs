using HtmlAgilityPack;
using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using SM.Crawler.Libs.Crawling.ObjectMapping.Expressions;
using SM.Libs.Crawling.ObjectMapping;
using SM.Libs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators
{
    public class Mapper<T> : IMapper where T : class, new()
    {
        private Dictionary<string, Func<IMapper, EvaluationContext>> mappingExpressions = new Dictionary<string, Func<IMapper, EvaluationContext>>();

        public string XpathRoot { get; }

        public Mapper() : this(null)
        {
        }
        public Mapper(string xpathRoot)
        {
            XpathRoot = xpathRoot;
        }
        public IMapper Map(string propertyName, string xpath)
        {
            mappingExpressions.Add(propertyName, (ev) =>
            {
                var expression = new TextExpression()
                {
                    Expression = xpath
                };
                return new EvaluationContext()
                {
                    MappingExpression = expression,
                    XpathRoot = this.XpathRoot
                };
            });
            return this;
        }

        public IMapper MapHtml(string propertyName, string xpath)
        {
            mappingExpressions.Add(propertyName, (ev) =>
                {
                    var expression = new TextHtmlExpression()
                    {
                        Expression = xpath,
                    };
                    return new EvaluationContext()
                    {
                        MappingExpression = expression,
                        XpathRoot = this.XpathRoot
                    };
                });
            return this;
        }

        public IMapper Map(string propertyName, string xpath, string attribute)
        {
            mappingExpressions.Add(propertyName, (ev) =>
            {
                var expression = new AttributeExpression()
                {
                    Expression = xpath,
                    AttributeName = attribute
                };
                return new EvaluationContext()
                {
                    MappingExpression = expression,
                    XpathRoot = this.XpathRoot
                };
            });
            return this;
        }

        public IMapper MapObject(string propertyName, IMapper mapper)
        {
            return this.MapObject(propertyName, this.XpathRoot, mapper);
        }

        public IMapper MapObject(string propertyName, string xpathRoot, IMapper mapper)
        {
            mappingExpressions.Add(propertyName, (ev) =>
            {
                var expression = new ObjectExpression(mapper.GetTargetType(),
                    mapper.GetMappingExpressions().ToDictionary(x => x.Key, x => x.Value(mapper).MappingExpression))
                {
                    Expression = xpathRoot
                };
                return new EvaluationContext()
                {
                    MappingExpression = expression,
                    XpathRoot = this.XpathRoot
                };
            });
            return this;
        }

        public IMapper MapArray(string propertyName, string xpath)
        {
            mappingExpressions.Add(propertyName, (ev) =>
            {
                var expression = new ArrayExpression(typeof(string), new TextExpression())
                {
                    Expression = xpath
                };
                return new EvaluationContext()
                {
                    MappingExpression = expression,
                    XpathRoot = this.XpathRoot
                };
            });
            return this;
        }

        public IMapper MapArray(string propertyName, string xpath, IMapper mapper)
        {
            mappingExpressions.Add(propertyName, (ev) =>
            {
                var ex = new ObjectExpression(
                    mapper.GetTargetType(),
                    mapper.GetMappingExpressions().ToDictionary(x => x.Key, x => x.Value(mapper).MappingExpression));
                ArrayExpression expression = new ArrayExpression(mapper.GetTargetType(), ex)
                {
                    Expression = xpath
                };
                return new EvaluationContext()
                {
                    MappingExpression = expression,
                    XpathRoot = this.XpathRoot
                };
            });
            return this;
        }

        public Type GetTargetType()
        {
            return typeof(T);
        }

        public Dictionary<string, Func<IMapper, EvaluationContext>> GetMappingExpressions()
        {
            return this.mappingExpressions;
        }
    }

}
