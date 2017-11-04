﻿using AtNet.Cms.Domain.Interface.Models;
using AtNet.Cms.Infrastructure;

namespace AtNet.Cms.Domain.Interface._old
{
    public interface ITable
    {
        OperateResult AddColumn(Models.TableColumn column);
        OperateResult AddTable(Models.Table table, Models.TableColumn[] columns);
        OperateResult DeleteColumn(int tableID, int columnID);
        OperateResult DeleteRow(int tableId, int rowId);
        OperateResult DeleteTable(int tableID);
        Models.TableColumn GetColumn(int columnID);
        System.Collections.Generic.IList<TableColumn> GetColumns(int tableID);
        System.Data.DataTable GetPagedRecords(int tableID, string keyword, int pageSize, int currentPageIndex, out int recordCount, out int pageCount);
        Models.TableRow GetRecord(int rowId);
        int GetRowsCount(int tableID);
        Models.Table GetTable(int tableID);
        System.Collections.Generic.IList<Table> GetTables();
        int SubmitRow(int tableId, System.Collections.Specialized.NameValueCollection form);
        OperateResult UpdateColumn(Models.TableColumn tableColumn);
        OperateResult UpdateTable(Models.Table table, Models.TableColumn[] columns);
    }
}
