﻿/*
 * Copyright(C) 2010-2012 OPSoft Inc
 * 
 * File Name	: Module
 * Author	: Newmin (new.min@msn.com)
 * Create	: 2012/9/30 11:25:15
 * Description	:
 * Mofify:
 *   2013-05-18  10:27  newmin [!]:SetProperty_POST
 */



namespace Spc.WebManager
{
    using Ops.Cms;
    using Spc.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;

	internal class Module:BasePage
	{
		#region Module

		public void All_GET()
		{
			int siteID = base.CurrentSite.SiteID;
			StringBuilder sb = new StringBuilder();
			TemplateBind tb;                                        //模板绑定
			IList<Models.Module> tables = new List<Models.Module>(CmsLogic.Module.GetSiteModules(siteID));
			foreach (var m in tables)
			{
				if (!m.IsDelete)
				{
					sb.Append("<tr><td class=\"center\">").Append(m.ID).Append("</td><td>").Append(m.Name)
						.Append("</td><td>").Append(m.IsSystem ? "<span style=\"color:red\">系统</span>" : "自定义")
						.Append("</td><td class=\"center\">");

					//获取栏目模板绑定
					tb = CmsLogic.TemplateBind.GetBind(TemplateBindType.ModuleCategoryTemplate, m.ID.ToString());

					sb.Append(tb == null ? "默认" :"/"+ tb.TplPath).Append("</td><td class=\"center\">");

					//获取文档模板绑定
					tb = CmsLogic.TemplateBind.GetBind(TemplateBindType.ModuleArchiveTemplate, m.ID.ToString());

					sb.Append(tb == null ? "默认" : "/"+tb.TplPath).Append("</td>");
					if (m.IsSystem && UserState.Administrator.Current.SiteID != 0)
					{
						sb.Append("<td colspan=\"3\">系统模块，无权修改</td>");
					}
					else
					{
						sb.Append("<td><button class=\"edit\" /></td><td class=\"center\"><button class=\"file\" /></td><td><button class=\"delete\" /></td>");
					}

					sb.Append("</tr>");
				}
			}

            base.RenderTemplate(ResourceMap.ModuleList, new
			                    {
			                    	moduleListHtml=sb.ToString(),
			                    	count=tables.Count.ToString()
			                    });
		}

		
		public void Create_GET()
		{
			StringBuilder sb = new StringBuilder();
			string archiveTplOpts,
			categoryTplOpts;

			//模板目录
			DirectoryInfo dir = new DirectoryInfo(
				String.Format("{0}templates/{1}",
				              AppDomain.CurrentDomain.BaseDirectory,
				              Settings.TPL_MultMode ? "" :base.CurrentSite.Tpl + "/"
				             ));

			EachClass.EachTemplatePage(dir, sb, TemplatePageType.Archive);
			archiveTplOpts = sb.ToString();

			sb.Remove(0, sb.Length);

			EachClass.EachTemplatePage(dir, sb, TemplatePageType.Category);
			categoryTplOpts = sb.ToString();

            base.RenderTemplate(ResourceMap.EditModule, new
			                    {
			                    	btnText = "添加",
			                    	tplName = "",
			                    	categoryTplPath = "",
			                    	archiveTplPath = "",
			                    	category_tpls = categoryTplOpts,
			                    	archive_tpls = archiveTplOpts
			                    });
		}

