﻿using System;
using AtNet.DevFw.Data;

namespace AtNet.Cms.Sql
{
    public abstract class SqlPack
    {
        public static SqlPack Factory(DataBaseType dbType)
        {
            switch (dbType)
            {
                case DataBaseType.OLEDB: return new OleDbSqlPack();
				case DataBaseType.MonoSQLite:
				case DataBaseType.SQLite:
					return new SQLiteSqlPack();
                case DataBaseType.SQLServer: return new SqlServerSqlPack();
                case DataBaseType.MySQL: return new MySQLSqlPack();
			}
            throw new Exception("暂不支持此数据库！无法生成SQL包实例!");
        }

        #region 文档相关



        /// <summary>
        /// 增加文档
        /// </summary>
        public abstract string Archive_Add { get; }


        /// <summary>
        /// 更新文档
        /// </summary>
        public abstract string Archive_Update { get; }

        /// <summary>
        /// 重新发布文档
        /// </summary>
        public readonly string Archive_Republish = @"UPDATE $PREFIX_archive 
                                SET createdate=@CreateDate WHERE id IN (SELECT id FROM
                                (SELECT $PREFIX_archive.id FROM $PREFIX_archive INNER JOIN 
                                $PREFIX_category ON $PREFIX_category.id=$PREFIX_archive.cid
                                WHERE siteid=@siteid AND $PREFIX_archive.id=@id) t)";

        /// <summary>
        /// 删除文档
        /// </summary>
        public readonly string Archive_Delete = @"DELETE FROM $PREFIX_archive
                                WHERE id in (SELECT id FROM (SELECT $PREFIX_archive.id FROM $PREFIX_archive INNER JOIN 
                                $PREFIX_category ON $PREFIX_category.id=$PREFIX_archive.cid
                                WHERE siteid=@siteid AND $PREFIX_archive.id=@id) t)";

        /// <summary>
        /// 检查别名是否存在
        /// </summary>
        public readonly string Archive_CheckAliasIsExist =@"SELECT alias FROM $PREFIX_archive INNER JOIN 
                                $PREFIX_category ON $PREFIX_category.id=$PREFIX_archive.cid
                                WHERE siteid=@siteid AND (alias=@alias or $PREFIX_archive.strid=@alias)";


        /// <summary>
        /// 增加浏览数量
        /// </summary>
        public readonly string Archive_AddViewCount = @"UPDATE $PREFIX_archive SET viewcount=viewcount+@count
                                WHERE id in (select id from (SELECT $PREFIX_archive.id FROM $PREFIX_archive INNER JOIN 
                                $PREFIX_category ON $PREFIX_category.id=$PREFIX_archive.cid
                                WHERE siteid=@siteid AND $PREFIX_archive.id=@id) t)";

        /// <summary>
        /// 根据站点编号获取文档
        /// </summary>
        public readonly string Archive_GetArchiveByStrIDOrAlias = @"
                    SELECT * FROM $PREFIX_archive INNER JOIN $PREFIX_category
                    ON $PREFIX_category.id=$PREFIX_archive.cid  WHERE siteid=@siteid 
                    AND (alias=@strid OR $PREFIX_archive.strid=@strid)";

        /// <summary>
        /// 根据文档编号获取文档
        /// </summary>
        public readonly string Archive_GetArchiveById = @"SELECT * FROM $PREFIX_archive INNER JOIN $PREFIX_category
                    ON $PREFIX_category.id=$PREFIX_archive.cid  WHERE siteid=@siteid AND $PREFIX_archive.id=@id";


        /// <summary>
        /// 获取所有文档
        /// </summary>
        public abstract string Archive_GetAllArchive { get; }

        /// <summary>
        /// 根据条件获取文档
        /// </summary>
        public abstract string Archive_GetArchivesByCondition { get; }

        /// <summary>
        /// 根据栏目左右值获取获取指定数量的文档,包括子栏目的文档
        /// </summary>
        public abstract string Archive_GetSelfAndChildArchives { get; }

        /// <summary>
        /// 获取自己包含子栏目的文档扩展信息
        /// </summary>
        public abstract string Archive_GetSelfAndChildArchiveExtendValues { get; }

        /// <summary>
        /// 获取栏目的扩展信息
        /// </summary>
        public abstract string Archive_GetArchivesExtendValues { get; }

        /// <summary>
        /// 根据栏目别名获取文档
        /// </summary>
        public abstract string Archive_GetArchivesByCategoryAlias { get; }

        /// <summary>
        /// 根据模块编号获取文档
        /// </summary>
        public abstract string Archive_GetArchivesByModuleID { get; }

        /// <summary>
        /// 获取指定栏目浏览最多的文档
        /// </summary>
        public abstract string Archive_GetArchivesByViewCountDesc { get; }

        /// <summary>
        /// 获取指定栏目浏览最多的文档(使用栏目别名)
        /// </summary>
        public abstract string Archive_GetArchivesByViewCountDesc_Tag { get; }

        /// <summary>
        /// 获取指定模块浏览最多的文档
        /// </summary>
        public abstract string Archive_GetArchivesByModuleIDAndViewCountDesc { get; }


