using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;

namespace WebContentReader
{
    public class GoogleNewsDownloader
    {
        private const string API_KEY = "AIzaSyCwIFwmGJb8CYvB0QKVOD42oRMFBJZ9Ba4";
        private const string SEARCH_ID = "010984902545343945155:o-ravktbs-e";
        
        public GoogleNewsDownloader()
        {
        }

        public List<NewsItems> GetNews(string searchKey)
        {
            var searchService = new CustomsearchService(new BaseClientService.Initializer() { ApiKey = API_KEY });
            var listReq = searchService.Cse.List(searchKey);
            listReq.Cx = SEARCH_ID;
            Search search = listReq.Execute();
            var news = new List<NewsItems>();

            foreach (var item in search.Items)
            {
                news.Add(new NewsItems()
                {
                    Title = item.Title,
                    Description = item.Link
                });
            }

            return news;
        }
    }

    public class NewsItems
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
