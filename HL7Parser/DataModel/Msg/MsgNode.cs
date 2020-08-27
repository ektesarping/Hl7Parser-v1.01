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

        private int level = 0;

        public int Level { get => level; set => level = value; }

        public int Index { get; set; }

        /// <summary>
        /// Index for oppslag mot mapping. Ulik this.Index ved repeterende sekvenser av felter.
        /// F.eks OBR28 Kopimottakere som har 15 repeterende felter for hver av mottakerne.
        /// </summary>
        public int MappingIndex { get; set; }

        public MsgNode Parent { get; set; }

        public MsgNodes Children { get; set; } = new MsgNodes();

        public string SourceString { get; set; } = String.Empty;

        /// <summary>
        /// Temp variable for å lage source inkludert name
        /// </summary>
        public string SourceStringRaw { get; set; }

        /// <summary>
        /// Navn på section i mappingen (= 1. nivå) Brukes til å linke til rett mapping segment.
        /// </summary>
        public string MappingSectionName { get; set; }

        /// <summary>
        /// Peker til segment i den innleste mappingen. Brukes til å vise korrekt navn på noden.
        /// </summary>
        public HL7MappingSegmentString MappingSegment { get; set; }


        public TreeNode Treenode { get; set; }


        #region -- Constructor -- 
        public MsgNode()
        { }

        public MsgNode(int index) : this()
        {
            this.Index = index;
        }


        public MsgNode(string name) : this()
        {
            this.Name = name;
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
            this.SourceString = strRaw; //.Substring(name.Length);
        }

        #endregion -- Constructor --

        public bool ExtractNameAndSourceStringFirstLevel(char[] separator)
        {
            string[] fields = this.SourceStringRaw.Split(separator);
            if (fields.Length > 0)
            {
                this.Name = fields[0];
                this.MappingSectionName = this.Name; // Brukes til å linke til mapping segment.
                // Henter ut souceString. Bruker Math.Min(..) i tilfelle det kun finnes én subnode. 
                this.SourceString = SourceStringRaw.Substring(Math.Min(this.Name.Length + 1, SourceStringRaw.Length));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Oppretter subnode nivå 1. Setter ParentNode og legger til i Parentnode.Children.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="useFirstFieldAsName"></param>
        /// <param name="trimLastCharacter">Fjerner linefeed char i slutten av stringen.</param>
        /// <returns></returns>
        public void CreateChildNodes_L1(char[] separator, bool useFirstFieldAsName, bool trimLastCharacter = false)
        {
            string[] strNodesLevel = this.SourceString.Split(separator);
            int indexsubnode = 1;
            foreach (string strNode in strNodesLevel)
            {
                //string name = GetSectionNameFromSourceString(strNode, separator);
                MsgNode msgsubnode = new MsgNode(indexsubnode);
                msgsubnode.SourceStringRaw = strNode;
                //msgsubnode.Level = this.Level + 1;

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
                if (msgsubnode.Parent != null)
                {
                    msgsubnode.MappingSectionName = msgsubnode.Parent.MappingSectionName; // Kopierer mapping section name fra parent.
                }
                this.Children.Add(msgsubnode);
                indexsubnode++;
            }
        }


        /// <summary>
        /// Oppretter subnodei nivå 2. Setter ParentNode og legger til i Parentnode.Children.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="useFirstFieldAsName"></param>
        /// <param name="trimLastCharacter">Fjerner linefeed char i slutten av stringen.</param>
        /// <returns></returns>
        public void CreateChildNodes_L3(char[] separator, bool useFirstFieldAsName, bool trimLastCharacter = false)
        {
            string[] strNodesLevel = this.SourceString.Split(separator);
            int indexsubnode = 1;
            foreach (string strNode in strNodesLevel)
            {
                //string name = GetSectionNameFromSourceString(strNode, separator);
                MsgNode msgsubnode = new MsgNode(indexsubnode); // Bruker samme index som for parent node. 
                msgsubnode.SourceStringRaw = strNode;
                //msgsubnode.Level = this.Level + 1;

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
                if (msgsubnode.Parent != null)
                {
                    msgsubnode.MappingSectionName = msgsubnode.Parent.MappingSectionName; // Kopierer mapping section name fra parent.
                }
                this.Children.Add(msgsubnode);
                indexsubnode++;
            }
        }

        public string TreeNodeText
        {
            get
            {
                string indexTmp = String.Empty;
                switch (this.Level)
                {
                    case 1:
                        indexTmp = this.Index.ToString();
                        break;
                    case 2:
                        indexTmp = this.Index.ToString();
                        break;
                    case 3:
                        indexTmp = this.Parent.Index.ToString() + "." + this.Index.ToString();
                        break;
                    default:
                        indexTmp = "<unknown>";
                        break;
                }


                string str = "[" + this.MappingSectionName + " " + indexTmp + " ";
                if (this.MappingSegment != null)
                {
                    str += this.MappingSegment.SegmentName;
                }
                else
                {
                    str += this.Name;
                }
                str += " Level:" + this.Level.ToString();
                str += "] = " + this.Value;
                return str;
            }
        }

        public override string ToString()
        {
            string sep = "\t";
            return this.Name + sep + " L:" + this.Level.ToString() + "/I:" + this.Index.ToString() + sep + " Src: " + this.SourceStringRaw;
        }
    }
}
