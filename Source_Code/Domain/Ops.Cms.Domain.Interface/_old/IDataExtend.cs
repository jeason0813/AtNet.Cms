﻿using System;
using Spc.Models;
using Spc.Share;
using System.Collections.Generic;
using System.Data;


namespace Spc.Logic
{
    public interface IDataExtend
    {
        /// <summary>
        /// 获取扩展
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataExtend GetExtend(int id);

        /// <summary>
        /// 获取扩展
        /// </summary>
        /// <param name="special"></param>
        /// <returns></returns>
         IEnumerable<DataExtend> GetExtends(SpecialEnum special);

        /// <summary>
        /// 获取扩展属性
        /// </summary>
        /// <param name="extendID"></param>
        /// <returns></returns>
        IEnumerable<DataExtendAttr> GetExtendAttrs(int extendID);

        /// <summary>
        /// 获取相关联的字段值列表
        /// </summary>
        /// <param name="relationID"></param>
        /// <returns></returns>
        IList<DataExtendField> GetExtendFileds(int relationID);

        /// <summary>
        /// 获取包含扩展名称，扩展值的数据表
        /// </summary>
        /// <param name="relationID"></param>
        /// <returns></returns>
        DataTable GetExtendFiledsTable(int relationID);

        /// <summary>
        /// 根据模块获取扩展字段
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IEnumerable<DataExtend> GetExtendsByModule(Module module);

        /// <summary>
        /// 更新扩展字段列
        /// </summary>
        /// <param name="extendData"></param>
        void UpdateExtendFileds(int relationID, IDictionary<int, string> extendData);


        /// <summary>
        /// 获取字段字典
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        IDictionary<string, string> GetExtendFiledDictionary(int relationID);

        #region old

        bool AddExtendAttr(DataExtendAttr pro);
        string BuildUIString(Spc.PropertyUI ui, string key, params string[] data);
        bool DeleteExtendAttr(int propertyID);
        DataExtendAttr GetExtendAttr(int extendID, string key);
        string GetUIHtml(DataExtendAttr pro, string value);
        bool UpdateExtendAttr(DataExtendAttr pro);
        void RebuiltExtends();

        #endregion 
    }
}
