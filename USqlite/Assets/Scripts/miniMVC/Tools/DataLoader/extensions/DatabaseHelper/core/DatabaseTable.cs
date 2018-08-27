﻿
using System;
using System.Collections.Generic;

namespace miniMVC.USqlite
{
    public class SqliteTable
    {
        private readonly IDictionary<int, DatabaseRow> m_dbRows = null;

        public SqliteTable()
        {
            m_dbRows = new Dictionary<int, DatabaseRow>();
        }

        public void AddRow(int id,Type rowType,string rowName , string rowUpperName ,Func<int,object> getValue)
        {
            if (!m_dbRows.ContainsKey(id))
            {
                m_dbRows.Add(id,new DatabaseRow()
                {
                    rowType = rowType,
                    rowName = rowName,
                    rowUpperName = rowUpperName,
                    GetValue = getValue
                });
            }
        }

        public bool ContainRow( int rowId )
        {
            return m_dbRows.ContainsKey(rowId);
        }

        public string GetRow(int rowId,ref Type type,ref string upperName ,ref object value)
        {
            if (m_dbRows.ContainsKey(rowId))
            {
                var row = m_dbRows[rowId];
                type = row.rowType;
                upperName = row.rowUpperName;
                value = row.GetValue(rowId);
                return row.rowName;
            }
            return null;
        }

        public DatabaseRow GetRow( int rowId )
        {
            return m_dbRows[rowId];
        }
    }

    public class DatabaseRow // 每一列
    {
        public Type rowType;
        public string rowName;
        public string rowUpperName;
        public Func<int,object> GetValue = null;
    }

    /// <summary>
    /// 数据库一行数据
    /// </summary>
    public class SqliteRow : ICloneable
    {
        public IList<SqliteCell> cellData = new List<SqliteCell>();
        public SqliteCell primayKey
        {
            get
            {
                foreach(SqliteCell data in cellData)
                    if(data.isPrimaryKey)
                        return data;
                return null;
            }
        }

        public string[] columeNames
        {
            get
            {
                string[] names = new string[cellData.Count];
                for (int i = 0; i < names.Length; i++)
                    names[i] = cellData[i].name;
                return names;
            }
        }

        public string[] columeValues
        {
            get
            {
                string[] values = new string[cellData.Count];
                for(int i = 0; i < values.Length; i++)
                    values[i] = (null == cellData[i].value) ? "" : cellData[i].value.ToString();
                return values;
            }
        }

        public void AddCell(SqliteCell cell)
        {
            cellData.Add(cell);
        }

        public void Clear()
        {
            cellData.Clear();
        }

        public override string ToString()
        {
            return string.Format("<color=#FF0000>主键:[{0}]</color> , 共[{1}]列数据",primayKey,cellData.Count);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 数据一个单元格数据
    /// </summary>
    public class SqliteCell : ICloneable
    {
        public bool isPrimaryKey { get; set; }
        public Type type;
        public object value;
        public string strValue;
        public string name;
        public string upperName;// 大写名字，用于属性字段赋值时进行匹配

        public SqliteCell()
        {
            isPrimaryKey = false;
        }

        public SqliteCell(Type dataType,string dataName,object dataValue)
        {
            isPrimaryKey = false;
            this.name = dataName;
            this.value = dataValue;
            this.type = dataType;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}