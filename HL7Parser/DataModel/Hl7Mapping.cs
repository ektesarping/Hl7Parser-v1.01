using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer.DataModel
{
    public class Hl7Mapping : IComparable
    {
        public string Name { get; set; }
        public string VersionInfo { get; set; }

        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Liste med navn på hovedseksjonene i mappingen. F.eks MSH, PID osv.
        /// </summary>
        public List<string> SectionNames { get; set; } = new List<string>();

        // 201009-01        public Hl7MappingSections Hl7MappingSections { get; set; } = new Hl7MappingSections();

        public HL7MappingSegments _HL7Segments { get; set; } = new HL7MappingSegments();

        public static string COMMENT_CHAR = "//";

        /// <summary>
        /// Viser navnet på mappingen, f.eks Røntgensvar, Labsvar etc.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Full path til mappingfilen.
        /// </summary>
        public string MappingFileFullPath { get; set; }


        // Parts of input files.
        private const int INDEX_SECTION = 0;
        private const int INDEX_INDEX_L1 = 1;
        private const int INDEX_INDEX_L2 = 2;
        //private const int INDEX_LENGTH = 3; // obsolete
        //private const int INDEX_DT = 4;// obsolete
        //private const int INDEX_OPT = 5;// obsolete
        //private const int INDEX_RP = 6;// obsolete

        private const int INDEX_COLLAPSED_DEFAULT = 3;
        private const int INDEX_DISPLAY_STYLE_VALUE = 4;
        private const int INDEX_CONSTRAINT = 5;
        private const int INDEX_NAME = 6;

        private const string DISPLAY_NAME = "DISPLAYNAME";
        private const string STR_COLLAPSE_DEFAULT = "Y";
        private const string STR_HIDE_VALUE = "H";
        private const string STR_BOLD_VALUE = "B";

        #region -- Constructor --
        public Hl7Mapping()
        { }

        public Hl7Mapping(FileInfo fi) : this()
        {
            this.FileInfo = fi;
            this.ImportMapping(fi);
        }
        #endregion  -- Constructor --

        //public void ImportMapping()
        //{
        //    this.ImportMapping(this.FileInfo);
        //}

        public Hl7Mapping ImportMapping(FileInfo fi)
        {
            if (!fi.Exists)
            {
                MessageBox.Show("Mappingfilen (oversikt over feltnavn):\r\n" + fi.FullName + "\r\nikke funnet. Ignorer mappingen", "Innlesing av mapping fil feilet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return this;
            }

            string str = String.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(fi.FullName, Encoding.ASCII))
                {
                    HL7MappingSegment parentSegment = null;
                    _HL7Segments = new HL7MappingSegments();
                    this.FileInfo = fi;
                    this.MappingFileFullPath = fi.FullName;

                    while (!sr.EndOfStream)
                    {
                        str = sr.ReadLine();

                        // -- Fjern kommentar --
                        str = HL7.TrimComment(str, COMMENT_CHAR);

                        string[] fields = str.Split(new char[] { '\t' });
                        {
                            string strTmp = GetFieldAsString(fields, INDEX_SECTION);
                            if (String.IsNullOrEmpty(strTmp))
                            {
                                continue; // Do not save segments with empty names
                            }


                            if (String.IsNullOrWhiteSpace(GetFieldAsString(fields, INDEX_SECTION)))
                            {
                                continue; // Do not save segments with empty names
                            }

                            if (GetFieldAsString(fields, INDEX_SECTION).ToUpper().Contains(DISPLAY_NAME))
                            {
                                DisplayName = GetFieldAsString(fields, 1);
                                continue;
                            }

                            // -- Parse feltene i segmentet --
                            HL7MappingSegment segment = new HL7MappingSegment();
                            segment.SectionName = GetFieldAsString(fields, INDEX_SECTION);

                            segment.Index_L1 = GetFieldAsint(fields, INDEX_INDEX_L1);

                            segment.Index_L2 = GetFieldAsint(fields, INDEX_INDEX_L2);

                            if (GetFieldAsString(fields, INDEX_COLLAPSED_DEFAULT).ToUpper() == STR_COLLAPSE_DEFAULT)
                            {
                                segment.CollapsedDefault = true;
                            }

                            // -- Styrer om verdien skal erstattes av *****
                            if (GetFieldAsString(fields, INDEX_DISPLAY_STYLE_VALUE).ToUpper() == STR_HIDE_VALUE)
                            {
                                segment.HideValue = true;
                            }
                            // -- Styrer om verdien skal vises i bold font
                            if (GetFieldAsString(fields, INDEX_DISPLAY_STYLE_VALUE).ToUpper() == STR_BOLD_VALUE)
                            {
                                segment.ShowValueAsBold = true;
                            }


                            // -- Constraints - Numerisk verdi og feltlengder --
                            string constraintsTmp = String.Empty;
                            char CONSTRAINTS_SEPARATOR = ';';
                            int MAX_FIELD_LENGTH = 9999;
                            constraintsTmp = GetFieldAsString(fields, INDEX_CONSTRAINT).ToUpper();

                            if (!string.IsNullOrEmpty(constraintsTmp))
                            {
                                string[] fieldsConstr = constraintsTmp.Split(CONSTRAINTS_SEPARATOR);
                                foreach (string constr in fieldsConstr)
                                {
                                    if (constr == "N")
                                    {
                                        segment.IsNumeric = true;
                                    }
                                    if (constr.Contains("L"))
                                    {
                                        string tmp = constr.Replace("L", "");
                                        string[] fieldsLength = tmp.Split(',');
                                        if (fieldsLength.Length == 2)
                                        {
                                            int minTmp = 0;
                                            if (int.TryParse(fieldsLength[0], out minTmp))
                                            {
                                                segment.MinLenght = minTmp;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Kunne ikke lese inn min lengde for segmentet " + segment.ToString());
                                            }

                                            int maxTmp = MAX_FIELD_LENGTH;
                                            if (int.TryParse(fieldsLength[1], out maxTmp))
                                            {
                                                segment.MaxLenght = maxTmp;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Kunne ikke lese inn max lengde for segmentet " + segment.ToString());
                                            }
                                        }
                                    }
                                }
                            }

                            string strName = GetFieldAsString(fields, INDEX_NAME);
                            strName = HL7.TrimComment(strName, COMMENT_CHAR);
                            segment.SegmentName = strName;

                            // -- Håndtering av segment --
                            if (segment.Index_L2 <= 0)
                            {
                                // Current segment er på nivå 1
                                segment.Level = 1;
                                _HL7Segments.Add(segment);

                                //   segment.MappingSection.Segments.Add(segment);
                                //segment.ParentSegment = segment.MappingSection.Segment; SEGMENTER I DETTE NIVÅET HAR IKKE PARENT SEGMENT. KUN SECTION.
                            }
                            else if (segment.Index_L2 > 0)
                            {
                                // Segmentet er et subsegment. Parentsegment ble satt i FORRIGE iterasjon.
                                segment.Level = 2;

                                // Legger til parent segment.
                                HL7MappingSegment parentSegmentTmp = this._HL7Segments.GetSegment(segment.SectionName, segment.Index_L1, 0);  // segment.MappingSection.Segments.GetSegment(segment.SectionName, segment.Index_L1, 0);
                                segment.ParentSegment = parentSegmentTmp;

                                segment.ParentSegment.SubSegments.Add(segment);
                            }
                            else
                            {
                                MessageBox.Show("Nivå 3 ikke implementert i mappingen. Navnene på feltene vises ikke korrekt", "Importere navn for meldingsvisning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    sr.Close();
                    // PopulateListOfSections();
                    return this;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Feil under innlesing av mappingfilen (oversikt over feltnavn)\r\n" + str + "\r\n\n" + ex.Message + "\r\n\n" + ex.StackTrace, "Innlesing av mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public Hl7Mapping ImportMapping()
        {
            return ImportMapping(this.FileInfo);
        }


        /// <summary>
        /// Returnerer felt som string i string array.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetFieldAsString(string[] fields, int index)
        {
            if (index + 1 <= fields.Length)
            {
                string strTmp = fields[index];
                return strTmp;
            }
            else
            {
                MessageBox.Show("Feil under innlesing av mapping.\r\n" + arrayToString(fields), "Innlesing av mapping", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return String.Empty;
            }
        }

        /// <summary>
        /// Parser int fra string-array.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetFieldAsint(string[] fields, int index)
        {
            string strTmp = GetFieldAsString(fields, index);
            if (!String.IsNullOrEmpty(strTmp))
            {
                int result = -1;
                if (int.TryParse(strTmp, out result))
                {
                    return result;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                //MessageBox.Show("Feil under innlesing av mapping.\r\n" + arrayToString(fields), "Innlesing av mapping", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }
        }



        /// <summary>
        /// Konverte string-array til tab-separert string.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private string arrayToString(string[] fields)
        {
            string str = String.Empty;
            for (int i = 0; i < fields.Length; i++)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str += "\t";
                }
                str += fields[i];
            }
            return str;
        }


        public HL7MappingSegment GetSegmentFromSection(string name, int index_L1, int index_L2)
        {
            //Hl7MappingSection sectionTmp = this.Hl7MappingSections.Get(name);

            //if (sectionTmp == null)
            //{
            //    return null;
            //}
            //else
            //{
            //    HL7MappingSegment segmentTmp = sectionTmp.Segments.GetSegment(name, index_L1, index_L2);
            //    return segmentTmp;
            //}

            return this._HL7Segments.GetSegment(name, index_L1, index_L2);
        }



        // 201009-01        /// <summary>
        // 201009-01        /// Scan gjennom alle segmentnavn i den innleste mappingen og ta vare på unike verdiee.
        // 201009-01        /// </summary>
        // 201009-01        /// <returns></returns>
        // 201009-01        [Obsolete]
        // 201009-01        public List<string> PopulateListOfSections()
        // 201009-01        {
        // 201009-01            //List<string> sectionNames = new List<string>(); -> Endret til public property
        // 201009-01
        // 201009-01            foreach (HL7MappingSegment segment in this._HL7Segments)
        // 201009-01            {
        // 201009-01                if ((!SectionNames.Contains(segment.SectionName)) && (!String.IsNullOrEmpty(segment.SectionName)))
        // 201009-01                {
        // 201009-01                    SectionNames.Add(segment.SectionName);
        // 201009-01                    this.Hl7MappingSections.Add(segment.SectionName);
        // 201009-01                }
        // 201009-01            }
        // 201009-01            return SectionNames;
        // 201009-01        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        public string ToReportHeading()
        {
            return "SectionName" + "\t" + "Level 1" + "\t" + "Level 2" + "\t" + "CollapseDefault" + "\t" + "Fieldname";
        }

        public string ToReport()
        {
            // 201009-01 return this.Name + "\r\n" + this.Hl7MappingSections.ToReport();
            return this.Name;
        }

        public int CompareTo(object obj)
        {
            Hl7Mapping compareTo = (Hl7Mapping)obj;
            return this.DisplayName.CompareTo(compareTo.DisplayName);
        }
    }
}
