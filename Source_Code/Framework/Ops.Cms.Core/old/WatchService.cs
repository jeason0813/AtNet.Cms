﻿//
// Copyright 2011 (C) OPS ,All right reseved.
// Name : WatchService.cs
// Author: newmin
// Create: 2011/07/25
//

namespace Ops.Cms
{
    using Ops.Cms.DataTransfer;
    using Ops.Cms.Infrastructure;
    using Spc.Models;


    /// <summary>
    /// 监视服务
    /// </summary>
    public class WatchService
    {
        /// <summary>
        /// 当更新缓存时发生
        /// </summary>
        public static event WatchBehavior OnClearingCache;

        /// <summary>
        /// 当文档发布后发生
        /// </summary>
        public static event ArchiveHandler OnArchivePublished;


        /// <summary>
        /// 当文档删除之前发生
        /// </summary>
        public static event ArchiveHandler OnPrevArchiveDelete;

        /// <summary>
        /// 当文档删除后发生
        /// </summary>
        public static event ArchiveHandler OnArchiveDeleted;




        /// <summary>
        /// 当文档更新之前发生
        /// </summary>
        public static event ArchiveHandler OnPrevArchiveUpdate;

        /// <summary>
        /// 当文档更新后发生
        /// </summary>
        public static event ArchiveHandler OnArchiveUpdated;



        /// <summary>
        /// 当文档显示后发生
        /// </summary>
        public static event ArchiveHandler OnArchiveViewed;

        /// <summary>
        /// 当栏目创建后发生
        /// </summary>
        public static event CategoryBahavior OnCategoryCreated;

        /// <summary>
        /// 当栏目更改后发生
        /// </summary>
        public static event CategoryBahavior OnCategoryChanged;

        /// <summary>
        /// 当栏目被移除后发生
        /// </summary>
        public static event CategoryBahavior OnCategoryRemoved;



        #region 系统操作

        public static void ClearCache()
        {
            if (OnClearingCache != null) OnClearingCache();
        }
        #endregion


        #region 文档

        /// <summary>
        /// 执行发布文档操作
        /// </summary>
        /// <param name="archive"></param>
        public static void PublishArchive(ArchiveDto archive)
        {
            if (OnArchivePublished != null)OnArchiveUpdated(archive);
        }
        
        /// <summary>
        /// 删除文档前发生
        /// </summary>
        /// <param name="archive"></param>
        public static void PrevDeleteArchive(ArchiveDto archive)
        {
            if (OnPrevArchiveDelete != null) OnPrevArchiveDelete(archive);
        }
        /// <summary>
        /// 执行删除文章操作
        /// </summary>
        /// <param name="archive"></param>
        public static void DeleteArchive(ArchiveDto archive)
        {
            if (OnArchiveDeleted != null) OnArchiveDeleted(archive);
        }

        /// <summary>
        /// 更新文档之前发生
        /// </summary>
        /// <param name="archive"></param>
        public static void PrevUpdateArchive(ArchiveDto archive)
        {
            if (OnPrevArchiveUpdate != null) OnPrevArchiveUpdate(archive);
        }
        /// <summary>
        /// 执行文档更新操作
        /// </summary>
        /// <param name="archive"></param>
        public static void UpdateArchive(ArchiveDto archive)
        {
            if (OnArchiveUpdated != null) OnArchiveUpdated(archive);
        }

        /// <summary>
        /// 执行文档查看操作
        /// </summary>
        /// <param name="archive"></param>
        public static void ViewArchive(ArchiveDto archive)
        {
            if (OnArchiveViewed != null) OnArchiveViewed(archive);
        }

        #endregion

        #region 栏目

        public static void CategoryCreated(Category category)
        {
            if (OnCategoryCreated != null) OnCategoryCreated(category);
        }

        public static void CategoryChanged(Category category)
        {
            if (OnCategoryChanged != null) OnCategoryChanged(category);
        }

        public static void CategoryRemoved(Category category)
        {
            if (OnCategoryRemoved != null) OnCategoryRemoved(category);
        }

        #endregion

    }
}
