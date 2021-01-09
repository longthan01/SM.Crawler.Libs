# SMation - SM.Crawler.Libs - A common library for html-to-object mapping
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

Another example:

```C#
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
```

Use `MapArrayBySingleLine` to process array element that cannot fully selected by just using a single xpath

```C#
public IMapper MapArrayBySingleLine(string propertyName, string xpath, string delimiter);

// html structure
// <tr>
//     <td title="Giải sáu">G6</td>
//     <td>
//         <p>1904 5547 3574</p>
//     </td>
//     <td>2</td>
//     <td>1, 2, 4, 7</td>
// </tr>

mapper.MapArrayBySingleLine("Prize6", "./tr[4]/td[2]/p[1]", " ")
```
You can also add a post-map callback to post-process data before it is merged into the final object

```C#
IMapper MapHtml(string propertyName, string xpath, Func<object, object> postmapCallback);

// html structure
// <tr>
//     <td rowspan="2" title="Giải tư">G4</td>
//     <td rowspan="2">
//         <p>51221 47340 66352<br>62852 58289 92781 93524</p>
//     </td>
//     <td>4</td>
//     <td>0, 2, 6, 7</td>
// </tr>

mapper.MapHtml("Prize4", "./tr[6]/td[2]/p[1]", obj =>
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
                });
```