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
    public class Mapper : IMapper
    {
        private Dictionary<string, Func<IMapper, EvaluationContext>> mappingExpressions = new Dictionary<string, Func<IMapper, EvaluationContext>>();

        public string XpathRoot { get; }
        private Type TargetType { get; }

        public Mapper(Type targetType) : this(targetType, "/")
        {
        }
        public Mapper(Type targetType, string xpathRoot)
        {
            XpathRoot = xpathRoot;
            TargetType = targetType;
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
            return this.MapHtml(propertyName, xpath, null);
        }

        public IMapper MapHtml(string propertyName, string xpath, Func<object, object> postmapCallback)
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
                    XpathRoot = this.XpathRoot,
                    PostMapCallback = postmapCallback
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
            return this.MapArray(propertyName, xpath, postmapCallback: null);
        }

        public IMapper MapArrayBySingleLine(string propertyName, string xpath, string delimiter)
        {
            return this.MapArrayBySingleLine(propertyName, xpath, manipulationCallback: obj =>
            {
                string element = obj as string;
                string[] tokens = element.Split(delimiter);
                List<string> result = new List<string>();
                foreach (var s in tokens)
                {
                    result.Add(s);
                }
                return result.ToArray();
            });
        }
        public IMapper MapArrayBySingleLine(string propertyName, string xpath, Func<object, Array> manipulationCallback)
        {
            return this.MapArray(propertyName, xpath, postmapCallback: obj =>
            {
                // obj should be an array
                Array arr = obj as Array;
                string element = arr.GetValue(0) as string;
                return manipulationCallback(element);
            });
        }
        private IMapper MapArray(string propertyName, string xpath, Func<object, object> postmapCallback)
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
                    XpathRoot = this.XpathRoot,
                    PostMapCallback = postmapCallback

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
                    mapper.GetMappingExpressions()
                        .ToDictionary(x => x.Key, x => x.Value(mapper).MappingExpression));

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
            return this.TargetType;
        }

        public Dictionary<string, Func<IMapper, EvaluationContext>> GetMappingExpressions()
        {
            return this.mappingExpressions;
        }
    }

}
