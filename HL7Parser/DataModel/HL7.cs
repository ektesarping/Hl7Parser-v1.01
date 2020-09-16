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
                //string strMappingSelected = Properties.Settings.Default.LastMappingSelected2;

                //this.mappingSelected = this.HL7Mappings.Get(strMappingSelected);
                return this.mappingSelected;
            }
            set
            {
                this.mappingSelected = value;
                if (this.mappingSelected != null)
                {
                    if (this.MappingSelected.FileInfo != null)
                    {
                        Properties.Settings.Default.LastMappingSelected2 = this.MappingSelected.FileInfo.FullName;
                        Properties.Settings.Default.Save();
                    } 
                }
            }
        }

        /// <summary>
        /// HEnter siste valgte mapping fra default properties (under User Properties).
        /// </summary>
        private void SetSelectedMappingFromDefaultProperties()
        {
            // -- Hent navnet på selected mapping fra Properties.Settings --
            string strMappingSelected = Properties.Settings.Default.LastMappingSelected2;
            this.MappingSelected = this.HL7Mappings.Get(strMappingSelected); // Kan være 'null' hvis mappingen ikke finnes.

            // -- Hvis det finnes kun en mapping, sett denne som selected. --
            if ((this.mappingSelected == null) && (this.HL7Mappings.Count == 1))
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
        public HL7Viewer.DataModel.Msg.MsgNode msgRootnode { get; set; } = new MsgNode(0);


        public HL7SegmentCategories _HL7SegmentCategories { get; set; } = new HL7SegmentCategories();

        public FileInfo MsgFile { get; set; }

        public FileInfo Fi { get; set; }

        public string Content { get; set; }

        private HL7MappingSegments _HL7Segments { get; set; } = new HL7MappingSegments();

        public FileInfo MappingFileFi { get; set; }
        //        private const string MappingFileName = "Mapping_HL7.csv";

        #region -- Default settings --
        private const string DEFAULT_MAPPINGFOLDE_NAME = "Datamodel";
        private const string MAPPINGFILE_EXT = ".csv";
        #endregion -- Default settings --

        private char[] SEPARATOR_LEVEL_0 = new char[] { '\n' };
        private char[] SEPARATOR_LEVEL_1 = new char[] { '|' };
        private char[] SEPARATOR_LEVEL_2 = new char[] { '^' }; //, '~' };
        private char[] SEPARATOR_LEVEL_3 = new char[] { '~' };

        private const string MSG_NOT_INCLUDED_IN_MAPPING = "(*)";


        public HL7()
        {
            this.msgRootnode.Name = "RootNode";

            FileInfo fiApplication = new FileInfo(Application.ExecutablePath);
            DirectoryInfo di = new DirectoryInfo(Path.Combine(fiApplication.DirectoryName, DEFAULT_MAPPINGFOLDE_NAME));
            this.HL7Mappings = new HL7Mappings(di, MAPPINGFILE_EXT);

            SetSelectedMappingFromDefaultProperties();

            #region -- Slettes --
            //// -- Finn valgte mapping vha Properties.Settings.Default --
            //string strMappingSelected = null;
            //try
            //{
            //    strMappingSelected = Properties.Settings.Default.LastMappingSelected2;
            //}
            //catch (Exception ex)
            //{
            //    // -- Property ikke funnet. strMappingSelected er allerede satt lik null
            //}

            //if (!String.IsNullOrEmpty(strMappingSelected))
            //{
            //    try
            //    {
            //        FileInfo SelectedMappingFi = new FileInfo(strMappingSelected);
            //        if (this.HL7Mappings.Contains(SelectedMappingFi))
            //        {
            //            this.MappingSelected = this.HL7Mappings.Get(new FileInfo(strMappingSelected));
            //        }
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Ingen mapping valgt", "Importer mappingfiler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            //else
            //{
            //    this.mappingSelected = null;
            //}
            #endregion -- Slettes --
        }


        /// <summary>
        /// Importerer og parser
        /// </summary>
        /// <param name="fi"></param>
        public void ImportHL7MsgFile(FileInfo fi)
        {
            StreamReader sr = new StreamReader(fi.FullName, Encoding.GetEncoding(1252));

            string str = sr.ReadToEnd();
            sr.Close();
            ImportHL7MsgFile2(str);
        }


        public void ImportHL7MsgFile2(string strFileContent)
        {
            _HL7SegmentCategories = new HL7SegmentCategories();

            #region -- Import mapping --
            FileInfo executableFi = new FileInfo(Application.ExecutablePath); // Hent mappingfilen fra programfolderen.
                                                                              //MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
                                                                              //MappingSelected.ImportMapping(MappingFileFi);
            #endregion -- Import mapping --


            this.msgRootnode = new MsgNode("Root", strFileContent, 0, 0);

            msgRootnode.CreateChildNodes_L1(SEPARATOR_LEVEL_0); //, false);

            // Setter Name og ekstraherer SourceString for nodene i nivå 0
            foreach (MsgNode childnode in msgRootnode.Children)
            {
                childnode.ExtractNameAndSourceStringFirstLevel(SEPARATOR_LEVEL_1);
            }

            foreach (MsgNode subNode_L0 in msgRootnode.Children)
            {
                // -- Parse subnodes level 1 --
                subNode_L0.CreateChildNodes_L2(SEPARATOR_LEVEL_1); //, true); //, false);
                                                                   //subNode_L0.Level = 2;
                foreach (MsgNode subNode_L1 in subNode_L0.Children)
                {
                    if (
                        (subNode_L1.Value.Split(SEPARATOR_LEVEL_3).Length > 1)  // Inneholder skilletegn for repeterende noder  '~'
                        && (!(subNode_L1.MappingSectionName == "MSH" && subNode_L1.Level == 1 && subNode_L1.Index_L1 == 1 && subNode_L1.Index_L2 == 0)) //Hopp over node MSH 0,1. Den lister opp skilletegn
                        )
                    {
                        subNode_L1.CreateChildNodes_L4(SEPARATOR_LEVEL_2, SEPARATOR_LEVEL_3);
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
            foreach (MsgNode childnode in this.msgRootnode.Children)
            {
                childnode.MappingSegment = MappingSelected.GetSegmentFromSection(childnode.MappingSectionName, childnode.Index_L2, childnode.Index_L2);
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
            }
        }

        private HL7MappingSegments CreateSubSegments(string substringSource, HL7MappingSegment parentSegment, string segmentName)
        {
            HL7MappingSegments subsegments = new HL7MappingSegments();
            // Finne subsegmenter i fields[i]
            string[] subFields = substringSource.Split(SEPARATOR_LEVEL_1); // <-- Sjekk

            if (subFields.Length > 1)
            {
                for (int index_L2 = 0; index_L2 < subFields.Length; index_L2++)
                {
                    string value = subFields[index_L2];
                    HL7MappingSegment subsegment = new HL7MappingSegment(parentSegment.SectionName, segmentName, parentSegment.Index_L1, index_L2 + 1);
                    subsegment.ParentSegment = parentSegment;
                    subsegments.Add(subsegment);
                }
            }
            return subsegments;
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
    }
}
