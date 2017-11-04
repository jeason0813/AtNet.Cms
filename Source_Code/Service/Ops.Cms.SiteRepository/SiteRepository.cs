﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AtNet.Cms.DAL;
using AtNet.Cms.Domain.Implement.Site;
using AtNet.Cms.Domain.Interface.Common.Language;
using AtNet.Cms.Domain.Interface.Site;
using AtNet.Cms.Domain.Interface.Site.Category;
using AtNet.Cms.Domain.Interface.Site.Extend;
using AtNet.Cms.Domain.Interface.Site.Link;
using AtNet.Cms.Domain.Interface.Site.Template;
using AtNet.Cms.Infrastructure;

namespace AtNet.Cms.ServiceRepository
{
    public class SiteRepository:BaseSiteRepository,ISiteRepository
    {
        private static SiteDal siteDal = new SiteDal();
        private static LinkDAL linkDal = new LinkDAL();

        private IExtendFieldRepository _extendFieldRepository;
        private ICategoryRepository _categoryRep;
        private ITemplateRepository _tempRep;

        public SiteRepository(
            IExtendFieldRepository extendFieldRepository,
            ICategoryRepository categoryRepository,
            ITemplateRepository tempRep
            )
        {
            this._extendFieldRepository = extendFieldRepository;
            this._categoryRep = categoryRepository;
            this._tempRep = tempRep;
        }

        public ISite CreateSite(int siteid, string name)
        {
            return base.CreateSite(this,
                this._extendFieldRepository,
                this._categoryRep,
                this._tempRep,
                siteid,
                name);
        }
        public int SaveSite(ISite site)
        {
            if (site.Id == 0)
            {
                //
                //NOTO:创建站点时应创建一个ROOT栏目节点
                //

                int siteId=siteDal.CreateSite(site);
                if (siteId==-1)
                {
                    throw new ArgumentException("创建站点失败");
                }
                else
                {
                    //清理缓存
                    RepositoryDataCache._siteDict = null;
                    RepositoryDataCache._categories = null;
                }
            }
            else
            {
                if (siteDal.UpdateSite(site) != 1)
                {
                    throw new ArgumentException("站点不存在，保存失败");
                }

                //清理缓存
                RepositoryDataCache._siteDict = null;
                RepositoryDataCache._categories = null;
            }

            return site.Id;
        }

        public IList<ISite> GetSites()
        {
            if (RepositoryDataCache._siteDict == null)
            {
                RepositoryDataCache._siteDict = new Dictionary<int, ISite>();
                siteDal.LoadSites(rd =>
               {
                   while (rd.Read())
                   {
                       ISite site = this.CreateSite(Convert.ToInt32(rd["siteid"]), rd["name"].ToString());



                       //rd.CopyToEntity<ISite>(site);
                       site.DirName = rd["dirname"].ToString();
                       site.RunType = String.IsNullOrEmpty(site.DirName)
                           ? SiteRunType.Stand
                           : SiteRunType.VirtualDirectory;

                       site.Tpl = rd["tpl"].ToString();
                       site.State =(SiteState)int.Parse(rd["state"].ToString());

                       site.Location = rd["location"].ToString();
                       if (site.Location!= null  && site.Location.Trim() == "") site.Location = null;

                       site.Domain = rd["domain"].ToString();
                       site.Address = rd["proaddress"].ToString();
                       site.Email = rd["proemail"].ToString();
                       site.Fax = rd["profax"].ToString();
                       site.PostCode = rd["postcode"].ToString();
                       site.Note = rd["note"].ToString();
                       site.Notice = rd["pronotice"].ToString();
                       site.Phone = rd["prophone"].ToString();
                       site.Im = rd["im"].ToString();
                       site.SeoTitle = rd["seotitle"].ToString();
                       site.SeoKeywords = rd["seokeywords"].ToString();
                       site.SeoDescription = rd["seodescription"].ToString();
                       site.Slogan = rd["proslogan"].ToString();
                       site.Tel = rd["protel"].ToString();
                       site.Language = (Languages)int.Parse(rd["language"].ToString());

                       RepositoryDataCache._siteDict.Add(site.Id, site);
                   }
               });

            }
            return RepositoryDataCache._siteDict.Values.ToList();
        }


        public ISite GetSiteByUri(Uri uri)
        {
            ISite site = null;
            GetSiteByUri(uri, ref site);
            return site;
        }

