﻿/*
* Copyright(C) 2010-2012 S1N1.COM
* 
* File Name	: EachClass
* Author	: Administrator
* Create	: 2012/10/28 8:53:32
* Description	:
*
*/

using AtNet.Cms.Conf;
using AtNet.Cms.Core;

namespace AtNet.Cms.WebManager
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using AtNet.Cms;

    internal static class EachClass
    {
        private static string[] allowListedExt = new String[] {".html",".phtml",".css",".js"};
        /// <summary>
        /// 查找文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="sb"></param>
        /// <param name="ext"></param>
        /// <param name="isroot"></param>
        public static void EachFiles(DirectoryInfo dir, StringBuilder sb, string dirName, string ext, bool isroot)
        {
            if (!dir.Exists) return;
            string path;
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name!="cms.config" && (file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden 
                    && (String.IsNullOrEmpty(ext) || ext.IndexOf(file.Extension) != -1))
                {

                    sb.Append("<option value=\"").Append(dirName).Append("/");

                    if (isroot)
                    {
                        sb.Append(file.Name).Append("\">").Append(file.Name).Append("</option>");
                    }
                    else
                    {
                        path = dir.FullName.Substring(dir.FullName.IndexOf(dirName) + dirName.Length + 1).Replace("\\", "/");
                        sb.Append(path).Append("/").Append(file.Name).Append("\">").Append(path.Replace("/", "\\")).Append("\\").Append(file.Name).Append("</option>");
                    }
                }
            }

            foreach (DirectoryInfo _dir in dir.GetDirectories())
            {
                EachFiles(_dir, sb, dirName, ext, false);
            }

        }

        /// <summary>
        /// 模板页面
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="sb"></param>
        public static void IterialTemplateFiles(DirectoryInfo dir, StringBuilder sb, string tplName)
        {
            if (dir.Name == ".backup") return;

            string path = AtNet.Cms.Cms.PyhicPath + "templates/" + tplName + "/";
            string filePath;
            TemplatePageType pageType;
            IList<FileInfo> filelist = new List<FileInfo>();

            foreach (FileInfo file in dir.GetFiles())
            {
                if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                            && !file.Name.EndsWith(".bak")
                            && Array.Exists(allowListedExt, a => file.Name.IndexOf(a) != -1))
                {
                    filelist.Add(file);
                }
            }

            if (filelist.Count != 0)
            {
                if (String.Compare(dir.Name, tplName, true) == 0)
                {
                    pageType = TemplatePageType.Index;
                }
                else
                {
                    pageType = AtNet.Cms.Cms.Template.GetPageType(filelist[0].FullName.Substring(path.Length).Replace("\\","/"));
                }
                while (true)
                {
                    sb.Append("<optgroup label=\"").Append(AtNet.Cms.Cms.Template.GetPageDescript(pageType)).Append("\">");

                    for (int i = 0; i < filelist.Count; i++)
                    {
                        if (AtNet.Cms.Cms.Template.GetPageType(filelist[i].Name) == pageType)
                        {
                            filePath = filelist[i].FullName.Substring(path.Length).Replace("\\", "/");

                            sb.Append("<option value=\"templates/").Append(tplName).Append("/");

                            sb.Append(filePath).Append("\">/").Append(filePath).Append("</option>");

                            filelist.Remove(filelist[i]);
                            --i;
                        }
                    }
                    sb.Append("</optgroup>");

                    if (filelist.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        pageType = AtNet.Cms.Cms.Template.GetPageType(filelist[0].Name);
                    }
                }
            }

            /*
            foreach (FileInfo file in filelist)
            {
                if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                    && !file.Name.EndsWith(".bak")
                    && Array.Exists(allowListedExt, a => file.Name.IndexOf(a) != -1))
                {
                    filePath = file.FullName.Substring(path.Length).Replace("\\", "/");
                    pageType = Cms.Template.GetPageType(filePath);
                   

                    sb.Append("<option value=\"templates/").Append(tplName).Append("/");

                    sb.Append(filePath).Append("\">/").Append(filePath);
                    if (pageType != TemplatePageType.Custom)
                    {
                        sb.Append("(").Append( Cms.Template.GetPageDescript(pageType) ).Append(")");
                    }

                    sb.Append("</option>");
                }
            }
             */

            foreach (DirectoryInfo _dir in dir.GetDirectories())
            {
                IterialTemplateFiles(_dir, sb, tplName);
            }
        }

        /// <summary>
        /// 栏目，模块，页面选择模板
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="sb"></param>
        /// <param name="isroot"></param>
       public static void EachTemplatePage(DirectoryInfo dir, StringBuilder sb)
       {
           int rootDirLength = (AtNet.Cms.Cms.PyhicPath + "templates/").Length;
           string path;
           foreach (FileInfo file in dir.GetFiles())
           {
               if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && file.Name.EndsWith(".html"))
               {

                   sb.Append("<option value=\"templates/");

                   path = file.FullName.Substring(rootDirLength).Replace("\\", "/");
                   sb.Append(path).Append("\">/").Append(path).Append("</option>");

                   /*
                   if (isroot)
                   {
                       sb.Append(file.Name).Append("\">").Append(file.Name).Append("</option>");
                   }
                   else
                   {
                       path = dir.FullName.Replace(AppDomain.CurrentDomain.BaseDirectory, String.Empty).Replace("\\", "/");
                       sb.Append(path).Append("/").Append(file.Name).Append("\">").Append(path.Replace("/", "\\")).Append("\\").Append(file.Name).Append("</option>");
                   }
                    */
               }
           }

           foreach (DirectoryInfo _dir in dir.GetDirectories())
           {
               EachTemplatePage(_dir, sb);
           }
       }

        /// <summary>
        /// 迭代模板(指定类型)
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="sb"></param>
        /// <param name="pageType"></param>
       public static void EachTemplatePage(DirectoryInfo dir, StringBuilder sb, params TemplatePageType[] pageType)
       {
           if(!dir.Exists|| dir.Name == ".backup")return;
           int rootDirLength = (AtNet.Cms.Cms.PyhicPath + "templates/").Length;
           
           string path;
           foreach (FileInfo file in dir.GetFiles())
           {
               if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                   && file.Name.EndsWith(".html")
                   && Array.Exists(pageType, a => AtNet.Cms.Cms.Template.GetPageType(file.Name) == a)
                   && (Settings.TPL_MultMode || !AtNet.Cms.Cms.Template.IsSystemTemplate(file.Name))          //独享模式不显示系统模板
                   )
               {
                   sb.Append("<option value=\"templates/");

                   path = file.FullName.Substring(rootDirLength).Replace("\\", "/");
                   sb.Append(path).Append("\">/").Append(path).Append("</option>");

                   /*
                   if (isroot)
                   {
                       sb.Append(file.Name).Append("\">").Append(file.Name).Append("</option>");
                   }
                   else
                   {
                       path = dir.FullName.Replace(AppDomain.CurrentDomain.BaseDirectory, String.Empty).Replace("\\", "/");
                       sb.Append(path).Append("/").Append(file.Name).Append("\">").Append(path.Replace("/", "\\")).Append("\\").Append(file.Name).Append("</option>");
                   }
                    */
               }
           }

           foreach (DirectoryInfo _dir in dir.GetDirectories())
           {
               EachTemplatePage(_dir, sb, pageType);
           }
       }
    }
}
