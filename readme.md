# SMation - SM.Crawler.Libs - A common library for html-to-object parsing
[![Build Status](https://travis-ci.com/longthan01/SM.Crawler.Libs.svg?branch=master)](https://travis-ci.com/github/longthan01/SM.Crawler.Libs)


### Installation

Nuget

```sh
Install-Package SM.Crawler.Libs
```

Usage

```C#
            // model definitions
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
            // usages
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
```