        /// <summary>
        /// 根据栏目获取特殊文档(包括子类)
        /// </summary>
        public abstract string Archive_GetSpecialArchivesByCategoryID { get; }

        /// <summary>
        /// 根据栏目获取特殊文档（不包括子类)
        /// </summary>
        public abstract string Archive_GetSpecialArchivesByCategoryTag { get; }

        /// <summary>
        /// 获取指定模块的特殊文档
        /// </summary>
        public abstract string Archive_GetSpecialArchivesByModuleID { get; }


        /// <summary>
        /// 获取指定栏目的第一篇特殊文档
        /// </summary>
        public abstract string Archive_GetFirstSpecialArchiveByCategoryID { get; }

        /// <summary>
        /// 获取上一篇文档(@sameCategory=1表示同分类)
        /// </summary>
        public abstract string Archive_GetPreviousArchive { get; }

        /// <summary>
        /// 获取下一篇文档(@sameCategory=1表示同分类)
        /// </summary>
        public abstract string Archive_GetNextArchive { get; }

        /// <summary>
        /// 删除指定会员的文档
        /// </summary>
        public readonly string Archive_DeleteMemberArchives = "DELETE FROM $PREFIX_archive WHERE author=@Id";

        /// <summary>
        /// 切换作者
        /// </summary>
        public readonly string Archive_TransferAuthor = "UPDATE $PREFIX_archive SET author=@AnotherUsername WHERE author=@Username";

        /// <summary>
        /// 获取分页文档条数(前台调用)
        /// </summary>
        public readonly string Archive_GetPagedArchivesCountSql_pagerqurey = @"
            SELECT COUNT($PREFIX_archive.id) FROM $PREFIX_archive 
            INNER JOIN $PREFIX_category ON $PREFIX_archive.cid=$PREFIX_category.id 
            WHERE $PREFIX_category.siteId=@siteId AND " 
            + SqlConst.Archive_NotSystemAndHidden + " AND (lft>=@lft AND rgt<=@rgt)";

        /// <summary>
        /// 根据栏目获取分页文档
        /// </summary>
        public abstract string Archive_GetPagedArchivesByCategoryID_pagerquery { get; }


        /// <summary>
        /// 获取分页文档条数
        /// </summary>
        public abstract string Archive_GetpagedArchivesCountSql { get; }

        /*
            INNER JOIN $PREFIX_category c INNER JOIN $PREFIX_modules m ON
            a.cid=c.id AND c.moduleid=m.id
        */
        /// <summary>
        /// 获取分页文档条数
        /// </summary>
        public readonly string Archive_GetpagedArchivesCountSql2 = @"
            SELECT COUNT($PREFIX_archive.id) FROM $PREFIX_archive
            INNER JOIN $PREFIX_category ON $PREFIX_archive.cid=$PREFIX_category.id
            INNER JOIN $PREFIX_modules ON $PREFIX_category.moduleid=$PREFIX_modules.id
            Where {0}";
        
        /// <summary>
        /// 获取栏目下的文档数量
        /// </summary>
        public readonly string Archive_GetCategoryArchivesCount =@"
            SELECT COUNT($PREFIX_archive.id) FROM $PREFIX_archive 
            INNER JOIN $PREFIX_category ON $PREFIX_archive.cid=$PREFIX_category.id
            WHERE $PREFIX_category.lft BETWEEN @lft AND @rgt";
        
        /// <summary>
        /// 获取分页文档
        /// </summary>
        public abstract string Archive_GetPagedArchivesByCategoryId { get; }


        /// <summary>
        /// 获取搜索文档数量
        /// </summary>
        public readonly string Archive_GetSearchRecordCount = @"
                        SELECT COUNT(0) FROM $PREFIX_archive 
                        INNER JOIN $PREFIX_category ON $PREFIX_archive.cid=$PREFIX_category.id
                        WHERE $PREFIX_category.siteid={1} AND {2} AND 
                        (title LIKE '%{0}%' OR outline LIKE '%{0}%' OR content LIKE '%{0}%' OR tags LIKE '%{0}%')";

        /// <summary>
        /// 获取搜索分页符合条件的文档列表
        /// </summary>
        public abstract string Archive_GetPagedSearchArchives { get; }

        /// <summary>
        /// 获取根据模块搜索符合条件的文档数量文档数量
        /// </summary>
        public abstract string Archive_GetSearchRecordCountByModuleID { get; }

        /// <summary>
        /// 获取根据模块搜索符合条件的文档列表
        /// </summary>
        public abstract string Archive_GetPagedSearchArchivesByModuleID { get; }

        /// <summary>
        /// 根据栏目搜索符合条件的文档数量
        /// </summary>
        public abstract string Archive_GetSearchRecordCountByCategoryID{get;}

        /// <summary>
        ///获取根据栏目搜索符合条件的文档列表
        /// </summary>
        public abstract string Archive_GetPagedSearchArchivesByCategoryID { get; }

