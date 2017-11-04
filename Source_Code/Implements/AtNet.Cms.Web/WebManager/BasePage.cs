﻿//
// Copyright (C) 2007-2008 S1N1.COM,All rights reseved.
// 
// Project: AtNet.Cms.Manager
// FileName : BasePage.cs
// Author : PC-CWLIU (new.min@msn.com)
// Create : 2011/10/17 9:33:57
// Description :
//
// Get infromation of this software,please visit our site http://cms.ops.cc
// Modify:
// 2013-07-15 14:00 newmin [!]: 修改模板呈现方式
//
//

using AtNet.Cms.Conf;
using AtNet.DevFw;
using AtNet.DevFw.Framework.Extensions;

namespace AtNet.Cms.WebManager
{
    using AtNet.Cms;
    using AtNet.Cms.DataTransfer;
    using AtNet.Cms.Web;
    using AtNet.Cms.Web.WebManager;
    using AtNet.DevFw.Template;
    using System;
    using System.IO.Compression;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;


    public class BasePage
    {
        protected HttpResponse Response { get { return HttpContext.Current.Response; } }
        protected HttpRequest Request { get { return HttpContext.Current.Request; } }
        private static readonly string[] ignoreURI;
        protected SiteDto _site;

        /// <summary>
        /// 管理后台模板标签
        /// </summary>
        private static readonly ManagerTemplate tpl;

        static BasePage()
        {
            ignoreURI = new string[] {
				"module=archive&action=update",
				"module=template&action=editfile",
				"module=file&action=editfile"
			};


            tpl = new ManagerTemplate();
        }

        internal static string ReplaceHtml(string html, string tagKey, string tagValue)
        {
            return Regex.Replace(html, "\\${" + tagKey + "}", tagValue, RegexOptions.IgnoreCase);
        }

        internal static string CompressHtml(string html)
        {
            //return html;
            html = Regex.Replace(html, ">(\\s)+<", "><");

            //替换 //单行注释
            html = Regex.Replace(html, "[\\s|\\t]+\\/\\/[^\\n]*(?=\\n)", String.Empty);

            //替换多行注释
            //const string multCommentPattern = "";
            html = Regex.Replace(html, "/\\*[^\\*]+\\*/", String.Empty);

            //替换<!-- 注释 -->
            html = Regex.Replace(html, "<!--[^\\[][\\s\\S]*?-->", String.Empty);

            //html = Regex.Replace(html, "<!--[^\\[][\\s\\S]*?-->|(^?!=http:|https:)//(.+?)\r\n|\r|\n|\t|(\\s\\s)", String.Empty);
            html = Regex.Replace(html, "\r|\n|\t|(\\s\\s)", String.Empty);

            return html;
        }

        protected void RenderTemplateUseCache(string content, object dataObj)
        {
            if (!AtNet.Cms.Cms.Cache.CheckClientCacheExpiresByEtag()) { return; }

            string html = null;
            HttpResponse response = this.Response;

            MicroTemplateEngine _tpl = new MicroTemplateEngine(tpl);
            html = _tpl.Execute(content);

            if (dataObj != null)
            {
                //替换传入的标签参数
                PropertyInfo[] properties = dataObj.GetType().GetProperties();
                object dataValue;
                foreach (PropertyInfo p in properties)
                {
                    dataValue = p.GetValue(dataObj, null);
                    if (dataValue != null) html = ReplaceHtml(html, p.Name, dataValue.ToString());
                }
            }

            if (!Array.Exists(ignoreURI, a => HttpContext.Current.Request.Url.Query.IndexOf(a) != -1))
            {
                html = CompressHtml(html);
            }


        setHeader:

            //设置缓存
            AtNet.Cms.Cms.Cache.SetClientCacheByEtag(this.Response);

            //输出内容
            this.Response.Write(html);

            if (Settings.Opti_SupportGZip)
            {
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                response.AddHeader("Content-Encoding", "gzip");
            }

            response.AddHeader("X-AspNet-Version", String.Format("AtNet.Cms v{0}", AtNet.Cms.Cms.Version));
            response.AddHeader("Support-URL", "www.ops.cc/cms/");

        }

