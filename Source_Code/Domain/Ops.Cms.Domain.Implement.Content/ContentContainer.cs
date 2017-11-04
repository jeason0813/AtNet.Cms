﻿using System;
using System.Collections.Generic;
using AtNet.Cms.Domain.Interface.Content;
using AtNet.Cms.Domain.Interface.Content.Archive;
using AtNet.Cms.Domain.Interface.Site.Template;

namespace AtNet.Cms.Domain.Implement.Content
{
    public class ContentContainer : IContentContainer
    {
        private IArchiveRepository _archiveRep;
        private ITemplateRepository _tempRep;
        internal ContentContainer(
            IArchiveRepository archiveRep,
            ITemplateRepository tempRep,
            int siteId)
        {

            this._archiveRep = archiveRep;
            this._tempRep = tempRep;
            this.SiteId = siteId;
        }
        public int Id
        {
            get { return this.SiteId; }
        }

        public int SiteId
        {
            get;
            private set;
        }

        public IArchive CreateArchive(int id,string strId,int categoryId,string title)
        {
            return _archiveRep.CreateArchive(id, strId, categoryId, title);
        }


        public IArchive GetArchive(string id)
        {
            return this._archiveRep.GetArchive(this.SiteId, id);
        }


        public IArchive GetArchiveById(int archiveId)
        {
            return this._archiveRep.GetArchiveById(this.SiteId, archiveId);
        }


        public IEnumerable<IArchive> GetArchivesByCategoryTag(string categoryTag, int number)
        {
            return this._archiveRep.GetArchivesByCategoryTag(this.SiteId,categoryTag, number);
        }

        public IEnumerable<IArchive> GetArchivesContainChildCategories(int lft,int rgt, int number)
        {
            return this._archiveRep.GetArchivesContainChildCategories(this.SiteId, lft,rgt, number);
        }

        public IEnumerable<IArchive> GetArchivesByModuleId(int moduleId, int number)
        {
            return this._archiveRep.GetArchivesByModuleId(this.SiteId, moduleId, number);
        }

        public IEnumerable<IArchive> GetArchivesByViewCount(int lft,int rgt, int number)
        {
            return this._archiveRep.GetArchivesByViewCount(this.SiteId, lft,rgt, number);
        }

        public IEnumerable<IArchive> GetArchivesByViewCount(string categoryTag, int number)
        {
            return this._archiveRep.GetArchivesByViewCount(this.SiteId, categoryTag, number);
        }

        public IEnumerable<IArchive> GetSpecialArchivesByModuleId(int moduleId, int number)
        {
            return this._archiveRep.GetSpecialArchivesByModuleId(this.SiteId, moduleId, number);
        }

        public IEnumerable<IArchive> GetSpecialArchives(int lft,int rgt, int number)
        {
            return this._archiveRep.GetSpecialArchives(this.SiteId, lft,rgt, number);
        }

        public IEnumerable<IArchive> GetSpecialArchives(string categoryTag, int number)
        {
            return this._archiveRep.GetSpecialArchives(this.SiteId, categoryTag, number);
        }

        public IEnumerable<IArchive> GetArchivesByViewCountByModuleId(int moduleId, int number)
        {
            return this._archiveRep.GetArchivesByViewCountByModuleId(this.SiteId, moduleId, number);
        }


        public IArchive GetPreviousSiblingArchive(int id)
        {
            return this._archiveRep.GetPreviousArchive(this.SiteId, id,true,false);
        }

        public IArchive GetNextSiblingArchive(int id)
        {
            return this._archiveRep.GetNextArchive(this.SiteId, id,true,false);
        }

        public void RefreshArchive(int archiveId)
        {
            this._archiveRep.RefreshArchive(this.SiteId, archiveId);
        }

        public bool DeleteArchive(int archiveId)
        {
            IArchive archive = this.GetArchiveById(archiveId);
            if (archive == null)
                return false;
            if (ArchiveFlag.GetFlag(archive.Flags, BuiltInArchiveFlags.IsSystem))
            {
                throw new NotSupportedException("系统文档，不允许删除,请先取消系统设置后再进行删除！");
            }
            bool result = this._archiveRep.DeleteArchive(this.SiteId, archive.Id);

            if (result)
            {
                //删除模板绑定
                this._tempRep.RemoveBind(TemplateBindType.ArchiveTemplate, archive.Id);


                //
                //TODO:删除评论及点评
                //

                //删除评论
               // new CommentDAL().DeleteArchiveComments(archiveID);

                //删除点评
                //new CommentBLL().DeleteArchiveReviews(archiveID);

            }

            archive = null;

            return result;
        }




        public IEnumerable<IArchive> SearchArchivesByCategory(
            int categoryLft, int categoryRgt,
            string keyword, int pageSize,
            int pageIndex, out int records,
            out int pages, string orderBy)
        {
            return this._archiveRep.SearchArchivesByCategory(
                this.SiteId, categoryLft, categoryRgt, keyword, pageSize,
            pageIndex, out  records, out pages, orderBy);
        }

        public IEnumerable<IArchive> SearchArchives(
            string keyword, int pageSize, 
            int pageIndex, out int records,
            out int pages, string orderBy)
        {
            return this._archiveRep.SearchArchives(
                this.SiteId,keyword, pageSize,pageIndex, 
                 out  records, out pages, orderBy);
        }


        public void AddCountForArchive(int id, int count)
        {
            this._archiveRep.AddArchiveViewCount(this.SiteId, id, count);
        }



        public IBaseContent GetContent(string typeIndent, int contentId)
        {
            IBaseContent content = null;

            switch (typeIndent.ToLower())
            {
                case "1":
                case "archive":
                    //content = this._archiveRep.CreateArchive(contentId, null, -1, null);
                    content = this._archiveRep.GetArchiveById(this.SiteId, contentId);
                    break;
            }
            if (content == null)
            {
                throw new Exception("内容不存在");
            }
            return content;
        }
    }
}