        /// <summary>
        /// 获取最大的排序号码
        /// </summary>
        public string Archive_GetMaxSortNumber = @"SELECT MAX(sort_number) FROM  $PREFIX_archive 
                        INNER JOIN $PREFIX_category ON $PREFIX_archive.cid=$PREFIX_category.id
                        WHERE $PREFIX_category.siteid=@siteId";

        /// <summary>
        /// 更新排序号
        /// </summary>
        public string Archive_UpdateSortNumber = "UPDATE $PREFIX_archive SET sort_number=@sort_number WHERE id=@archiveId";

        #endregion

        #region 栏目相关

        /// <summary>
        /// 获取所有栏目
        /// </summary>
        public readonly string Category_GetAllCategories = "select * from $PREFIX_category ORDER BY lft";

        /// <summary>
        /// 更新栏目
        /// </summary>
        public readonly string Category_Update = @"
                    UPDATE $PREFIX_category SET /*lft=@lft,rgt=@rgt,*/
                    moduleid=@moduleID,name=@name,tag=@tag,icon=@icon,pagetitle=@pagetitle,
                    keywords=@keywords,description=@description,location=@location,
                    orderindex=@orderindex WHERE id=@id";

        /// <summary>
        /// 添加栏目
        /// </summary>
        public readonly string Category_Insert = @"
                    INSERT INTO $PREFIX_category(siteid,lft,rgt,moduleid,
                    name,tag,icon,pagetitle,keywords,description,location,orderindex)
                    VALUES (@siteid,@lft,@rgt,@moduleID,@name,@tag,@icon,@pagetitle,
                    @keywords,@description,@location,@orderindex)";

        /// <summary>
        /// 获取子栏目的数量
        /// </summary>
        //public string Category_GetChildCategoryCount = "SELECT COUNT(id) FROM $PREFIX_category WHERE pid=@pid";

        /// <summary>
        /// 删除栏目
        /// </summary>
        public readonly string Category_DeleteByLft = "DELETE FROM $PREFIX_category WHERE siteId=@siteId AND lft>=@lft AND rgt<=@rgt";

        public readonly string Category_UpdateInsertLeft = "UPDATE $PREFIX_category SET lft=lft+2 WHERE lft>@lft AND siteId=@siteId";
        public readonly string Category_UpdateInsertRight = "UPDATE $PREFIX_category SET rgt=rgt+2 WHERE rgt>@lft AND siteId=@siteId";

        public readonly string Category_UpdateDeleteLft = "UPDATE $PREFIX_category SET lft=lft-@val WHERE lft>@lft AND siteId=@siteId";
        public readonly string Category_UpdateDeleteRgt = "UPDATE $PREFIX_category SET rgt=rgt-@val WHERE rgt>@rgt AND siteId=@siteId";

        public string Category_GetMaxRight = "SELECT max(rgt) FROM $PREFIX_category WHERE siteId=@siteId";


        /*
        /// <summary>
        /// 更新左值比当前节点右值大,且小于新的父节点的左值的节点左值
        /// </summary>
        public string Category_ChangeUpdateTreeLeft = "UPDATE $PREFIX_category SET lft=lft-@rgt-@lft-1 WHERE lft>@rgt AND lft<=@tolft AND siteId=@siteId";

        /// <summary>
        /// 更新右值比当前节点右值大,且小于新的父节点的右值的节点右值
        /// </summary>
        public string Category_ChangeUpdateTreeRight="UPDATE $PREFIX_category SET rgt=rgt-@rgt-@lft-1 WHERE rgt>@rgt AND rgt<@tolft AND siteId=@siteId";
        
        /// <summary>
        /// 移动子类
        /// </summary>
        public string Category_ChangeUpdateTreeChildNodes = "UPDATE $PREFIX_category SET lft=lft+@tolft-@rgt,rgt=rgt+@tolft-@rgt WHERE lft>=@lft AND rgt<=@rgt AND siteId=@siteId";
        */


        public string Category_ChangeUpdateTreeLeft = "UPDATE $PREFIX_category SET lft=lft-(@rgt-@lft)-1 WHERE lft>@rgt AND rgt<=@torgt AND siteId=@siteId";
        public string Category_ChangeUpdateTreeRight = "UPDATE $PREFIX_category SET rgt=rgt-(@rgt-@lft)-1 WHERE rgt>@rgt AND rgt<@torgt AND siteId=@siteId";
        public string Category_ChangeUpdateTreeChildNodes = @"UPDATE $PREFIX_category SET lft=lft+(@torgt-@rgt-1),rgt=rgt+(@torgt-@rgt-1)
                                                           WHERE lft>=@lft AND rgt<=@rgt AND siteId=@siteId";

        public string Category_ChangeUpdateTreeLeft2 = "UPDATE $PREFIX_category SET lft=lft-(@rgt-@lft)+1 WHERE lft>@torgt AND lft<@lft AND siteId=@siteId";
        public string Category_ChangeUpdateTreeRight2 = "UPDATE $PREFIX_category SET rgt=rgt-(@rgt-@lft)+1 WHERE rgt>=@torgt AND rgt<@lft AND siteId=@siteId";
        public string Category_ChangeUpdateTreeBettown2 = @"UPDATE $PREFIX_category SET lft=lft-(@lft-@torgt),rgt=rgt-(@lft-@torgt)
                                                          WHERE lft>=@lft AND rgt<=@rgt AND siteId=@siteId";
        

