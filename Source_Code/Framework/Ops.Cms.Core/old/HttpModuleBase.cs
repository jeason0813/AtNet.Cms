﻿using System;
using System.Web;
using AtNet.Cms.Core;

namespace AtNet.Cms.old
{
    public class HttpModuleBase : IHttpModule,System.Web.SessionState.IRequiresSessionState
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += On_BeginRequest;
            context.Error += On_Error;
        }
        public virtual void On_BeginRequest(object o,EventArgs e)
        {
            
        }
        public virtual void On_Error(object o, EventArgs e)
        {
            HttpContext context =(o as HttpApplication).Context;
            Exception ex = context.Server.GetLastError().InnerException;
            if (ex == null) ex = context.Server.GetLastError();

           //context.ClearError();

            CmsContext.SaveErrorLog(ex);

        }
    }
}