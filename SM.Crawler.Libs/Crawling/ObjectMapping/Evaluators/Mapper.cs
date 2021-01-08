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
        private List<EvaluationContext> mappingExpressions = new List<EvaluationContext>();

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
            mappingExpressions.Add(new EvaluationContext
            {
                PropertyName = propertyName,
                MappingExpression = new TextExpression
                {
                    Expression = xpath
                },
                XpathRoot = this.XpathRoot
            });
            return this;
        }

        public IMapper MapHtml(string propertyName, string xpath)
        {
            return this.MapHtml(propertyName, xpath, null);
        }

        public IMapper MapHtml(string propertyName, string xpath, Func<object, object> postmapCallback)
        {
            mappingExpressions.Add(new EvaluationContext()
            {
                PropertyName = propertyName,
                MappingExpression = new TextHtmlExpression
                {
                    Expression = xpath
                },
                XpathRoot = this.XpathRoot,
                PostMapCallback = postmapCallback
            });
            return this;
        }

        public IMapper Map(string propertyName, string xpath, string attribute)
        {
            mappingExpressions.Add(new EvaluationContext
            {
                MappingExpression = new AttributeExpression()
                {
                    Expression = xpath,
                    AttributeName = attribute
                },
                XpathRoot = this.XpathRoot
            });
            return this;
        }

        public IMapper MapObject(string propertyName, IMapper mapper)
        {
            return this.MapObject(propertyName, this.XpathRoot, mapper);
        }

        public IMapper MapObject(string propertyName, string xpathRoot, IMapper mapper)
        {
            mappingExpressions.Add(new EvaluationContext
            {
                MappingExpression = new ObjectExpression(mapper)
                {
                    Expression = xpathRoot
                },
                XpathRoot = this.XpathRoot
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
            mappingExpressions.Add(new EvaluationContext()
            {
                PropertyName = propertyName,
                MappingExpression = new ArrayExpression(typeof(string), new TextExpression())
                {
                    Expression = xpath
                },
                XpathRoot = this.XpathRoot,
                PostMapCallback = postmapCallback

            });
            return this;
        }

        public IMapper MapArray(string propertyName, string xpath, IMapper mapper)
        {
            mappingExpressions.Add(new EvaluationContext()
            {
                MappingExpression = new ArrayExpression(mapper.GetTargetType(), new ObjectExpression(mapper))
                {
                    Expression = xpath
                },
                XpathRoot = this.XpathRoot,
                PropertyName = propertyName
            });
            return this;
        }

        public Type GetTargetType()
        {
            return this.TargetType;
        }

        public IEnumerable<EvaluationContext> GetEvaluationContext()
        {
            return this.mappingExpressions;
        }
    }

}
