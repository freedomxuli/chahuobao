using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// DataTable操作类
    /// </summary>
    public class DataTableHelper
    {

        #region DataTable转List<T>
        /// <summary>
        /// DataTable转List实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> List<T>(DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }
        #endregion

        #region 整理dataTable数据，以便于在有层次感的数据容器中使用
        /// <summary>整理dataTable数据，以便于在有层次感的数据容器中使用
        /// </summary>
        /// <param name="dtable">DataTable数据源</param>
        /// <param name="pkFiled">主键ID列名</param>
        /// <param name="parentIdFiled">父级ID列名</param>
        /// <returns></returns>
        public static DataTable DataTableTidyUp(DataTable dtable, string pkFiled, string parentIdFiled)
        {
            return DataTableTidyUp(dtable, pkFiled, parentIdFiled, 0);
        }

        /// <summary>
        /// 整理dataTable数据，以便于在有层次感的数据容器中使用
        /// </summary>
        /// <param name="table">DataTable数据源</param>
        /// <param name="pkFiled">主键ID列名</param>
        /// <param name="parentIdFiled">父级ID列名</param>
        /// <param name="parentId">父ID值，用于查询分类列表时，只显示指定一级分类下面的全部分类</param>
        /// <returns></returns>
        public static DataTable DataTableTidyUp(DataTable table, string pkFiled, string parentIdFiled, int parentId)
        {
            if (table == null)
                return null;

            //判断当前内存表中是否存在指定的主键列
            if (!table.Columns.Contains(pkFiled) || !table.Columns.Contains(parentIdFiled))
            {
                //不存在指定的主键列
                return null;
            }
            //设定主键列
            table.PrimaryKey = new DataColumn[] { table.Columns[pkFiled] };

            //克隆内存表中的结构与约束
            DataTable tidyUpdata = table.Clone();

            //父ID列表，用于使用条件查询时，只将指定父ID节点（根节点）以及它下面的子节点显示出来，其他节点不显示
            string parentIDList = ",";

            //循环读取表中的记录
            foreach (DataRow item in table.Rows)
            {
                //获取父ID值
                int pid = int.Parse(item[parentIdFiled].ToString());
                //判断当前的父ID是否为0（即是否是根节点），为0则直接加入,否则寻找其父id的位置
                if (pid == 0)
                {
                    //如果指定了只显示指定根节点以及它的子节点，则判断当前父节点是否为指定的父节点，不是则终止本次循环
                    if (parentId > 0 && int.Parse(item[pkFiled].ToString()) != parentId)
                    {
                        continue;
                    }
                    else
                    {
                        //如果指定了只显示指定根节点以及它的子节点，则将当前节点ID加入列表
                        if (parentId > 0)
                        {
                            parentIDList += item[pkFiled].ToString() + ",";
                        }
                        //添加一行记录
                        tidyUpdata.ImportRow(item);
                        continue;
                    }
                }

                //如果指定了只显示指定根节点以及它的子节点，且当前父ID不存在父ID列表中，则终止本次循环
                if (parentId > 0 && parentIDList.IndexOf("," + pid + ",") < 0)
                {
                    continue;
                }
                //将当前ID加入列表中
                if (parentId > 0)
                {
                    parentIDList += item[pkFiled].ToString() + ",";
                }

                //寻找父id的位置
                DataRow pdrow = tidyUpdata.Rows.Find(pid);
                //获取父ID所在行索引号
                int index = tidyUpdata.Rows.IndexOf(pdrow);

                int _pid = 0;
                //查找下一个位置的父ID与当前行的父ID是否一样，是的话将插入行向下移动
                do
                {
                    //索引号增加
                    index++;
                    if (index < tidyUpdata.Rows.Count)
                    {
                        try
                        {
                            //获取下一行的父ID值
                            _pid = ConvertHelper.ToInt0(tidyUpdata.Rows[index][parentIdFiled]);
                        }
                        catch (Exception)
                        {
                            _pid = 0;
                        }
                    }
                    else
                    {
                        _pid = 0;
                    }
                }
                //如果下一行的父ID值与当前要插入的ID值一样，则循环继续
                while (pid != 0 && pid == _pid);

                //当前行创建新行
                DataRow CurrentRow = tidyUpdata.NewRow();
                CurrentRow.ItemArray = item.ItemArray;

                //插入新行
                tidyUpdata.Rows.InsertAt(CurrentRow, index);
            }


            return tidyUpdata;

        }

        /// <summary>整理dataTable数据，以便于在有层次感的数据容器中使用
        /// </summary>
        /// <param name="dtable">DataTable数据源</param>
        /// <param name="pkFiled">主键ID列名</param>
        /// <param name="parentIDFiled">父级ID列名</param>
        /// <returns></returns>
        public static DataSet DataSetTidyUp(DataTable dtable, string pkFiled, string parentIDFiled)
        {

            DataSet dset = new DataSet();
            DataTable dt = DataTableTidyUp(dtable, pkFiled, parentIDFiled);
            dset.Tables.Add(dt);
            return dset;

        }
        #endregion

        #region 根据DataTable,返回第一行,各列数据到string[]
        /// <summary>根据DataTable,返回第一行,各列数据到string[]</summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static string[] GetColumnsString(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0] { };
            }

            int ti = dt.Columns.Count;
            string[] arr = new string[ti];
            for (int i = 0; i < ti; i++)
            {
                arr[i] = dt.Rows[0][i].ToString();
            }
            dt.Dispose();
            return arr;
        }
        #endregion

        #region 根据DataTable,返回指定列数据的string[]
        /// <summary>根据DataTable,返回指定列数据的string[]</summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static string[] GetArrayString(DataTable dt, string colName)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new string[0] { };
            }

            int t = dt.Rows.Count;
            string[] arr = new string[t];
            for (int i = 0; i < t; i++)
            {
                arr[i] = dt.Rows[i][colName].ToString();
            }
            dt.Dispose();
            return arr;
        }
        #endregion

        #region 筛选函数，将数据表里面指定的值查找出来
        /// <summary>
        /// 在dataTable中查找到定条件的记录，并返回新的DataTable
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="colName">要找查的名称（条件名，为空时表示查询全部）</param>
        /// <param name="colValue">要查找的值</param>
        /// <param name="sortName">排序字段名</param>
        /// <param name="orderby">升序或降序（Asc/Desc）</param>
        /// <returns>返回筛选后的数据表</returns>
        public static DataTable GetFilterData(DataTable dt, string colName, string colValue, string sortName, string orderby)
        {
            var wheres = string.IsNullOrEmpty(colName) ? "" : colName + "=" + colValue;
            string sort = null;
            if (!string.IsNullOrEmpty(sortName))
            {
                sort = sortName + " " + orderby;
            }
            return GetFilterData(dt, wheres, sort);
        }

        /// <summary>
        /// 筛选函数，将数据表里面指定的值查找出来
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="wheres">条件，例：Id=100 and xx=20</param>
        /// <param name="sort">排序，例：Id Desc</param>
        /// <returns>返回筛选后的数据表</returns>
        public static DataTable GetFilterData(DataTable dt, string wheres, string sort)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            try
            {
                DataTable _dt = null;
                DataRow[] drs = null;
                //查询
                if (!string.IsNullOrEmpty(wheres))
                {
                    //内存表中查询数据
                    drs = dt.Select(wheres);
                    //CopyToDataTable 必须 引用 System.Data.DataSetExtensions
                    _dt = drs.Length > 0 ? drs.CopyToDataTable() : dt.Clone();
                }
                else
                {
                    _dt = dt;
                }
                //设置排序
                if (!string.IsNullOrEmpty(sort) && _dt != null)
                {
                    _dt.DefaultView.Sort = sort;
                    _dt = _dt.DefaultView.ToTable();
                }

                dt.Dispose();
                return _dt;
            }
            catch { }

            return null;
        }
        #endregion

        #region 从DataTable中读取指定列数据,返回int[]
        /// <summary>
        /// 从DataTable中读取指定列数据,返回int[]
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">列名</param>
        public static int[] GetArrayInt(DataTable dt, string colName)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new int[0] { };
            }

            int t = dt.Rows.Count;
            var arr = new int[t];
            for (int i = 0; i < t; i++)
            {
                arr[i] = ConvertHelper.ToInt0(dt.Rows[i][colName]);
            }
            dt.Dispose();
            return arr;
        }
        #endregion
    }
}
