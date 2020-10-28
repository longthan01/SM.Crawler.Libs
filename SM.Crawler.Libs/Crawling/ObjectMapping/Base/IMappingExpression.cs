using SM.Libs.Crawling.ObjectMapping;

namespace SM.Crawler.Libs.Crawling.ObjectMapping.Base
{
    public interface IMappingExpression
    {
        string Expression { get; set; }
        object Map(MappingContext context);
    }

}
