using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace WechatBotManager
{
    public class HttpUtil
    { 
            #region 访问远程URL
            /// <summary>
            /// 访问远程URL
            /// </summary>
            /// <param name="url">远程URL</param>
            /// <returns>远程页面调用结果</returns>
            [Obsolete("此方法将废弃，其功能由方法GetDataFromServer替代实现")]
            public static string CallUrl(string url)
            {
                string data = string.Empty;
                HttpWebRequestMethod method = HttpWebRequestMethod.GET;
                return CallUrl(url, data, method);
            }

            /// <summary>
            /// 主要功能：访问远程URL      
            /// </summary>
            /// <param name="url">远程访问的地址</param>
            /// <param name="data">参数</param>
            /// <returns>远程页面调用结果</returns>
            public static string CallUrl(string url, string data)
            {
                HttpWebRequestMethod method = HttpWebRequestMethod.POST;
                return CallUrl(url, data, method);
            }

            /// <summary>
            /// 主要功能：访问远程URL
            /// </summary>
            /// <param name="url">远程访问的地址</param>
            /// <param name="data">参数</param>
            /// <param name="method">Http页面请求方法</param>
            /// <returns>远程页面调用结果</returns>
            public static string CallUrl(string url, string data, HttpWebRequestMethod method)
            {
                HttpWebRequest request = null;

                if (method == HttpWebRequestMethod.GET)
                {
                    if (url.IndexOf("?") > 0)
                        url = url + "&";
                    else
                        url = url + "?";
                }

                try
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                    //request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                    //request.Timeout = 3000;

                    switch (method)
                    {
                        case HttpWebRequestMethod.GET:
                            request.Method = HttpWebRequestMethod.GET.ToString();
                            break;
                        case HttpWebRequestMethod.POST:
                            {
                                request.Method = HttpWebRequestMethod.POST.ToString();

                                byte[] bdata = Encoding.UTF8.GetBytes(data);
                                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                                request.ContentLength = bdata.Length;

                                Stream streamOut = request.GetRequestStream();
                                streamOut.Write(bdata, 0, bdata.Length);
                                streamOut.Close();
                            }
                            break;
                        case HttpWebRequestMethod.DELETE:
                            request.Method = HttpWebRequestMethod.DELETE.ToString();
                            break;
                    }
                    #region
                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        response = (HttpWebResponse)ex.Response;
                    }

                    #endregion
                    Stream streamIn = response.GetResponseStream();
                    StreamReader reader = new StreamReader(streamIn);
                    string result = reader.ReadToEnd();
                    reader.Close();
                    streamIn.Close();
                    response.Close();
                    return result;
                }
                catch (Exception ex)
                {
                  
                    return string.Empty;
                }
                finally
                {

                }
            }


            /// <summary>
            /// 向服务器提交XML数据
            /// </summary>
            /// <param name="url">远程访问的地址</param>
            /// <param name="data">参数</param>
            /// <param name="method">Http页面请求方法</param>
            /// <returns>远程页面调用结果</returns>
            public static string PostJsonDataToServer(string url, string data, HttpWebRequestMethod method)
            {
                HttpWebRequest request = null;

                try
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                    //request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                    //request.Timeout = 3000;

                    switch (method)
                    {
                        case HttpWebRequestMethod.GET:
                            request.Method = HttpWebRequestMethod.GET.ToString();
                            break;
                        case HttpWebRequestMethod.POST:
                            {
                                request.Method = HttpWebRequestMethod.POST.ToString();

                                byte[] bdata = Encoding.UTF8.GetBytes(data);
                                request.ContentType = "application/json;charset=utf-8";
                                request.ContentLength = bdata.Length;

                                Stream streamOut = request.GetRequestStream();
                                streamOut.Write(bdata, 0, bdata.Length);
                                streamOut.Close();
                            }
                            break;
                    }
                    #region 待删除
                    //HttpWebResponse response = null;
                    //try
                    //{
                    //    response = (HttpWebResponse)request.GetResponse();
                    //}
                    //catch (WebException ex)
                    //{
                    //    //response = (HttpWebResponse)ex.Response;
                    //    /*********************************************************
                    //     * 网络异常，如404，500等错误
                    //     ********************************************************/
                    //    throw;
                    //}
                    #endregion
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream streamIn = response.GetResponseStream();

                    StreamReader reader = new StreamReader(streamIn);
                    string result = reader.ReadToEnd();
                    reader.Close();
                    streamIn.Close();
                    response.Close();

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                }
            }


            /// <summary>
            /// 通过HttpWebRequest方法从服务器取方法
            /// </summary>
            /// <param name="url">需请求的URL</param>
            /// <returns>返回的字符串</returns>
            public static string GetDataFromServer(string url)
            {
                return GetDataFromServer(url, string.Empty);
            }

            /// <summary>
            /// 通过HttpWebRequest方法从服务器取方法
            /// </summary>
            /// <param name="url">需请求的URL</param>
            /// <param name="inputCharset">字符集</param>
            /// <returns>返回的字符串</returns>
            public static string GetDataFromServer(string url, string inputCharset)
            {
                HttpWebRequest webRequest = null;
                try
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                  
                  
                    using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                    {
                        using (Stream resStream = response.GetResponseStream())
                        {
                            Encoding encoding = null;

                            if (!string.IsNullOrEmpty(inputCharset))
                            {
                                try
                                {
                                    encoding = Encoding.GetEncoding(inputCharset);
                                }
                                catch (Exception)
                                {
                                }
                            }

                            StreamReader reader = null;

                            if (encoding != null)
                            {
                                reader = new StreamReader(resStream, encoding);
                            }
                            else
                            {
                                reader = new StreamReader(resStream);
                            }

                            return reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public static string PostDataToServerForFlight(string url, string data, HttpWebRequestMethod method)
            {

                HttpWebRequest request = null;
                try
                {

                    //request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                    //request.Timeout = 3000;

                    switch (method)
                    {
                        case HttpWebRequestMethod.GET:
                            url = string.Format("{0}?{1}", url, data);
                            request = WebRequest.Create(url) as HttpWebRequest;
                            request.Method = HttpWebRequestMethod.GET.ToString();
                            break;
                        case HttpWebRequestMethod.POST:
                            {
                                request = WebRequest.Create(url) as HttpWebRequest;
                                request.Method = HttpWebRequestMethod.POST.ToString();

                                byte[] bdata = Encoding.UTF8.GetBytes(data);
                                request.ContentType = "application/x-www-form-urlencoded";
                                request.ContentLength = bdata.Length;

                                Stream streamOut = request.GetRequestStream();
                                streamOut.Write(bdata, 0, bdata.Length);
                                streamOut.Close();
                            }
                            break;
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream streamIn = response.GetResponseStream();

                    StreamReader reader = new StreamReader(streamIn);
                    string result = reader.ReadToEnd();
                    reader.Close();
                    streamIn.Close();
                    response.Close();

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                }
            }

            public static string PostDataToServer(string url, string data)
            {
                return PostDataToServer(url, data, HttpWebRequestMethod.POST);
            }

            /// <summary>
            /// 向服务器提交json数据
            /// </summary>
            /// <param name="url">远程访问的地址</param>
            /// <param name="data">参数</param>
            /// <param name="method">Http页面请求方法</param>
            /// <returns>远程页面调用结果</returns>
            public static string PostDataToServer(string url, string data, HttpWebRequestMethod method, string contentType = "application/json;charset=utf-8")
            {
                HttpWebRequest request = null;
                try
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                    //request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                    //request.Timeout = 3000;
 
                    switch (method)
                    {
                        case HttpWebRequestMethod.GET:
                            request.Method = HttpWebRequestMethod.GET.ToString();
                            break;
                        case HttpWebRequestMethod.POST:
                            {
                                request.Method = HttpWebRequestMethod.POST.ToString();

                                byte[] bdata = Encoding.UTF8.GetBytes(data);
                                request.ContentType = contentType;
                                request.ContentLength = bdata.Length; 
                                Stream streamOut = request.GetRequestStream();
                                streamOut.Write(bdata, 0, bdata.Length);
                                streamOut.Close();
                            }
                            break;
                    }
                    #region 待删除
                    //HttpWebResponse response = null;
                    //try
                    //{
                    //    response = (HttpWebResponse)request.GetResponse();
                    //}
                    //catch (WebException ex)
                    //{
                    //    //response = (HttpWebResponse)ex.Response;
                    //    /*********************************************************
                    //     * 网络异常，如404，500等错误
                    //     ********************************************************/
                    //    throw;
                    //}
                    #endregion
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream streamIn = response.GetResponseStream();
                    StreamReader reader = new StreamReader(streamIn);
                    string result = reader.ReadToEnd();
                    reader.Close();
                    streamIn.Close();
                    response.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }

            /// <summary>
            /// 向服务器提交XML数据,如果报错，返回报错信息
            /// </summary>
            /// <param name="url">远程访问的地址</param>
            /// <param name="data">参数</param>
            /// <param name="method">Http页面请求方法</param>
            /// <returns>远程页面调用结果</returns>
            public static string PostDataToServerException(string url, string data, HttpWebRequestMethod method, int time = 5000)
            {
                HttpWebRequest request = null;
                try
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.Timeout = time;
                    switch (method)
                    {
                        case HttpWebRequestMethod.GET:
                            request.Method = HttpWebRequestMethod.GET.ToString();
                            break;
                        case HttpWebRequestMethod.POST:
                            {
                                request.Method = HttpWebRequestMethod.POST.ToString();

                                byte[] bdata = Encoding.UTF8.GetBytes(data);
                                request.ContentType = "application/json;charset=utf-8";
                                request.ContentLength = bdata.Length;

                                Stream streamOut = request.GetRequestStream();
                                streamOut.Write(bdata, 0, bdata.Length);
                                streamOut.Close();
                            }
                            break;
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream streamIn = response.GetResponseStream();

                    StreamReader reader = new StreamReader(streamIn);
                    string result = reader.ReadToEnd();
                    reader.Close();
                    streamIn.Close();
                    response.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
                finally
                {
                }
            }

            /// <summary>
            /// 向服务器提交XML数据,如果报错，返回报错信息
            /// </summary>
            /// <param name="url">远程访问的地址</param>
            /// <param name="data">参数</param>
            /// <param name="method">Http页面请求方法</param>
            /// <param name="time">Http超时时间</param>
            /// <returns>远程页面调用结果</returns>
            public static string PostDataToServerStatus(string url, string data, HttpWebRequestMethod method, int time = 5000)
            {
                HttpWebRequest request = null;
                try
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.Timeout = time;
                    switch (method)
                    {
                        case HttpWebRequestMethod.GET:
                            request.Method = HttpWebRequestMethod.GET.ToString();
                            break;
                        case HttpWebRequestMethod.POST:
                            {
                                request.Method = HttpWebRequestMethod.POST.ToString();

                                byte[] bdata = Encoding.UTF8.GetBytes(data);
                                request.ContentType = "application/json;charset=utf-8";
                                request.ContentLength = bdata.Length;

                                Stream streamOut = request.GetRequestStream();
                                streamOut.Write(bdata, 0, bdata.Length);
                                streamOut.Close();
                            }
                            break;
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    HttpStatusCode code = response.StatusCode;
                    response.Close();
                    return code.ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
                finally
                {
                }
            }

            public static string CallUrl(string url, string data, HttpWebRequestMethod method, string inputCharset)
            {
                HttpWebRequest request = null;

                if (method == HttpWebRequestMethod.GET)
                {
                    if (url.IndexOf("?") > 0)
                        url = url + "&";
                    else
                        url = url + "?";
                }

                try
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                    //request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                    //request.Timeout = 3000;

                    switch (method)
                    {
                        case HttpWebRequestMethod.GET:
                            request.Method = HttpWebRequestMethod.GET.ToString();
                            break;
                        case HttpWebRequestMethod.POST:
                            {
                                request.Method = HttpWebRequestMethod.POST.ToString();

                                byte[] bdata = Encoding.GetEncoding(inputCharset).GetBytes(data);
                                request.ContentType = string.Format("application/x-www-form-urlencoded;charset={0}", inputCharset);
                                request.ContentLength = bdata.Length;

                                using (Stream stream = request.GetRequestStream())
                                {
                                    stream.Write(bdata, 0, bdata.Length);
                                }
                            }
                            break;
                        case HttpWebRequestMethod.PUT:
                            request.Method = HttpWebRequestMethod.PUT.ToString();
                            byte[] putData = Encoding.GetEncoding(inputCharset).GetBytes(data);
                            request.ContentType = string.Format("application/x-www-form-urlencoded;charset={0}", inputCharset);
                            request.ContentLength = putData.Length;

                            using (Stream stream = request.GetRequestStream())
                            {
                                stream.Write(putData, 0, putData.Length);
                            }
                            break;
                        case HttpWebRequestMethod.DELETE:
                            request.Method = HttpWebRequestMethod.DELETE.ToString();
                            byte[] deleteData = Encoding.GetEncoding(inputCharset).GetBytes(data);
                            request.ContentType = string.Format("application/x-www-form-urlencoded;charset={0}", inputCharset);
                            request.ContentLength = deleteData.Length;
                            using (Stream stream = request.GetRequestStream())
                            {
                                stream.Write(deleteData, 0, deleteData.Length);
                            }
                            break;
                            //default:
                            //    break;
                    }

                    string strResponse = string.Empty;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(inputCharset)))
                    {
                        strResponse = reader.ReadToEnd();
                    }
                    response.Close();

                    return strResponse;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                }
            }
            #endregion

            private static Encoding encoding = new UTF8Encoding();
            /// <summary>
            /// 发起HTTP请求
            /// </summary>
            /// <param name="m_Doc"></param>
            /// <param name="m_QuestURL"></param>
            /// <returns></returns>
            public static string GetRequest(string requestXml, string m_QuestURL)
            {
                string errMessage = string.Empty;
                string result = "";
                //Post请求地址
                try
                {
                    HttpWebRequest m_Request = (HttpWebRequest)WebRequest.Create(m_QuestURL);
                    //相应请求的参数
                    byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(requestXml);
                    m_Request.Method = "Post";
                    m_Request.ContentType = "application/x-www-form-urlencoded";
                    m_Request.ContentLength = data.Length;
                    m_Request.Headers.Add("Accept-Encoding", "gzip");
                    m_Request.Timeout = 1000000;
                    //请求流
                    Stream requestStream = m_Request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();



                    //响应流
                    HttpWebResponse m_Response = (HttpWebResponse)m_Request.GetResponse();
                    Stream st = m_Response.GetResponseStream();
                    if (m_Response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        st = new GZipStream(st, CompressionMode.Decompress);
                    }
                    //System.IO.Compression.DeflateStream responseStream = new System.IO.Compression.DeflateStream(m_Response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                    //StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                    //result = streamReader.ReadToEnd();

                    //Stream responseStream = m_Response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(st, Encoding.GetEncoding("UTF-8"));
                    //获取返回的信息
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                    //responseStream.Close();
                }
                catch (WebException webEx)
                {
                    if (webEx.Status != WebExceptionStatus.Timeout)
                    {
                        HttpWebResponse res = null;
                        try
                        {
                            res = (HttpWebResponse)webEx.Response;
                            StreamReader sr = new StreamReader(res.GetResponseStream(), encoding);
                            errMessage = sr.ReadToEnd();
                            res.Close();
                        }
                        catch (Exception ex)
                        {
                            errMessage = ex.Message;
                            throw ex;
                        }
                        throw webEx;
                    }
                    else
                    {
                        result = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            } 

            /// <summary>  
            /// 将Json字符串转化为对象  
            /// </summary>  
            /// <typeparam name="T">目标类型</typeparam>  
            /// <param name="strJson">Json字符串</param>  
            /// <returns>目标类型的一个实例</returns>  
            public static T GetObjFromJson<T>(string strJson)
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson)))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)jsonSerializer.ReadObject(ms);
                }
            }

            /// <summary>  
            /// 将对象转化为Json字符串   
            /// </summary>  
            /// <typeparam name="T">源类型</typeparam>  
            /// <param name="obj">源类型实例</param>  
            /// <returns>Json字符串</returns>  
            public static string GetJsonFromObj<T>(T obj)
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    jsonSerializer.WriteObject(ms, obj);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            /// <summary>
            /// 使用Get方法获取字符串结果（没有加入Cookie）
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public static string HttpGet(string url, Encoding encoding = null)
            {
                WebClient wc = new WebClient();
                wc.Encoding = encoding ?? Encoding.UTF8;
                //if (encoding != null)
                //{
                //    wc.Encoding = encoding;
                //}
                return wc.DownloadString(url);
            }

       
    }

    public enum HttpWebRequestMethod
    {
        GET,
        POST,
        DELETE,
        PUT
    }

}
