﻿/*
 * Copyright 2010 OPS,All rights reseved!
 * name     : ArchiveDAL
 * author   : newmin
 * date     : 2010/11/15
 * 
 * 2010/12/08 [!] newmin: GetArchivesOrderByCreateDate方法从数据库中选取了Archive的Content,需要删除
 * 2013-05-15 05:40 newmin [!]: 修改了GetPagedArchives基于左右值
 */

using System;
using System.Data;
using AtNet.Cms.Domain.Interface.Content.Archive;
using AtNet.DevFw.Data;

namespace AtNet.Cms.DAL
{
    public partial class ArchiveDal : DALBase
    {
        /// <summary>
        /// 插入文章并返回ID
        /// </summary>
        /// <param name="isSystem">是否为系统页面</param>
        /// <returns></returns>
        public bool Add(string strId, string alias, int categoryID,
            string author, string title, string smallTitle, string source, string thumbnail,
            string outline, string content, string tags, string flags, string location, int sortNumber)
        {
            string date = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            int rowcount = base.ExecuteNonQuery(
                 new SqlQuery(base.OptimizeSql(DbSql.Archive_Add),
                      new object[,]{
                {"@strId", strId},
                {"@alias", alias},
                {"@CategoryId", categoryID},
                {"@Author", author??""},
                {"@Title", title},
                {"@smallTitle", smallTitle??""},
                {"@Flags",flags},
                {"@location", location},
                {"@sortNumber",sortNumber},   
                {"@Source", source??""},   
                {"@thumbnail",thumbnail??""},
                {"@Outline", outline??""},
                {"@Content", content},
                {"@Tags", tags??""},
                {"@CreateDate",date},
                {"@LastModifyDate",date}
                     })
                 );


            return rowcount == 1;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="a"></param>
        public void Update(int id, int categoryID, string title, string smallTitle, string alias,
            string source, string thumbnail, string outline, string content,
            string tags, string flags, string location,int sortNumber)
        {
            string date = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            base.ExecuteNonQuery(new SqlQuery(base.OptimizeSql(DbSql.Archive_Update),
                 new object[,]{
                                {"@CategoryId", categoryID},
                                {"@Title", title},
                                {"@smallTitle", smallTitle??""},
                                {"@Flags", flags},
                                {"@Alias", alias??""}, 
                                {"@location", location},   
                                {"@sortNumber",sortNumber},
                                {"@Source", source??""},
                                {"@thumbnail",thumbnail??""},
                                {"@Outline", outline??""},
                                {"@Content", content},
                                {"@Tags", tags??""},
                                {"@lastModifyDate",date},
                                {"@id", id}
                 }));
        }

        /// <summary>
        /// 刷新文档
        /// </summary>
        /// <param name="archiveId"></param>
        public void RePublish(int siteId, int archiveId)
        {
            base.ExecuteNonQuery(
                new SqlQuery(base.OptimizeSql(DbSql.Archive_Republish),
                     new object[,]{
                {"@CreateDate",String.Format("{0:yyyy-MM-dd HH:mm:ss}",DateTime.Now)},
                {"@id", archiveId},
                {"@siteId",siteId}
        }));
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int siteId, int id)
        {
            base.ExecuteNonQuery(
                new SqlQuery(base.OptimizeSql(DbSql.Archive_Delete),
                     new object[,]{
                        {"@siteId",siteId},
                        {"@id", id}
                     }));
        }

        /// <summary>
        /// 检查别名是否存在
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public bool CheckAliasIsExist(int siteID, string alias)
        {
            return base.ExecuteScalar(
                new SqlQuery(base.OptimizeSql(DbSql.Archive_CheckAliasIsExist),
                     new object[,]{
                        {"@siteid",siteID},
                        {"@alias", alias}
                     })
                ) != null;
        }


        /// <summary>
        /// 添加查看次数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        public void AddViewCount(int siteId, int id, int count)
        {
            base.ExecuteNonQuery(new SqlQuery(base.OptimizeSql(DbSql.Archive_AddViewCount),
                 new object[,]{
                     {"@siteId",siteId},
                {"@Count", count},
                {"@id", id}
                 }));
        }

        #region 获取文档


        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void GetArchive(int siteId, string archiveIdOrAlias, DataReaderFunc func)
        {
            base.ExecuteReader(
                   new SqlQuery(base.OptimizeSql(DbSql.Archive_GetArchiveByStrIDOrAlias),
                        new object[,]{
                        {"@siteid",siteId},
                        {"@strid", archiveIdOrAlias}
                     }),
                   func
                   );
        }



        public void GetArchiveById(int siteId, int archiveId, DataReaderFunc func)
        {
            base.ExecuteReader(
                   new SqlQuery(base.OptimizeSql(DbSql.Archive_GetArchiveById),
                        new object[,]{
                        {"@siteid",siteId},
                        {"@id", archiveId}
                     }),
                   func
                   );
        }

        public DataTable GetAllArchives()
        {
            return base.GetDataSet(new SqlQuery(base.OptimizeSql(DbSql.Archive_GetAllArchive))).Tables[0];
        }

        [Obsolete]
        public DataTable GetArchives(string sqlcondition)
        {
            return base.GetDataSet(new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetArchivesByCondition), sqlcondition), null)).Tables[0];
        }


        /// <summary>
        /// 获取制定栏目的最新文档
        /// </summary>
        /// <param name="rgt"></param>
        /// <param name="number"></param>
        /// <param name="siteId"></param>
        /// <param name="lft"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public void GetSelftAndChildArchives(int siteId, int lft, int rgt, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
                    new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetSelfAndChildArchives), number),
                     new object[,]{
                    {"@siteId", siteId},
                    {"@lft", lft},
                    {"@rgt", rgt}
                     }), func
                );
        }

        /// <summary>
        /// 获取指定栏目和数量的文档
        /// </summary>
        public void GetArchives(int siteId, string categoryTag, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
              new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetArchivesByCategoryAlias), number),
                   new object[,]{
                {"@siteid", siteId},
                {"@tag", categoryTag}
                    }), func);
        }

        /// <summary>
        /// 获取指定模块类型的最新文档
        /// </summary>
        public void GetArchivesByModuleId(int siteId, int moduleId, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
                new SqlQuery(base.OptimizeSql(String.Format(DbSql.Archive_GetArchivesByModuleID, number)),
                 new object[,]{
                {"@siteId",siteId},
                {"@ModuleId", moduleId}
                 }), func);
        }

        /// <summary>
        /// 获取指定栏目浏览次数最多的档案
        /// </summary>
        /// <returns></returns>
        public void GetArchivesByViewCount(int siteId, int lft, int rgt, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
               new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetArchivesByViewCountDesc), number),
                new object[,]{
                    {"@siteId", siteId},
                    {"@lft", lft},
                    {"@rgt", rgt}
                 }), func);
        }



        public void GetArchivesByViewCount(int siteId, string categoryTag, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
              new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetArchivesByViewCountDesc_Tag), number),
                   new object[,]{
                {"@siteId",siteId},
                {"@tag", categoryTag}
                })
               , func);
        }

        /// <summary>
        /// 获取指定模块浏览次数最多的档案
        /// </summary>
        /// <returns></returns>
        public void GetArchivesByViewCountAndModuleId(int siteId, int moduleId, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
                new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetArchivesByModuleIDAndViewCountDesc), number),
                     new object[,]{
                         {"@siteId",siteId},
                {"@ModuleId", moduleId}
                     }), func);
        }


        /// <summary>
        /// 获取指定数量和栏目的特殊文档
        /// </summary>
        /// <param name="c"></param>
        /// <param name="rgt"></param>
        /// <param name="number"></param>
        /// <param name="siteId"></param>
        /// <param name="lft"></param>
        /// <param name="func"></param>
        public void GetSpecialArchives(int siteId, int lft, int rgt, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
                   new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetSpecialArchivesByCategoryID), number),
                        new object[,]{
                    {"@siteId", siteId},
                    {"@lft", lft},
                    {"@rgt", rgt}
                         }), func);
        }


        public void GetSpecialArchives(int siteId, string categoryTag, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
                   new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetSpecialArchivesByCategoryTag), number),
                        new object[,]{
                    {"@siteId",siteId},
                    {"@categoryTag", categoryTag}
                         }), func);
        }

        /// <summary>
        /// 获取指定数量和模块的特殊文章
        /// </summary>
        /// <param name="c"></param>
        /// <param name="siteId"></param>
        /// <param name="moduleId"></param>
        /// <param name="number"></param>
        /// <param name="func"></param>
        public void GetSpecialArchivesByModuleId(int siteId, int moduleId, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
                        new SqlQuery(String.Format(base.OptimizeSql(DbSql.Archive_GetSpecialArchivesByModuleID), number),
                         new object[,]{
                            {"@siteId",siteId},
                            {"@ModuleId", moduleId}
                         }), func);
        }

        /// <summary>
        /// 获取指定栏目的第一篇特殊文档
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="categoryId"></param>
        /// <param name="func"></param>
        public void GetFirstSpecialArchive(int siteId, int categoryId, DataReaderFunc func)
        {
            base.ExecuteReader(
                new SqlQuery(base.OptimizeSql(DbSql.Archive_GetFirstSpecialArchiveByCategoryID),
                     new object[,]{
                     {"@siteId", siteId},
                     {"@CategoryId", categoryId}
                     }),
                func
                );
        }


        /********* 上一篇和下一篇 **************/

        /// <summary>
        /// 获取上一篇文档
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <param name="sameCategory"></param>
        /// <param name="ingoreSpecial"></param>
        /// <param name="func"></param>
        public void GetPreviousArchive(int siteId, int id,bool sameCategory,bool ingoreSpecial, DataReaderFunc func)
        {
            base.ExecuteReader(
                    new SqlQuery(base.OptimizeSql(DbSql.Archive_GetPreviousArchive),
                         new object[,]{
                         {"@siteId",siteId},
                         {"@id", id},
                        {"@sameCategory", sameCategory?1:0},
                        {"@special",ingoreSpecial?1:0},
                         }),
                    func
                    );
        }

        /// <summary>
        /// 获取下一篇文档
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <param name="ingoreSpecial"></param>
        /// <param name="func"></param>
        /// <param name="sameCategory"></param>
        public void GetNextArchive(int siteId, int id, bool sameCategory,bool ingoreSpecial, DataReaderFunc func)
        {
            base.ExecuteReader(
                new SqlQuery(base.OptimizeSql(DbSql.Archive_GetNextArchive),
                    new object[,]
                    {
                        {"@siteId", siteId},
                        {"@id", id},
                        {"@sameCategory", sameCategory?1:0},
                        {"@special",ingoreSpecial?1:0},
                    }),
                func
                );
        }



        /// <summary>
        /// 获取文档的前几篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        /// 
        public DataTable GetPreviousArchives(string id, int number)
        {
            //
            //TODO:尚未实现
            //

            throw new NotImplementedException();
            //            return base.GetDataSet(@"select top " + number.ToString() + @" ID,Alias,Title from $PREFIX_archive,
            //                                 (select top 1 CID from $PREFIX_archive where ID=@id) as t
            //                                 where $PREFIX_archive.CID=t.CID and ID<@id order by ID desc",
            //                                 {"@id", id}).Tables[0];
        }

        /// <summary>
        /// 获取文档的后几篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public DataTable GetNextArchives(string id, int number)
        {
            throw new NotImplementedException();

            //
            //TODO:尚未实现
            //

            //按ID计算上一篇
            //            return base.GetDataSet(@"select top " + number.ToString() + @" ID,Alias,Title from $PREFIX_archive,
            //                                 (select top 1 CID from $PREFIX_archive where ID=@id) as t
            //                                 where $PREFIX_archive.CID=t.CID and ID>@id",
            //                                 {"@id", id}).Tables[0];
        }


        /************ 分页 ****************/

        /// <summary>
        /// 获取栏目分页文档
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="lft"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="rgt"></param>
        /// <param name="recordCount"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public DataTable GetPagedArchives(
            int siteId, int lft, int rgt,
            int pageSize, ref int currentPageIndex,
            out int recordCount, out int pages)
        {
            object[,] data = new object[,]{
                {"@siteId",siteId},
                {"@lft",lft},
                {"@rgt",rgt}
            };

            const string condition = " ";

            //数据库为ACCESS,页码为1时调用SQL
            const string sql1 = @"SELECT TOP $[pagesize] $PREFIX_archive.ID AS ID,* FROM $PREFIX_archive
                                  INNER JOIN $PREFIX_category ON $PREFIX_archive.[CID]=$PREFIX_category.[ID]
                                  WHERE $PREFIX_category.siteId=@siteId AND (lft>=@lft AND rgt<=@rgt) 
                                  AND flags LIKE '%st:''0''%'AND flags LIKE '%v:''1''%'
                                  ORDER BY sort_number DESC,$PREFIX_archive.id";

            //获取记录条数
            recordCount = int.Parse(base.ExecuteScalar(SqlQueryHelper.Format(DbSql.Archive_GetPagedArchivesCountSql_pagerqurey, data)).ToString());

            pages = recordCount / pageSize;
            if (recordCount % pageSize != 0) pages++;

            //验证当前页数
            if (currentPageIndex > pages && currentPageIndex != 1) currentPageIndex = pages;
            if (currentPageIndex < 1) currentPageIndex = 1;

            //计算分页
            int skipCount = pageSize * (currentPageIndex - 1);

            //如果调过记录为0条，且为OLEDB时候，则用sql1
            string sql = skipCount == 0 && base.DbType == DataBaseType.OLEDB ?
                       sql1 :
                       DbSql.Archive_GetPagedArchivesByCategoryID_pagerquery;

            sql = SQLRegex.Replace(sql, m =>
            {
                switch (m.Groups[1].Value)
                {
                    case "skipsize": return skipCount.ToString();
                    case "pagesize": return pageSize.ToString();
                }
                return null;
            });

            return base.GetDataSet(SqlQueryHelper.Format(sql, data)).Tables[0];
        }

        /// <summary>
        /// 获取栏目分页文档
        /// </summary>
        /// <param name="lft"></param>
        /// <param name="rgt"></param>
        /// <param name="author">指定作者，NULL表示不限，可显示所有作者发布的文档</param>
        /// <param name="siteId"></param>
        /// <param name="moduleId">参数暂时不使用为-1</param>
        /// <param name="flags"></param>
        /// <param name="orderByField"></param>
        /// <param name="orderAsc"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public DataTable GetPagedArchives(int siteId, int moduleId,
            int lft, int rgt, string author,
            string[,] flags, string orderByField, bool orderAsc,
            int pageSize, int currentPageIndex,
            out int recordCount, out int pages)
        {
            //SQL Condition Template
            const string conditionTpl = "$[siteid]$[module]$[category]$[author]$[flags]";


            //Get records count
            //{0}==$[condition]

            const string sql1 = @"SELECT TOP $[pagesize] a.id AS id,alias,title,
                                    c.name as CategoryName,cid,flags,author,content,source,
                                    createdate,viewcount,location,sort_number as sortNumber FROM $PREFIX_archive a
                                    INNER JOIN $PREFIX_category c ON a.cid=c.id
                                    WHERE $[condition] ORDER BY $[orderByField] $[orderASC],a.id";


            string condition,                                                             //SQL where condition
                    order = String.IsNullOrEmpty(orderByField) ? "sort_number" : orderByField,   //Order filed ( CreateDate | ViewCount | Agree | Disagree )
                    orderType = orderAsc ? "ASC" : "DESC";                                      //ASC or DESC


            string flag = ArchiveFlag.GetSQLString(flags);

            condition = SQLRegex.Replace(conditionTpl, match =>
            {
                switch (match.Groups[1].Value)
                {
                    case "siteid": return String.Format(" c.siteid={0}", siteId.ToString());

                    case "category": return lft <= 0 || rgt <= 0 ? ""
                        : String.Format(" AND lft>={0} AND rgt<={1}", lft.ToString(), rgt.ToString());

                    case "module": return moduleId <= 0 ? ""
                        : String.Format(" AND m.id={0}", moduleId.ToString());

                    case "author": return String.IsNullOrEmpty(author) ? null
                        : String.Format(" AND author='{0}'", author);

                    case "flags": return String.IsNullOrEmpty(flag) ? "" : " AND " + flag;

                }
                return null;
            });

            // throw new Exception(new SqlQuery(base.OptimizeSQL(String.Format(SP.Archive_GetpagedArchivesCountSql, condition)));

            //获取记录条数
            recordCount = int.Parse(base.ExecuteScalar(
                new SqlQuery(base.OptimizeSql(String.Format(DbSql.Archive_GetpagedArchivesCountSql, condition)), null)).ToString());


            pages = recordCount / pageSize;
            if (recordCount % pageSize != 0) pages++;
            //验证当前页数

            if (currentPageIndex > pages && currentPageIndex != 1) currentPageIndex = pages;
            if (currentPageIndex < 1) currentPageIndex = 1;
            //计算分页
            int skipCount = pageSize * (currentPageIndex - 1);


            //如果调过记录为0条，且为OLEDB时候，则用sql1
            string sql = skipCount == 0 && base.DbType == DataBaseType.OLEDB ?
                        sql1 :
                        DbSql.Archive_GetPagedArchivesByCategoryId;

            sql = SQLRegex.Replace(sql, m =>
            {
                switch (m.Groups[1].Value)
                {
                    case "skipsize": return skipCount.ToString();
                    case "pagesize": return pageSize.ToString();
                    case "condition": return condition;
                    case "orderByField": return order;
                    case "orderASC": return orderType;
                }
                return null;
            });
            //throw new Exception(new SqlQuery(base.OptimizeSQL(sql));
            //throw new Exception(sql+"-"+DbHelper.DbType.ToString()+"-"+new SqlQuery(base.OptimizeSQL(SP.ToString());
            //System.Web.HttpContext.Current.Response.Write(sql);
            //throw new Exception(sql);
            return base.GetDataSet(new SqlQuery(base.OptimizeSql(sql))).Tables[0];
        }


        #endregion


        #region 搜索相关

        /// <summary>
        /// 搜索关键词相关的内容
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public void SearchArchives(int siteId, string keyword, int pageSize, int currentPageIndex, out int recordCount, out int pageCount, string orderby, DataReaderFunc func)
        {
            /*
            string condition = ArchiveFlag.GetSQLString(new string[,]{
                    {"st","0"},
                    {"v","1"}
                });
             */

            const string condition = " flags LIKE '%st:''0''%'AND flags LIKE '%v:''1''%' ";

            //排序规则
            if (String.IsNullOrEmpty(orderby)) orderby = String.Intern("ORDER BY sort_number DESC");

            //数据库为OLEDB,且为第一页时
            const string sql1 = @"SELECT TOP $[pagesize] $PREFIX_archive.[ID] AS ID,* FROM $PREFIX_archive INNER JOIN $PREFIX_category ON $PREFIX_archive.[CID]=$PREFIX_category.[ID]
                    WHERE $[condition] AND ([Title] LIKE '%$[keyword]%' OR [Outline] LIKE '%$[keyword]%' OR [Content] LIKE '%$[keyword]%' OR [Tags] LIKE '%$[keyword]%')  $[orderby],$PREFIX_archive.[ID]";


            //记录数
            recordCount = int.Parse(base.ExecuteScalar(
                SqlQueryHelper.Format(DbSql.Archive_GetSearchRecordCount, keyword, siteId.ToString(), condition)
                ).ToString());

            //页数
            pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0) pageCount++;

            //对当前页数进行验证
            if (currentPageIndex > pageCount && currentPageIndex != 1) currentPageIndex = pageCount;
            if (currentPageIndex < 1) currentPageIndex = 1;

            //跳过记录数
            int skipCount = pageSize * (currentPageIndex - 1);

            //如果调过记录为0条，且为OLEDB时候，则用sql1
            string sql = skipCount == 0 && base.DbType == DataBaseType.OLEDB ?
                        sql1 :
                        DbSql.Archive_GetPagedSearchArchives;

            sql = SQLRegex.Replace(sql, (match) =>
            {
                switch (match.Groups[1].Value)
                {
                    case "condition": return condition;
                    case "siteid": return siteId.ToString();
                    case "pagesize": return pageSize.ToString();
                    case "skipsize": return skipCount.ToString();
                    case "keyword": return keyword;
                    case "orderby": return orderby;
                }
                return null;
            });

            base.ExecuteReader(new SqlQuery(base.OptimizeSql(sql)), func);
        }

        /// <summary>
        /// 在指定栏目下搜索关键词相关的内容
        /// </summary>
        /// <param name="categoryRgt"></param>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderby"></param>
        /// <param name="siteId"></param>
        /// <param name="categoryLft"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public void SearchArchivesByCategory(int siteId,
            int categoryLft, int categoryRgt, string keyword,
            int pageSize, int currentPageIndex,
            out int recordCount, out int pageCount,
            string orderby, DataReaderFunc func)
        {

            const string condition = " flags LIKE '%st:''0''%'AND flags LIKE '%v:''1''%' ";


            if (String.IsNullOrEmpty(orderby)) orderby = String.Intern("ORDER BY sort_number DESC");

            object[,] data = new object[,]
            {
                {"@siteId",siteId},
                {"@lft",categoryLft},
                {"@rgt",categoryRgt}
            };

            //为第一页时
            const string sql1 = @"SELECT TOP $[pagesize] $PREFIX_archive.[ID] AS ID,* 
                                  FROM $PREFIX_archive INNER JOIN $PREFIX_category 
                                  ON $PREFIX_archive.[CgID]=$PREFIX_category.[ID]
                                  WHERE $[condition] AND $PREFIX_category.siteid=@siteId AND ($PREFIX_category.lft>=@lft
                                   AND $PREFIX_category.rgt<=@rgt) AND ([Title] LIKE 
                                  '%$[keyword]%' OR [Outline] LIKE '%$[keyword]%' 
                                   OR [Content] LIKE '%$[keyword]%' OR [Tags] LIKE '%$[keyword]%')
                                   $[orderby],$PREFIX_archive.[ID]";

            //记录数
            recordCount = int.Parse(base.ExecuteScalar(
                SqlQueryHelper.Format(DbSql.Archive_GetSearchRecordCountByCategoryID,
                data,
                keyword,
                condition
                )).ToString());

            //页数
            pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0) pageCount++;

            //对当前页数进行验证
            if (currentPageIndex > pageCount && currentPageIndex != 1) currentPageIndex = pageCount;
            if (currentPageIndex < 1) currentPageIndex = 1;

            //跳过记录数
            int skipCount = pageSize * (currentPageIndex - 1);


            string sql = skipCount == 0 && base.DbType == DataBaseType.OLEDB ?
                sql1 :
                DbSql.Archive_GetPagedSearchArchivesByCategoryID;

            sql = SQLRegex.Replace(sql, (match) =>
             {
                 switch (match.Groups[1].Value)
                 {
                     case "siteid": return siteId.ToString();
                     case "condition": return condition;
                     case "pagesize": return pageSize.ToString();
                     case "skipsize": return skipCount.ToString();
                     case "keyword": return keyword;
                     case "orderby": return orderby;
                 }
                 return null;
             });


            base.ExecuteReader(SqlQueryHelper.Format(sql, data), func);
        }

        /// <summary>
        /// 在指定模块下搜索关键词相关的内容
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataTable SearchByModule(int moduleId, string keyword, int pageSize, int currentPageIndex, out int recordCount, out int pageCount, string orderby)
        {

            const string condition = " flags LIKE '%st:''0''%'AND flags LIKE '%v:''1''%' ";

            if (String.IsNullOrEmpty(orderby)) orderby = String.Intern("ORDER BY sort_number DESC");

            //为第一页时
            const string sql1 = @"SELECT TOP $[pagesize] $PREFIX_archive.[ID] AS ID,* FROM  $PREFIX_archive INNER JOIN $PREFIX_category ON $PREFIX_archive.[CID]=$PREFIX_category.[ID]
                    WHERE $[condition] AND $PREFIX_category.[ModuleID]=$[moduleid] AND ([Title] LIKE '%$[keyword]%' OR [Outline] LIKE '%$[keyword]%' OR [Content] LIKE '%$[keyword]%' OR [Tags] LIKE '%$[keyword]%') $[orderby],$PREFIX_archive.[ID]";

            //记录数
            recordCount = int.Parse(base.ExecuteScalar(
                new SqlQuery(base.OptimizeSql(String.Format(DbSql.Archive_GetSearchRecordCountByModuleID, moduleId,
                keyword, condition)), null)
                ).ToString());

            //页数
            pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0) pageCount++;

            //对当前页数进行验证
            if (currentPageIndex > pageCount && currentPageIndex != 1) currentPageIndex = pageCount;
            if (currentPageIndex < 1) currentPageIndex = 1;

            //跳过记录数
            int skipCount = pageSize * (currentPageIndex - 1);

            string sql = skipCount == 0 && base.DbType == DataBaseType.OLEDB ?
                         sql1 :
                        DbSql.Archive_GetPagedSearchArchivesByModuleID;

            sql = SQLRegex.Replace(sql, (match) =>
            {
                switch (match.Groups[1].Value)
                {
                    case "condition": return condition;
                    case "pagesize": return pageSize.ToString();
                    case "skipsize": return skipCount.ToString();
                    case "keyword": return keyword;
                    case "moduleid": return moduleId.ToString();
                    case "orderby": return orderby;
                }
                return null;
            });

            return base.GetDataSet(new SqlQuery(base.OptimizeSql(sql))).Tables[0];

        }

        #endregion


        /// <summary>
        /// 删除会员发布的文档
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMemberArchives(int id)
        {
            base.ExecuteNonQuery(
                new SqlQuery(base.OptimizeSql(DbSql.Archive_DeleteMemberArchives),
                     new object[,]{
                {"@id", id}
                     })
                );
        }

        public int TransferAuthor(string username, string anotherUsername)
        {
            return base.ExecuteNonQuery(
                 new SqlQuery(base.OptimizeSql(DbSql.Archive_TransferAuthor),
                      new object[,]{
                        {"@AnotherUsername", anotherUsername},
                        {"@Username", username}
                     })
                 );
        }

        public int GetCategoryArchivesCount(string id)
        {
            return int.Parse(base.ExecuteScalar(
                new SqlQuery(base.OptimizeSql(SQLRegex.Replace(DbSql.Archive_GetCategoryArchivesCount, (c) => { return id; })), null)).ToString());

        }



        public void GetSelftAndChildArchiveExtendValues(int siteId, int relationType, int lft, int rgt, int number, DataReaderFunc func)
        {
            /*
            base.ExecuteReader(
                   new SqlQuery(String.Format(base.OptimizeSQL(SP.Archive_GetSelfAndChildArchives), number),
                    new object[,]{
                     }), func
               );
            */
            base.ExecuteReader(
                SqlQueryHelper.Format(DbSql.Archive_GetSelfAndChildArchiveExtendValues,
                 new object[,]{
                    {"@siteId", siteId},
                    {"@lft", lft},
                    {"@rgt", rgt},
                     {"@relationType",relationType}
                 }, number.ToString()
                ), func);
        }

        public void GetArchivesExtendValues(int siteId, int relationType, string categoryTag, int number, DataReaderFunc func)
        {
            base.ExecuteReader(
                   SqlQueryHelper.Format(DbSql.Archive_GetArchivesExtendValues,
                    new object[,]{
                    {"@siteId", siteId},
                    {"@tag",categoryTag},
                     {"@relationType",relationType}
                 }, number.ToString()
                   ), func);
        }


        public int GetMaxSortNumber(int siteId)
        {
            return int.Parse(base.ExecuteScalar(
                SqlQueryHelper.Format(DbSql.Archive_GetMaxSortNumber,
                    new object[,]
                    {
                        {"@siteId", siteId},
                    })).ToString());
        }

        public void SaveSortNumber(int archiveId, int sortNumber)
        {
            base.ExecuteNonQuery(
                SqlQueryHelper.Format(DbSql.Archive_UpdateSortNumber,
                    new object[,]
                    {
                        {"@archiveId", archiveId},
                        {"@sort_number",sortNumber},
                    }));
        }
    }
}