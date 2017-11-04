﻿/*
* Copyright(C) 2010-2013 S1N1.COM
* 
* File Name	: CmsEventRegister.cs
* Author	: Newmin (new.min@msn.com)
* Create	: 2013/05/21 19:59:54
* Description	:
*
*/

using System.Linq;
using AtNet.Cms.BLL;
using AtNet.Cms.CacheService;
using AtNet.Cms.Conf;
using AtNet.Cms.Domain.Interface._old;
using AtNet.Cms.Resource;
using StructureMap;

namespace AtNet.Cms
{
    public class CmsEventRegister
    {
        /// <summary>
        /// CMS初始化
        /// </summary>
        public static void Init()
        {
            //设置依赖反转
            ObjectFactory.Configure(x =>
            {
                //x.For<IArchiveModel>().Singleton().Use<ArchiveBLL>();
                // x.For<ICategoryModel>().Singleton().Use<CategoryBLL>();
                x.For<IComment>().Singleton().Use<CommentBll>();
                // x.For<ILink>().Singleton().Use<LinkBLL>();
                x.For<Imember>().Singleton().Use<MemberBLL>();
                x.For<Imessage>().Singleton().Use<MessageBLL>();
                x.For<Imodule>().Singleton().Use<ModuleBLL>();
                //x.For<ISite>().Singleton().Use<SiteBLL>();
                // x.For<ITemplateBind>().Singleton().Use<TemplateBindBLL>();
                x.For<IUser>().Singleton().Use<UserBLL>();
                x.For<ITable>().Singleton().Use<TableBLL>();
            });


            //读取站点
            if (Cms.Installed)
            {
                Cms.RegSites(SiteCacheManager.GetAllSites().ToArray());
            }

            //内嵌资源释放
            SiteResourceInit.Init();

            //设置可写权限
            AtNet.Cms.Cms.Utility.SetDirCanWrite(CmsVariables.RESOURCE_PATH);
            AtNet.Cms.Cms.Utility.SetDirCanWrite("templates/");
            AtNet.Cms.Cms.Utility.SetDirCanWrite(CmsVariables.FRAMEWORK_PATH);
            AtNet.Cms.Cms.Utility.SetDirCanWrite(CmsVariables.PLUGIN_PATH);
            AtNet.Cms.Cms.Utility.SetDirCanWrite(CmsVariables.TEMP_PATH + "update");
            AtNet.Cms.Cms.Utility.SetDirHidden("config");
            AtNet.Cms.Cms.Utility.SetDirHidden("bin");
        }
    }
}