		public void Create_POST()
		{
			int siteID = base.CurrentSite.SiteID;
			string name = base.Request.Form["tplname"],
			categoryTplPath = base.Request.Form["categoryTplPath"],
			archiveTplPath = base.Request.Form["archiveTplPath"];
			

			//
			//TODO:加重名验证
			//

			bool result = CmsLogic.Module.AddModule(siteID,name);

			if (result)
			{
				int moduleID = CmsLogic.Module.GetModule(siteID,name).ID;

				//如果设置了栏目视图路径，则保存
				if (!String.IsNullOrEmpty(categoryTplPath))
				{
					CmsLogic.TemplateBind.SetBind(new TemplateBind
					                              {
					                              	BindID = moduleID.ToString(),
					                              	BindType = (int)TemplateBindType.ModuleCategoryTemplate,
					                              	TplPath = categoryTplPath
					                              });
				}

				//如果设置了文档视图路径，则保存
				if (!String.IsNullOrEmpty(archiveTplPath))
				{
					CmsLogic.TemplateBind.SetBind(new TemplateBind
					                              {
					                              	BindID = moduleID.ToString(),
					                              	BindType = (int)TemplateBindType.ModuleArchiveTemplate,
					                              	TplPath = archiveTplPath
					                              });
				}

				base.RenderSuccess("");
				return;
			}
			base.RenderError("模块已经存在!");
		}

		public void Edit_GET()
		{
			string archiveTpl,
			categoryTpl,
			archiveTplOpts,
			categoryTplOpts;


			var module = CmsLogic.Module.GetModule(int.Parse(HttpContext.Current.Request["id"]));

			StringBuilder sb = new StringBuilder();

			//模板目录
			DirectoryInfo dir = new DirectoryInfo(
				String.Format("{0}templates/{1}",
				              AppDomain.CurrentDomain.BaseDirectory,
				              Settings.TPL_MultMode ? "" :base.CurrentSite.Tpl+ "/"
				             ));

			EachClass.EachTemplatePage(dir, sb,TemplatePageType.Archive);
			archiveTplOpts=sb.ToString();

			sb.Remove(0,sb.Length);

			EachClass.EachTemplatePage(dir, sb, TemplatePageType.Category);
			categoryTplOpts=sb.ToString();



			TemplateBind tb;

			//获取文档模板绑定
			tb = CmsLogic.TemplateBind.GetBind(TemplateBindType.ModuleCategoryTemplate, module.ID.ToString());
			categoryTpl =tb==null?"": tb.TplPath;

			//获取文档模板绑定
			tb = CmsLogic.TemplateBind.GetBind(TemplateBindType.ModuleArchiveTemplate,module.ID.ToString());
			archiveTpl = tb == null ? "" : tb.TplPath;


            base.RenderTemplate(ResourceMap.EditModule, new
			                    {
			                    	btnText = "修改",
			                    	tplName =module.Name,
			                    	categoryTplPath =categoryTpl,
			                    	archiveTplPath=archiveTpl,
			                    	category_tpls= categoryTplOpts,
			                    	archive_tpls=archiveTplOpts
			                    });
		}

		public void Edit_POST()
		{
			HttpRequest request = HttpContext.Current.Request;
			int id = int.Parse(request.Form["id"]);
			string name = request.Form["tplname"];

			//
			//TODO:加重名验证
			//

			bool result=CmsLogic.Module.UpdateModule(new Models.Module
			                                         {
			                                         	ID = id,
			                                         	Name = name
			                                         });
			if (result)
			{

				string categoryTplPath = request.Form["categoryTplPath"],
				archiveTplPath = request.Form["archiveTplPath"];

				TemplateBind tb, tb2;

				tb = CmsLogic.TemplateBind.GetBind(TemplateBindType.ModuleCategoryTemplate, id.ToString());
				tb2 = CmsLogic.TemplateBind.GetBind(TemplateBindType.ModuleArchiveTemplate, id.ToString());

				//如果设置了栏目视图路径，则保存
				if (!String.IsNullOrEmpty(categoryTplPath) || tb!=null)
				{
					if (tb == null)
					{
						tb = new TemplateBind
						{
							BindType = (int)TemplateBindType.ModuleCategoryTemplate,
							BindID = id.ToString()
						};
					}
					tb.TplPath = categoryTplPath;
					CmsLogic.TemplateBind.SetBind(tb);
				}

				//如果设置了文档视图路径，则保存
				if (!String.IsNullOrEmpty(archiveTplPath) || tb2!=null)
				{
					if (tb2 == null)
					{
						tb2 = new TemplateBind
						{
							BindType = (int)TemplateBindType.ModuleArchiveTemplate,
							BindID = id.ToString()
						};
					}
					tb2.TplPath = archiveTplPath;
					CmsLogic.TemplateBind.SetBind(tb2);
				}
				base.RenderSuccess();
				return;
			}

			base.RenderError("");
		}

