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

        public class LoteryResultList
        {
            public IEnumerable<LoteryResult> LoteryResults { get; set; }
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
                .Map("Prize5", "./tr[5]/td[2]/p[1]")
                .MapHtml("Prize4", "./tr[6]/td[2]/p[1]", obj =>
                {
                    string element = obj as string;
                    string[] lineSplit = element.Split("<br>");
                    List<string> res = new List<string>();
                    foreach (var l in lineSplit)
                    {
                        string[] tokens = l.Split(" ");
                        res.AddRange(tokens);
                    }
                    return res.ToArray();
                })
                .MapArrayBySingleLine("Prize3", "./tr[8]/td[2]/p[1]", " ")
                .Map("Prize2", "./tr[9]/td[2]/p[1]")
                .Map("Prize1", "./tr[10]/td[2]/p[1]")
                .Map("Prize0", "./tr[11]/td[2]/em[1]")
                ;
            LoteryResult result = Evaluator.Evaluate(html, mapper) as LoteryResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("22", result.Prize8);
            Assert.AreEqual("617", result.Prize7);
            Assert.AreEqual(3, result.Prize6.Count());
            Assert.AreEqual("1904", result.Prize6.ElementAt(0));
            Assert.AreEqual("5547", result.Prize6.ElementAt(1));
            Assert.AreEqual("3574", result.Prize6.ElementAt(2));
            Assert.AreEqual("2127", result.Prize5);

            Assert.AreEqual("51221", result.Prize4.ElementAt(0));
            Assert.AreEqual("47340", result.Prize4.ElementAt(1));
            Assert.AreEqual("66352", result.Prize4.ElementAt(2));
            Assert.AreEqual("62852", result.Prize4.ElementAt(3));
            Assert.AreEqual("58289", result.Prize4.ElementAt(4));
            Assert.AreEqual("92781", result.Prize4.ElementAt(5));
            Assert.AreEqual("93524", result.Prize4.Last());

            Assert.AreEqual("94142", result.Prize3.ElementAt(0));
            Assert.AreEqual("10708", result.Prize3.ElementAt(1));

            Assert.AreEqual("89067", result.Prize2);
            
            Assert.AreEqual("42246", result.Prize1);

            Assert.AreEqual("760688", result.Prize0);
        }

        [Test]
        public void EvaluateListOfComplexType_ShouldSuccess()
        {
            string html = DefaultHttpUtility.GetHtmlStringAsync("https://xskt.com.vn/xshcm-xstp").Result;
            IMapper elementmapper = new Mapper(typeof(LoteryResult));
            elementmapper
                .Map("Prize8", "./tr[2]/td[2]/em[1]")
                .Map("Prize7", "./tr[3]/td[2]/p[1]")
                .MapArrayBySingleLine("Prize6", "./tr[4]/td[2]/p[1]", " ")
                .Map("Prize5", "./tr[5]/td[2]/p[1]")
                .MapHtml("Prize4", "./tr[6]/td[2]/p[1]", obj =>
                {
                    string element = obj as string;
                    string[] lineSplit = element.Split("<br>");
                    List<string> res = new List<string>();
                    foreach (var l in lineSplit)
                    {
                        string[] tokens = l.Split(" ");
                        res.AddRange(tokens);
                    }
                    return res.ToArray();
                })
                .MapArrayBySingleLine("Prize3", "./tr[8]/td[2]/p[1]", " ")
                .Map("Prize2", "./tr[9]/td[2]/p[1]")
                .Map("Prize1", "./tr[10]/td[2]/p[1]")
                .Map("Prize0", "./tr[11]/td[2]/em[1]");
            IMapper mapper = new Mapper(typeof(LoteryResultList));
            mapper.MapArray("LoteryResults", "//table[contains(@class, 'result')]", elementmapper);
            LoteryResultList result = Evaluator.Evaluate(html, mapper) as LoteryResultList;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.LoteryResults.Count());
        }
    }
}
