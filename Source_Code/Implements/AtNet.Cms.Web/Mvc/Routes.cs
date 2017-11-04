﻿/*
 * Copyright(C) 2010-2013 S1N1.COM
 * 
 * File Name	: Routes
 * Author	: Newmin (new.min@msn.com)
 * Create	: 2013/04/04 15:59:54
 * Description	:
 *
 */


using AtNet.Cms;
using AtNet.Cms.Conf;
using AtNet.Cms.Domain.Interface.Enum;
using AtNet.DevFw;
using AtNet.DevFw.PluginKernel;
using AtNet.DevFw.Web;
using AtNet.DevFw.Web.Plugin;

namespace AtNet.Cms.Web.Mvc
{
    using AtNet.Cms;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Routing;

	/// <summary>
	/// CMS路由设置
	/// </summary>
	public static class Routes
	{
		/// <summary>
		/// 注册路由
		/// </summary>
		/// <param name="routes">路由集合</param>
		public static void RegisterCmsRoutes(RouteCollection routes, Type cmsHandleType)
		{
			//路由前缀，前缀+虚拟路径
			//string routePrefix = (String.IsNullOrEmpty(prefix) ? "" : prefix + "/")
			//    + (String.IsNullOrEmpty(Settings.SYS_VIRTHPATH) ? String.Empty:Settings.SYS_VIRTHPATH + "/");

			// string urlPrefix = "/" + routePrefix;
			string urlPrefix=String.Empty , routePrefix = String.Empty;

			//Cms 控制器名称
			string cmsControllerName = cmsHandleType == null ? "Cms_Core" : Regex.Replace(cmsHandleType.Name, "controller$", String.Empty, RegexOptions.IgnoreCase);

			//MVC路由规则词典
			IDictionary<UrlRulePageKeys, string[]> dict = new Dictionary<UrlRulePageKeys, string[]>();

			dict.Add(UrlRulePageKeys.Common, new string[] { "cms_common", routePrefix + "{0}", urlPrefix + "{0}" });

			dict.Add(UrlRulePageKeys.Search, new string[] { "cms_search", routePrefix + "search", urlPrefix + "search?w={0}&c={1}" });
			dict.Add(UrlRulePageKeys.SearchPager, new string[] { null, null, urlPrefix + "search?w={0}&c={1}&p={2}" });

			dict.Add(UrlRulePageKeys.Tag, new string[] { "cms_tag", routePrefix + "tag", urlPrefix + "tag?t={0}" });
			dict.Add(UrlRulePageKeys.TagPager, new string[] { null, null, urlPrefix + "tag?t={0}&p={1}" });

			dict.Add(UrlRulePageKeys.Category, new string[] { "cms_category", routePrefix + "{*allcate}", urlPrefix + "{0}/" });
			dict.Add(UrlRulePageKeys.CategoryPager, new string[] { null, null, urlPrefix + "{0}/p{1}.html" });

			dict.Add(UrlRulePageKeys.Archive, new string[] { "cms_archive", routePrefix + "{*allhtml}", urlPrefix + "{0}/{1}.html" });
			dict.Add(UrlRulePageKeys.SinglePage, new string[] { null, null, urlPrefix + "{0}.html" });


			//注册插件路由
			//Cms.Plugins.Extends.MapRoutes(routes);
			
			//Cms.Plugins.MapRoutes(routes);


			#region 设置路由

			

			
			
			
			
			//获取所有的控制器名称，以"|"排除
			
			string controllerArr="Cms_Core";
			
			/*
           string controllerArr="";
           
           Regex ctrReg=new Regex("(^\\||controller)",RegexOptions.IgnoreCase);
           
           foreach(Type t in CmsMvc.GetAllControllers())
           {
           		controllerArr+="|"+t.Name;
           }
           controllerArr=ctrReg.Replace(controllerArr,"");
			 */
			
			#region 系统路由
			
			//忽略静态目录
			routes.IgnoreRoute("{staticdir}/{*pathInfo}", new { staticdir = "^(uploads|content|static|plugins|libs|scripts|images|style|themes)$" });

			//tempaltes路由处理(忽略静态文件)
			routes.IgnoreRoute("templates/{*pathInfo}", new { pathInfo = "^(.+?)\\.(jpg|jpeg|css|js|json|xml|gif|png|bmp)$" });
			routes.MapRoute("tpl_catchall", "templates/{*catchall}", new { controller = cmsControllerName, action = "Disallow" });

			//安装路由
			routes.Add("install_route",new Route("install/process",new CmsInstallRouteHandler()));
			
			//管理后台
			routes.Add("administrator_route",new Route(Settings.SYS_ADMIN_TAG,new CmsManagerRouteHandler()));
			
            //WebAPI接口
            //routes.Add("webapi", new Route("webapi/{*path}", new WebApiRouteHandler()));
            routes.Add("webapi_router", new Route("webapi", new WebApiRouteHandler()));
            routes.Add("webapi_subsite_router", new Route("{site}/webapi", new WebApiRouteHandler()));

			//插件服务路由
			IRouteHandler pluginHandler  = new PluginRouteHandler(); 
			//routes.Add("extend_do",new Route("{module}.do/{*path}",pluginHandler));
			routes.Add("plugin_cms_sh",new Route("{plugin}.sh/{*path}",pluginHandler));
            routes.Add("plugin_cms_aspx", new Route("{plugin}.sh.aspx/{*path}", pluginHandler));
			
			//支付
			//routes.Add(new Route(routePrefix + "netpay", new CmsNetpayHandler()));
			if(FwCtx.Mono())
			{
                routes.Add("mono_plugin_sh", new Route("{plugin}.sh", pluginHandler));
                routes.Add("mono_plugin_aspx", new Route("{plugin}.sh.aspx", pluginHandler));
			}

			#endregion
			

			//搜索档案
			routes.MapRoute(
				dict[UrlRulePageKeys.Search][0]+"_site","{site}/"+dict[UrlRulePageKeys.Search][1],
				new { controller = cmsControllerName, action = "Search", p = 1 }
			);


			//搜索档案
			routes.MapRoute(
				dict[UrlRulePageKeys.Search][0], dict[UrlRulePageKeys.Search][1],
				new { controller = cmsControllerName, action = "Search", p = 1 }
			);

			//标签档案
			routes.MapRoute(
				dict[UrlRulePageKeys.Tag][0], dict[UrlRulePageKeys.Tag][1],
				new { controller = cmsControllerName, action = "Tag", p = 1 }
			);



			//多站点
			//if (Cms.MultSiteVersion)
			//{
			//默认路由
			//    routes.MapRoute(
			//        "IndexPage",
			//        "{sitedir}",
			//        new { controller = cmsControllerName, action = "Index", id = UrlParameter.Optional }
			//    );
			//}

			
			//栏目档案列表
			routes.MapRoute(
				dict[UrlRulePageKeys.Category][0], dict[UrlRulePageKeys.Category][1],
				new { controller = cmsControllerName, action = "Category", page = 1 }, new { allcate = "^(?!"+controllerArr +")((.+?)/(p\\d+\\.html)?|([^/]+/)*[^\\.]+)$" }
			);

			#region Route For Mono

			
			//if (isMono)
			//{

			/*************Category Only for mono *******************/

			//包含前缀情况下对Mono平台的/{lang}/进行支持
			/*
                if (routePrefix != "")
                {
                    routes.MapRoute(
                    "cms_mono_index",
                    routePrefix,
                    new { controller = cmsControllerName, action = "Index" }
                );
                }*/
			/**********************************************/
			//}
			

			#endregion

			//显示档案
			routes.MapRoute(
				dict[UrlRulePageKeys.Archive][0], dict[UrlRulePageKeys.Archive][1],
				new { controller = cmsControllerName, action = "Archive" }, new { allhtml = "^(.+?).html$" }
			);

			//默认路由
			routes.MapRoute(
				"Default",                                                                                                       // Route name
				routePrefix + "{controller}/{action}/{id}",                                                         // URL with parameters
				new { controller = cmsControllerName, action = "Index", id = UrlParameter.Optional }         // Parameter defaults
			);


			routes.MapRoute("allpath", "{*path}", new { controller = cmsControllerName, action = "NotFound" });

			#endregion

			#region 设置地址

			IDictionary<UrlRulePageKeys, string> urlDict = new Dictionary<UrlRulePageKeys, string>();
			foreach (KeyValuePair<UrlRulePageKeys, string[]> p in dict)
			{
				urlDict.Add(p.Key, p.Value[2]);
			}

			//设置地址
			TemplateUrlRule.SetUrl(UrlRuleType.Mvc, urlDict);

			//使用MVC
			TemplateUrlRule.SetRule(UrlRuleType.Mvc);

			#endregion

		}

		/// <summary>
		/// 注册路由
		/// </summary>
		public static void RegisterCmsRoutes(RouteCollection routes)
		{
			RegisterCmsRoutes(routes, null);
		}
		
	}
}
