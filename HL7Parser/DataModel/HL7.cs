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

        /// <summary>
        /// Mapping med alle felter fra spec. Brukes som oppslag ved import av HL7 fil.
        /// </summary>
        public Hl7Mapping Mapping { get; set; } = new Hl7Mapping();


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
        private const string MappingFileName = "Mapping_HL7.csv";


        private char[] SEPARATOR_LEVEL_0 = new char[] { '\n' };
        private char[] SEPARATOR_LEVEL_1 = new char[] { '|' };
        private char[] SEPARATOR_LEVEL_2 = new char[] { '^', '~' };
        private char[] SEPARATOR_LEVEL_3 = new char[] { '~' };

        private const string MSG_NOT_INCLUDED_IN_MAPPING = "(*)";


        public HL7()
        {
            this.msgRootnode.Name = "RootNode";
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
            MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
            Mapping.ImportMapping(MappingFileFi);
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
                    subNode_L1.CreateChildNodes_L3(SEPARATOR_LEVEL_2); //, true, false);
                    //subNode_L1.Level = 3;
                    //subNode_L1.Index_L2 = subNode_L0.Index_L2;
                }
            }


            // -- Matcher mot mapping --
            foreach (MsgNode childnode in this.msgRootnode.Children)
            {
                childnode.MappingSegment = Mapping.GetSegmentFromSection(childnode.MappingSectionName, childnode.Index_L2, childnode.Index_L2);
                MatchMsgNodeToMappingRecursive(childnode);
            }
        }


        private void MatchMsgNodeToMappingRecursive(MsgNode node)
        {
            foreach (MsgNode childnode in node.Children)
            {
                childnode.MappingSegment = Mapping.GetSegmentFromSection(childnode.MappingSectionName, childnode.Index_L2, childnode.Index_L2);
                MatchMsgNodeToMappingRecursive(childnode);
            }
        }

        private HL7MappingSegments CreateSubSegments(string substringSource, HL7MappingSegmentBase parentSegment, string segmentName)
        {
            HL7MappingSegments subsegments = new HL7MappingSegments();
            // Finne subsegmenter i fields[i]
            string[] subFields = substringSource.Split(SEPARATOR_LEVEL_1); // <-- Sjekk

            if (subFields.Length > 1)
            {
                for (int subindex = 0; subindex < subFields.Length; subindex++)
                {
                    string value = subFields[subindex];
                    HL7MappingSegmentString subsegment = new HL7MappingSegmentString(segmentName, value, parentSegment.Index_L1, subindex + 1);
                    subsegment.ParentSegment = parentSegment;
                    subsegment.SectionName = parentSegment.SectionName;
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
                foreach (HL7MappingSegmentString segment in _HL7Segments)
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
