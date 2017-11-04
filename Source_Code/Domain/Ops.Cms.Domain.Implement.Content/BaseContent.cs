﻿using System;
using System.Collections.Generic;
using AtNet.Cms.Domain.Interface.Common;
using AtNet.Cms.Domain.Interface.Content;
using AtNet.Cms.Domain.Interface.Site.Category;
using AtNet.Cms.Domain.Interface.Site.Extend;
using AtNet.Cms.Domain.Interface.Site.Template;

namespace AtNet.Cms.Domain.Implement.Content
{
    public abstract class BaseContent : IBaseContent
    {
        protected readonly IExtendFieldRepository _extendRep;
        private readonly ITemplateRepository _templateRep;
        private readonly ICategoryRepository _categoryRep;

        protected IList<IExtendValue> _extendValues;
        private  ICategory _category;
        private readonly ITemplateBind _templateBind;
        private readonly IContentRepository _contentRep;
        private readonly ILinkRepository _linkRep;
        private  ILinkManager _linkManager;

        /// <summary>
        /// 内容模型标识
        /// </summary>
        public abstract int ContentModelIndent { get; }

        public BaseContent(
            IContentRepository contentRep,
            IExtendFieldRepository extendRep,
            ICategoryRepository categoryRep,
            ITemplateRepository templateRep,
            ILinkRepository linkRep,
            int id,
            int categoryId,
            string title)
        {
            this._contentRep = contentRep;
            this._linkRep = linkRep;
            this._extendRep = extendRep;
            this._categoryRep = categoryRep;
            this._templateRep = templateRep;

            this.Id = id;
            this._category = this._categoryRep.CreateCategory(categoryId, null);
            this.Title = title;
            this.Id = id;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 小标题
        /// </summary>
        public String SmallTitle { get; set; }

        /// <summary>
        /// 栏目编号
        /// </summary>
        public ICategory Category
        {
            get
            {
                if (this._category.Site == null)
                {
                    this._category = this._categoryRep.GetCategoryById(this._category.Id);
                }
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }

        /// <summary>
        /// 标签（关键词）
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 文档内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 显示次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int SortNumber { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifyDate { get; set; }


        public abstract string Uri { get; set; }

        public string Location { get; set; }

        public string Author
        {
            get;
            set;
        }

        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 保存内容，继承类应重写该类
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            this.LinkManager.SaveLinks();
            return -1;
        }

        public ILinkManager LinkManager
        {
            get
            {
                return this._linkManager
                    ?? (this._linkManager =
                    new ContentLinkManager(this._linkRep, this.ContentModelIndent, this.Id));
            }
        }

        /// <summary>
        /// 下移排序
        /// </summary>
        public abstract void SortLower();


        /// <summary>
        /// 上移排序
        /// </summary>
        public abstract void SortUpper();
    }
}
