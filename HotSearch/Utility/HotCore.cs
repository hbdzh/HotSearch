using HotSearch.Model;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotSearch.Utility
{
    internal class HotCore
    {
        /// <summary>
        /// 截取字符串
        /// </summary>
        private string TextIntercept(string text)
        {
            if (text.Length <= 0)
            {
                return "";
            }
            string[] texts = text.Split("。");
            string newText = "";
            foreach (var item in texts)
            {
                newText += item + "。";
                if (newText.Length >= 50)
                {
                    break;
                }
            }
            if (newText.Length >= 50)//防止有些题主一个句号都不写
            {
                newText = newText.Substring(0, 50);
            }
            return newText;
        }
        /// <summary>
        /// 百度热搜
        /// </summary>
        /// <returns></returns>
        internal ObservableCollection<HotInfo> BaiduHotSearch()
        {
            string url = "https://top.baidu.com/board?tab=realtime";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection HotSearchTitles = doc.DocumentNode.SelectNodes("//div[@class='c-single-text-ellipsis']");
            //var nodes2 = doc.DocumentNode.SelectNodes("//div[@class='class='hot-desc_1m_jR small_Uvkd3 ellipsis_DupbZ']");
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            for (int i = 0; i < HotSearchTitles.Count; i++)
            {
                HtmlNode HotSearchDescribes = HotSearchTitles[i].ParentNode.ParentNode;
                HtmlNode HotSearchLinks = HotSearchTitles[i].ParentNode;
                if (HotSearchDescribes.ChildNodes.Count >= 7)
                {
                    HotSearchDescribes = HotSearchDescribes.ChildNodes[7];
                }
                else
                {
                    HotSearchDescribes = HotSearchTitles[i];
                }
                list.Add(new HotInfo()
                {
                    Number = (i + 1).ToString(),
                    Title = HotSearchTitles[i].InnerText,
                    Describe = HotSearchDescribes.InnerText.Replace(" 查看更多&gt;", ""),
                    Link = HotSearchLinks.GetAttributeValue("href", "default")
                });
            }
            return list;
        }
        /// <summary>
        /// 微博热搜
        /// </summary>
        /// <returns></returns>
        internal async Task<ObservableCollection<HotInfo>> WeiboHotSearch()
        {
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            string url = "https://weibo.com/ajax/statuses/hot_band";
            using (HttpClient httpClient = new HttpClient())
            {
                int number = 1;
                string jsonCode = await httpClient.GetStringAsync(url);
                foreach (var item in JObject.Parse(jsonCode)["data"]["band_list"])
                {
                    string title = item["note"].ToString();
                    string describe;
                    if (item["mblog"] != null)
                    {
                        Regex regex = new Regex(@"<[^>]+>|</[^>]+>");
                        describe = regex.Replace(item["mblog"]["text"].ToString(), "");
                    }
                    else
                    {
                        describe = "";
                    }
                    string link = "https://s.weibo.com/weibo?q=" + title;
                    list.Add(new HotInfo()
                    {
                        Number = number.ToString(),
                        Title = title,
                        Describe = TextIntercept(describe),
                        Link = link
                    });
                    number++;
                }
            }
            return list;
        }
        /// <summary>
        /// 知乎热搜
        /// </summary>
        /// <returns></returns>
        internal ObservableCollection<HotInfo> ZhihuHotSearch()
        {
            string url = "https://www.zhihu.com/topsearch";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection HotSearchTitles = doc.DocumentNode.SelectNodes("//div[@class='TopSearchMain-title']");
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            int number = 1;
            foreach (var item in HotSearchTitles)
            {
                list.Add(new HotInfo()
                {
                    Number = number.ToString(),
                    Title = item.InnerText,
                    Describe = "",
                    Link = "https://www.zhihu.com/search?q=" + item.InnerText
                });
                number++;
            }
            return list;
        }
        /// <summary>
        /// 知乎热榜
        /// </summary>
        /// <returns></returns>
        internal async Task<ObservableCollection<HotInfo>> ZhihuHotList()
        {
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            string url = "https://www.zhihu.com/api/v3/feed/topstory/hot-lists/total?limit=50&desktop=true";
            using (HttpClient httpClient = new HttpClient())
            {
                int number = 1;
                string jsonCode = await httpClient.GetStringAsync(url);
                foreach (var item in JObject.Parse(jsonCode)["data"])
                {
                    string id = item["target"]["id"].ToString();
                    string title = item["target"]["title"].ToString();
                    string describe = item["target"]["excerpt"].ToString();
                    string link = "https://www.zhihu.com/question/" + id;
                    list.Add(new HotInfo()
                    {
                        Number = number.ToString(),
                        Title = title,
                        Describe = TextIntercept(describe),
                        Link = link
                    });
                    number++;
                }
            }
            return list;
        }
        /// <summary>
        /// 抖音热榜
        /// </summary>
        /// <returns></returns>
        internal async Task<ObservableCollection<HotInfo>> DouyinHotList()
        {
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            string url = "https://www.iesdouyin.com/web/api/v2/hotsearch/billboard/word/";
            using (HttpClient httpClient = new HttpClient())
            {
                int number = 1;
                string jsonCode = await httpClient.GetStringAsync(url);
                foreach (var item in JObject.Parse(jsonCode)["word_list"])
                {
                    string title = item["word"].ToString();
                    string link = "https://www.douyin.com/search/" + title;
                    list.Add(new HotInfo()
                    {
                        Number = number.ToString(),
                        Title = title,
                        Describe = "",
                        Link = link
                    });
                    number++;
                }
            }
            return list;
        }
        /// <summary>
        /// 快手热榜
        /// </summary>
        /// <returns></returns>
        internal ObservableCollection<HotInfo> KuaishouHotList()
        {
            string url = "https://www.kuaishou.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection HotSearchTitles = doc.DocumentNode.SelectNodes("//p[@class='rank-name']");
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new HotInfo()
                {
                    Number = (i + 1).ToString(),
                    Title = HotSearchTitles[i].InnerText,
                    Describe = "",
                    Link = "https://www.kuaishou.com/search/video?searchKey=" + HotSearchTitles[i].InnerText
                });
            }
            return list;
        }
        /// <summary>
        /// 微信热词
        /// </summary>
        /// <returns></returns>
        internal ObservableCollection<HotInfo> WeixinHotWords()
        {
            string url = "https://weixin.sogou.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection HotSearchTitles = doc.DocumentNode.SelectNodes("//ol[@class='hot-news']/li/a");
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            for (int i = 0; i < HotSearchTitles.Count; i++)
            {
                list.Add(new HotInfo()
                {
                    Number = (i + 1).ToString(),
                    Title = HotSearchTitles[i].InnerText,
                    Describe = "",
                    Link = HotSearchTitles[i].GetAttributeValue("href", "default")
                });
            }
            return list;
        }
        /// <summary>
        /// 贴吧热议
        /// </summary>
        /// <returns></returns>
        internal ObservableCollection<HotInfo> TiebaHotDiscussion()
        {
            string url = "https://tieba.baidu.com/hottopic/browse/topicList?res_type=1";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection HotSearchTitles = doc.DocumentNode.SelectNodes("//a[@class='topic-text']");
            HtmlNodeCollection HotSearchDescribes = doc.DocumentNode.SelectNodes("//p[@class='topic-top-item-desc']");
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            for (int i = 0; i < HotSearchTitles.Count; i++)
            {
                list.Add(new HotInfo()
                {
                    Number = (i + 1).ToString(),
                    Title = HotSearchTitles[i].InnerText,
                    Describe = HotSearchDescribes[i].InnerText,
                    Link = HotSearchTitles[i].GetAttributeValue("href", "default")
                });
            }
            return list;
        }
        /// <summary>
        /// 头条热榜
        /// </summary>
        /// <returns></returns>
        internal async Task<ObservableCollection<HotInfo>> ToutiaoHotList()
        {
            ObservableCollection<HotInfo> list = new ObservableCollection<HotInfo>();
            string url = "https://www.toutiao.com/hot-event/hot-board/?origin=toutiao_pc&_signature=_02B4Z6wo00f01pfYp2AAAIDD9NJnCaEBLXqX.KPAAMcze5";
            using (HttpClient httpClient = new HttpClient())
            {
                int number = 1;
                string jsonCode = await httpClient.GetStringAsync(url);
                foreach (var item in JObject.Parse(jsonCode)["data"])
                {
                    string id = item["ClusterId"].ToString();
                    string title = item["Title"].ToString();
                    string link = "https://www.toutiao.com/trending/" + id;
                    list.Add(new HotInfo()
                    {
                        Number = number.ToString(),
                        Title = title,
                        Describe = "",
                        Link = link
                    });
                    number++;
                }
            }
            return list;
        }
    }
}