        #endregion

        #region 用户相关

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        public readonly string User_GetUserByUsername = "SELECT * FROM $PREFIX_user WHERE username=@username";

        /// <summary>
        /// 根据用户名和密码获取用户信息
        /// </summary>
        public readonly string User_GetUser = "SELECT * FROM $PREFIX_user WHERE username=@Username AND password=@password";

        /// <summary>
        /// 获取所有用户
        /// </summary>
        public readonly string User_GetAllUsers = "SELECT * FROM $PREFIX_user ORDER BY createdate DESC";

        /// <summary>
        /// 检测用户名是否存在
        /// </summary>
        public readonly string User_DectUserNameIsExist = "SELECT name FROM $PREFIX_user WHERE username=@username";


        /// <summary>
        /// 创建用户
        /// </summary>
        public readonly string User_CreateUser = "INSERT INTO $PREFIX_user(siteid,username,password,name,groupID,available,createdate,lastlogindate) VALUES(@siteid,@UserName,@Password,@Name,@GroupID,@available,@CreateDate,@LastLoginDate)";


        /// <summary>
        /// 修改用户密码
        /// </summary>
        public readonly string User_ModifyPassword = "UPDATE $PREFIX_user SET password=@Password WHERE username=@UserName";

        /// <summary>
        /// 更新用户
        /// </summary>
        public readonly string User_UpdateUser = "UPDATE $PREFIX_user SET siteid=@siteid,name=@Name,GroupID=@GroupID,available=@available WHERE username=@username";

        /// <summary>
        /// 删除用户
        /// </summary>
        public readonly string User_DeleteUser = "DELETE FROM $PREFIX_user WHERE  username=@username";


        /****************** 操作相关 ******************/
        /// <summary>
        /// 检查操作的路径是否存在
        /// </summary>
        public readonly string Operation_CheckPathExist = "SELECT path FROM $PREFIX_operation WHERE path=@path";


        /// <summary>
        /// 创建新操作
        /// </summary>
        public readonly string Operation_CreateOperation = "INSERT INTO $PREFIX_operation (name,path,available) VALUES (@Name,@Path,@available)";

        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        public string Member_UpdateUserLastLoginDate = "UPDATE $PREFIX_user SET LastLoginDate=@LastLoginDate WHERE username=@username";



        #endregion

        #region 用户操作相关

        /// <summary>
        /// 删除操作
        /// </summary>
        public readonly string Operation_DeleteOperation = "DELETE FROM $PREFIX_operation WHERE id=@Id";

        /// <summary>
        /// 获取操作
        /// </summary>
        public readonly string Operation_GetOperation = "SELECT * FROM $PREFIX_operation WHERE id=@Id";

        /// <summary>
        /// 获取所有操作
        /// </summary>
        public readonly string Operation_GetOperations = "SELECT * FROM $PREFIX_operation";

        /// <summary>
        /// 更新操作
        /// </summary>
        public readonly string Operation_UpdateOperation = "UPDATE $PREFIX_operation SET name=@Name,path=@Path,available=@available WHERE id=@Id";


        /// <summary>
        /// 获取操作数
        /// </summary>
        public readonly string Operation_GetOperationCount = "SELECT COUNT(0) FROM $PREFIX_operation";

        /// <summary>
        /// 获取分页操作列表
        /// </summary>
        public abstract string Archive_GetPagedOperations { get; }


        /// <summary>
        /// 获取可用或不可用的操作数量
        /// </summary>
        public string Operation_GetOperationsCountByAvailable = "SELECT COUNT(0) FROM $PREFIX_operation WHERE {0}";


        /// <summary>
        /// 获取可用或不可用的操作分页列表
        /// </summary>
        public abstract string Archive_GetPagedOperationsByAvialble { get; }

        #endregion

        #region 用户组相关

        /// <summary>
        /// 获取所有用户组
        /// </summary>
        public readonly string UserGroup_GetAll = "SELECT * FROM $PREFIX_usergroup";

        /// <summary>
        /// 更新用户权限
        /// </summary>
        public readonly string UserGroup_UpdatePermissions = "UPDATE $PREFIX_usergroup SET permissions=@Permissions WHERE id=@GroupId";

        /// <summary>
        /// 重命名用户名
        /// </summary>
        public readonly string UserGroup_RenameGroup = "UPDATE $PREFIX_usergroup SET name=@Name WHERE ID=@GroupId";

        #endregion

        #region 评论相关

        /// <summary>
        /// 插入评论
        /// </summary>
        public abstract string Comment_AddComment { get; }

        /// <summary>
        /// 查询文档评论数目
        /// </summary>
        public readonly string Comment_QueryCommentsCountForArchive = "SELECT count(id) FROM $PREFIX_comment WHERE archiveid=@ArchiveId";

        /// <summary>
        /// 获取文档的评论
        /// </summary>
        public abstract string Comment_GetCommentsForArchive { get; }

