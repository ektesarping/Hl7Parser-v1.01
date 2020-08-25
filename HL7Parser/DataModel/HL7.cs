﻿using System;
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

        public HL7SegmentCategories _HL7SegmentCategories { get; set; } = new HL7SegmentCategories();

        public FileInfo MsgFile { get; set; }

        public FileInfo Fi { get; set; }

        public string Content { get; set; }

        private HL7Segments _HL7Segments { get; set; } = new HL7Segments();

        public FileInfo MappingFileFi { get; set; }
        private const string MappingFileName = "Mapping_HL7.csv";


        private char[] SEPARATOR_SECTIONS = new char[] { '|' };
        private char[] SEPARATOR_SUBSECTIONS = new char[] { '^', '~' };

        //private char[] SEPARATOR_SUBSECTIONS_REPEAT = new char[] { '~' };
        private const string MSG_NOT_INCLUDED_IN_MAPPING = "(*)";


        public HL7()
        { }


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


        public void ImportHL7MsgFile2(string str)
        {
            _HL7SegmentCategories = new HL7SegmentCategories();

            #region -- Import mapping --
            FileInfo executableFi = new FileInfo(Application.ExecutablePath); // Hent mappingfilen fra programfolderen.
            MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
            Mapping.ImportMapping(MappingFileFi);
            #endregion -- Import mapping --


            // -- Leser inn meldingsfilen, linje for linje. --
            this._HL7SegmentCategories = new HL7SegmentCategories();

            //// -- Søker gjennom meldingsfilen på nytt for å finne evt seksjoner som ikke ligger i mappingfilen. --


            //// -- Finner hoveddelene av filen, F.eks MSH, PV1, PID, OBR osv --
            ////List<string> sectionNames = Mapping.GetSectionNames();
            //foreach (string sectionName in Mapping.SectionNames)
            //{
            //    HL7SegmentCategory category = new HL7SegmentCategory();
            //    category.CategoryName = sectionName;
            //    _HL7SegmentCategories.Add(category);
            //}

            #region -- Parser meldingsfilen til sectionpairs --

            // -- Opprettet sectionpairs fra meldingen --
            SectionIndexPairs sectionPairs = new SectionIndexPairs();

            // -- Oppretter sectionpairs ut for hver av linjene i meldingen. --
            string[] lines = str.Split(new char[] { '\r' });
            foreach (string line in lines)
            {
                string sectionNameTmp = GetSectionNameFromMessageFile(line);
                if (sectionNameTmp != null)
                {
                    {
                        HL7SegmentCategory catTmp = new HL7SegmentCategory(sectionNameTmp);
                        this._HL7SegmentCategories.Add(catTmp);


                        string key = sectionNameTmp + "|";
                        int pos = line.IndexOf(key);
                        //pair.startIndex = pos;

                        //    if (pos >= 0)
                        //    {
                        SectionIndexPair pair = new SectionIndexPair(sectionNameTmp, pos + key.Length - 1);   // Skilletegnet etter sectionname skal inkluderes!
                        sectionPairs.Add(pair);
                        //    }
                    }
                }
            }

            #region -- Splitter innholdet i linjen i respektive sectionPair, og legger inn hver del i .SourceString --
            for (int i = 0; i < sectionPairs.Count; i++)
            {
                SectionIndexPair sectionPair = sectionPairs[i]; // Oppretter tmp variabel for enklere å vise properties i debugging.
                SectionIndexPair sectionPairNext = sectionPairs[i + 1]; // Oppretter tmp variabel for enklere å vise properties i debugging.

                int posStart = sectionPair.startPos;

                if (i < sectionPairs.Count - 1)
                {
                    int length = sectionPairNext.startPos - sectionPairNext.Name.Length - 1 - posStart;  // - 1 er for skilletegnet etter section navnet.
                    sectionPair.SourceString = str.Substring(posStart, length);  // - Legger Sectionname for i sourceString for å justere mot feltindex 0 for seksjonen i mappingen.

                    sectionPair.SourceString = sectionPair.SourceString.Trim();

                    // Legger inn én ekstra | fordi feltet separators  inneholder en separator i feltverdien.
                    if (sectionPair.Name == "MSH")
                    {
                        sectionPair.SourceString = sectionPair.Name + "|" + sectionPair.SourceString;
                    }
                }
                else
                {
                    sectionPair.SourceString = str.Substring(posStart);
                }
            }
            #endregion -- Splitter innholdet i HL7 filen i respektive sectionPair, og legger inn hver del i .SourceString --



            // -- Finner startpos for hver av seksjonene i HL7 meldingen. --
            // Søker gjennom meldingsfilen og finnes start for hver av seksjonene i mappingen.
            // Bruker SectionIndexPairs
            foreach (string sectionName in Mapping.SectionNames)
            {
                string key = sectionName + "|";
                int pos = str.IndexOf(key);
                //pair.startIndex = pos;

                //    if (pos >= 0)
                //    {
                SectionIndexPair pair = new SectionIndexPair(sectionName, pos + key.Length - 1);   // Skilletegnet etter sectionname skal inkluderes!
                sectionPairs.Add(pair);
                //    }
                //    else
                //    {
                //        // Meldingsfilen inneholder ikke den aktuelle seksjonen.
                //    }
            }

            sectionPairs.Sort();
            #endregion -- Parser meldingsfilen til sectionpairs --


        }





        /// <summary>
        /// Importerer og parser msg som string.
        /// </summary>
        /// <param name="str"></param>
        public void ImportHL7MsgFile(string str)
        {
            try
            {
                _HL7SegmentCategories = new HL7SegmentCategories();

                #region -- Import mapping --
                FileInfo executableFi = new FileInfo(Application.ExecutablePath); // Hent mappingfilen fra programfolderen.
                MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
                Mapping.ImportMapping(MappingFileFi);
                #endregion -- Import mapping --


                // -- Søker gjennom meldingsfilen på nytt for å finne evt seksjoner som ikke ligger i mappingfilen. --
                string[] lines = str.Split(new char[] { '\r' });
                foreach (string line in lines)
                {
                    string sectionNameTmp = GetSectionNameFromMessageFile(line);
                    if (sectionNameTmp != null)
                    {
                        if (!Mapping.SectionNames.Contains(sectionNameTmp))
                        {
                            // -- utkommentert. Signatur endret  HL7SegmentCategory newCategory = new HL7SegmentCategory(sectionNameTmp, true);
                            //                            Mapping.SectionNames.Add  <<<--- Fortsett her!
                            //Mapping._HL7Segments.Add(new HL7SegmentCategory(sectionNameTmp, true));
                        }
                    }
                }

                #region -- Parser meldingsfilen til sectionpairs --

                // -- Finner hoveddelene av filen, F.eks MSH, PV1, PID, OBR osv --
                //List<string> sectionNames = Mapping.GetSectionNames();
                foreach (string sectionName in Mapping.SectionNames)
                {
                    HL7SegmentCategory category = new HL7SegmentCategory();
                    category.CategoryName = sectionName;
                    _HL7SegmentCategories.Add(category);
                }

                // -- Kun section names fra mappingen i første omgang --
                SectionIndexPairs sectionPairs = new SectionIndexPairs();

                // -- Finner startpos for hver av seksjonene i HL7 meldingen. --
                // Søker gjennom meldingsfilen og finnes start for hver av seksjonene i mappingen.
                // Bruker SectionIndexPairs
                foreach (string sectionName in Mapping.SectionNames)
                {
                    string key = sectionName + "|";
                    int pos = str.IndexOf(key);
                    //pair.startIndex = pos;

                    if (pos >= 0)
                    {
                        SectionIndexPair pair = new SectionIndexPair(sectionName, pos + key.Length - 1);   // Skilletegnet etter sectionname skal inkluderes!
                        sectionPairs.Add(pair);
                    }
                    else
                    {
                        // Meldingsfilen inneholder ikke den aktuelle seksjonen.
                    }
                }




                sectionPairs.Sort();
                #endregion -- Parser meldingsfilen til sectionpairs --

                #region -- Splitter innholdet i HL7 filen i respektive sectionPair, og legger inn hver del i .SourceString --
                for (int i = 0; i < sectionPairs.Count; i++)
                {
                    SectionIndexPair sectionPair = sectionPairs[i]; // Oppretter tmp variabel for enklere å vise properties i debugging.
                    SectionIndexPair sectionPairNext = sectionPairs[i + 1]; // Oppretter tmp variabel for enklere å vise properties i debugging.

                    int posStart = sectionPair.startPos;

                    if (i < sectionPairs.Count - 1)
                    {
                        int length = sectionPairNext.startPos - sectionPairNext.Name.Length - 1 - posStart;  // - 1 er for skilletegnet etter section navnet.
                        sectionPair.SourceString = str.Substring(posStart, length);  // - Legger Sectionname for i sourceString for å justere mot feltindex 0 for seksjonen i mappingen.

                        sectionPair.SourceString = sectionPair.SourceString.Trim();

                        // Legger inn én ekstra | fordi feltet separators  inneholder en separator i feltverdien.
                        if (sectionPair.Name == "MSH")
                        {
                            sectionPair.SourceString = sectionPair.Name + "|" + sectionPair.SourceString;
                        }
                    }
                    else
                    {
                        sectionPair.SourceString = str.Substring(posStart);
                    }
                }
                #endregion -- Splitter innholdet i HL7 filen i respektive sectionPair, og legger inn hver del i .SourceString --


                #region -- Importerer segmentene i nivå 1 --

                HL7SegmentCategory currentCategory = null;

                foreach (SectionIndexPair sectPair in sectionPairs)
                {
                    // Bruker Sectionpair.Name til å finne currentCategory
                    // Slår opp segmentnavnet i categories. Bruker denne som current category hvis den finnes.
                    HL7SegmentCategory catTmp = _HL7SegmentCategories.Get(sectPair.Name);
                    if (catTmp != null)
                    {
                        currentCategory = catTmp;
                    }

                    // Importer i første omgang alle segmenter som string
                    string[] fieldsSegments = sectPair.SourceString.Split(SEPARATOR_SECTIONS);
                    for (int index = 1; index < fieldsSegments.Length; index++)
                    {
                        string value = fieldsSegments[index];
                        // Opprett segmentet
                        HL7SegmentString segment = (HL7SegmentString)Mapping._HL7Segments.GetSegment(sectPair.Name, index, 0);


                        if (segment == null)
                        {
                            segment = new HL7SegmentString(sectPair.Name + MSG_NOT_INCLUDED_IN_MAPPING, value, index, 0);
                            segment._HL7SegmentCategory = currentCategory;
                            Mapping._HL7Segments.Add(segment);
                        }
                        else
                        {
                            segment._HL7SegmentCategory = currentCategory;
                            //segment.Value = sectPair.Name;
                            currentCategory._HL7Segments.Add(segment);
                            //_HL7Segments.Add(segment);
                            segment.Value = value;

                            // Erstatter "" med tom string
                            if (segment.Value == "\"\"")
                            {
                                segment.Value = "";
                            }

                            // Bruker meldingsinnholdet til å opprette midlertidige subsegmenter. Slår opp i 
                            // eksisterende segmenter for å se om de allerede eksisterer
                            HL7Segments subSegmentsTmp = CreateSubSegments(value, segment, segment.SegmentName);
                            int subindex = 1;
                            foreach (HL7SegmentString subsegmentTmp in subSegmentsTmp)
                            {
                                // Erstatter "" med tom string
                                if (subsegmentTmp.Value == "\"\"")
                                {
                                    subsegmentTmp.Value = "";
                                }

                                HL7SegmentString subsegment = (HL7SegmentString)Mapping._HL7Segments.GetSegment(currentCategory.CategoryName, subsegmentTmp.Index, subsegmentTmp.SubIndex);

                                if (subsegment != null)
                                {
                                    subsegment.Value = subsegmentTmp.Value;
                                }
                                else
                                {
                                    //subsegment = new HL7SegmentString(segment.SegmentName + " <unmapped subsegment>", value, segment.Index, subindex);
                                    // Subsegmentet eksisterer ikke i den importerte mappingen. Oppretter nytt segment. Legger til '<missing in mapping>' i segmentnavnet.
                                    subsegmentTmp.SegmentName += MSG_NOT_INCLUDED_IN_MAPPING;
                                    subsegmentTmp.Value = subsegmentTmp.Value;
                                    subsegmentTmp.ParentSegment = segment;
                                    segment.SubSegments.Add(subsegmentTmp);
                                }
                                subindex++;
                            }

                        }
                    }
                }
                #endregion -- Importerer segmentene i nivå 1 --

                // -- Separere HL7 meldingen under respektive segmentnavn. --
                // PopulateCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kan ikke vise meldingen. \r\nFeilmelding:" + ex.Message + "\r\n\n" + ex.StackTrace);
            }
        }




        /// <summary>
        /// Finner Section name fra en linge i meldingsfilen. Returnerer null hvis den ikke finner section name i linjen. 
        /// </summary>
        /// <param name="line">linje fra meldingsfilen</param>
        /// <returns></returns>
        private string GetSectionNameFromMessageFile(string line)
        {
            if (line == null)
            { return null; }

            string[] strTmp = line.Split(SEPARATOR_SECTIONS);
            if (strTmp.Length > 0)
            {
                return strTmp[0];
            }
            else return null;
        }

        private HL7Segments CreateSubSegments(string substringSource, HL7SegmentBase parentSegment, string segmentName)
        {
            HL7Segments subsegments = new HL7Segments();
            // Finne subsegmenter i fields[i]
            string[] subFields = substringSource.Split(SEPARATOR_SUBSECTIONS);

            if (subFields.Length > 1)
            {
                for (int subindex = 0; subindex < subFields.Length; subindex++)
                {
                    string value = subFields[subindex];
                    HL7SegmentString subsegment = new HL7SegmentString(segmentName, value, parentSegment.Index, subindex + 1);
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
                foreach (HL7SegmentString segment in _HL7Segments)
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
