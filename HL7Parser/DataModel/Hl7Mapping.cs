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

        public Hl7MappingSections Hl7MappingSections { get; set; } = new Hl7MappingSections();

        public HL7MappingSegments _HL7Segments { get; set; } = new HL7MappingSegments();

        /// <summary>
        /// Viser navnet på mappingen, f.eks Røntgensvar, Labsvar etc.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Full path til mappingfilen.
        /// </summary>
        public string  MappingFileFullPath { get; set; }


        // Parts of input files.
        private const int INDEX_SECTION = 0;
        private const int INDEX_INDEX_L1 = 1;
        private const int INDEX_INDEX_L2 = 2;
        //private const int INDEX_LENGTH = 3; // obsolete
        //private const int INDEX_DT = 4;// obsolete
        //private const int INDEX_OPT = 5;// obsolete
        //private const int INDEX_RP = 6;// obsolete

        private const int INDEX_COLLAPSED_DEFAULT = 3;
        private const int INDEX_NAME = 4;

        private const string DISPLAY_NAME = "DISPLAYNAME";

        public void ImportMapping(FileInfo fi)
        {
            using (StreamReader sr = new StreamReader(fi.FullName, Encoding.ASCII))
            {
                string str = String.Empty;
                HL7MappingSegmentString parentSegment = null;
                _HL7Segments = new HL7MappingSegments();
                this.MappingFileFullPath = fi.FullName;

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

                        if (fields[INDEX_SECTION].ToUpper().Contains(DISPLAY_NAME))
                        {
                            DisplayName = fields[1];
                            continue;
                        }

                        // -- Parse feltene i segmentet --
                        HL7MappingSegmentString segment = new HL7MappingSegmentString();
                        segment.SectionName = fields[INDEX_SECTION];

                        int.TryParse(fields[INDEX_INDEX_L1], out int index_LTmp);
                        segment.Index_L1 = index_LTmp;

                        int.TryParse(fields[INDEX_INDEX_L2], out int index_L2Tmp);
                        segment.Index_L2 = index_L2Tmp;


                        if (fields[INDEX_COLLAPSED_DEFAULT].ToUpper() == "Y")
                        {
                            segment.CollapsedDefault = true;
                        }

                        segment.SegmentName = fields[INDEX_NAME];


                        // -- Legger til section hvis den ikke allerede finnes --
                        // Oppretter section hvis en ikke finnes
                        Hl7MappingSection mappingSectionTmp = this.Hl7MappingSections.Get(segment.SectionName);
                        if (mappingSectionTmp == null)
                        {
                            this.Hl7MappingSections.Add(segment.SectionName);
                        }
                        // Legger til som subsegment i mappingsection --
                        segment.MappingSection = mappingSectionTmp;
                        segment.MappingSection.Segments.Add(segment);

                        // -- Håndtering av parent/subsegment --
                        if (segment.Index_L2 <= 0)
                        {
                            // Current segment er på nivå 1
                            segment.Level = 1;
                            _HL7Segments.Add(segment);
                            segment.ParentSegment = segment.MappingSection.Segment;
                        }
                        else
                        {
                            // Segmentet er et subsegment. Parentsegment ble satt i FORRIGE iterasjon.
                            segment.Level = 2;

                            // Legger til parent segment.
                            HL7MappingSegmentString parentSegmentTmp = segment.MappingSection.Segments.GetSegment(segment.SectionName, segment.Index_L1, segment.Index_L2);
                            segment.ParentSegment = parentSegmentTmp;
                            parentSegment.SubSegments.Add(segment);
                        }
                    }
                }
                sr.Close();
                // PopulateListOfSections();
            }
        }

        public HL7MappingSegmentString GetSegmentFromSection(string name, int index_L1, int index_L2)
        {
            Hl7MappingSection sectionTmp = this.Hl7MappingSections.Get(name);

            if (sectionTmp == null)
            {
                return null;
            }
            else
            {
                HL7MappingSegmentString segmentTmp = sectionTmp.Segments.GetSegment(name, index_L1, index_L2);
                return segmentTmp;
            }
        }




        /// <summary>
        /// Scan gjennom alle segmentnavn i den innleste mappingen og ta vare på unike verdiee.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public List<string> PopulateListOfSections()
        {
            //List<string> sectionNames = new List<string>(); -> Endret til public property

            foreach (HL7MappingSegmentBase segment in this._HL7Segments)
            {
                if ((!SectionNames.Contains(segment.SectionName)) && (!String.IsNullOrEmpty(segment.SectionName)))
                {
                    SectionNames.Add(segment.SectionName);
                    this.Hl7MappingSections.Add(segment.SectionName);
                }
            }
            return SectionNames;
        }
    }
}
