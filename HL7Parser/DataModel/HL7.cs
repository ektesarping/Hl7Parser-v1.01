using HL7Viewer.DataModel.Msg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer.DataModel
{
    public class HL7
    {
        // **************************************
        public static bool DEBUG_GLOBAL = false;
        // **************************************



        public string Name { get; set; }
        public string VersionInfo { get; set; }


        private Hl7Mapping mappingSelected;
        /// <summary>
        /// Mapping med alle felter fra spec. Brukes som oppslag ved import av HL7 fil.
        /// </summary>
        public Hl7Mapping MappingSelected
        {
            get
            {
                //string strMappingSelected = Properties.Settings.Default.LastMappingSelected;
                //this.mappingSelected = this.HL7Mappings.Get(strMappingSelected);
                return this.mappingSelected;
            }
            set
            {
                this.mappingSelected = value;
                if (this.mappingSelected != null)
                {
                    if (this.mappingSelected.FileInfo != null)
                    {
                        Properties.Settings.Default.LastMappingSelected = this.mappingSelected.FileInfo.FullName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Henter siste valgte mapping fra default properties (under User Properties).
        /// </summary>
        private void SetSelectedMappingFromSettingsProperties()
        {
            // -- Hent navnet på selected mapping fra Properties.Settings --
            string strMappingSelected = Properties.Settings.Default.LastMappingSelected;
            this.MappingSelected = this.HL7Mappings.Get(strMappingSelected); // Kan være 'null' hvis mappingen ikke finnes.

            // -- Hvis det finnes kun en mapping, sett denne som selected. --
            if ((this.MappingSelected == null) && (this.HL7Mappings.Count == 1))
            {
                this.MappingSelected = this.HL7Mappings[0];
            }
        }


        /// <summary>
        /// Alle Hl7 mappings som er valgt.
        /// </summary>
        public HL7Mappings HL7Mappings { get; set; }


        /// <summary>
        /// Root node i meldingsstrukturen.
        /// </summary>
        public HL7Viewer.DataModel.Msg.MsgNode MsgRootnode { get; set; } = new MsgNode(0);


        public HL7SegmentCategories _HL7SegmentCategories { get; set; } = new HL7SegmentCategories();

        public FileInfo MsgFile { get; set; }

        public string Content { get; set; }

        private HL7MappingSegments _HL7Segments { get; set; } = new HL7MappingSegments();

        public FileInfo MappingFileFi { get; set; }
        //        private const string MappingFileName = "Mapping_HL7.csv";


        public HL7Viewer.DataModel.GUI.UcHL7 _UcHl7 { get; set; } // TODO: 201008 Fjernes hvis eventhandler fra FileUpdated ikke skal brukes

        #region -- Default settings --
        private const string DEFAULT_MAPPINGFOLDE_NAME = "Datamodel";
        private const string MAPPINGFILE_EXT = ".csv";
        #endregion -- Default settings --

        public static string LINEFEED = "\r\n";
        private char[] SEPARATOR_LEVEL_0 = new char[] { '\n', '\r' };
        private char[] SEPARATOR_LEVEL_1 = new char[] { '|' };
        private char[] SEPARATOR_LEVEL_2 = new char[] { '^' }; //, '~' };
        private char[] SEPARATOR_LEVEL_3_REPEATING_FIELD = new char[] { '~' };

        private const string MSG_NOT_INCLUDED_IN_MAPPING = "(*)";

        public const string MSG_NODE_VALUE_HIDDEN = "******";


        public HL7()
        { }

        public void PostInitialize()
        {
            this.MsgRootnode.Name = "RootNode";

            FileInfo fiApplication = new FileInfo(Application.ExecutablePath);
            DirectoryInfo di = new DirectoryInfo(Path.Combine(fiApplication.DirectoryName, DEFAULT_MAPPINGFOLDE_NAME));
            this.HL7Mappings = new HL7Mappings(di, MAPPINGFILE_EXT);  // Leser inn mappinger som er listet i Properties.Settings.Default

            SetSelectedMappingFromSettingsProperties();
        }


        public void ImportHL7MsgFile()
        {
            if (this.MsgFile != null)
            {
                ImportHL7MsgFile(this.MsgFile);
            }
            else
            {
                //MessageBox.Show("Sist åpnede meldingsfil ikke funnet.\r\n" + Properties.Settings.Default.MsgFilename, "Åpne HL7 meldingsfil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// Importerer og parser
        /// </summary>
        /// <param name="fi"></param>
        public void ImportHL7MsgFile(FileInfo fi)
        {
            StreamReader sr = new StreamReader(fi.FullName, Encoding.GetEncoding(1252));
            this.MsgFile = fi;
            string str = sr.ReadToEnd();
            sr.Close();
            ImportHL7MsgFile(str);
        }


        /// <summary>
        /// Oppretter meldingsnoder for de ulike nivåene i meldingsfilen. 
        /// </summary>
        /// <param name="strFileContent"></param>
        public void ImportHL7MsgFile(string strFileContent)
        {
            // _HL7SegmentCategories = new HL7SegmentCategories();

            #region -- Find executable path--
            FileInfo executableFi = new FileInfo(Application.ExecutablePath); // Hent mappingfilen fra programfolderen.
                                                                              //MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
                                                                              //MappingSelected.ImportMapping(MappingFileFi);
            #endregion -- Find executable path --

            this.MsgRootnode = new MsgNode("Root", strFileContent, 0, 0);

            MsgRootnode.CreateChildNodes_L1(SEPARATOR_LEVEL_0); //, false);

            // Setter Name og ekstraherer SourceString for nodene i nivå 0
            foreach (MsgNode childnode in MsgRootnode.Children)
            {
                childnode.ExtractNameAndSourceStringFirstLevel(SEPARATOR_LEVEL_1);
            }

            // -- Populerer SectionIndex for hver msgNode --
            int sectionIndesTmp = 0;
            foreach (MsgNode childnode in MsgRootnode.Children)
            {
                childnode.SectionIndex = sectionIndesTmp;
                sectionIndesTmp++;
            }


            foreach (MsgNode subNode_L0 in MsgRootnode.Children)
            {
                // -- Parse subnodes level 1 --
                subNode_L0.CreateChildNodes_L2(SEPARATOR_LEVEL_1); //, true); //, false);
                                                                   //subNode_L0.Level = 2;

                foreach (MsgNode subNode_L1 in subNode_L0.Children)
                {
                    if (
                        (subNode_L1.Value.Split(SEPARATOR_LEVEL_3_REPEATING_FIELD).Length > 1)  // Inneholder skilletegn for repeterende noder  '~'
                        && (!(subNode_L1.MappingSectionName == "MSH" && subNode_L1.Level == 1 && subNode_L1.Index_L1 == 1 && subNode_L1.Index_L2 == 0)) //Hopp over node MSH 0,1. Den lister opp skilletegn
                        )
                    {
                        // -- Legge inn node for hver av de reperende feltene. F.eks varienende antall kopimottakere i OBR 28.
                        subNode_L1.CreateChildNodes_L4(SEPARATOR_LEVEL_2, SEPARATOR_LEVEL_3_REPEATING_FIELD);
                    }
                    else
                    {
                        subNode_L1.CreateChildNodes_L3(SEPARATOR_LEVEL_2); //, true, false);
                                                                           //subNode_L1.Level = 3;
                                                                           //subNode_L1.Index_L2 = subNode_L0.Index_L2;
                    }
                }
            }

            // -- Matcher mot mapping --
            foreach (MsgNode childnode in this.MsgRootnode.Children)
            {
                childnode.MappingSegment = MappingSelected.GetSegmentFromSection(childnode.MappingSectionName, childnode.Index_L2, childnode.Index_L2);

                // -- Erstatt verdien til segmentet hvis det skal være skjult --
                if (childnode.MappingSegment != null)
                {
                    // -- Sjekker om verdien skal skjules som *****  --
                    if (childnode.MappingSegment.HideValue)
                    {
                        childnode.Value = MSG_NODE_VALUE_HIDDEN;
                    }
                    // -- Sjekker om verdien skal vises i bold font --
                    if (childnode.MappingSegment.ShowValueAsBold)
                    {
                        childnode.ShowAsBoldFont = true;
                    }
                    childnode.ValidateNode();
                }
                MatchMsgNodeToMappingRecursive(childnode);
            }
        }


        private void MatchMsgNodeToMappingRecursive(MsgNode node)
        {
            foreach (MsgNode childnode in node.Children)
            {
                // -- Hvis mappingsegment == null indikeres det med annen treenode farge. --
                childnode.MappingSegment = MappingSelected.GetSegmentFromSection(childnode.MappingSectionName, childnode.Index_L1, childnode.Index_L2);
                MatchMsgNodeToMappingRecursive(childnode);


                // -- Erstatt verdien til segmentet hvis det skal være skjult --
                if (childnode.MappingSegment != null)
                {
                    // -- Sjekker om verdien skal skjules som *****  --
                    if (childnode.MappingSegment.HideValue)
                    {
                        childnode.Value = MSG_NODE_VALUE_HIDDEN;
                    }
                    // -- Sjekker om verdien skal vises i bold font --
                    if (childnode.MappingSegment.ShowValueAsBold)
                    {
                        childnode.ShowAsBoldFont = true;
                    }

                    childnode.ValidateNode();
                }
            }
        }

        //private HL7MappingSegments CreateSubSegments(string substringSource, HL7MappingSegment parentSegment, string segmentName)
        //{
        //    HL7MappingSegments subsegments = new HL7MappingSegments();
        //    // Finne subsegmenter i fields[i]
        //    string[] subFields = substringSource.Split(SEPARATOR_LEVEL_1); // <-- Sjekk

        //    if (subFields.Length > 1)
        //    {
        //        for (int index_L2 = 0; index_L2 < subFields.Length; index_L2++)
        //        {
        //            string value = subFields[index_L2];
        //            HL7MappingSegment subsegment = new HL7MappingSegment(parentSegment.SectionName, segmentName, parentSegment.Index_L1, index_L2 + 1);
        //            subsegment.ParentSegment = parentSegment;
        //            subsegments.Add(subsegment);
        //        }
        //    }
        //    return subsegments;
        //}

        //public static string AddLinefeed(string str)
        //{
        //    str += Environment.NewLine;
        //    return str;
        //}

        /// <summary>
        /// Fjerner kommentar fra en streng. 
        /// </summary>
        /// <param name="str"></param>
        public static string TrimComment(string str, string commentKey)
        {
            if (str.Contains(commentKey))
            {
                int pos = str.IndexOf(commentKey);
                int length = str.Length - pos;
                str = str.Substring(0, pos);
            }
            return str;
        }

        public string ToReport()
        {
            string str = String.Empty;
            if (this.MsgRootnode != null)
            {
                str = this.MsgRootnode.Children.ToReport();
            }
            return str;
        }




        /// <summary>
        /// Kopierer _HL7Segments inn i HL7SegmentCategories
        /// </summary>
        [Obsolete]
        private void PopulateCategories()
        {
            foreach (HL7SegmentCategory cat in this._HL7SegmentCategories)
            {
                foreach (HL7MappingSegment segment in _HL7Segments)
                {
                    if (segment.SectionName == cat.CategoryName)
                    {
                        cat._HL7Segments.Add(segment);
                    }
                }
            }
        }

        private const string SPACE = " ";
        private const string LINEBREAK = "\r\n";

        /// <summary>
        /// Legger inn linjeskift i en streng slik at maks lnjelengde er lik argumentet lineLength.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLineLength"></param>
        /// <returns></returns>
        public static string InsertLinebreaks(string str, int maxLineLength)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }
            //            str = @"Linje01-.10...:....20...:....30...:....40...: ...50...:.. .60...:....70...:..XLinje02-.10...:....20...:....30...:....40...:....50...:....60...:... 70...:..XLinje03-.10...:....20... ....30...:....40...:....50...:....60...:....70...:..X";
            string result = String.Empty;
            int pos = 0;
            int lineNo = 0;
            int posLineBreak = 0;

            if (str.Length <= maxLineLength)
            {
                result = str;
                return result;
            }

            while (str.Length > 0)
            {
                if (str.Length > maxLineLength)
                {
                    // Kopier første del av strengen med lengde 'lineLength'
                    string tmp = str.Substring(0, maxLineLength);

                    // Finn siste space for å legge inn linjeskift i den posisjonen.
                    int posLastSpace = tmp.LastIndexOf(SPACE);

                    // Hent ut delen som skal skilles ut i ny linje
                    if (posLastSpace == 0)
                    {
                        tmp = str.Substring(0, maxLineLength);
                    }
                    else if (posLastSpace > 0)
                    {
                        tmp = str.Substring(0, posLastSpace);
                    }

                    if (!String.IsNullOrEmpty(result))
                    {
                        result += LINEBREAK;
                    }

                    result += tmp;
                    str = str.Replace(tmp, "");
                }
                else
                {
                    result += LINEBREAK + str;
                    return result;
                }
            }
            return result;
        }
    }
}
