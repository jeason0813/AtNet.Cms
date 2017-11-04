//
// HTML����
//  �޸�˵����
//      2012-12-29  newmin  [+]:Link & SAList
//      2013-03-05  newmin  [+]: ��ǩ��д
//      2013-03-06  newmin  [+]: ����ģ��
//      2013-03-07  newmin  [+]: ������ݱ�ǩ������ "[ ]",��Ҫ����outline[200]
//      2013-03-11  newmin  [+]: ���authorname�У�������ʾ��������
//  2013-04-25  22:28 newmin [+]:PagerArchiveList���
//  2013-06-07  22:15 newmin [+]:MCategoryTree,MCategoryList
//  2013-06-08  10:02 newmin [!]:CategoryTree_Iterator ����TreeResultHandle,������isRoot,�ж�root���Ƿ�ģ����ͬ
//  2013-06-08  10:22 newmin [-]:ɾ��MCategoryTree
//  2013-09-05  07:14 newmin [+]:���region
//

using AtNet.Cms;
using AtNet.Cms.BLL;
using AtNet.Cms.Cache.CacheCompoment;
using AtNet.Cms.CacheService;
using AtNet.Cms.Domain.Interface.Enum;
using AtNet.Cms.Domain.Interface.Models;
using AtNet.Cms.Domain.Interface.Site.Category;
using AtNet.DevFw.Framework.Xml.AutoObject;
using AtNet.DevFw.Toolkit.Region;

namespace AtNet.Cms.Template
{
    using AtNet.Cms;
    using AtNet.Cms.DataTransfer;
    using AtNet.DevFw.Template;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    [XmlObject("CmsTemplateTag", "ģ���ǩ")]
    public partial class CmsTemplates : CmsTemplateDataMethod
    {
        private static Type __type;

        //��ǩ�ļ�
        static CmsTemplates()
        {
            __type = typeof(CmsTemplates);
        }


        #region �ĵ�

        /* ==================================== �鿴�ĵ� ====================================*/

        /// <summary>
        /// �ĵ�����
        /// </summary>
        /// <param name="idOrAlias"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�ĵ�", @"
        	<b>������</b><br />
        	==========================<br />
			1:�ĵ��ı�Ż�����������ĵ��б��в�ѯ��
			2:HTML��ʽ
		")]
        public string Archive(string idOrAlias, string format)
        {
            object id;
            if (Regex.IsMatch(idOrAlias, "^\\d+$"))
            {
                id = int.Parse(idOrAlias);
            }
            else
            {
                id = idOrAlias;
            }
            return base.Archive(this.site.SiteId, id, format);
        }

