using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.Common.Entity
{
    /// <summary>
    /// 适用于树
    /// </summary>
    public class KeyTitle
    {
        public string key { get; set; }
        public string title { get; set; }
        public int level { get; set; }
        public bool isLeaf { get; set; }
        public List<KeyTitle> children { get; set; }
        public KeyTitle()
        {

        }
        public KeyTitle(string key, string title)
        {
            this.key = key;
            this.title = title;
        }
        public KeyTitle(string key, string title, bool isLeaf)
        {
            this.key = key;
            this.title = title;
            this.isLeaf = isLeaf;
        }

        public KeyTitle(string key, string title, List<KeyTitle> children)
        {
            this.key = key;
            this.title = title;
            this.children = children;
        }
        public KeyTitle(string key, string title, bool isLeaf, List<KeyTitle> children)
        {
            this.key = key;
            this.title = title;
            this.isLeaf = isLeaf;
            this.children = children;
        }
        public static List<Dictionary<string, object>> ToKeyTitleDictionary(DataTable dt, string key, string parentKey, string title)
        {
            List<Dictionary<string, object>> lstKeyTitle = new List<Dictionary<string, object>>();
            // DataColumn pCol = dt.Columns[key];
            List<DataRow> parent = dt.AsEnumerable().Where(c => string.IsNullOrEmpty(c[parentKey].ToString()) || c[parentKey].ToString() == "0").ToList();
            int level = 0;
            foreach (DataRow r in parent)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("key", r[key].ToString());
                dic.Add("title", r[title].ToString());
                dic.Add("level", level);
                foreach (DataColumn col in dt.Columns)
                {
                    if (!dic.ContainsKey(col.ColumnName))
                    {
                        dic.Add(col.ColumnName, r[col.ColumnName]);
                    }
                }
                //KeyTitle kt = new KeyTitle { key = r[key].ToString(), title = r[title].ToString(), level = level };
                GetChildren(dt, dic, r, key, parentKey, title, level);
                lstKeyTitle.Add(dic);
            }
            return lstKeyTitle;
        }
        static void GetChildren(DataTable dt, Dictionary<string, object> kt, DataRow r, string key, string parentKey, string title, int level)
        {
            level++;
            //kt["children"] = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> dicChildren = new List<Dictionary<string, object>>();
            List<DataRow> children = dt.AsEnumerable().Where(c => c[parentKey].ToString() == r[key].ToString()).ToList();
            if (children.Count > 0)
                kt["isLeaf"] = false;
            else
                kt["isLeaf"] = true;
            foreach (DataRow c in children)
            {
                Dictionary<string, object> ktChildren = new Dictionary<string, object>();
                ktChildren.Add("key", c[key].ToString());
                ktChildren.Add("title", c[title].ToString());
                ktChildren.Add("level", level);
                foreach (DataColumn col in dt.Columns)
                {
                    if (!ktChildren.ContainsKey(col.ColumnName))
                    {
                        ktChildren.Add(col.ColumnName, c[col.ColumnName]);
                    }
                }
                GetChildren(dt, ktChildren, c, key, parentKey, title, level);
                // kt.children.Add(ktChildren);
                dicChildren.Add(ktChildren);
            }
            kt.Add("children", dicChildren);
        }

        public static List<KeyTitle> ToKeyTitle(DataTable dt, string key, string parentKey, string title)
        {
            List<KeyTitle> lstKeyTitle = new List<KeyTitle>();
            DataColumn pCol = dt.Columns[key];
            List<DataRow> parent = dt.AsEnumerable().Where(c => string.IsNullOrEmpty(c[parentKey].ToString()) || c[parentKey].ToString() == "0").ToList();
            int level = 0;
            foreach (DataRow r in parent)
            {
                KeyTitle kt = new KeyTitle { key = r[key].ToString(), title = r[title].ToString(), level = level };
                GetChildren(dt, kt, r, key, parentKey, title, level);
                lstKeyTitle.Add(kt);
            }
            return lstKeyTitle;
        }
        static void GetChildren(DataTable dt, KeyTitle kt, DataRow r, string key, string parentKey, string title, int level)
        {
            level++;
            kt.children = new List<KeyTitle>();
            List<DataRow> children = dt.AsEnumerable().Where(c => c[parentKey].ToString() == r[key].ToString()).ToList();
            if (children.Count > 0)
                kt.isLeaf = false;
            else
                kt.isLeaf = true;
            foreach (DataRow c in children)
            {
                KeyTitle ktChildren = new KeyTitle { key = c[key].ToString(), title = c[title].ToString(), level = level };
                GetChildren(dt, ktChildren, c, key, parentKey, title, level);
                kt.children.Add(ktChildren);

            }
        }
    }

    public class KeyTitleHandler
    {
        public string Key { get; set; }
        public string ParentKey { get; set; }
        public string title { get; set; }

    }
}
