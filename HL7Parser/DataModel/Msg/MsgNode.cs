using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel.Msg
{
    public class MsgNode
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public int Level { get; set; }

        public int Index { get; set; }

        public MsgNode Parent { get; set; }

        public MsgNodes Children { get; set; } = new MsgNodes();

        public string SourceString { get; set; } = String.Empty;

        /// <summary>
        /// Temp variable for å lage source inkludert name
        /// </summary>
        public string SourceStringRaw { get; set; }

        public MsgNode()
        { }

        public MsgNode(int level, int index) : this()
        {
            this.Level = level;
            this.Index = index;
        }

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

        /// <summary>
        /// Oppretter msgNode med navn og setter sourcestring med name ekskludert.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="strRaw"></param>
        public MsgNode(string name, string strRaw, int level, int index)
        {
            this.Name = name;
            this.SourceStringRaw = strRaw;
            this.Level = level;
            this.Index = Index;
            this.SourceString = strRaw.Substring(name.Length);
        }

        #endregion -- Constructor --

        public bool ExtractNameAndSourceString(char[] separator)
        {
            string[] fields = this.SourceStringRaw.Split(separator);
            if (fields.Length > 0)
            {
                this.Name = fields[0];
                // Henter ut souceString. Bruker Math.Min(..) i tilfelle det kun finnes én subnode. 
                this.SourceString = SourceStringRaw.Substring(Math.Min(this.Name.Length + 1, SourceStringRaw.Length));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Oppretter subnode. Setter ParentNode og legger til i Parentnode.Children.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="trimLastCharacter">Fjerner linefeed char i slutten av stringen.</param>
        /// <returns></returns>
        public MsgNodes CreateChildNodes_L0(char[] separator, bool trimLastCharacter = false)
        {
            MsgNodes nodes = new MsgNodes();
            string[] strNodesLevel = this.SourceStringRaw.Split(separator);
            int indexsubnode = 0;
            foreach (string strNode in strNodesLevel)
            {
                //string name = GetSectionNameFromSourceString(strNode, separator);
                MsgNode msgsubnode = new MsgNode(this.Level + 1, indexsubnode);
                msgsubnode.SourceStringRaw = strNode;
                msgsubnode.SourceString = strNode.Substring(this.Name.Length + 1);
                if (trimLastCharacter)
                {
                    msgsubnode.SourceStringRaw = msgsubnode.SourceStringRaw.Substring(0, msgsubnode.SourceStringRaw.Length - 1);
                    msgsubnode.SourceString = msgsubnode.SourceString.Substring(0, msgsubnode.SourceString.Length - 1);
                }
                else
                {
                    msgsubnode.SourceString = msgsubnode.SourceStringRaw;
                }

                msgsubnode.Value = msgsubnode.SourceString;
                msgsubnode.Parent = this;
                this.Children.Add(msgsubnode);
                indexsubnode++;
            }
            return nodes;
        }
    }
}
