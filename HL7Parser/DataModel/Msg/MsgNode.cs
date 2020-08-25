using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel.Msg
{
    public class MsgNode
    {
        public string Name { get; set; }

        public string Value{ get; set; }

        public MsgNode Parent { get; set; }

        public MsgNodes Children { get; set; }

        public string SourceString { get; set; }

        public MsgNode()
        { }

        #region -- Constructor --
        public MsgNode(string name) : this()
        {
            this.Name = name;
        }

        public MsgNode(string name, MsgNode parent) : this(name)
        {
            this.Parent = parent;
            this.Parent.Children.Add(this);
        }
        #endregion -- Constructor --


        /// <summary>
        /// Oppretter subnode. Setter ParentNode og legger til i Parentnode.Children.
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="parent"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public MsgNodes CreateChildNodes(char[] separator)
        {
            MsgNodes nodes = new MsgNodes();
            string[] strNodesLevel = this.SourceString.Split(separator);
            foreach (string strNode in strNodesLevel)
            {
                //string name = GetSectionNameFromSourceString(strNode, separator);
                MsgNode msgsubnode = new MsgNode();
                msgsubnode.Value = strNode;
                msgsubnode.Parent = this;
                msgsubnode.Children.Add(msgsubnode);
            }
            return nodes;
        }
    }
}
