﻿/*
* Copyright(C) 2010-2012 S1N1.COM
* 
* File Name	: TplBindType
* Author	: Newmin (new.min@msn.com)
* Create	: 2012/10/28 7:15:54
* Description	:
*
*/


namespace AtNet.Cms.Domain.Interface.Site.Template
{
    /// <summary>
    /// 模板绑定类型(顺序不可修改)
    /// </summary>
    public enum TemplateBindType:int
    {
        /// <summary>
        /// 模块栏目列表模板
        /// </summary>
        ModuleCategoryTemplate=1,

        /// <summary>
        /// 模块文档模板
        /// </summary>
        ModuleArchiveTemplate=2,

        /// <summary>
        /// 栏目模板
        /// </summary>
        CategoryTemplate=3,

        /// <summary>
        /// 栏目文档模板
        /// </summary>
        CategoryArchiveTemplate=4,

        /// <summary>
        /// 文档模板
        /// </summary>
        ArchiveTemplate=5
    }
}
