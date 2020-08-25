using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public TreeNode Treenode { get; set; }

        public string NodeText
        {
            get { return this.Parent.Name + " " + this.Name + " " + this.Index.ToString() + ": " + this.Value; }
        }

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

        //public MsgNode(string name, MsgNode parent) : this(name)
        //{
        //    this.Parent = parent;
        //    this.Parent.Children.Add(this);
        //}

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
            this.SourceString = strRaw; //.Substring(name.Length);
        }

        #endregion -- Constructor --

        public bool ExtractNameAndSourceStringFirstLevel(char[] separator)
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
        /// <param name="useFirstFieldAsName"></param>
        /// <param name="trimLastCharacter">Fjerner linefeed char i slutten av stringen.</param>
        /// <returns></returns>
        public void CreateChildNodes_L0(char[] separator, bool useFirstFieldAsName, bool trimLastCharacter = false)
        {
            string[] strNodesLevel = this.SourceString.Split(separator);
            int indexsubnode = 0;
            foreach (string strNode in strNodesLevel)
            {
                //string name = GetSectionNameFromSourceString(strNode, separator);
                MsgNode msgsubnode = new MsgNode(this.Level + 1, indexsubnode);
                msgsubnode.SourceStringRaw = strNode;

                // -- I nivå 1 ligger navnet til parent noden i første felt. Ignoreres for andre nivå enn nivå 0. --
                if (useFirstFieldAsName)
                {
                    msgsubnode.SourceString = strNode;
                }
                else
                {
                    msgsubnode.SourceString = strNode.Substring(this.Name.Length + 1);
                }

                // -- Fjer siste char som er \r ( halvparten av CR / linjeskift ) --
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
        }

        public override string ToString()
        {
            string sep = "\t";
            return this.Name + sep + " L:" + this.Level.ToString() + "/I:" + this.Index.ToString() + sep + " Src: " + this.SourceStringRaw;
        }
    }
}
