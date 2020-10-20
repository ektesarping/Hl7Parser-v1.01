using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Flagg for nodene som kan være repeterende. F.eks 'OBR 28  Kopimottakere'.
        /// </summary>
        public bool ExtraLevelforRepeatingNodes { get; set; } = false;

        /// <summary>
        /// Index for den enkelte section. Brukes til å skille sections fra hverandre.
        /// F.eks når det er flere OBX sections i samme meldingsfil.
        /// </summary>
        public int SectionIndex { get; set; }

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

        public string ErrorMsg { get; set; } = String.Empty;

        /// <summary>
        /// Peker til segment i den innleste mappingen. Brukes til å vise korrekt navn på noden.
        /// </summary>
        public HL7MappingSegment MappingSegment { get; set; }


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
        /// <returns></returns>
        public void CreateChildNodes_L1(char[] separator/*, bool useFirstFieldAsName*/) //, bool trimLastCharacter = false)
        {
            string[] strNodesLevel = this.SourceString.TrimEnd().Split(separator);
            int indexsubnode = 0;

            #region -- Mangler linefeeds. Legger inn linefeeds for å parse (forhåpentligvis) korrekt --
            // Hvis det skal legges inn linefeed må det legges inn liste med lovlige section names, og søke etter [space]+[sectionname]+|
            //if (strNodesLevel.Length == 1)
            //{
            //   string strTmp = SourceString.TrimEnd();
            //    strTmp = strTmp.TrimStart();
            //    strTmp = InsertLineFeed_L1(SourceString, separator.ToString());
            //    strNodesLevel = strTmp.Split(separator);
            //}
            #endregion -- Mangler linefeeds

            foreach (string strNode in strNodesLevel)
            {
                if (String.IsNullOrEmpty(strNode))
                {
                    continue;
                }

                if (String.IsNullOrWhiteSpace(strNode))
                {
                    continue;
                }

                Console.WriteLine(strNode); // fjernes
                MsgNode msgsubnode = new MsgNode();
                msgsubnode.Level = 0;

                if (strNode.Length > 0)
                {
                    /// -- Fjerner siste char hvis den er \r (halvparten av CR / linjeskift ) --
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


        ///// <summary>
        ///// Splitter opp string med [space]+[3 char section name]+separator
        ///// </summary>
        ///// <param name="strOriginal"></param>
        ///// <param name="separator"></param>
        ///// <returns></returns>
        //private string InsertLineFeed_L1(string strOriginal, string separator)
        //{
        //    string pattern = @"\s[A-Z]{3}[|]";
        //    RegexOptions options = RegexOptions.Multiline;

        //    bool finished = false;
        //    int pos = 0;
        //    string strRes = string.Empty;

        //    while (!finished)
        //    {
        //        Match match = Regex.Match(strOriginal, pattern, options);

        //        if (match.Index >= 0)
        //        {
        //            strRes = ReplaceCharAtPos(strOriginal, match.Index, HL7.LINEFEED);
        //        }
        //        else
        //        {
        //            finished = true;
        //        }
        //    }
        //    return strRes;
        //}


        //private string ReplaceCharAtPos(string strOriginal, int pos, string replacementChar)
        //{
        //    if ((pos <= 0) || (pos > strOriginal.Length + 1))
        //    {
        //        return strOriginal;
        //    }

        //    string str1 = strOriginal.Substring(0, pos);
        //    string str2 = strOriginal.Substring(pos + 1);
        //    string strRes = str1 + replacementChar + str2;
        //    return strRes;
        //}


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
                msgsubnode.SectionIndex = this.SectionIndex;
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
                msgsubnode.SectionIndex = this.SectionIndex;

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

        /// <summary>
        /// Oppretter subnode i nivå 3. Spesialtilfelle for repeterende subnoder. F.eks kopimottakere.
        /// </summary>
        /// <param name="separator_L2"></param>
        /// <param name="useFirstFieldAsName"></param>
        /// <param name="trimLastCharacter">Fjerner linefeed char i slutten av stringen.</param>
        /// <returns></returns>
        public void CreateChildNodes_L4(char[] separator_L2, char[] separator_L3)
        {
            string[] nodeRepeating = this.Value.Split(separator_L3);

            foreach (string str in nodeRepeating)
            {
                MsgNode nodeRepeat = new MsgNode(str);
                nodeRepeat.ExtraLevelforRepeatingNodes = true;
                nodeRepeat.MappingSectionName = this.MappingSectionName;
                nodeRepeat.Value = str;
                nodeRepeat.Parent = this;
                //nodeRepeat.Level = nodeRepeat.Parent.Level + 1;
                this.Children.Add(nodeRepeat);
                nodeRepeat.SectionIndex = this.SectionIndex;

                string[] strNodesLevel3 = str.Split(separator_L2);
                int indexsubnode = 1;
                foreach (string strNode in strNodesLevel3)
                {
                    // -- Oppretter ikke subnoder hvis noden har kun en verdi --
                    if (strNodesLevel3.Length <= 1)
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
                    msgsubnode.SectionIndex = this.SectionIndex;

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
                    msgsubnode.Parent = nodeRepeat;
                    if (msgsubnode.Parent != null)
                    {
                        msgsubnode.MappingSectionName = msgsubnode.Parent.MappingSectionName; // Kopierer mapping section name fra parent.
                    }
                    nodeRepeat.Children.Add(msgsubnode);
                    indexsubnode++;
                }
            }
        }

        /// <summary>
        /// Validerer noden ift. valideringsreglene i mappingen.
        /// </summary>
        public void ValidateNode()
        {
            if (this.MappingSegment != null)
            {
                if (this.level > 0)
                {
                    int notUsed;
                    if ((this.MappingSegment.IsNumeric) && (!int.TryParse(this.Value, out notUsed)))
                    {
                        this.ErrorMsg = "Verdien skal være numerisk.;";
                    }

                    int lengthTmp = this.Value.ToString().Length;
                    string strTmp = string.Empty;
                    // -- Forklaring av feil lengde av stringen. 

                    if (this.MappingSegment.MinLenght >= 0)
                    {
                        if ((this.MappingSegment.MinLenght == this.MappingSegment.MaxLenght))
                        {
                            strTmp = "Skal være " + this.MappingSegment.MinLenght.ToString() + " karakterer; ";
                        }
                        else
                        {
                            strTmp = "Skal være mellom " + this.MappingSegment.MinLenght.ToString() + " og " + this.MappingSegment.MaxLenght.ToString() + " karakterer; ";
                        }
                    }

                    if (lengthTmp < this.MappingSegment.MinLenght)
                    {
                        this.ErrorMsg += "Verdien for kort. " + strTmp;
                    }
                    else if (lengthTmp > this.MappingSegment.MaxLenght)
                    {
                        this.ErrorMsg += "Verdien for lang. " + strTmp;
                    }

                    // Hack for å sette inn space mellom tekstene:
                    this.ErrorMsg = this.ErrorMsg.Replace(";", " ");
                }
            }
        }

        /// <summary>
        ///  Setter tekst som vises i treeview.
        /// </summary>
        public string TreeNodeText
        {
            get
            {
                string mappingSegmentNameTmp = String.Empty;
                // -- Markerer noden hvis den ikke finnes i mappingen. --
                if (this.MappingSegment != null)
                {
                    mappingSegmentNameTmp = this.MappingSegment.SegmentName;
                }
                else
                {
                    mappingSegmentNameTmp = this.Name + " ???";
                }

                string str = String.Empty;
                if (HL7.DEBUG_GLOBAL)
                {
                    str += "L:" + this.Level.ToString() + " ";
                }
                switch (this.Level)
                {
                    case 0:
                        if (this.ExtraLevelforRepeatingNodes)
                        {
                            // -- Spesialhåndtering av parentnode for repeterende noder. F.eks varierende antall kopimottakere. --
                            //str += "[" /*+ this.MappingSectionName + " " + Index_L1.ToString() + ": "*/;
                            // str += mappingSegmentNameTmp;
                            str += this.Parent.MappingSegment.SegmentName;
                            //str += "]"; // = " + this.Value;
                        }
                        else
                        {
                            str += "[" + this.MappingSectionName + " "; //+ Index_L1.ToString() + ": ";
                            str += mappingSegmentNameTmp;
                            str += "]"; // = " + this.Value;
                        }
                        break;

                    case 1:
                        str += "[" + this.MappingSectionName + " " + Index_L1.ToString() + ": ";
                        str += mappingSegmentNameTmp;
                        str += "] = " + this.Value;

                        break;
                    case 2:
                        str += "[" + this.MappingSectionName + " " + Index_L1.ToString() + "." + Index_L2.ToString() + ": ";
                        //       if (this.MappingSegment != null)
                        str += mappingSegmentNameTmp;
                        str += "] = " + this.Value;

                        break;
                    case 3:
                        str += "[" + this.Parent.MappingSectionName + " " + Index_L1.ToString() + "." + Index_L2.ToString() + " ";
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
            return this.MappingSegment.SegmentName + sep + this.Name + sep + " L:" + this.Level.ToString() + "/I:" + this.Index_L2.ToString() + sep + " Src: " + this.SourceString;
        }


        private const string sep = "\t";
        public static string ToReportHeader()
        {
            return "MappingSectionName" + sep + "SectionIndex" + sep + "Level" + sep + "Index_L1" + sep + "Index_L2" + sep + "MappingSegment" + sep + "Value" + sep + "this.SourceString";
        }


        /// <summary>
        /// Viser kun verdi for noder som ikke har children.
        /// </summary>
        /// <returns></returns>
        public string ToReport()
        {
            string str = String.Empty;


            str = this.MappingSectionName + sep + this.SectionIndex.ToString() + sep + this.Level.ToString() + sep + this.Index_L1.ToString() + sep + this.Index_L2.ToString() + sep; // + this.MappingSegment.SegmentName; // + sep + this.Value; // + sep + this.SourceString;
            if (this.MappingSegment != null)
            {
                if (this.MappingSegment.SegmentName != null)
                {
                    str += this.MappingSegment.SegmentName;
                }
            }
            str += sep + this.Value;

            return str;
        }
    }
}
