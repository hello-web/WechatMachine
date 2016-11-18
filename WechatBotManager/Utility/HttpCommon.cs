using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatBotManager
{
    /// <summary>
    /// 简化http代码
    /// </summary>
    public class HttpCommon
    {
        private HttpCommon() { }
        public static readonly HttpCommon instance = new HttpCommon();

        HttpHelper http = new HttpHelper();
        public string GetHttp(string url, ContentType type = ContentType.html, string cookie = "")
        {

            string accept = string.Empty;
            string contentType = string.Empty;
            if (type == ContentType.html)
            {
                accept = "text/html, application/xhtml+xml, */*";
                contentType = "text/html";
            }
            else if (type == ContentType.json)
            {
                accept = "application/json, text/plain, */*";
                contentType = "application/json; charset=UTF-8";
            }
            else if (type == ContentType.javascript)
            {
                contentType = "text/javascript";
                accept = "*/*";
            }

            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "GET",//URL     可选项 默认为Get         
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
                Accept = accept,//    可选项有默认值    
                ContentType = contentType,//返回类型    可选项有默认值     
                Cookie = cookie,//字符串Cookie     可选项  
                Encoding = Encoding.UTF8,
            };

            HttpResult result = http.GetHtml(item);
            return result.Html;

        }

        public string Login(string url,string cookie)
        {
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "GET",//URL     可选项 默认为Get         
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值    
                Accept = "*/*",//    可选项有默认值    
                ContentType = "text/javascript",//返回类型    可选项有默认值     
                Cookie = cookie,//字符串Cookie     可选项  
                Encoding = Encoding.UTF8,
                Referer = "https://wx.qq.com/?&lang=zh_CN", 
            };

            HttpResult result = http.GetHtml(item);
            return result.Html;
        }

        public string PostHttp(string url, string postData, ContentType type = ContentType.html, string cookie = "")
        {
            string accept = string.Empty;
            string contentType = string.Empty;
            if (type == ContentType.html)
            {
                accept = "text/html, application/xhtml+xml, */*";
                contentType = "text/html";
            }
            else if (type == ContentType.json)
            {
                accept = "application/json, text/plain, */*";
                contentType = "application/json; charset=UTF-8";
            }

            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "POST",//URL     可选项 默认为Get         
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
                Accept = accept,//    可选项有默认值    
                ContentType = contentType,//返回类型    可选项有默认值     
                Cookie = cookie,//字符串Cookie     可选项  
                Encoding = Encoding.UTF8,
                Postdata = postData,//Post数据     可选项GET时不需要写    
            };
           
                HttpResult result = http.GetHtml(item);
                return result.Html; 
         
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public Image GetImage(string url, string cookie = "")
        { 
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "GET",//URL     可选项 默认为Get         
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值    
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值    
                ContentType = "text/html",//返回类型    可选项有默认值
                Cookie = cookie,//字符串Cookie     可选项   
            };
            
                return http.GetImage(item); 
           
        }

    }

    public enum ContentType
    {
           html=0,
           json=1,
           javascript= 2
    }

}
