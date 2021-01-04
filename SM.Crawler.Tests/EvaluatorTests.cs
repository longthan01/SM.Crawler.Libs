using NUnit.Framework;
using SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators;
using SM.Libs.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SM.Crawler.Tests
{
    public class Proxy
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        public string Country { get; set; }
        public string Ping { get; set; }
        public string Type { get; set; }
        public string Anonymity { get; set; }
    }
    public class ArrayProxy
    {
        public IEnumerable<Proxy> Proxies { get; set; }
    }

    [TestFixture]
    public class EvaluatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EvaluateComplexType_WithNestedTypeXpathTheSameLevelAsOuterType_ShouldSuccess()
        {
            string html = File.ReadAllText("test.html");
            IMapper mapper = new Mapper(typeof(Proxy), "//*[@id=\"loadPage\"]/tr[1]");
            mapper
                .Map("Host", "./th[1]")
                .Map("Port", "./td[1]")
                .MapObject("Metadata", new Mapper(typeof(Metadata))
                    .Map("Country", "./td[2]")
                    .Map("Ping", "./td[3]/div[1]/div[1]")
                    .Map("Type", "./td[4]")
                    .Map("Anonymity", "./td[5]")
                 );
            Proxy proxy = Evaluator.Evaluate(html, mapper) as Proxy;
            Assert.IsNotNull(proxy);
            Assert.AreEqual(proxy.Host, "31.206.38.40");
            Assert.AreEqual(proxy.Port, "37630");
            Assert.AreEqual(proxy.Metadata.Country.Trim(), "Turkey, Diyarbakir");
            Assert.AreEqual(proxy.Metadata.Ping, "89");
            Assert.AreEqual(proxy.Metadata.Type.Replace("\r", "").Replace("\n", "").Trim(), "SOCKS4");
            Assert.AreEqual(proxy.Metadata.Anonymity, "Anonymous");
        }
        [Test]
        public void EvaluateComplexType_WithNestedTypeXpathDifferentLevelFromOuterType_ShouldSuccess()
        {
            string html = File.ReadAllText("test.html");
            IMapper mapper = new Mapper(typeof(Proxy), "//*[@id=\"loadPage\"]/tr[1]");
            mapper
                .Map("Host", "./th[1]")
                .Map("Port", "./td[1]")
                .MapObject("Metadata", "//*[@id=\"loadPage\"]/tr[100]", new Mapper(typeof(Metadata))
                    .Map("Country", "./td[2]")
                    .Map("Ping", "./td[3]/div[1]/div[1]")
                    .Map("Type", "./td[4]")
                    .Map("Anonymity", "./td[5]")
                 );
            Proxy proxy = Evaluator.Evaluate(html, mapper) as Proxy;
            Assert.IsNotNull(proxy);
            Assert.AreEqual(proxy.Host, "31.206.38.40");
            Assert.AreEqual(proxy.Port, "37630");
            Assert.AreEqual(proxy.Metadata.Country.Trim(), "Poland, Sosnowiec");
            Assert.AreEqual(proxy.Metadata.Ping, "33");
            Assert.AreEqual(proxy.Metadata.Type.Replace("\r", "").Replace("\n", "").Trim(), "SOCKS4");
            Assert.AreEqual(proxy.Metadata.Anonymity, "Anonymous");
        }
        [Test]
        public void EvaluateArrayType_WithNestedTypeXpathTheSameLevelFromOuterType_ShouldSuccess()
        {
            string html = File.ReadAllText("test.html");
            IMapper proxyMapper = new Mapper(typeof(Proxy));
            proxyMapper
                .Map("Host", "./th[1]")
                .Map("Port", "./td[1]")
                .MapObject("Metadata", new Mapper(typeof(Metadata))
                    .Map("Country", "./td[2]")
                    .Map("Ping", "./td[3]/div[1]/div[1]")
                    .Map("Type", "./td[4]")
                    .Map("Anonymity", "./td[5]")
                 );
            IMapper arrayProxyMapper = new Mapper(typeof(ArrayProxy), ".")
                .MapArray("Proxies", "//*[@id=\"loadPage\"]/tr", proxyMapper);

            ArrayProxy arrayProxy = Evaluator.Evaluate(html, arrayProxyMapper) as ArrayProxy;

            Assert.IsNotNull(arrayProxy.Proxies);
            Assert.AreEqual(100, arrayProxy.Proxies.Count());
            Proxy proxy = arrayProxy.Proxies.ElementAt(0);
            Assert.IsNotNull(proxy);
            Assert.AreEqual(proxy.Host, "31.206.38.40");
            Assert.AreEqual(proxy.Port, "37630");
            Assert.AreEqual(proxy.Metadata.Country.Trim(), "Turkey, Diyarbakir");
            Assert.AreEqual(proxy.Metadata.Ping, "89");
            Assert.AreEqual(proxy.Metadata.Type.Replace("\r", "").Replace("\n", "").Trim(), "SOCKS4");
            Assert.AreEqual(proxy.Metadata.Anonymity, "Anonymous");
        }
    }
}