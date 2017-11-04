﻿//
// MessageBLL   消息逻辑
// Copryright 2011 @ S1N1.COM,All rights reseved !
// Create by newmin @ 2011/04/06
//

using System.Collections.Generic;
using AtNet.Cms.DAL;
using AtNet.Cms.Domain.Interface.Models;
using AtNet.Cms.Domain.Interface._old;
using AtNet.DevFw.Data.Extensions;

namespace AtNet.Cms.BLL
{
    /// <summary>
    /// 会员数据访问
    /// </summary>
    public sealed class MessageBLL : Imessage
    {
        private static MessageDAL dal = new MessageDAL();

       /// <summary>
       /// 
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public Message GetMessage(int id)
        {
            Message m=null;
            dal.GetMessage(id, reader =>
            {
                if (reader.HasRows)
                {
                    m = reader.ToEntity<Message>();
                }

            });
            return m;
        }

        /// <summary>
        /// 写消息
        /// </summary>
        /// <param name="sendUid"></param>
        /// <param name="receiveUid"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        public void SendMessage(int sendUid, int receiveUid, string subject, string content)
        {
            dal.WriteMessage(sendUid, receiveUid, subject, content);
        }

        /// <summary>
        /// 设为已读
        /// </summary>
        /// <param name="receiveUid"></param>
        /// <param name="id"></param>
        public bool SetRead(int receiveUid,int id)
        {
            return dal.SetRead(receiveUid, id) == 1;
        }

        /// <summary>
        /// 回收消息
        /// </summary>
        /// <param name="receiveUid"></param>
        /// <param name="id"></param>
        public bool SetRecycle(int receiveUid, int id)
        {
            return dal.SetRecycle(receiveUid, id) == 1;
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="receiveUid"></param>
        /// <param name="id"></param>
        public bool Delete(int receiveUid,int id)
        {
            return dal.Delete(receiveUid, id) == 1;
        }

        /// <summary>
        /// 获取分页消息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="type">消息类型</param>
        /// <param name="pageSize"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IEnumerable<Message> GetPagedMessage(
            int uid, MessageType type, 
            int pageSize, ref int currentPageIndex,
            out int recordCount, out int pageCount)
        {
            return dal.GetPagedMessage(uid, (int)type, pageSize,
                ref currentPageIndex, out recordCount, out pageCount)
                .ToEntityList<Message>();
        }
    }
}