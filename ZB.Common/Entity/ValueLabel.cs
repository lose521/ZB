using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.Common.Entity
{
    /// <summary>
    /// 适用于下拉框
    /// </summary>
    public class ValueLabel
    {
        public string value { get; set; }
        public string label { get; set; }
        public int level { get; set; }
        public bool isLeaf { get; set; }
        public List<ValueLabel> children { get; set; }
        public ValueLabel()
        {

        }
        public ValueLabel(string value,string label)
        {
            this.value = value;
            this.label = label;
        }
        public ValueLabel(string value, string label, bool isLeaf)
        {
            this.value = value;
            this.label = label;
            this.isLeaf = isLeaf;
        }

        public ValueLabel(string value, string label, List<ValueLabel> children)
        {
            this.value = value;
            this.label = label;
            this.children = children;
        }
        public ValueLabel(string value, string label,bool isLeaf, List<ValueLabel> children)
        {
            this.value = value;
            this.label = label;
            this.isLeaf = isLeaf;
            this.children = children;
        }
    }
}
