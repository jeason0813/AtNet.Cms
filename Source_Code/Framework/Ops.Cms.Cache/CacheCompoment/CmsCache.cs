﻿
/*
* Copyright(C) 2010-2013 S1N1.COM
* 
* File Name	: TemplateUrlRule
* Author	: Newmin (new.min@msn.com)
* Create	: 2013/05/21 19:59:54
* Description	:
*
*/

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using AtNet.Cms.Infrastructure;
using AtNet.DevFw.Framework;
using AtNet.DevFw.Framework.IO;
using AtNet.DevFw.Framework.Web.Cache;

namespace AtNet.Cms.Cache.CacheCompoment
{
    /// <summary>
    /// Cms注入缓存
    /// </summary>
    public class CmsDependCache : CmsCacheBase
    {
        private static readonly string cacheDependFile;

        internal CmsDependCache() { }
        static CmsDependCache()
        {
            cacheDependFile = Variables.PhysicPath + "config/cache.pid";
        }

        public override void Insert(string key, object value, DateTime absoluteExpireTime)
        {
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(cacheDependFile), absoluteExpireTime, TimeSpan.Zero);
        }

        public override void Insert(string key, object value)
        {
            HttpRuntime.Cache.Insert(key, value, new CacheDependency(cacheDependFile),
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 重建缓存
        /// </summary>
        public override string Rebuilt()
        {
            //初始化config文件夹
            if (!Directory.Exists(String.Concat(Variables.PhysicPath, "config/")))
            {
                Directory.CreateDirectory(String.Concat(Variables.PhysicPath, "config/")).Create();
            }

            using (FileStream fs = new FileStream(cacheDependFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] pid = Encoding.UTF8.GetBytes(new Random().Next(1000, 5000).ToString());
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(pid, 0, pid.Length);
                fs.Flush();
            }

            return  IoUtil.GetFileSHA1(cacheDependFile);
            
            //FileInfo file = new FileInfo(cacheDependFile);
            //file.LastWriteTimeUtc = DateTime.UtcNow;
        }

    }



    /// <summary>
    /// CMS缓存处理
    /// </summary>
    public class CmsCache : ICmsCache
    {
        private ICmsCache dependCache;
        private static string cacheSha1ETag;    //客户端ETag

        internal CmsCache(ICmsCache cache)
        {
            dependCache = cache;
            this.Reset(null);
        }

        /// <summary>
        /// 获取缓存结果
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public T GetCachedResult<T>(string cacheKey, BuiltCacheResultHandler<T> func)
        {
            return dependCache.GetCachedResult<T>(cacheKey, func);
        }

        public T GetResult<T>(string cacheKey, BuiltCacheResultHandler<T> func)
        {
            return dependCache.GetResult<T>(cacheKey, func);
        }

        /// <summary>
        /// 重置系统缓存(不包括Framework Cache)
        /// </summary>
        /// <param name="handler"></param>
        public void Reset(CmsHandler handler)
        {
            //清除系统缓存
            cacheSha1ETag = dependCache.Rebuilt();

            if (handler != null)
            {
                handler();
            }
        }
         
        public bool CheckClientCacheExpires(int seconds)
        {
            return CacheUtil.CheckClientCacheExpires(seconds);
        }

        public bool CheckClientCacheExpiresByEtag()
        {
            return CacheUtil.CheckClientCacheExpires(cacheSha1ETag);
        }


        public void SetClientCache(HttpResponse response, int seconds)
        {
            CacheUtil.SetClientCache(response, seconds);
        }

        public void SetClientCacheByEtag(HttpResponse response)
        {
            CacheUtil.SetClientCache(response, cacheSha1ETag);
        }
        
        public void ETagOutput(HttpResponse response,StringCreatorHandler handler)
        {
        	CacheUtil.Output(response,cacheSha1ETag,handler);
        }

        #region 接口方法

        public void Insert(string key, object value, DateTime absoluteExpireTime)
        {
            dependCache.Insert(key, value, absoluteExpireTime);
        }

        public void Insert(string key, object value, string filename)
        {
            dependCache.Insert(key, value, filename);
        }

        public void Insert(string key, object value)
        {
            dependCache.Insert(key, value);
        }

        public void Clear(string keySign)
        {
            dependCache.Clear(keySign);
        }

        public object Get(string cacheKey)
        {
            return dependCache.Get(cacheKey);
        }

        public string Rebuilt()
        {
            return dependCache.Rebuilt();
        }

        #endregion


    }
}
