//
//
//  Copyright 2011 (C) OPSoft INC.All rights reseved.
//
//  Project : OSite
//  File Name : ICategoryDAL.cs
//  Date : 2011/8/22
//  Author : 
//
//

namespace Spc.IDAL
{
    using System.Data;
    using Ops.Data;

    //��Ŀ���ݽӿ�
    public interface ICategoryDAL
    {
        void Insert(int siteID,int lft, int rgt, int moduleID, string name, string alias, string pagetitle, string keywords, string description, int orderIndex);
       

        /// <summary>
        /// ������Ŀ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lft"></param>
        /// <param name="rgt"></param>
        /// <param name="moduleID"></param>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="keywords"></param>
        /// <param name="description"></param>
        /// <param name="orderIndex"></param>
        /// <returns></returns>
        bool Update(int id, int lft, int rgt, int moduleID, string name, string alias, string pagetitle, string keywords, string description, int orderIndex);

        /// <summary>
        /// ɾ��ָ��lft,rgt�ļ����µ���Ŀ
        /// </summary>
        /// <param name="lft"></param>
        /// <param name="rgt"></param>
        /// <returns></returns>
        bool Delete(int lft,int rgt);

        /// <summary>
        /// ��ȡ������Ŀ
        /// </summary>
        /// <param name="func"></param>
        void GetAllCategories(DataReaderFunc func);

        
        /// <summary>
        /// ����ӽڵ�ʱ��������ֵ
        /// </summary>
        /// <param name="left"></param>
        void UpdateInsertLftRgt(int left);

        /// <summary>
        /// ɾ���ڵ�ʱ��������ֵ
        /// </summary>
        /// <param name="lft"></param>
        /// <param name="rgt"></param>
        void UpdateDeleteLftRgt(int lft, int rgt);


        /// <summary>
        /// ת��Ŀ����Ŀ��������ֵ
        /// </summary>
        /// <param name="toLeft"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        void UpdateMoveLftRgt(int toLeft, int left, int right);

        void UpdateMoveLftRgt2(int toRgt, int left, int right);

        /// <summary>
        /// ��ȡ������ֵ
        /// </summary>
        /// <returns></returns>
        int GetMaxRight();

    }
}
