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
                HL7MappingSegmentString parentSegment = null;
                _HL7Segments = new HL7MappingSegments();

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

                        // -- Parse feltene i segmentet --
                        HL7MappingSegmentString segment = new HL7MappingSegmentString();
                        segment.SectionName = fields[INDEX_SECTION];

                        int.TryParse(fields[INDEX_INDEX], out int indexTmp);
                        segment.Index = indexTmp;

                        int.TryParse(fields[INDEX_SUBINDEX], out int subIndexTmp);
                        segment.SubIndex = subIndexTmp;


                        if (fields[INDEX_COLLAPSED_DEFAULT].ToUpper() == "Y")
                        {
                            segment.CollapsedDefault = true;
                        }

                        segment.SegmentName = fields[INDEX_NAME];

                        // -- Håndtering av parent/subsegment --
                        if (segment.SubIndex <= 0)
                        {

                            // Current segment er på nivå 1
                            segment.Level = 1;
                            parentSegment = segment; // 'parentSegment' settes på parent nivå, og hentes for subsegmenter.
                            _HL7Segments.Add(segment);

                            // -- Opprette section hvis ikke den allerede er opprettet --
                            this.Hl7MappingSections.Add(segment.SectionName);
                            // -- Legger til som subsegmenter i mappingsection --
                            Hl7MappingSection parentsection = this.Hl7MappingSections.Get(segment.SectionName);
                            segment.MappingSection = parentsection;
                            parentsection.Segments.Add(segment);

                        }
                        else
                        {
                            // Segmentet er et subsegment. Parentsegment ble satt i FORRIGE iterasjon.
                            segment.Level = 2;
                            parentSegment.SubSegments.Add(segment);
                        }
                    }
                }
                sr.Close();
                // PopulateListOfSections();
            }
        }

        public HL7MappingSegmentString GetSegmentFromSection(string name, int index, int subindex)
        {
            Hl7MappingSection sectionTmp = this.Hl7MappingSections.Get(name);

            if (sectionTmp == null)
            {
                return null;
            }
            else
            {
                HL7MappingSegmentString segmentTmp = sectionTmp.Segments.GetSegment(name, index, subindex);
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