		public void Delete_POST()
		{
			HttpRequest request = HttpContext.Current.Request;
			int id = int.Parse(request.Form["id"]);
			var m = CmsLogic.Module.GetModule(id);
			if (m.IsSystem)
			{
				HttpContext.Current.Response.Write("系统模块不允许删除！");
				return;
			}
			m.IsDelete = true;
			CmsLogic.Module.UpdateModule(m);

		}

		#endregion

		#region 自定义属性

		public void SetProperty_GET()
		{
			int id = int.Parse(HttpContext.Current.Request.QueryString["id"]);
			StringBuilder sb = new StringBuilder();
			IList<DataExtendAttr> list=new List<DataExtendAttr>(CmsLogic.DataExtend.GetExtendAttrs(id));
			if (list != null && list.Count != 0)
			{
				foreach (var p in list)
				{
					sb.Append("addProperty('").Append(p.ID.ToString()).Append("','")
						.Append(p.AttrName.Replace("'","\\'")).Append("','")
						.Append(p.AttrType).Append("', '")
						.Append(p.AttrVal.Replace("'","\\'")).Append("',")
						.Append(p.Enabled?"true":"false").Append(");");
				}
			}
			else
			{
				sb.Append("addProperty();");
			}

            base.RenderTemplate(ResourceMap.SetProperties, new
			                    {
			                    	init=sb.ToString()
			                    });
		}
		public void SetProperty_POST()
		{
			//
			//TODO:优化
			//

			var form = HttpContext.Current.Request.Form;
			int id = int.Parse(form["id"]);

			DataExtendAttr p;

			//迭代已经存在属性，并清理
			IList<DataExtendAttr> list = new List<DataExtendAttr>(CmsLogic.DataExtend.GetExtendAttrs(id));
			bool listNotNull = list != null && list.Count != 0;


			if (form.Keys.Count <= 4)
			{
				goto clear;
			}

			string isExits;


			//索引列表
			int[] indexList = new int[(form.Keys.Count - 3) / 5];  //3个其他参数,5对属性值
			int i = 0;
			foreach (string key in form.Keys)
			{
				if (key.StartsWith("attrID_"))
				{
					indexList[i] = int.Parse(key.Replace("attrID_", String.Empty));
					++i;
				}
			}

			//排序
			Array.Sort(indexList, (a, b) => { return a > b ? 1 : -1; });


			foreach (int index in indexList)
			{

				
				//迭代属性并进行保存和新增操作
				p = new DataExtendAttr
				{
					Enabled = form["enabled_" + index.ToString()]=="on",
					AttrVal = form["attrVal_" + index.ToString()],
					ExtendID = id,
					AttrType = form["attrType_" + index.ToString()],
					AttrName = form["attrName_" + index.ToString()]
				};

				isExits = form["exist_" + index.ToString()];
				
				//判断是更新还是添加属性
				if (isExits != null && form["exist_" + index.ToString()] == "yes")
				{
					//获取编号
					foreach (var m in list)
					{
						if (m.ID==int.Parse(form["attrID_" + index.ToString()]))
						{
							p.ID = m.ID;
							list.Remove(m); //删除更新的项目
							break;
						}
					}

					//更新属性
					CmsLogic.DataExtend.UpdateExtendAttr(p);

				}
				else
				{
					CmsLogic.DataExtend.AddExtendAttr(p);
				}
			}

			//清理
		clear:

			//清理删除的属性
			if (listNotNull)
			{
				foreach (var m in list)
				{
					CmsLogic.DataExtend.DeleteExtendAttr(m.ID);

				}
			}


			//清理缓存
			CmsLogic.DataExtend.RebuiltExtends();

		}




		#endregion
	}
}