        /// <summary>
        /// 获取文档的评论及用户信息
        /// </summary>
        public abstract string Comment_GetCommentDetailsListForArchive { get; }


        /// <summary>
        /// 删除指定会员的评论
        /// </summary>
        public readonly string Comment_DeleteMemberComments = "DELETE FROM $PREFIX_comment WHERE memberid=@id";


        /// <summary>
        /// 删除评论
        /// </summary>
        public readonly string Comment_Delete = "DELETE FROM $PREFIX_comment WHERE id=@Id";

        /// <summary>
        /// 删除指定文档的评论
        /// </summary>
        public readonly string Comment_DeleteArchiveComments = "DELETE FROM $PREFIX_comment WHERE archiveid=@ArchiveId";


        #endregion

        #region 链接

        /// <summary>
        /// 获取链接
        /// </summary>
        public readonly string Link_GetSiteLinksByLinkType =@"SELECT * FROM $PREFIX_link WHERE $PREFIX_link.type=@linkType AND siteid=@siteId ORDER BY $PREFIX_link.orderIndex";

        public readonly string Link_GetSiteLinkById = "SELECT * FROM $PREFIX_link WHERE id=@linkId AND siteid=@siteId";

        /// <summary>
        /// 添加链接
        /// </summary>
        public abstract string Link_AddSiteLink { get; }

        /// <summary>
        /// 更新链接
        /// </summary>
        public abstract string Link_UpdateSiteLink { get; }

        /// <summary>
        /// 删除链接
        /// </summary>
        public readonly string Link_DeleteSiteLink = "DELETE FROM $PREFIX_link WHERE id=@LinkId AND siteid=@siteId";

        #endregion

        #region 消息相关

        /// <summary>
        /// 获取信息
        /// </summary>
        public readonly string Message_GetMessage = "SELECT * FROM $PREFIX_Message WHERE [ID]=@Id";

        /// <summary>
        /// 写入新消息
        /// </summary>
        public readonly string Mesage_InsertMessage = @"INSERT INTO $PREFIX_Message ([SendUID],[ReceiveUID],[Subject],[Content],[HasRead],[Recycle],[SendDate])
                                                        VALUES(@SendUID,@ReceiveUID,@Subject,@Content,@HasRead,@Recycle,@SendDate)";

        /// <summary>
        /// 设置消息为已读
        /// </summary>
        public readonly string Message_SetRead = "UPDATE $PREFIX_Message SET [HasRead]=1 WHERE [receiveUID]=@ReceiveUID AND [ID]=@id";

        /// <summary>
        /// 将消息设为回收
        /// </summary>
        public readonly string Message_SetRecycle = "UPDATE $PREFIX_Message SET [Recycle]=1 WHERE [receiveUID]=@ReceiveUID AND [ID]=@id";

        /// <summary>
        /// 删除消息
        /// </summary>
        public readonly string Message_Delete = "DELETE FROM $PREFIX_Message WHERE [receiveUID]=@ReceiveUID AND [ID]=@id";

        /// <summary>
        /// 获取分页消息条数
        /// {0}->$[condition]
        /// </summary>
        public readonly string Message_GetPagedMessagesCount = "SELECT COUNT([ID]) FROM $PREFIX_Message WHERE Recycle=0 AND {0}";

        /// <summary>
        /// 获取分页消息
        /// </summary>
        public abstract string Message_GetPagedMessages { get; }

        #endregion

        #region 会员相关

        /// <summary>
        /// 检测用户名是否存在
        /// </summary>
        public readonly string Member_DetectUsernameAndNickNameHasExits = "SELECT id FROM $PREFIX_member WHERE username=@Username OR nickname=@Nickname";
        /// <summary>
        /// 注册会员
        /// </summary>
        public abstract string Member_RegisterMember { get; }
        /// <summary>
        /// 写入会员的详细信息
        /// </summary>
        public readonly string Member_InsertMemberDetails = "INSERT INTO $PREFIX_memberdetails values(@UID,@Status,@RegIP,@RegTime,@LastLoginTime,@Token)";
        /// <summary>
        /// 根据会员ID获取会员信息及详细信息
        /// </summary>
        public readonly string Member_GetMemberInfoAndDetails = "SELECT * FROM $PREFIX_member INNER JOIN $PREFIX_memberdetails ON $PREFIX_member.id=$PREFIX_memberdetails.uid WHERE id=@id OR username=@Username";


        /// <summary>
        /// 检测会员名是否存在
        /// </summary>
        public readonly string Member_DetectUsername = "SELECT id FROM $PREFIX_member WHERE username=@Username";

        /// <summary>
        /// 检测昵称是否存在
        /// </summary>
        public readonly string Member_DetectNickname = "SELECT id FROM $PREFIX_member WHERE nickname=@Nickname";

        /// <summary>
        /// 获取会员号
        /// </summary>
        public readonly string Member_GetMemberUID = "SELECT id FROM $PREFIX_member WHERE username=@Username";


        /// <summary>
        /// 验证会员名和密码是否匹配，匹配则返回会员信息
        /// </summary>
        public readonly string Member_VerifyMember = "SELECT * FROM $PREFIX_member WHERE username=@UserName AND password=@password";


