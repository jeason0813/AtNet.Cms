﻿/**
 * Copyright (C) 2007-2015 S1N1.COM,All rights reseved.
 * Get more infromation of this software,please visit site http://cms.ops.cc
 * 
 * name : Site.cs
 * author : newmin (new.min@msn.com)
 * date : 2014/12/01 23:00:00
 * description : 
 * history : 
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AtNet.Cms.Domain.Implement.Site.Extend;
using AtNet.Cms.Domain.Implement.Site.Link;
using AtNet.Cms.Domain.Interface;
using AtNet.Cms.Domain.Interface.Common.Language;
using AtNet.Cms.Domain.Interface.Site;
using AtNet.Cms.Domain.Interface.Site.Category;
using AtNet.Cms.Domain.Interface.Site.Extend;
using AtNet.Cms.Domain.Interface.Site.Link;
using AtNet.Cms.Domain.Interface.Site.Template;
using AtNet.Cms.Infrastructure;
using AtNet.Cms.Infrastructure.Tree;

namespace AtNet.Cms.Domain.Implement.Site
{
    internal class Site:ISite
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IExtendFieldRepository _extendRepository;
        private readonly ICategoryRepository _categoryRep;
        private IExtendManager _extendManager;
        private IList<ICategory> _categories;
        private readonly ITemplateRepository _tempRep;
        private ISiteLinkManager _linkManager;
        private string _fullDomain;

        internal Site(ISiteRepository siteRepository,
            IExtendFieldRepository extendRepository,
            ICategoryRepository categoryRep,
            ITemplateRepository tempRep,
            int siteId,
            string name)
        {
            this.Id = siteId;
            this.Name = name;
            this._siteRepository = siteRepository;
            this._categoryRep = categoryRep;
            this._extendRepository = extendRepository;
            this._tempRep = tempRep;
        }


        /// <summary>
        /// 站点ID
        /// </summary>
       public int Id { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
       public string Name { get; private set; }


        /// <summary>
        /// 目录名称
        /// </summary>
       public string DirName { get; set; }

        /// <summary>
        /// 域名绑定
        /// </summary>
       public string Domain { get; set; }

        /// <summary>
       /// 重定向地址
        /// </summary>
       public string Location
       {
           get;
           set;
       }

        /// <summary>
        /// 
        /// </summary>
       public string FullDomain
       {
           get
           {
               if (this._fullDomain == null)
               {
                   string host = String.IsNullOrEmpty(this.Domain)? "#": this.Domain;
                   string appPath = HttpApp.GetApplicationPath();
                   string siteAppPath;

                   switch (this.RunType)
                   {
                       default:
                       case SiteRunType.Stand:
                           siteAppPath = "/";
                           break;

                        case SiteRunType.VirtualDirectory:
                           siteAppPath = "/" + this.DirName+"/";
                           break;
                   }


                   this._fullDomain = String.Format("http://{0}{1}{2}",
                                   host,
                                   appPath == "/" ? "" : appPath,
                                  siteAppPath
                                  );
               }

               return this._fullDomain;
           }
       }

        /// <summary>
        /// 站点使用语言
        /// </summary>
       public Languages Language { get; set; }


        /// <summary>
        /// 模板
        /// </summary>
       public string Tpl { get; set; }

        /// <summary>
        /// 站点备注
        /// </summary>
       public string Note { get; set; }

        /// <summary>
        /// 站点状态
        /// </summary>
       public SiteState State { get; set; }

        /// <summary>
        /// SEO标题
        /// </summary>
       public string SeoTitle { get; set; }

        /// <summary>
        /// SEO关键字
        /// </summary>
       public string SeoKeywords { get; set; }

        /// <summary>
        /// SEO描述
        /// </summary>
       public string SeoDescription { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
       public string Tel { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
       public string Phone { get; set; }

        /// <summary>
        /// 传真号码
        /// </summary>
       public string Fax { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
       public string Address { get; set; }


        /// <summary>
        /// 电子邮箱
        /// </summary>
       public string Email { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
       public string Im { get; set; }

        /// <summary>
        /// MSN账号
        /// </summary>
       public string PostCode { get; set; }

        /// <summary>
        /// 网站公告
        /// </summary>
       public string Notice { get; set; }

        /// <summary>
        /// 网站标语
        /// </summary>
       public string Slogan { get; set; }


       public int Save()
       {
          return _siteRepository.SaveSite(this);
       }


       public SiteRunType RunType
       {
           get;
           set;
       }


       public IExtendManager Extend
       {
           get {
               return _extendManager ?? (_extendManager = new ExtendManager(this._extendRepository,this.Id));
           }
       }

        public ISiteLinkManager LinkManager
       {
           get
           {
               return _linkManager ?? (_linkManager = new SiteLinkManager(this._siteRepository,this));
           }
       }


       public IList<ICategory> Categories
       {
           get {
               return _categories ?? (_categories = this._categoryRep.GetCategories(this.Id));
           }
       }

       public ICategory RootCategory
       {
           get
           {
               //
               //NOTO:应为Lft最小的一个，但分类已按Lft排序，所以获取第一个
               //
               ICategory category = this.Categories.FirstOrDefault();
               if (category == null)
               {
                   throw new Exception("站点栏目信息异常!");
               }
               return category;
           }
       }


        public ICategory GetCategory(int categoryId)
        {
            int lft = this._categoryRep.GetCategoryLftById(this.Id, categoryId);
            if (lft > 0)
            {
                return BinarySearch.IntSearch(this.Categories, 0, this.Categories.Count, lft, a => a.Lft);
            }

            foreach (ICategory c in this.Categories)
            {
                if (c.Id == categoryId) return c;
            }
            return null;
        }



        public IEnumerable<ICategory> GetCategories(int lft, int rgt, CategoryContainerOption option)
       {
           return this._categoryRep.GetCategories(this.Id,lft, rgt, option);
       }

       public ICategory GetCategoryByTag(string categoryTag)
       {
           //如果包含映射，则二分查找
           int lft = this._categoryRep.GetCategoryLftByTag(this.Id, categoryTag);
           if (lft != -1)
           {
               return BinarySearch.IntSearch(this.Categories, 0, this.Categories.Count, lft, a => a.Lft);
           }

           //如果没有找到映射
           // foreach (ICategory category in this.Categories)
           //{
           //   if (string.Compare(category.Tag, categoryTag, true, CultureInfo.InvariantCulture) == 0)
           //       return category;
           //}
           return null;
       }




       public ICategory GetCategoryByName(string categoryName)
       {
           foreach (ICategory category in this.Categories)
           {
               if (String.Compare(category.Name, categoryName, true, CultureInfo.InvariantCulture) == 0) return category;
           }
           return null;
       }
       public ICategory GetCategoryByLft(int lft)
       {
           return BinarySearch.IntSearch(this.Categories, 0, this.Categories.Count, lft, a => a.Lft);
       }

       public bool DeleteCategory(int lft)
       {

           //
           //TODO:还需要删除模板
           //

           ICategory category=this.GetCategoryByLft(lft);

           if(category.Childs.Count()!=0)
           {
               throw new Exception("栏目包含子栏目!");
           }

           if (this._categoryRep.GetArchiveCount(this.Id, lft, category.Rgt) != 0)
           {
               throw new Exception("栏目包含文档!");
           }

           this._categoryRep.DeleteCategory(this.Id, lft, category.Rgt);
           this.Categories.Remove(category);

           foreach (ITemplateBind bind in category.Templates)
           {
               this._tempRep.RemoveBind(bind.BindType, category.Id);
           }

           category = null;

           return true;
       }


       public void ItreCategoryTree(StringBuilder sb, int categoryLft)
       {

           //
           //TODO:需要重构
           //

           int lft=categoryLft,rgt;
           ICategory category = this.GetCategoryByLft(categoryLft);
           rgt=category.Rgt;
           int level = this.GetCategories(lft,rgt, CategoryContainerOption.Parents).Count();

           //if (level >= 2) return;  //到2级就跳过了
           bool iscollage = level >= 2;

           if (lft != 1) sb.Append("<dd>");

           IEnumerable<ICategory> cates = this.GetCategories(lft, rgt, CategoryContainerOption.NextLevel);
           ICategory nextCategory = category.Next;
           ICategory tempCategory = null;
           ICategory parentNextCategory = null;
           string className = "";

           //获取父类及父类的下一个类目
           if (lft != 1)
           {
               tempCategory = category.Parent;
               parentNextCategory = tempCategory.Next;
           }


           if (cates.Count() != 0)
           {
               if (lft != 1)
               {
                   sb.Append("<dl><dt")
                       .Append(" lft=\"").Append(lft.ToString()).Append("\" level=\"")
                       .Append(level.ToString()).Append("\">");

                   for (int i = 0; i < level; i++)
                   {
                       //最后一个竖线不显示
                       if (i != 0 && nextCategory == null 
                           && (i != level - 1 || (i == level - 1 && parentNextCategory == null))
                           && category.Childs.Count()==0
                           )
                       {
                           sb.Append("<img src=\"framework/assets/sys_themes/default/icon_trans.png\" width=\"24\" height=\"24\"/>");
                       }
                       else
                       {
                           sb.Append("<img class=\"tree-line\" src=\"framework/assets/sys_themes/default/icon_trans.png\" width=\"24\" height=\"24\"/>");
                       }
                   }

                   //tree-expand:已展开
                   //tree-expand-last:已经展开最后一个

                   //tree-collage:未开展
                   //tree-collage:未展开最后一个

                   if (level == 0)
                   {
                       if (nextCategory == null)
                       {
                           className = "tree-expand tree-expand-last";
                       }
                       else
                       {
                           className = "tree-expand";
                       }
                   }
                   else
                   {
                       if (nextCategory == null)
                       {
                           className = "tree-collage tree-collage-last";
                       }
                       else
                       {
                           className = "tree-collage";
                       }
                   }


                   sb.Append("<img class=\"").Append(className)
                   .Append("\" src=\"framework/assets/sys_themes/default/icon_trans.png\" width=\"24\" height=\"24\"/><span class=\"txt parent\" cid=\"")
                   .Append(category.Id.ToString()).Append("\" lft=\"")
                   .Append(category.Lft.ToString()).Append("\">")
                   .Append(category.Name).Append("</span></dt>");
               }

               foreach (var c in cates)
               {
                   this.ItreCategoryTree(sb, c.Lft);
               }
               if (lft != 1)
               {
                   sb.Append("</dl>");
               }
           }
           else
           {
               if (lft != 1) { 
               for (int i = 0; i < level; i++)
               {
                   if (i != 0 && i == level - 1 && nextCategory == null)
                   {
                       sb.Append("<img src=\"framework/assets/sys_themes/default/icon_trans.png\"/>");
                   }
                   else if (parentNextCategory != null)
                   {
                       sb.Append("<img class=\"tree-line\" src=\"framework/assets/sys_themes/default/icon_trans.png\" width=\"24\" height=\"24\"/>");
                   }
                   else
                   {
                       sb.Append("<img src=\"framework/assets/sys_themes/default/icon_trans.png\" width=\"24\" height=\"24\"/>");
                   }
               }

               sb.Append("<img class=\"tree-item\" src=\"framework/assets/sys_themes/default/icon_trans.png\" width=\"24\" height=\"24\"/><span class=\"txt archvie\" cid=\"")
                   .Append(category.Id.ToString()).Append("\">").Append(category.Name).Append("</span>");
               }
           }

           if (lft != 1)
           {
               sb.Append("</dd>");
           }
       }


       public void HandleCategoryTree(int lft, CategoryTreeHandler treeHandler)
       {
           /*
             * 好吧，现在整个树都在一个查询中了。现在就要像前面的递归函数那样显示这个树，
             * 我们要加入一个ORDER BY子句在这个查询中。如果你从表中添加和删除行，你的表可能就顺序不对了，
             * 我们因此需要按照他们的左值来进行排序。
             * 
             * SELECT * FROM tree WHERE lft BETWEEN 2 AND 11 ORDER BY lft ASC;
             * 
             * 就只剩下缩进的问题了。要显示树状结构，子节点应该比他们的父节点稍微缩进一些。
             * 我们可以通过保存一个右值的一个栈。每次你从一个节点的子节点开始时，
             * 你把这个节点的右值 添加到栈中。你也知道子节点的右值都比父节点的右值小，
             * 这样通过比较当前节点和栈中的前一个节点的右值，你可以判断你是不是在
             * 显示这个父节点的子节点。当你显示完这个节点，你就要把他的右值从栈中删除。
             * 要获得当前节点的层数，只要数一下栈中的元素。
             * 
             * 
             */

           int rootLft,
               rootRgt;

           IList<int> arr = new List<int>();

           // 获得root节点的左边和右边的值
           ICategory root = this.GetCategoryByLft(lft);

           if (root == null)
               throw new Exception("栏目不存在!");

           rootLft = root.Lft;
           rootRgt = root.Rgt;

           //获取root节点的所有子节点
           IEnumerable<ICategory> childNodes =this.GetCategories(root.Lft, root.Rgt, CategoryContainerOption.Childs);
           /* SELECT * FROM tree WHERE lft BETWEEN @rootLft AND @rootRgt ORDER BY lft ASC'); */

           foreach (ICategory c in childNodes)
           {
               if (arr.Count > 0)
               {
                   //判断最后一个值是否小于
                   int i;
                   while ((i = arr[arr.Count - 1]) < c.Rgt)
                   {
                       arr.Remove(i);
                       if (arr.Count == 0) break;
                   }
               }


               //树的层级= 列表arr的数量
               treeHandler(c, arr.Count);

               //把所有栏目的右值,再加入到列表中
               arr.Add(c.Rgt);
           }


           // int right = left + 1;

           //func(category);
           // foreach (Category c in this.GetCategories(a => a.PID == category.ID))
           // {
           //    Foreach(c, func);
           // }
       }

       /// <summary>
       /// 获取栏目树Json格式
       /// </summary>
       /// <returns></returns>
       public TreeNode GetCategoryTree(int lft)
       {
           TreeNode node;
           ICategory root = lft == 1 ?
               this.RootCategory :
               this.GetCategoryByLft(lft);

           node = lft == 1 ?
               new TreeNode(this.Name, "1", "javascript:;",true, "") :
               new TreeNode(root.Name, String.Format("{0}cid:{1},lft:1{3}", "{", root.Id.ToString(), "}"),
                            "javascript:;", true, "");

           ItrNodeTree(node, root);

           return node;
       }

       private void ItrNodeTree(TreeNode node, ICategory root)
       {
           TreeNode tnode;
           foreach (ICategory c in root.NextLevelChilds)
           {
               tnode = new TreeNode(c.Name, 
                   String.Format("{0}cid:{1},lft:{2}{3}","{",c.Id.ToString(),c.Lft.ToString(),"}"),
                   "javascript:;",true,
                   "");
               node.childs.Add(tnode);
               ItrNodeTree(tnode, c);
           }
       }


       public void ClearSelf()
       {
           this._categories = null;
       }




    }
}
