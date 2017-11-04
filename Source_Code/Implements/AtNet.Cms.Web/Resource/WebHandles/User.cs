﻿

namespace OPSite.WebHandler
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using Ops.Web;
    using Ops.Cms;
    using Ops.Cms.BLL;

    [WebExecuteable]
    public class User
    {
        private static UserBLL bll = new UserBLL();

        /// <summary>
        /// 创建新操作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public bool CreateOperation(string name, string path)
        {
            return bll.CreateNewOperation(name, path);
        }
        public void UpdateOperation(string id, string name, string path,string available)
        {
            bll.UpdateOperation(int.Parse(id), name, path, available=="true");
        }

        [Post]
        public void UpdatePermission(string groupid, string permissionStr)
        {
            bll.UpdateUserGroupPermissions((UserGroups)(int.Parse(groupid)),
                bll.ConvertToPermissionArray(permissionStr.Replace("|",",")));
        }

    }
}