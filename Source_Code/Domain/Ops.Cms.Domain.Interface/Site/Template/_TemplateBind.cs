﻿/*
* Copyright(C) 2010-2012 OPSoft Inc
* 
* File Name	: TemplateBind
* Author	: Newmin (new.min@msn.com)
* Create	: 2012/10/28 7:19:59
* Description	:
*
*/


namespace Spc.Models
{
    /// <summary>
    /// 模板绑定
    /// </summary>
    public class TemplateBind
    {
        /// <summary>
        /// 绑定关系编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 绑定类型
        /// </summary>
        public int BindType { get; set; }

        /// <summary>
        /// 绑定关联编号
        /// </summary>
        public string BindID { get; set; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public string TplPath { get; set; }


    }
}
