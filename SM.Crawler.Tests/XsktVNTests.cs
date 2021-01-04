using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators;
using SM.Libs.Utils;

namespace SM.Crawler.Tests
{
    [TestFixture]
    public class XsktVNTests
    {
        public class LoteryResult
        {
            public string Prize8 { get; set; }
            public string Prize7 { get; set; }
            public IEnumerable<string> Prize6 { get; set; }
            public string Prize5 { get; set; }
            public IEnumerable<string> Prize4 { get; set; }
            public IEnumerable<string> Prize3 { get; set; }
            public string Prize2 { get; set; }
            public string Prize1 { get; set; }
            public string Prize0 { get; set; }
        }
        [Test]
        public void EvaluateComplexType_ShouldSuccess()
        {
            string html = File.ReadAllText("xsktcomvn.html");
            IMapper mapper = new Mapper(typeof(LoteryResult), "//*[@id=\"AG0\"]");
            mapper
                .Map("Prize8", "./tr[2]/td[2]/em[1]")
                .Map("Prize7", "./tr[3]/td[2]/p[1]")
                .MapArrayBySingleLine("Prize6", "./tr[4]/td[2]/p[1]", " ")
                ;
            LoteryResult result = Evaluator.Evaluate(html, mapper) as LoteryResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("22", result.Prize8);
            Assert.AreEqual("617", result.Prize7);
            Assert.AreEqual(3, result.Prize6.Count());
            Assert.AreEqual("1904", result.Prize6.ElementAt(0));
            Assert.AreEqual("5547", result.Prize6.ElementAt(1));
            Assert.AreEqual("3574", result.Prize6.ElementAt(2));
        }
    }
}
