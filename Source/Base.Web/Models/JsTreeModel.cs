//using Base.Web.Utilities;
using Base.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Base.Web.Models
{
    public class JsTreeModel
    {

        public JsTreeNodeState state { get; set; }

        public string text { get; set; }

        public string icon { get; set; }

        //public JsTreeNodeData data { get; set; }

        public JsTreeAttribute li_attr { get; set; }

        public List<JsTreeModel> children { get; set; }

        public JsTreeModel()
        {
            children = new List<JsTreeModel>();
            state = new JsTreeNodeState { disabled = false, opened = true, selected = false };
        }
    }
}