        public ISite GetSiteById(int siteId)
        {
            IList<ISite> sites = this.GetSites();
            return BinarySearch.IntSearch(sites, 0, sites.Count, siteId, a => a.Id);
            return sites[0];

            //            foreach(ISite site in sites)
            //            {
            //                if (site.Id == siteId) return site;
            //            }
            //            throw new Exception("站点不存在");
        }

        public ISite GetSingleOrDefaultSite(Uri uri)
        {
            ISite site = null;
            IList<ISite> sites = GetSiteByUri(uri, ref site);
            if (site == null)
            {
                if (sites.Count == 0)
                    throw new Exception("系统缺少站点!");

                //获取host和dir均为空的站点
                foreach (ISite _site in sites)
                {
                    if (_site.Id == 7)
                    {
                        return _site;
                    }
                    if (_site.Domain == "" && _site.DirName == "")
                    {
                        return _site;
                    }
                }

                return sites[0];
            }
            return site;
        }

        private IList<ISite> GetSiteByUri(Uri uri, ref ISite site)
        {

            string hostName = uri.Host;
            IList<ISite> sites = this.GetSites();

            //获取使用域名标识的网站
            string _hostName = String.Concat(
                "^",
                hostName.Replace(".", "\\."),
                "$|\\s+",
                hostName.Replace(".", "\\."),
                "\\s*|\\s*",
                hostName.Replace(".", "\\."),
                "\\s+");

            foreach (ISite s in sites)
            {
                if (!String.IsNullOrEmpty(s.Domain))
                {
                    if (Regex.IsMatch(s.Domain, _hostName, RegexOptions.IgnoreCase))
                    {
                        site = s;
                        site.RunType = SiteRunType.Stand;
                        break;
                    }
                }

            }

            //获取使用目录绑定的网站
            if (site == null)
            {
                string[] segments = uri.Segments;
                if (segments.Length >= 2)
                {
                    string dirName = segments[1].Replace("/", "");
                    foreach (ISite s in sites)
                    {
                        if (String.Compare(s.DirName, dirName,true,CultureInfo.InvariantCulture) == 0)
                        {
                            site = s;
                            site.RunType = SiteRunType.VirtualDirectory;
                            break;
                        }
                    }
                }
            }
            return sites;
        }


        public ISiteLink CreateLink(ISite site, int id, string text)
        {
            return base.CreateLink(this, site, id, text);
        }


        public int SaveSiteLink(int siteId, ISiteLink link)
        {
            if(link.Id <=0 )
            {
               return linkDal.AddSiteLink(siteId, link);
            }

            return linkDal.UpdateSiteLink(siteId, link);
        }

        public ISiteLink ConvertToILink(int siteId, DbDataReader reader)
        {
            ISiteLink link = this.CreateLink(
                       this.GetSiteById(siteId),
                       int.Parse(reader["id"].ToString()),
                       reader["text"].ToString()
                       );

            link.Bind = reader["bind"].ToString();
            link.ImgUrl = reader["imgUrl"].ToString();
            link.OrderIndex = int.Parse(reader["orderIndex"].ToString());
            link.Pid = int.Parse(reader["pid"].ToString());
            link.Target = reader["target"].ToString();
            link.Type = (SiteLinkType)int.Parse(reader["type"].ToString());
            link.Uri = reader["uri"].ToString();
            link.Visible = Convert.ToBoolean(reader["visible"]);


            return link;
        }

        public bool DeleteSiteLink(int siteId, int linkId)
        {
            return linkDal.DeleteSiteLink(siteId, linkId) == 1;
        }


        public ISiteLink GetSiteLinkById(int siteId, int linkId)
        {
            ISiteLink link = null;
            linkDal.GetSiteLinkById(siteId, linkId, rd =>
            {
                if (rd.Read())
                {
                    link = this.ConvertToILink(siteId, rd);
                }
            });

            return link;
        }

        public IEnumerable<ISiteLink> GetSiteLinks(int siteId, SiteLinkType type)
        {
            IList<ISiteLink> links = new List<ISiteLink>();
            linkDal.GetAllSiteLinks(siteId, type, rd =>
            {
                while(rd.Read())
                {
                    links.Add( this.ConvertToILink(siteId,rd));
                }
            });
            return links;
        }
    }
}
