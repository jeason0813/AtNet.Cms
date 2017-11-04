﻿using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AtNet.Cms.Conf
{
    internal class WebConfig
    {
        internal static bool IsDebug()
        {
            bool isDebug = false;
            using (StreamReader fs = new StreamReader(String.Format("{0}Web.config", AppDomain.CurrentDomain.BaseDirectory)))
            {
                string content;
                Regex reg = new Regex("<compilation[^(debug)]+debug=\"([^\"]+)\"", RegexOptions.IgnoreCase);
                while ((content = fs.ReadLine()) != null)
                {
                    if (reg.IsMatch(content))
                    {
                        Match m = reg.Match(content);
                        if (m.Groups[1].Value == "true")
                        {
                            isDebug = true;
                        }
                    }
                }
                fs.Dispose();
                return isDebug;
            }
        }

        /// <summary>
        /// 设置调试
        /// </summary>
        /// <param name="isDebug"></param>
        internal static void SetDebug(bool isDebug)
        {
            string content = String.Empty;
            bool isChange = false;

            using (StreamReader fs = new StreamReader(String.Format("{0}Web.config", AppDomain.CurrentDomain.BaseDirectory)))
            {
                content = fs.ReadToEnd();
                fs.Dispose();
            }

            Regex reg = new Regex("<compilation([^(debug)])+debug=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            if (reg.IsMatch(content))
            {
                Match m = reg.Match(content);
                if ((m.Groups[2].Value == "true" && !isDebug) || (m.Groups[2].Value == "false" && isDebug))
                {
                    content = reg.Replace(content, String.Format("<compilation$1debug=\"{0}\"", isDebug ? "true" : "false"));
                    isChange = true;
                }
            }

            if (isChange)
            {
                using (FileStream fs = new FileStream(String.Format("{0}Web.config", AppDomain.CurrentDomain.BaseDirectory),
                    FileMode.Truncate, FileAccess.Write,FileShare.ReadWrite))
                {
                    byte[] data = Encoding.UTF8.GetBytes(content);
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                    fs.Dispose();
                }
            }
        }
    }
}