        [TemplateTag]
        [XmlObjectProperty("��ȡ�ĵ�", @"
        	<b>������</b><br />
        	==========================<br />
			1:HTML��ʽ
		")]
        public string Archive(string format)
        {
            object id = HttpContext.Current.Items["archive.id"];
            if (id== null || id == String.Empty)
            {
                return this.TplMessage("Error: �˱�ǩֻ�����ĵ�ҳ���е���!");
            }
            return base.Archive(this.site.SiteId, id, format);
        }


        [TemplateTag]
        [XmlObjectProperty("��ȡ��һƪ�ĵ�", @"
        	<b>������</b><br />
        	==========================<br />
			1:HTML��ʽ
		")]
        public string Prev_Archive(string format)
        {
            object id = HttpContext.Current.Items["archive.id"];
            if (id == null)
            {
                return this.TplMessage("Error: �˱�ǩֻ�����ĵ�ҳ���е���!");
            }
            return PrevArchive(id.ToString(), format);
        }

        [TemplateTag]
        [XmlObjectProperty("��ȡ��һƪ�ĵ�", @"
        	<b>������</b><br />
        	==========================<br />
			1:HTML��ʽ
		")]
        public string Next_Archive(string format)
        {
            object id = HttpContext.Current.Items["archive.id"];
            if (id == null)
            {
                return this.TplMessage("Error: �˱�ǩֻ�����ĵ�ҳ���е���!");
            }
            return NextArchive(id.ToString(), format);
        }


        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ȡ�ĵ�(Ĭ�ϸ�ʽ)", @"")]
        public string Archive()
        {
            return this.Archive(base.TplSetting.CFG_ArchiveFormat);
        }

        [TemplateTag]
        [XmlObjectProperty("��ȡ��һƪ�ĵ�(Ĭ�ϸ�ʽ)", @"")]
        public string Prev_Archive()
        {
            return Prev_Archive(base.TplSetting.CFG_PrevArchiveFormat);
        }

        [TemplateTag]
        [XmlObjectProperty("��ȡ��һƪ�ĵ�(Ĭ�ϸ�ʽ)", @"")]
        public string Next_Archive()
        {
            return Next_Archive(base.TplSetting.CFG_NextArchiveFormat);
        }


        #endregion

        #region �ĵ��б�

        //====================== ��ͨ�б� ==========================//

        /// <summary>
        /// �ĵ��б�
        /// </summary>
        [XmlObjectProperty("��ȡ��Ŀ�µ��ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag<br />
        	2����Ŀ<br />
			3��HTML��ʽ<br />
			4���Ƿ��������Ŀ,��ѡֵ[0|1]
		")]
        private string Archives(string param, string num, string format, bool container)
        {
            int _num;
            IEnumerable<ArchiveDto> archives = null;
            CategoryDto category = default(CategoryDto);
            int.TryParse(num, out _num);

            //��Ŀ
            category = ServiceCall.Instance.SiteService.GetCategory(this.siteId, param);

            if (!(category.Id > 0))
            {
                return String.Format("ERROR:ģ�����Ŀ������!����:{0}", param);
            }
            archives = container ? ServiceCall.Instance.ArchiveService.GetArchivesContainChildCategories(this.siteId, category.Lft, category.Rgt, _num) :
                ServiceCall.Instance.ArchiveService.GetArchivesByCategoryTag(this.siteId, category.Tag, _num);

            return this.ArchiveList(archives, format);
        }

        /// <summary>
        /// �ĵ��б�(��������)
        /// </summary>
        /// <param name="categoryTag"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ��Ŀ����������Ŀ���µ��ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag<br />
        	2����Ŀ<br />
			3��HTML��ʽ
		")]
        public string Archives(string param, string num, string format)
        {
            if (Regex.IsMatch(param, "^\\d+$"))
            {
                int _num;
                IEnumerable<ArchiveDto> dt = null;
                Module module = null;
                int.TryParse(num, out _num);

                module = CmsLogic.Module.GetModule(int.Parse(param));
                if (module != null)
                {
                    dt = ServiceCall.Instance.ArchiveService.GetArchivesByModuleId(this.siteId, module.ID, _num);
                    return ArchiveList(dt, format);
                }
            }

            return this.Archives(param, num, format, true);
        }


        /// <summary>
        /// �ĵ��б�(��������)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ��Ŀ����������Ŀ���µ��ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ<br />
			2��HTML��ʽ
		")]
        public string Archives(string num, string format)
        {
            string tag = HttpContext.Current.Items["category.tag"] as string;
            if (String.IsNullOrEmpty(tag))
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Archives(tag, num, format, true);
        }



        /// <summary>
        /// �ĵ��б�(��������)
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ȡ��Ŀ����������Ŀ���µ��ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ
		")]
        public string Archives(string num)
        {
            return Archives(num, base.TplSetting.CFG_ArchiveLinkFormat);
        }

        /// <summary>
        /// �ĵ��б�(����������)
        /// </summary>
        /// <param name="categoryTag"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ��Ŀ����������Ŀ���µ��ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag<br />
        	2����Ŀ<br />
			3��HTML��ʽ
		")]
        public string Self_Archives(string categoryTag, string num, string format)
        {
            return this.Archives(categoryTag, num, format, false);
        }

        /// <summary>
        /// �ĵ��б�(����������)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ��Ŀ����������Ŀ���µ��ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ<br />
			2��HTML��ʽ
		")]
        public string Self_Archives(string num, string format)
        {
            string tag = HttpContext.Current.Items["category.tag"] as string;
            if (String.IsNullOrEmpty(tag))
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Self_Archives(tag, num, format);
        }

        /// <summary>
        /// �ĵ��б�(����������)
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ȡ��Ŀ����������Ŀ���µ��ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ
		")]
        public string Self_Archives(string num)
        {
            return Self_Archives(num, base.TplSetting.CFG_ArchiveLinkFormat);
        }



        //====================== �����ĵ��б� ==========================//

        /// <summary>
        /// �����ĵ�
        /// </summary>
        /// <param name="tag">��Ŀ��ǩ</param>
        /// <param name="container">�Ƿ��������</param>
        /// <param name="param"></param>
        /// <param name="num">����</param>
        /// <param name="format">��ʽ
        /// </param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ���Ϊ���Ƽ������ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ���<br />
        	2����Ŀ<br />
			3��HTML��ʽ<br />
			4���Ƿ��������Ŀ����ѡֵΪ[0,1],1���������
		")]
        public string Special_Archives(string param, string num, string format, bool container)
        {
            int _num;
            IEnumerable<ArchiveDto> dt = null;
            CategoryDto category = default(CategoryDto);
            Module module = null;
            int.TryParse(num, out _num);

            //�������Ϊģ����
            if (Regex.IsMatch(param, "^\\d+$"))
            {
                module = CmsLogic.Module.GetModule(int.Parse(param));
                if (module != null)
                {
                    dt = ServiceCall.Instance.ArchiveService.GetSpecialArchivesByModuleId(this.siteId, module.ID, _num);
                }
            }

            if (module == null)
            {
                //��ȡ��Ŀ
                category = ServiceCall.Instance.SiteService.GetCategory(this.siteId, param);

                if (!(category.Id > 0))
                {
                    return String.Format("ERROR:ģ�����Ŀ������!����:{0}", param);
                }

                dt = container ?
                    ServiceCall.Instance.ArchiveService.GetSpecialArchives(this.siteId, category.Lft, category.Rgt, _num) :
                    ServiceCall.Instance.ArchiveService.GetSpecialArchives(this.siteId, category.Tag, _num);
            }


            return this.ArchiveList(dt, format);
        }


        /// <summary>
        /// �����ĵ�(��������Ŀ)
        /// </summary>
        /// <param name="param"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ���Ϊ���Ƽ������ĵ��б�,Ĭ�ϰ�������Ŀ", @"
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ���<br />
        	2����Ŀ<br />
			3��HTML��ʽ
		")]
        public string Special_Archives(string param, string num, string format)
        {
            return this.Special_Archives(param, num, format, true);
        }

        /// <summary>
        /// �����ĵ�(��������Ŀ)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ���Ϊ���Ƽ������ĵ��б�,Ĭ�ϰ�������Ŀ", @"
        	<p class=""red"">��������Ŀҳ�У�Ĭ���ļ���category.html) ʹ�ô˱�ǩ</p>
        	
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ<br />
			2��HTML��ʽ
		")]
        public string Special_Archives(string num, string format)
        {
            string id = HttpContext.Current.Items["category.tag"] as string;
            if (String.IsNullOrEmpty(id))
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Special_Archives(id, num, format, true);
        }

        /// <summary>
        /// �����ĵ�(��������Ŀ)
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ȡ���Ϊ���Ƽ������ĵ��б�,Ĭ�ϰ�������Ŀ,Html��ʽʹ�ú�̨���õĸ�ʽ��", @"
        	<p class=""red"">��������Ŀҳ�У�Ĭ���ļ���category.html) ʹ�ô˱�ǩ</p>
        	<b>������</b><br />
        	==========================<br />
        	1����Ŀ
		")]
        public string Special_Archives(string num)
        {
            return Special_Archives(num, base.TplSetting.CFG_ArchiveLinkFormat);
        }

        /// <summary>
        /// �����ĵ�(��������Ŀ)
        /// </summary>
        /// <param name="param"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ���Ϊ���Ƽ������ĵ��б�,Ĭ�ϰ�������Ŀ,Html��ʽʹ�ú�̨���õĸ�ʽ��", @"
        	<p class=""red"">��������Ŀҳ�У�Ĭ���ļ���category.html) ʹ�ô˱�ǩ</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag<br />
        	2����ʾ����<br />
        	3��HTML��ʽ
		")]
        public string Self_Special_Archives(string param, string num, string format)
        {
            return this.Special_Archives(param, num, format, false);
        }


        /// <summary>
        /// �����ĵ�(��������Ŀ)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ���Ϊ���Ƽ������ĵ��б�,Ĭ�ϰ�������Ŀ,Html��ʽʹ�ú�̨���õĸ�ʽ��", @"
        	<p class=""red"">��������Ŀҳ�У�Ĭ���ļ���category.html) ʹ�ô˱�ǩ</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����<br />
        	2��HTML��ʽ
		")]
        public string Self_Special_Archives(string num, string format)
        {
            string id = HttpContext.Current.Items["category.tag"] as string;
            if (String.IsNullOrEmpty(id))
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Special_Archives(id, num, format, false);
        }

        /// <summary>
        /// �����ĵ�(��������Ŀ)
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ���Ϊ���Ƽ������ĵ��б�,Ĭ�ϰ�������Ŀ,Html��ʽʹ�ú�̨���õĸ�ʽ��", @"
        	<p class=""red"">��������Ŀҳ�У�Ĭ���ļ���category.html) ʹ�ô˱�ǩ</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����
		")]
        public string Self_Special_Archives(string num)
        {
            return Self_Special_Archives(num, base.TplSetting.CFG_ArchiveLinkFormat);
        }


        //====================== ��������б� ==========================//

        /// <summary>
        /// ����������ĵ��б�
        /// </summary>
        /// <param name="container"></param>
        /// <param name="categoryTag"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�����ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag<br />
        	2����ʾ����<br />
        	3��HTML��ʽ<br />
        	4���Ƿ��������Ŀ
		")]
        public string Hot_Archives(string categoryTag, string num, string format, bool container)
        {
            int _num;
            IEnumerable<ArchiveDto> dt = null;
            CategoryDto category = default(CategoryDto);
            int.TryParse(num, out _num);

            category = ServiceCall.Instance.SiteService.GetCategory(this.siteId, categoryTag);
            if (!(category.Id > 0))
            {
                return String.Format("ERROR:ģ�����Ŀ������!����:{0}", categoryTag);
            }

            dt = container ?
                ServiceCall.Instance.ArchiveService.GetArchivesByViewCount(this.siteId, category.Lft, category.Rgt, _num) :
                ServiceCall.Instance.ArchiveService.GetArchivesByViewCount(this.siteId, category.Tag, _num);


            return this.ArchiveList(dt, format);
        }

        /// <summary>
        /// ����������ĵ��б�
        /// </summary>
        /// <param name="param"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�����ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag<br />
        	2����ʾ����<br />
        	3��HTML��ʽ
		")]
        public string Hot_Archives(string param, string num, string format)
        {
            //�������Ϊģ����
            if (Regex.IsMatch(param, "^\\d+$"))
            {
                int _num = 0;
                IEnumerable<ArchiveDto> dt = null;
                Module module = null;
                int.TryParse(num, out _num);

                module = CmsLogic.Module.GetModule(int.Parse(param));
                if (module != null)
                {
                    dt = ServiceCall.Instance.ArchiveService.GetArchivesByViewCountByModuleId(this.siteId, module.ID, _num);
                    return this.ArchiveList(dt, format);
                }
            }

            return Hot_Archives(param, num, format, true);
        }

        /// <summary>
        /// ����������ĵ��б�
        /// </summary>
        /// <param name="container"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�����ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����<br />
        	2��HTML��ʽ
		")]
        public string Hot_Archives(string num, string format)
        {
            string tag = HttpContext.Current.Items["category.tag"] as string;
            if (String.IsNullOrEmpty(tag))
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Hot_Archives(tag, num, format, true);
        }

        /// <summary>
        /// ����������ĵ��б�
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�����ĵ��б�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����
		")]
        public string Hot_Archives(string num)
        {
            return Hot_Archives(num, base.TplSetting.CFG_ArchiveLinkFormat);
        }


        /// <summary>
        /// ����������ĵ��б�
        /// </summary>
        /// <param name="param"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�����ĵ��б�(��������Ŀ)", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag<br />
        	2����ʾ����<br />
        	3��HTML��ʽ
		")]
        public string Self_Hot_Archives(string param, string num, string format)
        {
            return Hot_Archives(param, num, format, false);
        }

        /// <summary>
        /// ����������ĵ��б�
        /// </summary>
        /// <param name="container"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�����ĵ��б�(��������Ŀ)", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����<br />
        	2��HTML��ʽ
		")]
        public string Self_Hot_Archives(string num, string format)
        {
            string tag = HttpContext.Current.Items["category.tag"] as string;
            if (String.IsNullOrEmpty(tag))
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Hot_Archives(tag, num, format, false);
        }

        /// <summary>
        /// ����������ĵ��б�
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ȡ�����ĵ��б�(��������Ŀ)", @"
        	<b>������</b><br />
        	==========================<br />
        	2����ʾ����
		")]
        public string Self_Hot_Archives(string num)
        {
            return Self_Hot_Archives(num, base.TplSetting.CFG_ArchiveLinkFormat);
        }

        /// <summary>
        /// ����ģ���ȡ�ĵ�
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
       // [TemplateTag]
        [Obsolete]
        protected string ArchivesByCount2(string num, string format)
        {
            return "";
            object moduleID = AtNet.Cms.Cms.Context.Items["module.id"];
            if (moduleID == null)
            {
                return this.TplMessage("�˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            // return HotArchives(moduleID.ToString(),"true",num, format);
        }


        //
        //TODO:�����ĵ��������
        //

        #endregion

        #region ��Ŀ

        /// <summary>
        /// ��Ŀ�����б�
        /// </summary>
        /// <param name="param"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        //[TemplateTag]
        protected string CategoryList_Old(string param, string format)
        {
            /*
            //
            // @param : ���Ϊint,�򷵻�ģ���µ���Ŀ��
            //                 ���Ϊ�ַ���tag���򷵻ظ������µ���Ŀ
            //

            #region ȡ����Ŀ
            IEnumerable<Category> categories1;
            if (param == String.Empty)
            {
                categories1 = CmsLogic.Category.GetCategories();
            }
            else
            {
                if (Regex.IsMatch(param, "^\\d+$"))
                {
                    int moduleID = int.Parse(param);
                    categories1 = CmsLogic.Category.GetCategories(a => a.ModuleID == moduleID);
                }
                else
                {
                    Category c = CmsLogic.Category.Get(a =>a.SiteId==this.site.SiteId && String.Compare(a.Tag, param, true) == 0);
                    if (c != null)
                    {
                        throw new NotImplementedException();
                        //categories1 = CmsLogic.Category.getc(c.Lft, c.Rgt);
                    }
                    else
                    {
                        categories1 = null;
                    }
                }
            }
            #endregion

            if (categories1 == null) return String.Empty;
            else
            {
                IList<Category> categories = new List<Category>(categories1);
                StringBuilder sb = new StringBuilder(400);
                int i = 0;

                foreach (Category c in categories)
                {
                    sb.Append(tplengine.FieldTemplate(format, field =>
                    {
                        switch (field)
                        {
                            default: return String.Empty;

                            case "domain": return Settings.SYS_DOMAIN;

                            case "name": return c.Name;
                            case "url": return this.GetCategoryUrl(c, 1);
                            case "tag": return c.Tag;
                            case "id": return c.ID.ToString();

                            //case "pid":  return c.PID.ToString();

                            case "description": return c.Description;
                            case "keywords": return c.Keywords;
                            case "class":
                                if (i == categories.Count - 1) return " class=\"last\"";
                                else if (i == 0) return " class=\"first\"";
                                return string.Empty;
                        }
                    }));
                    ++i;
                }
                return sb.ToString();
            }
             * */
            return "";
        }

        [TemplateTag]
        [XmlObjectProperty("��ʾ��Ŀ�б�", @"
        	����1����ĿTag<br />
        	����2����ʾ��ʽ
		")]
        public string Categories(string categoryTag, string format)
        {
            return base.CategoriesList(categoryTag, format);
        }

        [TemplateTag]
        [XmlObjectProperty("��ʾ��Ŀ�б���Ŀҳ���ĵ�ҳ�У�", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ��ʽ
		")]
        public string Categories(string format)
        {
            string id = HttpContext.Current.Items["category.tag"] as string;
            if (String.IsNullOrEmpty(id))
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Categories(id, format);
        }

        [TemplateTag]
        [XmlObjectProperty("��ʾ��Ŀ�б���Ŀҳ���ĵ�ҳ�У�", @"")]
        public string Categories()
        {
            return Categories(base.TplSetting.CFG_CategoryLinkFormat);
        }

        //        [TemplateTag]
        //        [XmlObjectProperty("��ʾ��Ŀ(��������Ŀ)�б�", @"
        //        	<b>������</b><br />
        //        	==========================<br />
        //        	1����ĿTag<br />
        //        	2����ʾ��ʽ
        //		")]
        //        public string Categories2(string categoryTag, string format)
        //        {
        //            return Categories(categoryTag, format, !true);
        //        }

        //        [TemplateTag]
        //        [XmlObjectProperty("��ʾ��Ŀ(��������Ŀ)�б���Ŀҳ���ĵ�ҳ�У�", @"
        //        	<b>������</b><br />
        //        	==========================<br />
        //        	1����ʾ��ʽ
        //		")]
        //        public string Categories2(string format)
        //        {
        //            string id = HttpContext.Current.Items["category.tag"] as string;
        //            if (String.IsNullOrEmpty(id))
        //            {
        //                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
        //            }
        //            return Categories(id, format, !true);
        //        }

        //[TemplateTag]
        //[XmlObjectProperty("��ʾ��Ŀ(��������Ŀ)�б���Ŀҳ���ĵ�ҳ�У�",@"")]
        //public string Categories2()
        //{
        //    return Categories2(base.TplSetting.CFG_CategoryLinkFormat);
        //}

        #endregion


        [TemplateTag]
        [XmlObjectProperty("��ʾ��Ŀ��ҳ�ĵ����", @"
        	<p class=""red"">ֻ������Ŀҳ���ĵ�ҳ��ʹ�ã�</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����<br />
        	2����ʾ��ʽ
		")]
        public string Pager_Archives(string pageSize, string format)
        {
            string tag = HttpContext.Current.Items["category.tag"] as string;
            object pageindex = HttpContext.Current.Items["page.index"];


            if (String.IsNullOrEmpty(tag) || pageindex == null)
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return Pager_Archives(tag, pageindex.ToString(), pageSize, format);
        }




        [TemplateTag]
        [XmlObjectProperty("��ʾ�ĵ��������", @"
        	<p class=""red"">ֻ��������ҳ��ʹ�ã�</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����<br />
        	2����ʾ��ʽ
		")]
        public string Search_Archives(string pageSize, string format)
        {
            string key = HttpContext.Current.Items["search.key"] as string;
            string param = HttpContext.Current.Items["search.param"] as string;
            object pageindex = HttpContext.Current.Items["page.index"];


            if (String.IsNullOrEmpty(key) || pageindex == null)
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return this.Search_Archives(param, key, pageindex.ToString(), pageSize, format);
        }

        [TemplateTag]
        [XmlObjectProperty("��ʾ�ĵ��������", @"
        	<b>������</b><br />
        	==========================<br />
        	1���ؼ���<br />
        	2����ʾ����<br />
        	3����ʾ��ʽ
		")]
        public string Search_Archives(string keyword, string pageSize, string format)
        {
            string pageindex = HttpContext.Current.Items["page.index"] as string;
            return this.Search_Archives(null, keyword, pageindex, pageSize, format);
        }

        /// <summary>
        /// �Զ����ҳ����
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="format"></param>
        /// <param name="pagerLinkPath">��ҳ��ַ·��</param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ʾ�ĵ��������", @"
        	<b>������</b><br />
        	==========================<br />
        	1���ؼ���<br />
        	2����ʾ����<br />
        	3����ʾ��ʽ<br />
        	4��ҳ��·��(�������Զ�������ҳ��URL)
		")]
        public string Search_Archives(string keyword, string pageSize, string format, string pagerLinkPath)
        {
            int pageIndex,
                recordCount,
                pageCount;
            string c = Request("c");
            int.TryParse(Request("p"), out pageIndex);
            if (pageIndex < 1) pageIndex = 1;

            string html = this.SearchArchives(
                this.site.SiteId,
                c,
                keyword,
                pageIndex.ToString(),
                pageSize,
                format,
                out pageCount,
                out recordCount);

            //��Ӳ�ѯ����
            pagerLinkPath += pagerLinkPath.IndexOf("?") == -1 ? "?" : "&";

            //�滻����
            Regex reg = new Regex("([^\\?]+\\?*)(.+)", RegexOptions.IgnoreCase);

            string link1 = String.Format(TemplateUrlRule.Urls[TemplateUrlRule.RuleIndex, (int)UrlRulePageKeys.Search], HttpUtility.UrlEncode(keyword), c ?? ""),
                   link2 = String.Format(TemplateUrlRule.Urls[TemplateUrlRule.RuleIndex, (int)UrlRulePageKeys.SearchPager], HttpUtility.UrlEncode(keyword), c ?? "", "{0}");

            this.SetPager(
                        pageIndex,
                        pageCount,
                        recordCount,
                        reg.Replace(link1, String.Format("{0}$2", pagerLinkPath)),
                        reg.Replace(link2, String.Format("{0}$2", pagerLinkPath))
                    );

            return html;
        }

        /// <summary>
        /// �����ĵ��б�
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryTagOrModuleId"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("��ʾָ����Ŀ�µ��ĵ��������", @"
        	<b>������</b><br />
        	==========================<br />
        	1����ĿTag
        	2���ؼ���<br />
        	3����ǰҳ��<br />
        	4����ʾ����<br />
        	5����ʾ��ʽ
		")]
        public string Search_Archives(string categoryTagOrModuleId, string keyword, string pageIndex, string pageSize, string format)
        {
            int pageCount, recordCount;
            return this.SearchArchives(this.site.SiteId, categoryTagOrModuleId, keyword, pageIndex, pageSize, format, out pageCount, out recordCount);
        }


        /// <summary>
        /// ����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="number"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("�Զ�����ʾ����", @"
        	<b>������</b><br />
        	==========================<br />
        	1���������ͣ���ѡ[1|2|3],1:����,2:��������,3:�Զ�������]<br />
        	2����ʾ����<br />
        	3����ʾ��ʽ
		")]
        public string Links(string type, string number,string format)
        {
            return base.Link(type, format, int.Parse(number), "-1");
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [XmlObjectProperty("ʹ���Զ����ʽ��ʾ<b>ȫ��</b>����", @"
        	<b>������</b><br />
        	==========================<br />
        	1���������ͣ���ѡ[1|2|3],1:����,2:��������,3:�Զ�������]<br />
        	2����ʾ��ʽ
		")]
        public string Link(string type,string format)
        {
            return base.Link(type, format, 1000, "-1");
        }

        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ʾTags", @"
        	<p class=""red"">�������ĵ�ҳ��ʹ�ô˱�ǩ</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ǩ���ݣ������ǩ��"",""�������磺����,�ֻ�,���
		")]
        public string Tags(string tags)
        {
            return base.Tags(tags, base.TplSetting.CFG_ArchiveTagsFormat);
        }

        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ʾTags", @"
        	<p class=""red"">ֻ�����ĵ�ҳ��ʹ��,��ʽ�ڡ�ģ�����á��н�������</p>
		")]
        public string Tags()
        {
            if (!(archive.Id > 0)) return this.TplMessage("����ʹ�ñ�ǩ$require('id')��ȡ�ĵ����ٵ�������");
            return Tags(archive.Tags, base.TplSetting.CFG_ArchiveTagsFormat);
        }

        [TemplateTag]
        [XmlObjectProperty("��ʾ��ǩ�ĵ����", @"
        	<p class=""red"">ֻ���ڱ�ǩ����ҳ(tags.html)��ʹ�ã�</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ����<br />
        	2����ʾ��ʽ
		")]
        public string Tag_Archives(string pageSize, string format)
        {
            string key = HttpContext.Current.Items["tag.key"] as string;
            object pageindex = HttpContext.Current.Items["page.index"];

            if (String.IsNullOrEmpty(key) || pageindex == null)
            {
                return this.TplMessage("Error: �˱�ǩ�������ڵ�ǰҳ���е���!");
            }
            return base.TagArchives(key, pageindex.ToString(), pageSize, format);
        }


        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ʾ���ۿ�", @"
        	<p class=""red"">ֻ�����ĵ�ҳ��ʹ�ã�</p>
		")]
        public string Comment_Editor()
        {
            return this.Comment_Editor(base.TplSetting.CFG_AllowAmousComment ? "true" : "false",
                base.TplSetting.CFG_CommentEditorHtml);
        }



        [TemplateTag]
        [XmlObjectProperty("��ʾվ���ͼ", @"
        	<p class=""red"">ֻ������Ŀҳ���ĵ�ҳ��ʹ�ã�</p>
		")]
        public string Sitemap()
        {
            string tag = AtNet.Cms.Cms.Context.Items["category.tag"] as string;
            if (string.IsNullOrEmpty(tag))
            {
                return this.TplMessage("�޷��ڵ�ǰҳ����ô˱�ǩ!\r\n�������:ʹ�ñ�ǩ$sitemap('��Ŀ��ǩ')������Cms.Context.Items[\"category.tag\"]");
            }
            return Sitemap(tag);
        }


        /// <summary>
        /// ������ĵ���
        /// </summary>
        /// <returns></returns>
        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ʾ��վ����", @"
        	<p class=""red"">��ʾ��ʽ�ڡ�ģ�����á����޸�.</p>
		")]
        public string Navigator()
        {
           string cache =  SiteLinkCache.GetNavigatorBySiteId(siteId);
           if (cache == null)
           {
               cache = base.Navigator(base.TplSetting.CFG_NavigatorLinkFormat, base.TplSetting.CFG_NavigatorChildFormat, "-1");
               SiteLinkCache.SetNavigatorForSite(siteId,cache);
           }
           return cache;
        }

        [TemplateTag]
        [ContainSetting]
        [XmlObjectProperty("��ʾ��������", @"
        	<p class=""red"">��ʾ�������Լ���ʽ���ڡ�ģ�����á����޸�.</p>
		")]
        public string Friend_Link()
        {
            return this.Friend_Link(base.TplSetting.CFG_FriendShowNum.ToString(), base.TplSetting.CFG_FriendLinkFormat);
        }

        [TemplateTag]
        [XmlObjectProperty("��ʾ��������", @"
        	<p class=""red"">�������ĵ�ҳ��ʹ�ô˱�ǩ</p>
        	<b>������</b><br />
        	==========================<br />
        	1����ʾ��Ŀ<br />
			2��HTML��ʽ,��:<a href=""{url}"" {target}>{text}</a>
		")]
        public string Friend_Link(string num, string format)
        {
            string cache = SiteLinkCache.GetFLinkBySiteId(siteId);
            if (cache == null)
            {
                cache = this.Link("2", format, int.Parse(num), "-1");
                SiteLinkCache.SetFLinkForSite(siteId, cache);
            }
            return cache;
        }



        #region ����Region��ǩ

        [TemplateTag]
        protected string Province(string path)
        {
            bool hasQuery = path.IndexOf('?') != -1;
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"provinces\">");
            foreach (Province p in Region.Provinces)
            {
                sb.Append("<li><a href=\"").Append(path)
                    .Append(hasQuery ? "&" : "?").Append("prv=").Append(p.ID.ToString()).Append("\">")
                    .Append(p.Text).Append("</a></li>");
            }
            sb.Append("</ul>");

            return sb.ToString();
        }


        [TemplateTag]
        protected string City(string path)
        {
            bool hasQuery = path.IndexOf('?') != -1;
            int provinceID = int.Parse(this.Request("prv") ?? "1");
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"cities\">");
            foreach (City p in Region.GetCities(provinceID))
            {
                sb.Append("<li><a href=\"").Append(path)
                    .Append(hasQuery ? "&" : "?").Append("prv=").Append(p.Pid.ToString()).Append("&cty=").Append(p.ID.ToString()).Append("\">")
                    .Append(p.Text).Append("</a></li>");
            }
            sb.Append("</ul>");

            return sb.ToString();
        }

        #endregion

        #region ���ݱ�ǩ
        protected string PagerArchiveList(string categoryTag, string pageIndex, string pageSize, string format)
        {
            return base.Pager_Archives(categoryTag, pageIndex, pageSize, format);
        }

        protected string PagerArchiveList(string pageSize, string format)
        {
            return this.Pager_Archives(pageSize, format);
        }


        [TemplateTag]
        protected string SearchList(string categoryTagOrModuleID, string keyword, string pageIndex, string pageSize, string format)
        {
            int pageCount, recordCount;
            return this.SearchArchives(this.site.SiteId, categoryTagOrModuleID, keyword, pageIndex, pageSize, format, out pageCount, out recordCount);
        }

        [TemplateTag]
        protected string SearchList(string keyword, string pageSize, string format)
        {
            string pageindex = HttpContext.Current.Items["page.index"] as string;
            return Search_Archives(null, keyword, pageindex, pageSize, format);
        }

        [TemplateTag]
        protected string SearchList(string pageSize, string format)
        {
            return Search_Archives(pageSize, format);
        }

        /// <summary>
        /// �Զ����ҳ����
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="format"></param>
        /// <param name="pagerLinkPath">��ҳ��ַ·��</param>
        /// <returns></returns>
        [TemplateTag]
        protected string SearchList(string keyword, string pageSize, string format, string pagerLinkPath)
        {
            return this.Search_Archives(keyword, pageSize, format, pagerLinkPath);
        }



        /// <summary>
        /// ��ǩ�ĵ��б�
        /// </summary>
        /// <param name="categoryTag"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="flags">t,o,p</param>
        /// <param name="url">�Ƿ�webform</param>
        /// <returns></returns>
        [TemplateTag]
        protected string TagPagerArchiveList(string tag, string pageIndex, string pageSize, string format)
        {
            return base.TagArchives(tag, pageIndex, pageSize, format);
        }

        [TemplateTag]
        protected string TagPagerArchiveList(string pageSize, string format)
        {
            return this.Tag_Archives(pageSize, format);
        }


        /// <summary>
        /// �ĵ��б�
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="container"></param>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        protected string Archives(string param, string container, string num, string format)
        {
            return this.IsTrue(container) ? this.Archives(param, num, format) : this.Self_Archives(param, num, format);
        }

        /// <summary>
        /// �����ĵ�
        /// </summary>
        /// <param name="tag">��Ŀ��ǩ</param>
        /// <param name="container">�Ƿ��������</param>
        /// <param name="num">����</param>
        /// <param name="format">��ʽ
        /// </param>
        /// <returns></returns>
        [TemplateTag]
        protected string SpecialArchives(string param, string container, string num, string format)
        {
            return this.IsTrue(container) ? this.Special_Archives(param, num, format) : this.Self_Special_Archives(param, num, format);
        }

        #endregion

        #region ��д

        /********************************  ��д  **************************/
        [TemplateTag]
        protected string SAList(string categoryTagOrModuleID, string num, string format)
        {
            return Special_Archives(categoryTagOrModuleID, num, format);
        }

        [TemplateTag]
        protected string SList(string categoryTagOrModuleId, string keyword, string pageIndex, string pageSize, string format)
        {
            return SearchList(categoryTagOrModuleId, keyword, pageIndex, pageSize, format);
        }

        [TemplateTag]
        protected string TAList(string keyword, string pageIndex, string pageSize, string format)
        {
            return TagPagerArchiveList(keyword, pageIndex, pageSize, format);
        }

        [TemplateTag]
        protected string PAList(string categoryTag, string pageIndex, string pageSize, string format)
        {
            return Pager_Archives(categoryTag, pageIndex, pageSize, format);
        }

        [TemplateTag]
        protected string CList(string param, string format)
        {
            return Categories(param, format);
        }

        [TemplateTag]
        protected string AList(string categoryTagOrModuleID, string num, string format)
        {
            return Archives(categoryTagOrModuleID, num, format, false);
        }



        [TemplateTag]
        protected string NA(string id, string format)
        {
            return NextArchive(id, format);
        }

        [TemplateTag]
        protected string PA(string id, string format)
        {
            return PrevArchive(id, format);
        }



        /******************************* ��д���� **************************/

        #endregion

        #region ����

        [Obsolete]
        [TemplateTag]
        protected string ArchiveList(string param, string num, string format)
        {
            return this.Archives(param, num, format, false);
        }

        [Obsolete]
        [TemplateTag]
        protected string ArchiveList(string num, string format)
        {
            return this.Archives(num, format);
        }

        /// <summary>
        /// ����ģ��ID����
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [Obsolete]
        protected string ArchiveList2(string num, string format)
        {
            return Archives(num, format);
        }

        /// <summary>
        /// �����ĵ�����
        /// </summary>
        /// <param name="categoryTagOrModuleId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [TemplateTag]
        [Obsolete]
        protected string SpecialArchiveList(string categoryTagOrModuleId, string num, string format)
        {
            return Special_Archives(categoryTagOrModuleId, num, format, false);
        }

        /// <summary>
        /// �����ĵ�����
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [Obsolete]
        protected string SpecialArchiveList(string num, string format)
        {
            return Special_Archives(num, format);
        }

        /// <summary>
        /// �����ĵ�����
        /// </summary>
        /// <param name="num"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [Obsolete]
        protected string SpecialArchiveList2(string num, string format)
        {
            return Self_Special_Archives(num, format);
        }


        [Obsolete]
        [TemplateTag]
        protected string A(string idOrAlias, string format)
        {
            return Archive(idOrAlias, format);
        }




        /// <summary>
        /// ģ����Ŀ��ǩ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [Obsolete]
        protected string MCategoryList(string id, String root, string format)
        {
            IList<ICategory> categories = new List<ICategory>();
            bool onlyRoot = this.IsTrue(root);

            if (String.IsNullOrEmpty(id))
            {
                return TplMessage("��ָ������:id��ֵ");
            }

            if (Regex.IsMatch(id, "^\\d+$"))
            {
                //��ģ�����
                int moduleID = int.Parse(id);
                if (CmsLogic.Module.GetModule(moduleID) != null)
                {
                    ServiceCall.Instance.SiteService.HandleCategoryTree(this.siteId, 1, (c, level) =>
                    {
                        if (!onlyRoot || (onlyRoot && level == 0))
                        {
                            if (c.ModuleId == moduleID)
                            {
                                categories.Add(c);
                            }
                        }
                    });
                }
            }

            if (categories.Count == 0) return String.Empty;

            StringBuilder sb = new StringBuilder(400);
            int i = 0;

            foreach (ICategory c in categories.OrderBy(a => a.OrderIndex | a.Lft))
            {
                sb.Append(TplEngine.FieldTemplate(format, field =>
                {
                    switch (field)
                    {
                        default: return String.Empty;

                        case "name": return c.Name;

                        //
                        //TODO:
                        //
                        //case "url": return this.GetCategoryUrl(c, 1);
                        case "tag": return c.Tag;
                        case "id": return c.Id.ToString();

                        //case "pid":  return c.PID.ToString();

                        case "description": return c.Description;
                        case "keywords": return c.Keywords;
                        case "class":
                            if (i == categories.Count - 1) return " class=\"last\"";
                            else if (i == 0) return " class=\"first\"";
                            return string.Empty;
                    }
                }));
                ++i;
            }
            return sb.ToString();

        }

        [TemplateTag]
        [Obsolete]
        protected string MCategoryList(string id, string format)
        {
            return this.MCategoryList(id, "true", format);
        }

        [TemplateTag]
        [Obsolete]
        protected string MCategoryList(string format)
        {
            object id = AtNet.Cms.Cms.Context.Items["module.id"];
            if (id == null)
            {
                return this.TplMessage("�˱�ǩ�������ڵ�ǰҳ���е���!��ʹ��$MCategoryList(moduleid,isRoot,format)��ǩ����");
            }

            return this.MCategoryList(id.ToString(), "true", format);
        }


        [Obsolete]
        protected string _MCategoryTree(string moduleID)
        {
            //��ȡ����
            string cacheKey = String.Format("{0}_site{1}_mtree_{2}", CacheSign.Category.ToString(), this.siteId.ToString(), moduleID);

            BuiltCacheResultHandler<String> bh = () =>
            {
                //�޻���,�����ִ��
                StringBuilder sb = new StringBuilder(400);

                int _moduleID = int.Parse(moduleID);

                //��ģ�����
                if (CmsLogic.Module.GetModule(_moduleID) == null)
                {
                    return TplMessage("������ģ��!ID:" + moduleID);
                }
                sb.Append("<div class=\"category_tree mtree\">");

                CategoryDto dto = ServiceCall.Instance.SiteService.GetCategoryByLft(this.siteId, 1);

                this.CategoryTree_Iterator(dto, sb, a => { return a.ModuleId == _moduleID; }, true);

                sb.Append("</div>");

                return sb.ToString();
            };


            return AtNet.Cms.Cms.Cache.GetCachedResult(cacheKey, bh);
        }


        /// <summary>
        /// ��Ŀ�����б�
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [TemplateTag]
        [Obsolete]
        protected string CategoryList2(string format)
        {
            return "tag install";

            object id = AtNet.Cms.Cms.Context.Items["module.id"];
            if (id == null)
            {
                return this.TplMessage("�˱�ǩ�������ڵ�ǰҳ���е���!��ʹ��$categorylist('categorytag','format')��ǩ����");
            }
            return Categories(id.ToString(), format);
        }



        #endregion



    }
}

