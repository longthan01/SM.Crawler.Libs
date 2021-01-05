using SM.Crawler.Libs.Crawling.ObjectMapping.Base;
using System;
using System.Collections.Generic;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators
{
    public interface IMapper
    {
        Type GetTargetType();
        IMapper Map(string propertyName, string xpath);
        IMapper Map(string propertyName, string xpath, string attribute);
        IMapper MapHtml(string propertyName, string xpath);
        IMapper MapHtml(string propertyName, string xpath, Func<object, object> postmapCallback);
        IMapper MapObject(string propertyName, IMapper evaluator);
        IMapper MapObject(string propertyName, string xpath, IMapper evaluator);
        IMapper MapArray(string propertyName, string xpath);
        IMapper MapArray(string propertyName, string xpath, IMapper evaluator);
        IMapper MapArrayBySingleLine(string propertyName, string xpath, string delimiter);
        IMapper MapArrayBySingleLine(string propertyName, string xpath, Func<object, Array> manipulationCallback);
        Dictionary<string, Func<IMapper, EvaluationContext>> GetMappingExpressions();
    }
}