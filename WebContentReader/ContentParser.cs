using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebContentReader
{
    public class ContentParser
    {
        private readonly HtmlDocument _document = new HtmlDocument();

        public ContentParser(string html)
        {
            _document.LoadHtml(html);
        }

        public HtmlNode[] GetTitles()
        {
            return _document.DocumentNode.SelectNodes("//h1").ToArray();
        }

        public HtmlNode[] GetContents(string tag)
        {
            //return _document.DocumentNode.Descendants(tag).ToArray();
            return _document.DocumentNode.SelectNodes(tag).ToArray();
        }
    }
}
