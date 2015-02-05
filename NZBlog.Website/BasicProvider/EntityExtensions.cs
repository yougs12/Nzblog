using NZBlog.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using NZBlog.Entity;

namespace NZBlog.Website.BasicProvider
{
    public static class EntityExtensions
    {
        public static string ToXmlString(this RssItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<item>");
            sb.Append(ToXmlItem<RssItem>(item));
            sb.AppendLine("</item>");
            return sb.ToString();
        }

        public static string ToXmlString(this RssImage image)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<image>");
            sb.Append(ToXmlItem<RssImage>(image));
            sb.AppendLine("</image>");
            return sb.ToString();
        }

        public static string ToXmlString(this RssEntity rss)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<rss version=\"2.0\">");
            sb.AppendLine("<channel>");
            sb.AppendLine(ToXmlItem<RssEntity>(rss));
            sb.AppendLine("</channel>");
            sb.AppendLine("</rss>");
            return sb.ToString();
        }

        public static RssEntity ToDefaultRss(this List<BlogDetail> articleList)
        {
            RssEntity rss = new RssEntity()
            {
                Title = "郑雪平的博客",
                Copyright = "Copyright 2013 zhengxueping.com",
                Generator = "最新文章",
                Link = "http://www.zhengxueping.com/",
                Description = "郑雪平的博客，本博客采用Asp.net MVC4.0开发，数据交互层采用基于Dapper扩展的ORM框架，界面基于Bootstrap,本博客为记录自己技术心得而建，文章仅代表自己观点",
                WebMaster = "zhengxueping",
                Image = new RssImage()
                {
                    Link = "http://www.zhengxueping.com/images/base/me.gif",
                    Title = "郑雪平",
                    Url = "http://www.zhengxueping.com/",
                    Description = "郑雪平"
                }
            };
            int ind = 1;
            foreach (BlogDetail article in articleList)
            {
                rss.Items.Add(new RssItem()
                {
                    Title = ind + "." + article.Title,
                    Author = "郑雪平",
                    Category = article.TypeName,
                    Link = "http://www.zhengxueping.com//blog/" + article.BlogId,
                    Guid = article.BlogId.ToString(),
                    PubData = article.CreatTimes,
                    Description = article.BlogContent
                });
                ind++;
            }
            return rss;
        }

        private static string ToXmlItem<DType>(DType data)
            where DType : class
        {
            StringBuilder sb = new StringBuilder();
            Type type = data.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo p in pis)
            {
                if (p.PropertyType == typeof(DateTime))
                {
                    sb.AppendFormat("<{0}>{1:R}</{0}>\r\n", p.Name.ToLower(), p.GetValue(data, null));
                }
                else if (p.PropertyType == typeof(RssImage))
                {
                    sb.AppendLine(((RssImage)p.GetValue(data, null)).ToXmlString());
                }
                else if (p.PropertyType == typeof(IList<RssItem>))
                {
                    IList<RssItem> rssItems = p.GetValue(data, null) as IList<RssItem>;
                    foreach (RssItem item in rssItems)
                    {
                        sb.AppendLine(item.ToXmlString());
                    }
                }
                else
                {
                    sb.AppendFormat("<{0}><![CDATA[{1}]]></{0}>\r\n", p.Name.ToLower(), p.GetValue(data, null));
                }
            }
            return sb.ToString();
        }
    }
}