        /// <summary>
        /// 根据会员ID获取会员信息
        /// </summary>
        public readonly string Member_GetMemberByID = "SELECT * FROM $PREFIX_member WHERE id=@id";

        /// <summary>
        /// 根据会员名获取会员信息
        /// </summary>
        public readonly string Member_GetMemberByUsername = "SELECT * FROM $PREFIX_member WHERE username=@Username";

        /// <summary>
        /// 会员资料更新
        /// </summary>
        public abstract string Member_Update { get; }

        /// <summary>
        /// 删除会员
        /// </summary>
        public string Member_DeleteMember = "DELETE FROM $PREFIX_member where ID=@id";

        /// <summary>
        /// 删除会员详细信息
        /// </summary>
        public string Member_DeleteMemberDetails = "DELETE FROM $PREFIX_memberdetails WHERE uid=@id";

        /// <summary>
        /// 获取会员数
        /// </summary>
        public string Member_GetMemberCount = "SELECT COUNT(0) FROM $PREFIX_member INNER JOIN $PREFIX_memberdetails ON id=$PREFIX_memberdetails.uid";

        /// <summary>
        /// 获取分页会员列表
        /// </summary>
        public abstract string Member_GetPagedMembers { get; }

        #endregion

        #region 点评相关

        /// <summary>
        /// 创建点评数据
        /// </summary>
        public readonly string Reviews_Create = "INSERT INTO $PREFIX_review VALUES(@id,'')";

        /// <summary>
        /// 获取参与点评的会员数
        /// </summary>
        public readonly string Reviews_GetMember = "SELECT members FROM $PREFIX_review WHERE id=@id";

        /// <summary>
        /// 更新同意点评
        /// </summary>
        public string Reviews_UpdateEvaluate_Agree = "Update $PREFIX_archive set Agree=agree+1 where id=@id";

        /// <summary>
        /// 更新不同意点评
        /// </summary>
        public string Reviews_UpdateEvaluate_Disagree = "UPDATE $PREFIX_archive SET disagree=disagree+1 WHERE id=@id";

        /// <summary>
        /// 更新点评
        /// </summary>
        public string Reviews_UpdateReviews = "UPDATE $PREFIX_review SET members=@Members WHERE id=@id";

        /// <summary>
        /// 删除点评
        /// </summary>
        public string Reviews_Delete = "DELETE FROM $PREFIX_review WHERE id=@id";

        #endregion

        #region 模块相关
        public readonly string Module_Add = "INSERT INTO $PREFIX_modules(siteid,name,isSystem,isDelete) VALUES(@siteid,@name,@isSystem,@isDelete)";

        // public readonly string Module_Delete = "UPDATE $PREFIX_Modules SET isDelete=1 WHERE id=@id";
        public readonly string Module_Delete = "DELETE FROM $PREFIX_modules WHERE id=@id";

        public readonly string Module_Update = "UPDATE $PREFIX_modules SET name=@name, isDelete=@isDelete WHERE id=@id";
        
        public readonly string Module_GetAll = "SELECT * FROM  $PREFIX_modules";
        public readonly string Module_GetByID = "SELECT * FROM  $PREFIX_modules WHERE id=@id";
        public readonly string Module_GetByName = "SELECT * FROM  $PREFIX_modules WHERE name=@name";     
        #endregion

        #region 扩展相关

        public readonly string DataExtend_CreateField = @"
                INSERT INTO $PREFIX_extendField(siteId,name,type,defaultValue,regex,message)
                VALUES(@siteId,@name,@type,@defaultValue,@regex,@message)";


        public readonly string DataExtend_DeleteExtendField =
                @"DELETE FROM $PREFIX_extendField WHERE siteId=@siteId AND id=@id";

        /// <summary>
        /// 获取分类扩展属性绑定次数
        /// </summary>
        public readonly string DataExtend_GetCategoryExtendRefrenceNum = @"
                SELECT Count(0) FROM $PREFIX_extendValue v
                INNER JOIN $PREFIX_archive a ON v.relationId=a.id
                INNER JOIN $PREFIX_category c ON c.id=a.cid
                AND v.relationType=1 AND c.siteId=@siteId AND a.cid=@categoryId AND v.fieldId=@fieldId";


        public readonly string DataExtend_UpdateField = @"
                UPDATE $PREFIX_extendField SET name=@name,type=@type,regex=@regex,
                defaultValue=@defaultValue,message=@message WHERE id=@id AND siteId=@siteId";




        /// <summary>
        /// 获取所有的扩展字段
        /// </summary>
        public readonly string DataExtend_GetAllExtends = @"SELECT * FROM $PREFIX_extendField";

        /// <summary>
        /// 获取相关联的数据
        /// </summary>
        public readonly string DataExtend_GetExtendValues = @"
            SELECT v.id as id,relationId,fieldId,f.name as fieldName,fieldValue
	        FROM $PREFIX_extendValue v INNER JOIN $PREFIX_extendField f ON v.fieldId=f.id
	        WHERE relationId=@relationId AND f.siteId=@siteId AND relationType=@relationType";

