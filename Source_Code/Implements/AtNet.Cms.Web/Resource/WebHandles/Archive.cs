﻿
namespace OPSite.WebHandler
{
    using System;
    using System.Text;
    using Ops.Cms;
    using Ops.Cms.BLL;
    using Ops.Web;

    [WebExecuteable]
    public class Archive
    {

        /// <summary>
        /// 提交点评
        /// </summary>
        /// <param name="archiveID">文档编号</param>
        /// <param name="agree">如果为true则表示赞同,反之表示反对</param>
        /// <returns></returns>
        [Post(AllowRefreshMillliSecond=2000)]
        public string SubmitReviews(string archiveID, string agree)
        {
            global::Ops.Cms.Models.Member m = UserState.Member.Current;
            if (m == null) return "-1";

            //提交点评
            bool result=new CommentBLL().SubmitReviews(archiveID,m.ID,agree.ToLower()=="true");

            //清除文章缓存
           // if (true) ArchiveCache.ClearSingleCache(archiveID);

            return result.ToString(); ;
        }
        [Post(AllowRefreshMillliSecond=2000)]
        public string SubmitComment(string archiveID, string content)
        {
            global::Ops.Cms.Models.Member m = UserState.Member.Current;
            if(m==null)return null;
            //检查长度是否正确,不正确则退出
            if (content.Length< 6||content.Length>120)return null;
                new CommentBLL().InsertComment(archiveID, m.ID, content);

            StringBuilder sb = new StringBuilder();
            sb.Append("new Object({uid:'").Append(m.ID).Append("',avatar:'").Append(String.IsNullOrEmpty(m.Avatar) ? "/images/noavatar.gif" : m.Avatar)
                .Append("',nickname:'").Append(m.Nickname).Append("',time:'").Append(string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now))
                .Append("',content:'").Append(content).Append("'})");
            return sb.ToString() ;
        }
        /// <summary>
        /// 获取评论JSON数据
        /// </summary>
        /// <param name="archiveID"></param>
        /// <returns></returns>
        [Post(AllowRefreshMillliSecond=1000)]
        public string GetCommentsJSON(string archiveID)
        {
            return new ArchiveBLL().GetCommentDetailsJSON(archiveID);
        }

        [Permission("archive:DeleteComment")]
        [Permission("archvie,DeleteComment")]
        public bool DeleteComment(string id)
        {
            new CommentBLL().DeleteComment(int.Parse(id));
            return true;
        }
    }
}
