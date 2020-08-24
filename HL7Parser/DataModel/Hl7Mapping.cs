using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class Hl7Mapping
    {
        public string Name { get; set; }
        public string VersionInfo { get; set; }

        /// <summary>
        /// Liste med navn på hovedseksjonene i mappingen. F.eks MSH, PID osv.
        /// </summary>
        public List<string> SectionNames { get; set; } = new List<string>();

        public HL7Segments _HL7Segments { get; set; } = new HL7Segments();



        // Parts of input files.
        private const int INDEX_SECTION = 0;
        private const int INDEX_INDEX = 1;
        private const int INDEX_SUBINDEX = 2;
        //private const int INDEX_LENGTH = 3; // obsolete
        //private const int INDEX_DT = 4;// obsolete
        //private const int INDEX_OPT = 5;// obsolete
        //private const int INDEX_RP = 6;// obsolete

        private const int INDEX_COLLAPSED_DEFAULT = 3;
        private const int INDEX_NAME = 4;


        public void ImportMapping(FileInfo fi)
        {
            using (StreamReader sr = new StreamReader(fi.FullName, Encoding.ASCII))
            {
                string str = String.Empty;
                HL7SegmentString parentSegment = null;
                _HL7Segments = new HL7Segments();

                while (!sr.EndOfStream)
                {
                    str = sr.ReadLine();
                    string[] fields = str.Split(new char[] { '\t' });
                    //for (int i = 0; i < fields.Length; i++)
                    {
                        if (String.IsNullOrEmpty(fields[INDEX_SECTION]))
                        {
                            continue; // Do not save segments with empty names
                        }

                        if (fields[INDEX_SECTION].Contains("//"))
                        {
                            continue;
                        }


                        HL7SegmentString segment = new HL7SegmentString();
                        segment.SectionName = fields[INDEX_SECTION];

                        int.TryParse(fields[INDEX_INDEX], out int indexTmp);
                        segment.Index = indexTmp;

                        int.TryParse(fields[INDEX_SUBINDEX], out int subIndexTmp);
                        segment.SubIndex = subIndexTmp;

                        //int.TryParse(fields[INDEX_LENGTH], out int lengthTmp);
                        //segment.Length = lengthTmp;

                        //segment.Dt = fields[INDEX_DT];
                        //segment.Opt = fields[INDEX_OPT];
                        //segment.Rp = fields[INDEX_RP];

                        if (fields[INDEX_COLLAPSED_DEFAULT].ToUpper() == "Y")
                        {
                            segment.CollapsedDefault = true;
                        }

                        segment.SegmentName = fields[INDEX_NAME];

                        // -- Håndtering av parent/subsegment --
                        if (segment.SubIndex <= 0)
                        {
                            // Current segment er på nivå 1
                            parentSegment = segment; // 'parentSegment' settes på parent nivå, og hentes for subsegmenter.
                            _HL7Segments.Add(segment);
                        }
                        else
                        {
                            // Segmentet er et subsegment. Parentsegment ble satt i FORRIGE iterasjon.
                            parentSegment.SubSegments.Add(segment);
                        }
                    }
                   
                }
                sr.Close();
                PopulateListOfSectionNames();
            }
        }


        /// <summary>
        /// Scan gjennom alle segmentnavn i den innleste mappingen og ta vare på unike verdiee.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public List<string> PopulateListOfSectionNames()
        {
            //List<string> sectionNames = new List<string>(); -> Endret til public property

            foreach (HL7SegmentBase segment in this._HL7Segments)
            {
                if ((!SectionNames.Contains(segment.SectionName)) && (!String.IsNullOrEmpty(segment.SectionName)))
                {
                    SectionNames.Add(segment.SectionName);
                }
            }
            return SectionNames;
        }
    }
}
