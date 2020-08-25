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

        #region -- Parent --
        public MsgNode(string name) : this()
        {
            this.Name = name;
        }

        public MsgNode(string name, MsgNode parent) : this(name)
        {
            this.Parent = parent;
            this.Parent.Children.Add(this);
        }
        #endregion -- Parent --




    }
}