        protected void RenderTemplate(string content, object dataObj)
        {
            string html = null;
            HttpResponse response = this.Response;

            MicroTemplateEngine _tpl = new MicroTemplateEngine(tpl);
            html = _tpl.Execute(content);

            if (dataObj != null)
            {
                //替换传入的标签参数
                PropertyInfo[] properties = dataObj.GetType().GetProperties();
                object dataValue;
                foreach (PropertyInfo p in properties)
                {
                    dataValue = p.GetValue(dataObj, null);
                    if (dataValue != null) html = ReplaceHtml(html, p.Name, dataValue.ToString());
                }
            }


            if (!Array.Exists(ignoreURI, a => HttpContext.Current.Request.Url.Query.IndexOf(a) !=-1))
            {
                html = CompressHtml(html);
            }

            if (this.Request["ajax"] == "1" || HttpContext.Current.Items["ajax"] == "1")
            {
                const string ajaxPartern = "<body([^>]*)>([\\s\\S]+)</body>";
                if (Regex.IsMatch(html, ajaxPartern))
                {
                    Match match = Regex.Match(html, ajaxPartern);

                    response.Write(match.Groups[2].Value);
                    goto setHeader;
                }
            }

            //输出内容
            response.Write(html);

        setHeader:

            if (Settings.Opti_SupportGZip)
            {
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                response.AddHeader("Content-Encoding", "gzip");
            }
        }


        #region 页面返回呈现
        internal string ReturnSuccess(string message)
        {
            return ReturnSuccess(message,string.Empty);
        }

        internal string ReturnSuccess(string message,string data)
        {
            StringBuilder sb = new StringBuilder();
            if (this.Request["json"] == "1")
            {
                sb.Append("{'result':true,'message':'");
                if (message != null)
                {
                    sb.Append(message.Replace("'", "\\'"));
                }
                sb.Append("',data:'").Append(data).Append("'}");
                this.Response.ContentType = "application/json";
            }
            else
            {
                if (message != null)
                {
                    sb.Append(message);
                }
            }
            return sb.ToString();
        }
        internal string ReturnSuccess()
        {
            return ReturnSuccess("操作执行成功！");
        }
        internal void RenderSuccess()
        {
            this.RenderSuccess("操作执行成功！");
        }

        internal void RenderSuccess(string message)
        {
            this.Response.Write(this.ReturnSuccess(message));
        }

        internal string ReturnError(string message)
        {
            StringBuilder sb = new StringBuilder();
            if (this.Request["json"] == "1")
            {
                sb.Append("{'result':false,'message':'");
                if (message != null)
                {
                    sb.Append(message.Replace("'", "\\'"));
                }
                sb.Append("'}");
                this.Response.ContentType = "application/json";
            }
            else
            {
                sb.Append("<span style=\"color:red\">");
                if (message != null)
                {
                    sb.Append(message);
                }
                sb.Append("</span>");
            }
            return sb.ToString();
        }

        internal string ReturnError()
        {
            return ReturnError("对不起，操作失败！");
        }

        internal void RenderError(string message)
        {
            this.Response.Write(this.ReturnError(message));
        }

        internal void Render(string html)
        {
            this.Response.Write(html);
        }

        #endregion

        /// <summary>
        /// 输出分页数据
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pager"></param>
        internal void PagerJson2(string html, string pager)
        {
            const string fmt = "{'html':'%html%','pager':'%pager%'}";
            this.Response.Write(fmt.Template(html.Replace("'", "\\'"), pager.Replace("'", "\\'")));
            this.Response.ContentType = "application/json";
        }

        /// <summary>
        /// 输出分页数据
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pager"></param>
        internal void PagerJson(object rows, string pager)
        {
            const string fmt = "{'pager':'%pager%','rows':%html%}";
            this.Response.Write(fmt.Template(
               pager.Replace("'", "\\'"),
               JsonSerializer.Serialize(rows)
               ));
            this.Response.ContentType = "application/json";
        }



        /// <summary>
        /// 当前管理站点
        /// </summary>
        internal SiteDto CurrentSite
        {
            get
            {
                if (_site.SiteId <= 0)
                {
                    _site = CmsWebMaster.CurrentManageSite;
                }
                return _site;
            }
            set
            {
                HttpCookie cookie = this.Request.Cookies.Get(CmsWebMaster.cookieNameKey);
                if (cookie != null)
                {
                    if (value.SiteId <= 0)
                    {
                        cookie.Expires = cookie.Expires.AddYears(-2);
                    }
                    else
                    {
                        cookie.Value = value.SiteId.ToString();
                        cookie.Expires = DateTime.Now.AddDays(2);
                    }
                }
                else
                {
                    cookie = new HttpCookie(CmsWebMaster.cookieNameKey, value.SiteId.ToString());
                    cookie.Expires = DateTime.Now.AddDays(2);
                    cookie.Path = "/" + Settings.SYS_ADMIN_TAG;
                }

                this.Response.Cookies.Add(cookie);
                HttpContext.Current.Session[CmsWebMaster.currentSiteSessionStr] = value;
            }
        }

        protected int SiteId
        {
            get { return this.CurrentSite.SiteId; }
        }

        /// <summary>
        /// 比较是否为当前站点的分类
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        internal bool CompareSite(int siteId)
        {
            //当前站点
            var site = this.CurrentSite;
            if (siteId != site.SiteId)
            {
                this.RenderError("分类不存在！");
                return true;
            }
            return false;
        }
    }
}
