using HL7Viewer.DataModel.Msg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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


            this.msgRootnode = new MsgNode("Root", strFileContent, 1, 0);

            msgRootnode.CreateChildNodes_L1(SEPARATOR_LEVEL_0, true); //, false);

            // Setter Name og ekstraherer SourceString for nodene i nivå 0
            foreach (MsgNode childnode in msgRootnode.Children)
            {
                childnode.ExtractNameAndSourceStringFirstLevel(SEPARATOR_LEVEL_1);
                childnode.Level = 1;
            }

            foreach (MsgNode subNode_L0 in msgRootnode.Children)
            {
                // -- Parse subnodes level 1 --
                subNode_L0.CreateChildNodes_L2(SEPARATOR_LEVEL_1, true); //, false);
                subNode_L0.Level = 2;
                foreach (MsgNode subNode_L1 in subNode_L0.Children)
                {
                    subNode_L1.CreateChildNodes_L3(SEPARATOR_LEVEL_2, true, false);
                    subNode_L1.Level = 3;
                    subNode_L1.Index_L2 = subNode_L0.Index_L2;
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



        /// <summary>
        /// Importerer og parser msg som string.
        /// </summary>
        /// <param name="str"></param>
        //public void ImportHL7MsgFile(string str)
        //{
        //    try
        //    {
        //        _HL7SegmentCategories = new HL7SegmentCategories();

        //        #region -- Import mapping --
        //        FileInfo executableFi = new FileInfo(Application.ExecutablePath); // Hent mappingfilen fra programfolderen.
        //        MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
        //        Mapping.ImportMapping(MappingFileFi);
        //        #endregion -- Import mapping --


        //        // -- Søker gjennom meldingsfilen på nytt for å finne evt seksjoner som ikke ligger i mappingfilen. --
        //        string[] lines = str.Split(new char[] { '\r' });
        //        foreach (string line in lines)
        //        {
        //            string sectionNameTmp = GetSectionNameFromSourceString(line);
        //            if (sectionNameTmp != null)
        //            {
        //                if (!Mapping.SectionNames.Contains(sectionNameTmp))
        //                {
        //                    // -- utkommentert. Signatur endret  HL7SegmentCategory newCategory = new HL7SegmentCategory(sectionNameTmp, true);
        //                    //                            Mapping.SectionNames.Add  <<<--- Fortsett her!
        //                    //Mapping._HL7Segments.Add(new HL7SegmentCategory(sectionNameTmp, true));
        //                }
        //            }
        //        }

        //        #region -- Parser meldingsfilen til sectionpairs --

        //        // -- Finner hoveddelene av filen, F.eks MSH, PV1, PID, OBR osv --
        //        //List<string> sectionNames = Mapping.GetSectionNames();
        //        foreach (string sectionName in Mapping.SectionNames)
        //        {
        //            HL7SegmentCategory category = new HL7SegmentCategory();
        //            category.CategoryName = sectionName;
        //            _HL7SegmentCategories.Add(category);
        //        }

        //        // -- Kun section names fra mappingen i første omgang --
        //        SectionIndexPairs sectionPairs = new SectionIndexPairs();

        //        // -- Finner startpos for hver av seksjonene i HL7 meldingen. --
        //        // Søker gjennom meldingsfilen og finnes start for hver av seksjonene i mappingen.
        //        // Bruker SectionIndexPairs
        //        foreach (string sectionName in Mapping.SectionNames)
        //        {
        //            string key = sectionName + "|";
        //            int pos = str.IndexOf(key);
        //            //pair.startIndex = pos;

        //            if (pos >= 0)
        //            {
        //                SectionIndexPair pair = new SectionIndexPair(sectionName, pos + key.Length - 1);   // Skilletegnet etter sectionname skal inkluderes!
        //                sectionPairs.Add(pair);
        //            }
        //            else
        //            {
        //                // Meldingsfilen inneholder ikke den aktuelle seksjonen.
        //            }
        //        }




        //        sectionPairs.Sort();
        //        #endregion -- Parser meldingsfilen til sectionpairs --

        //        #region -- Splitter innholdet i HL7 filen i respektive sectionPair, og legger inn hver del i .SourceString --
        //        for (int i = 0; i < sectionPairs.Count; i++)
        //        {
        //            SectionIndexPair sectionPair = sectionPairs[i]; // Oppretter tmp variabel for enklere å vise properties i debugging.
        //            SectionIndexPair sectionPairNext = sectionPairs[i + 1]; // Oppretter tmp variabel for enklere å vise properties i debugging.

        //            int posStart = sectionPair.startPos;

        //            if (i < sectionPairs.Count - 1)
        //            {
        //                int length = sectionPairNext.startPos - sectionPairNext.Name.Length - 1 - posStart;  // - 1 er for skilletegnet etter section navnet.
        //                sectionPair.SourceString = str.Substring(posStart, length);  // - Legger Sectionname for i sourceString for å justere mot feltindex 0 for seksjonen i mappingen.

        //                sectionPair.SourceString = sectionPair.SourceString.Trim();

        //                // Legger inn én ekstra | fordi feltet separators  inneholder en separator i feltverdien.
        //                if (sectionPair.Name == "MSH")
        //                {
        //                    sectionPair.SourceString = sectionPair.Name + "|" + sectionPair.SourceString;
        //                }
        //            }
        //            else
        //            {
        //                sectionPair.SourceString = str.Substring(posStart);
        //            }
        //        }
        //        #endregion -- Splitter innholdet i HL7 filen i respektive sectionPair, og legger inn hver del i .SourceString --


        //        #region -- Importerer segmentene i nivå 1 --

        //        HL7SegmentCategory currentCategory = null;

        //        foreach (SectionIndexPair sectPair in sectionPairs)
        //        {
        //            // Bruker Sectionpair.Name til å finne currentCategory
        //            // Slår opp segmentnavnet i categories. Bruker denne som current category hvis den finnes.
        //            HL7SegmentCategory catTmp = _HL7SegmentCategories.Get(sectPair.Name);
        //            if (catTmp != null)
        //            {
        //                currentCategory = catTmp;
        //            }

        //            // Importer i første omgang alle segmenter som string
        //            string[] fieldsSegments = sectPair.SourceString.Split(SEPARATOR_LEVEL_1);
        //            for (int index = 1; index < fieldsSegments.Length; index++)
        //            {
        //                string value = fieldsSegments[index];
        //                // Opprett segmentet
        //                HL7SegmentString segment = (HL7SegmentString)Mapping._HL7Segments.GetSegment(sectPair.Name, index, 0);


        //                if (segment == null)
        //                {
        //                    segment = new HL7SegmentString(sectPair.Name + MSG_NOT_INCLUDED_IN_MAPPING, value, index, 0);
        //                    segment._HL7SegmentCategory = currentCategory;
        //                    Mapping._HL7Segments.Add(segment);
        //                }
        //                else
        //                {
        //                    segment._HL7SegmentCategory = currentCategory;
        //                    //segment.Value = sectPair.Name;
        //                    currentCategory._HL7Segments.Add(segment);
        //                    //_HL7Segments.Add(segment);
        //                    segment.Value = value;

        //                    // Erstatter "" med tom string
        //                    if (segment.Value == "\"\"")
        //                    {
        //                        segment.Value = "";
        //                    }

        //                    // Bruker meldingsinnholdet til å opprette midlertidige subsegmenter. Slår opp i 
        //                    // eksisterende segmenter for å se om de allerede eksisterer
        //                    HL7Segments subSegmentsTmp = CreateSubSegments(value, segment, segment.SegmentName);
        //                    int subindex = 1;
        //                    foreach (HL7SegmentString subsegmentTmp in subSegmentsTmp)
        //                    {
        //                        // Erstatter "" med tom string
        //                        if (subsegmentTmp.Value == "\"\"")
        //                        {
        //                            subsegmentTmp.Value = "";
        //                        }

        //                        HL7SegmentString subsegment = (HL7SegmentString)Mapping._HL7Segments.GetSegment(currentCategory.CategoryName, subsegmentTmp.Index, subsegmentTmp.SubIndex);

        //                        if (subsegment != null)
        //                        {
        //                            subsegment.Value = subsegmentTmp.Value;
        //                        }
        //                        else
        //                        {
        //                            //subsegment = new HL7SegmentString(segment.SegmentName + " <unmapped subsegment>", value, segment.Index, subindex);
        //                            // Subsegmentet eksisterer ikke i den importerte mappingen. Oppretter nytt segment. Legger til '<missing in mapping>' i segmentnavnet.
        //                            subsegmentTmp.SegmentName += MSG_NOT_INCLUDED_IN_MAPPING;
        //                            subsegmentTmp.Value = subsegmentTmp.Value;
        //                            subsegmentTmp.ParentSegment = segment;
        //                            segment.SubSegments.Add(subsegmentTmp);
        //                        }
        //                        subindex++;
        //                    }

        //                }
        //            }
        //        }
        //        #endregion -- Importerer segmentene i nivå 1 --

        //        // -- Separere HL7 meldingen under respektive segmentnavn. --
        //        // PopulateCategories();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Kan ikke vise meldingen. \r\nFeilmelding:" + ex.Message + "\r\n\n" + ex.StackTrace);
        //    }
        //}




        /// <summary>
        /// Finner Section name fra en linge i meldingsfilen. Returnerer null hvis den ikke finner section name i linjen. 
        /// </summary>
        /// <param name="line">linje fra meldingsfilen</param>
        /// <returns></returns>
        private string GetSectionNameFromSourceString(string line, char[] separator)
        {
            if (line == null)
            { return null; }

            string[] strTmp = line.Split(separator);
            if (strTmp.Length > 0)
            {
                return strTmp[0];
            }
            else return null;
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
                    HL7MappingSegmentString subsegment = new HL7MappingSegmentString(segmentName, value, parentSegment.Index, subindex + 1);
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
