﻿/*
* Copyright(C) 2010-2013 S1N1.COM
* 
* File Name	: SiteState
* Author	: Newmin (new.min@msn.com)
* Create	: 2013/05/21 19:59:54
* Description	:
*
*/

namespace AtNet.Cms.Domain.Interface.Site
{

    /// <summary>
    /// 站点状态
    /// </summary>
    public enum SiteState:int
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal=1,

        /// <summary>
        /// 暂停访问
        /// </summary>
        Paused=2,

        /// <summary>
        /// 关闭
        /// </summary>
        Closed=3
    }
}
