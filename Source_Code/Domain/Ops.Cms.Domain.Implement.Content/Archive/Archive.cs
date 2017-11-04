﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AtNet.Cms.Domain.Interface.Common;
using AtNet.Cms.Domain.Interface.Content;
using AtNet.Cms.Domain.Interface.Content.Archive;
using AtNet.Cms.Domain.Interface.Site.Category;
using AtNet.Cms.Domain.Interface.Site.Extend;
using AtNet.Cms.Domain.Interface.Site.Template;

//
// 2012-10-01 添加文档扩展属性
// 2013-06-09 14:00 newmin [+]: Thumbnail
//


namespace AtNet.Cms.Domain.Implement.Content.Archive
{
    /// <summary>
    /// 文档
    /// </summary>
    public sealed class Archive : BaseContent, IArchive
    {
        private ITemplateRepository _templateRep;
        private ITemplateBind _templateBind;
        private string _firstImageUrl;
        private IArchiveRepository _archiveRep;
        private string _uri;

        internal Archive(
            IContentRepository contentRep,
            IArchiveRepository archiveRep,
            ILinkRepository linkRep,
            IExtendFieldRepository extendRep,
            ICategoryRepository categoryRep,
            ITemplateRepository templateRep,
            int id,
            string strId,
            int categoryId,
            string title)
            : base(
                contentRep,
                extendRep,
                categoryRep,
                templateRep,
                linkRep,
                id,
                categoryId,
                title
                )
        {
            this.StrId = strId;
            this._archiveRep = archiveRep;
            this._templateRep = templateRep;
        }

        public override int ContentModelIndent
        {
            get { return 1; }
        }

        /// <summary>
        /// 文章别名
        /// </summary>
        public string Alias { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public string Flags { get; set; }


        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 大纲,导读
        /// </summary>
        public string Outline { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 支持数
        /// </summary>
        public int Agree { get; set; }

        /// <summary>
        /// 反对数
        /// </summary>
        public int Disagree { get; set; }


        public ITemplateBind Template
        {
            get
            {
                if (this._templateBind == null)
                {
                    //if (!this._templateBindLoaded)
                    //{
                    this._templateBind = this._templateRep.GetTemplateBind(
                        this.Id,
                        TemplateBindType.ArchiveTemplate);

                    //如果没有的话，则获取分类的模板
                    if (this._templateBind == null)
                    {
                        foreach (ITemplateBind tplBind in this.Category.Templates)
                        {
                            if (tplBind.BindType == TemplateBindType.CategoryArchiveTemplate)
                            {
                                this._templateBind = tplBind;
                                break;
                            }
                        }
                    }

                    //this._templateBindLoaded=true;
                }
                return this._templateBind;
            }
            set
            {
                _templateBind = value;
            }
        }

        public void UpdateTemplateBind(string templatePath)
        {
            bool templateIsNull = String.IsNullOrEmpty(templatePath);

            if (this.Template != null)
            {
                //处理自身的模板
                if (this.Template.BindRefrenceId == this.Id)
                {
                    if (templateIsNull)
                    {
                        this.Template.TplPath = null;
                    }
                    else
                    {
                        this.Template.TplPath = templatePath;
                    }
                    return;
                }
            }

            //为文档新建模板绑定
            if (!templateIsNull)
            {
                this.Template = this._templateRep.CreateTemplateBind(
                       -1,
                       TemplateBindType.ArchiveTemplate,
                       templatePath);
                this.Template.BindRefrenceId = this.Id;
            }
        }

        public IList<IExtendValue> ExtendValues
        {
            get
            {
                return _extendValues ?? (_extendValues = new List<IExtendValue>(this._extendRep.GetExtendFieldValues(this)));
            }
            set
            {
                _extendValues = value;
            }
        }

        public override int Save()
        {
            //初始化
            if (this.Id<=0)
            {
                int sortNum = this._archiveRep.GetMaxSortNumber(this.Category.Site.Id);
                if (this.SortNumber == 0)
                {
                    this.SortNumber = sortNum + 1;
                }
            }

            this.Id = this._archiveRep.SaveArchive(this);
            this._extendRep.UpdateArchiveRelationExtendValues(this);

            //保存文档绑定的模板
            if (this._templateBind != null)
            {
                if (this._templateBind.BindRefrenceId == this.Id)
                {
                    this._templateRep.SaveTemplateBind(this._templateBind, this.Id);

                    if (this._templateBind.TplPath == null)
                    {
                        this._templateBind = null;
                    }
                }
            }

            //保存其他
            base.Save();

            return this.Id;
        }

        public override void SortLower()
        {
            int siteId = this.Category.Site.Id;
            IArchive prev = this._archiveRep.GetNextArchive(siteId, this.Id,true,true);
            this.SwapSortNumber(prev);
        }


        public override void SortUpper()
        {
            int siteId = this.Category.Site.Id;
            IArchive next = this._archiveRep.GetPreviousArchive(siteId, this.Id,true,true);

            this.SwapSortNumber(next);
        }

        private void SwapSortNumber(IArchive archive)
        {
            if (archive == null) return;
            int sortN = archive.SortNumber;
            archive.SortNumber = this.SortNumber;
            this.SortNumber = sortN;

            archive.SaveSortNumber();
            this.Save();
        }

        public string FirstImageUrl
        {
            get
            {
                return "";
                const string imgTagRegPattern = "<img[^>]*\\bsrc=\"(?<imguri>[^\"]+)\"[^>]*>";
                const bool ignoreBase64 = true;

                //用""来表示为空
                if (this._firstImageUrl == null)
                {
                    Regex reg = new Regex(imgTagRegPattern);

                    if (reg.IsMatch(this.Content))
                    {
                        //忽略base64格式的图片
                        if (ignoreBase64)
                        {
                            //匹配结果
                            string matchResult;
                            MatchCollection mcs = reg.Matches(this.Content);
                            foreach (Match match in mcs)
                            {
                                matchResult = match.Groups["imguri"].Value;
                                if (!Regex.IsMatch(matchResult, "^data:image/[a-z]+;base64", RegexOptions.IgnoreCase))
                                {
                                    this._firstImageUrl = matchResult;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this._firstImageUrl = reg.Match(this.Content).Groups["imguri"].Value;
                        }
                    }

                    if (this._firstImageUrl == null) this._firstImageUrl = "";
                }

                return this._firstImageUrl == "" ? null : this._firstImageUrl;
            }
        }


        public string StrId
        {
            get;
            private set;
        }

        public override string Uri
        {
            get
            {
                if (this._uri == null)
                {
                    if (String.IsNullOrEmpty(this.Location))
                    {
                        this._uri = this.Location;
                    }
                    else
                    {
                        this._uri = String.Concat(
                            this.Category.UriPath,
                            "/", String.IsNullOrEmpty(this.Alias) ? this.StrId : this.Alias
                            , ".html");
                    }
                }
                return this._uri;
            }
            set { this._uri = value; }
        }


        public void SaveSortNumber()
        {
            //UPDATE cms_archive SET sort_number = id
            this._archiveRep.SaveSortNumber(this.Id, this.SortNumber);
        }
    }
}
