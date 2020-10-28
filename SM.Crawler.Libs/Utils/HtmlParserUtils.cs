using System.Collections.Generic;
using HtmlAgilityPack;

namespace SM.Libs.Utils
{
    public static class HtmlParserUtils
    {
        public static List<HtmlNode> FindHtmlNodesByText(string url, string text)
        {
            string html = DefaultHttpUtility.GetHtmlStringAsync(url).Result;
            HtmlDocument document = new HtmlDocument
            {
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = false
            };
            document.LoadHtml(html);
            var nodes = FindHtmlNodeByText(document.DocumentNode, text);
            return nodes;
        }
        public static List<HtmlNode> FindHtmlNodeByText(HtmlNode root, string text)
        {
            Stack<HtmlNode> nodes = new Stack<HtmlNode>();
            List<HtmlNode> results = new List<HtmlNode>();

            nodes.Push(root);
            while (nodes.Count > 0)
            {
                HtmlNode element = nodes.Pop();
                if (element.OuterHtml.ToLower().Contains(text.ToLower()))
                {
                    results.Add(element);
                    foreach (var item in element.ChildNodes)
                    {
                        nodes.Push(item);
                    }
                }
            }

            return results;
        }
    }
}