        /// <summary>
        /// 获取相关联的数据
        /// </summary>
        public readonly string DataExtend_GetExtendValuesList = @"
            SELECT v.id as id,relationId,fieldId,f.name as fieldName,fieldValue
	        FROM $PREFIX_extendValue v INNER JOIN $PREFIX_extendField f ON v.fieldId=f.id
	        WHERE  relationType=@relationType AND f.siteId=@siteId AND relationId IN ({0})";


        public readonly string DataExtend_ClearupExtendFielValue = @"
                DELETE FROM $PREFIX_extendValue WHERE /*fieldId=@fieldId AND*/
                relationId=@relationId AND relationType=@relationType;";

        /// <summary>
        /// 修改扩展字段值
        /// </summary>
        //public readonly string DataExtend_UpdateFieldValue = "UPDATE $PREFIX_dataExtendField SET attrVal=@attrVal WHERE attrId=@attrId AND rid=@rid";
        public readonly string DataExtend_InsertOrUpdateFieldValue = @"
                    INSERT INTO $PREFIX_extendValue
                    (relationId,fieldId,fieldValue,relationType)
                    VALUES (@relationId,@fieldId,@fieldValue,@relationType)
                ";

        /// <summary>
        /// 获取栏目的扩展属性
        /// </summary>
        public readonly string DataExtend_GetCategoryExtendIdList = @"
                SELECT extend.extendId FROM $PREFIX_categoryExtend extend
                INNER JOIN $PREFIX_category c ON c.id=extend.categoryId
                WHERE c.siteid=@siteId AND c.id=@categoryId
                ";
        #endregion

        #region 模板绑定

        public readonly string TplBind_Add = "INSERT INTO $PREFIX_tplbind (bindID,bindType,tplPath) VALUES(@bindID,@bindType,@tplPath)";
        public readonly string TplBind_Update = "UPDATE $PREFIX_tplbind SET tplPath=@tplPath WHERE bindID=@bindID AND bindType=@bindType";
        public readonly string TplBind_GetBind = "SELECT * FROM $PREFIX_tplbind WHERE bindID=@bindID AND bindType=@bindType";
        public readonly string TplBind_CheckExists = "SELECT count(0) FROM $PREFIX_tplbind WHERE bindID=@bindID AND bindType=@bindType";
        public readonly string TplBind_RemoveBind = "DELETE FROM $PREFIX_tplbind WHERE bindID=@bindID AND bindType=@bindType";
        public readonly string TplBind_GetBindList = "SELECT * FROM $PREFIX_tplbind";

        /// <summary>
        /// 删除未关联的栏目模版
        /// </summary>
        public readonly string TplBind_RemoveErrorCategoryBind = "DELETE FROM $PREFIX_tplbind WHERE bindID NOT IN (SELECT id FROM $PREFIX_category) AND bindType IN(3,4)";

        #endregion

        #region 表格

        /// <summary>
        /// 检查表格的名称是否存在
        /// </summary>
        public readonly string Table_GetTableIDByName = "SELECT id FROM $PREFIX_table WHERE name=@name";

        /// <summary>
        /// 添加表格
        /// </summary>
        public  readonly string Table_Add = "INSERT INTO $PREFIX_table (name,note,apiserver,issystem,available) VALUES(@name,@note,@apiserver,@issystem,@available)";

        /// <summary>
        /// 删除表格
        /// </summary>
        public readonly string Table_DeleteTable = "DELETE FROM $PREFIX_table WHERE id=@tableid";

        /// <summary>
        /// 添加表格列
        /// </summary>
        public readonly string Table_CreateColumn = "INSERT INTO $PREFIX_table_column (tableid,name,note,validformat,orderindex) VALUES(@tableid,@name,@note,@validformat,@orderindex)";

        /// <summary>
        /// 更新表格列
        /// </summary>
        public readonly string Table_UpdateColumn = "UPDATE $PREFIX_table_column SET name=@name,note=@note,validformat=@validformat,orderindex=@orderindex WHERE id=@columnid";

        /// <summary>
        /// 获取列
        /// </summary>
        public readonly string Table_GetColumn = "SELECT * FROM $PREFIX_table_column WHERE id=@columnid";


        /// <summary>
        /// 删除列
        /// </summary>
        public readonly string Table_DeleteColumn = "DELETE FROM $PREFIX_table_column WHERE tableid=@tableid AND id=@columnid";

        /// <summary>
        /// 删除所有列
        /// </summary>
        public readonly string Table_DeleteColumns = "DELETE FROM $PREFIX_table_column WHERE tableid=@tableid";

        /// <summary>
        /// 获取表格记录
        /// </summary>
        public readonly string Table_GetRowsCount = "SELECT count(0) FROM $PREFIX_table_row WHERE tableid=@tableid";


        /// <summary>
        /// 修改表格
        /// </summary>
        public readonly string Table_Update = "UPDATE $PREFIX_table set name=@name,note=@note,apiserver=@apiserver,issystem=@issystem,available=@available WHERE id=@tableid";

