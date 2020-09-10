using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer.DataModel.Msg
{
    [DebuggerDisplay("Tostring:: value is {ToString()}")]
    public class MsgNode
    {
        public string Name { get; set; }

        public string Value { get; set; }

        private int level = 0;

        public int Level { get => level; set => level = value; }

        public int Index_L1 { get; set; }

        public int Index_L2 { get; set; }

        //public int Index_L3 { get; set; }

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
        //[Obsolete]
        //public string SourceStringRaw { get; set; }

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
            this.Index_L2 = index;
        }


        public MsgNode(string name) : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Oppretter msgNode med navn og setter sourcestring med name ekskludert.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceString"></param>
        public MsgNode(string name, string sourceString, int level, int index)
        {
            this.Name = name;
            //this.SourceStringRaw = strRaw;
            this.Level = level;
            this.Index_L2 = Index_L2;
            this.SourceString = sourceString; //.Substring(name.Length);
        }

        #endregion -- Constructor --

        public bool ExtractNameAndSourceStringFirstLevel(char[] separator)
        {
            string[] fields = this.SourceString.Split(separator);
            if (fields.Length > 0)
            {
                this.Name = fields[0];
                this.MappingSectionName = this.Name; // Brukes til å linke til mapping segment.
                // Henter ut souceString. Bruker Math.Min(..) i tilfelle det kun finnes én subnode. 
                //this.SourceString = SourceString.Substring(Math.Min(this.Name.Length + 1, SourceString.Length));
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
        public void CreateChildNodes_L1(char[] separator/*, bool useFirstFieldAsName*/) //, bool trimLastCharacter = false)
        {
            string[] strNodesLevel = this.SourceString.Split(separator);
            int indexsubnode = 0;
            foreach (string strNode in strNodesLevel)
            {
                MsgNode msgsubnode = new MsgNode();
                msgsubnode.Level = 0;

                /// -- Fjerner siste char hvis den er \r ( halvparten av CR / linjeskift ) --
                string tmpForDebuggingSlettes = (strNode.Substring(strNode.Length - 1, 1));
                //string strNodeTrimmed = strNode;
                if (strNode.Substring(strNode.Length - 1, 1) == "\r")
                {
                    msgsubnode.SourceString = strNode.Substring(0, strNode.Length - 1);
                }
                else
                {
                    msgsubnode.SourceString = strNode;
                }

                msgsubnode.Index_L1 = 0;
                msgsubnode.Index_L2 = 0;

                msgsubnode.Value = msgsubnode.SourceString;
                msgsubnode.Parent = this;
                //msgsubnode.MappingSectionName = msgsubnode.Name;  // Settes i metoden 'ExtractNameAndSourceStringFirstLevel'
                this.Children.Add(msgsubnode);
                indexsubnode++;
            }
        }


        /// <summary>
        /// Oppretter subnode nivå 1. Setter ParentNode og legger til i Parentnode.Children.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="useFirstFieldAsName"></param>
        /// <param name="trimLastCharacter">Fjerner linefeed char i slutten av stringen.</param>
        /// <returns></returns>
        public void CreateChildNodes_L2(char[] separator) //, bool useFirstFieldAsName) //, bool trimLastCharacter = false)
        {
            string[] strNodesLevel = this.SourceString.Split(separator);
            int indexsubnode = 0; // Starter på 0 for å fjerne section name som kommer først i stringen.
            foreach (string strNode in strNodesLevel)
            {
                // -- Ignorer leddet med section name. --
                if (indexsubnode == 0)
                {
                    indexsubnode++;
                    continue;
                }

                MsgNode msgsubnode = new MsgNode();
                msgsubnode.SourceString = strNode;

                msgsubnode.Level = 1;
                msgsubnode.Index_L1 = indexsubnode;
                msgsubnode.Index_L2 = 0;

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
        /// Oppretter subnode i nivå 2. Setter ParentNode og legger til i Parentnode.Children.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="useFirstFieldAsName"></param>
        /// <param name="trimLastCharacter">Fjerner linefeed char i slutten av stringen.</param>
        /// <returns></returns>
        public void CreateChildNodes_L3(char[] separator) //, bool useFirstFieldAsName, bool trimLastCharacter = false)
        {
            string[] strNodesLevel = this.SourceString.Split(separator);
            int indexsubnode = 1;
            foreach (string strNode in strNodesLevel)
            {
                // -- Oppretter ikke subnoder hvis noden har kun en verdi --
                if (strNodesLevel.Length <= 1)
                {
                    continue;
                }

                // -- MSH 1 inneholder separatorer. Ikke splitt i subnodes. --
                if ((this.MappingSectionName.ToUpper() == "MSH") && (this.Index_L1 == 2))
                {
                    continue;
                }

                //string name = GetSectionNameFromSourceString(strNode, separator);
                MsgNode msgsubnode = new MsgNode(); // Bruker samme index som for parent node. 
                msgsubnode.level = 2;
                msgsubnode.Index_L1 = this.Index_L1;
                msgsubnode.Index_L2 = indexsubnode;

                // -- I nivå 1 ligger navnet til parent noden i første felt. Ignoreres for andre nivå enn nivå 0. --
                //if (useFirstFieldAsName)
                //{
                msgsubnode.SourceString = strNode;
                //}
                //else
                //{
                //    msgsubnode.SourceString = strNodeTrimmed.Substring(this.Name.Length + 1);
                //}

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
                string mappingSegmentNameTmp = String.Empty;
                if (this.MappingSegment != null)
                {
                    mappingSegmentNameTmp = this.MappingSegment.SegmentName;
                }
                else
                {
                    mappingSegmentNameTmp = this.Name + " ???";
                }
               

                string str = "L:" + this.Level.ToString() + " ";
                switch (this.Level)
                {
                    case 0:
                        str += "[" + this.MappingSectionName + " " + Index_L1.ToString() + ": ";
                        str += mappingSegmentNameTmp;
                        str += "]" ; // = " + this.Value;

                        break;

                    case 1:
                        str += "[" + this.MappingSectionName + " " + Index_L1.ToString() + ": ";
                        str += mappingSegmentNameTmp;
                        str += "] = " + this.Value;

                        break;
                    case 2:
                        str += "[" + this.MappingSectionName + " " + Index_L1.ToString() + ": ";
                 //       if (this.MappingSegment != null)
                            str += mappingSegmentNameTmp;
                        str += "] = " + this.Value;

                        break;
                    case 3:
                        str += "[" + this.MappingSectionName + " " + Index_L1.ToString() + "." + Index_L2.ToString() + " " ;
                        if (this.MappingSegment != null)
                            str += mappingSegmentNameTmp;
                        str += "] = " + this.Value;

                        break;
                    default:
                        //indexTmp = "<unknown>";
                        break;
                }


                //string nameOrSegment = String.Empty;
                //if (this.MappingSegment != null)
                //{
                //    nameOrSegment = this.MappingSegment.SegmentName;
                //}
                //else
                //{
                //    nameOrSegment = this.Name;
                //}


                //string indexTmp = String.Empty;
                //switch (this.Level)
                //{
                //    case 1:
                //        indexTmp = this.Index_L2.ToString();
                //        break;
                //    case 2:
                //        indexTmp = this.Index_L2.ToString();
                //        break;
                //    case 3:
                //        indexTmp = this.Parent.Index_L2.ToString() + "." + this.Index_L2.ToString();
                //        break;
                //    default:
                //        indexTmp = "<unknown>";
                //        break;
                //}


                //string str = "[" + this.MappingSectionName + " " + indexTmp + " ";
                //if (this.MappingSegment != null)
                //{
                //    str += this.MappingSegment.SegmentName;
                //}
                //else
                //{
                //    str += this.Name;
                //}
                //str += " Level:" + this.Level.ToString();
                //str += "] = " + this.Value;
                return str;
            }
        }

        public override string ToString()
        {
            string sep = " ";
            return this.Name + sep + " L:" + this.Level.ToString() + "/I:" + this.Index_L2.ToString() + sep + " Src: " + this.SourceString;
        }
    }
}