        /// <summary>
        /// 获取表格
        /// </summary>
        public readonly string Table_GetTableByID = "SELECT * FROM $PREFIX_table WHERE id=@tableid";

        /// <summary>
        /// 获取最小的表格编号
        /// </summary>
        public readonly string Table_GetMinTableID = "SELECT min(id) FROM $PREFIX_table";

        /// <summary>
        /// 是否存在系统表格
        /// </summary>
        public readonly string Table_HasExistsSystemTale = "SELECT count(0) FROM $PREFIX_table WHERE id=@tableid AND issystem";

        /// <summary>
        /// 获取所有表格
        /// </summary>
        public readonly string Table_GetTables = "SELECT * FROM $PREFIX_table";

        /// <summary>
        /// 获取所有列根据表格编号
        /// </summary>
        public readonly string Table_GetColumnsByTableID = "SELECT * FROM $PREFIX_table_column WHERE tableid=@tableid ORDER BY orderindex ASC";

        /// <summary>
        /// 创建行
        /// </summary>
        public readonly string Table_CreateRow = "INSERT INTO $PREFIX_table_row (tableid,submittime) VALUES(@tableid,@submittime)";

        /// <summary>
        /// 删除行
        /// </summary>
        public readonly string Table_DeleteRow = "DELETE FROM $PREFIX_table_row WHERE id=@rowid AND tableid=@tableid";

        /// <summary>
        /// 获取行
        /// </summary>
        public readonly string Table_GetRow = "SELECT * FROM $PREFIX_table_row WHERE id=@rowid";

        /// <summary>
        /// 获取行数据
        /// </summary>
        public readonly string table_GetRowData = "SELECT * FROM $PREFIX_table_rowdata WHERE rid IN ($[range])";


        /// <summary>
        /// 清理删除的列在数据行中的数据
        /// </summary>
        public readonly string Table_ClearDeletedColumnData = "DELETE FROM $PREFIX_table_rowdata WHERE rid IN (SELECT id FROM $PREFIX_table_row WHERE tableid=@tableid) AND cid=@columnid";


        /// <summary>
        /// 清理删除的行在数据行中的数据
        /// </summary>
        public readonly string Table_ClearDeletedRowData = "DELETE FROM $PREFIX_table_rowdata WHERE rid = (SELECT id FROM $PREFIX_table_row WHERE tableid=@tableid AND id=@rowid)";


      
    




        /// <summary>
        /// 获取最后创建的行号
        /// </summary>
        public abstract string Table_GetLastedRowID { get; }

        /// <summary>
        /// 插入数据
        /// </summary>
        public abstract string Table_InsertRowData { get; }

        /// <summary>
        /// 获取分页行
        /// </summary>
        public abstract string Table_GetPagedRows { get; }
       
        #endregion

        #region 站点

        /// <summary>
        /// 创建站点
        /// </summary>
        public readonly string Site_CreateSite = @"INSERT INTO $PREFIX_site(name,dirname,domain,location,tpl,language,
                                    note,seotitle,seokeywords,seodescription,state,protel,prophone,profax,proaddress,
                                    proemail,im,postcode,pronotice,proslogan)VALUES
                                    (@name,@dirname,@domain,@location,@tpl,@language,@note,@seotitle,@seokeywords,@seodescription,@state,
                                    @protel,@prophone,@profax,@proaddress,@proemail,@im,@postcode,@pronotice,@proslogan)";
       
        /// <summary>
        /// 获取所有站点
        /// </summary>
        public readonly string Site_GetSites = "SELECT * FROM $PREFIX_site";

        /// <summary>
        /// 更新站点
        /// </summary>
        public readonly string Site_EditSite = @"UPDATE $PREFIX_site SET name=@name,dirname=@dirname,
                                        domain=@domain,location=@location,tpl=@tpl,
                                        language=@language,note=@note,seotitle=@seotitle,seokeywords=@seokeywords,
                                        seodescription=@seodescription,state=@state,protel=@protel,prophone=@prophone,
                                        profax=@profax,proaddress=@proaddress,proemail=@proemail,im=@im,
                                        postcode=@postcode,pronotice=@pronotice,proslogan=@proslogan WHERE siteid=@siteid";

       
       
       
       
      
     
      


        #endregion
        public readonly String Link_RemoveRelatedLinks = @"DELETE FROM $PREFIX_related_link
                        WHERE typeIndent = @typeIndent AND relatedId = @relatedId
                        AND id in ({0})
                        ";

        public readonly string Link_GetRelatedLinks =@"SELECT * FROM $PREFIX_related_link
                        WHERE typeIndent = @typeIndent AND relatedId = @relatedId";

        public readonly String Link_InsertRelatedLink = @"
                        INSERT INTO $PREFIX_related_link(typeIndent,relatedId,name,title,uri,enabled)
                        VALUES (@typeIndent,@relatedId,@name,@title,@uri,@enabled)";

        public readonly String Link_UpdateRelatedLink=@"UPDATE $PREFIX_related_link
	                    SET name = @name,title = @title,uri = @uri,enabled = @enabled
	                    WHERE id=@id AND typeIndent = @typeIndent AND relatedId = @relatedId";
    }
}