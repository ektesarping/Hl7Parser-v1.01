using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Viewer.DataModel
{
    public class HL7Mappings : List<Hl7Mapping>
    {
        public DirectoryInfo DefaultMappingFolderDi { get; set; }

        public string MappingFileExtention { get; set; }


        private string propertyString = String.Empty;
        private char[] MAPPING_FILE_SEPARATOR = new char[] { ';' };

        private void SaveMappingsPropertyString()
        {
            string tmp = String.Empty;
            foreach (Hl7Mapping mapping in this)
            {
                tmp += mapping.FileInfo.FullName + MAPPING_FILE_SEPARATOR[0].ToString();
            }
            Properties.Settings.Default.Mappingfiler = tmp;
            Properties.Settings.Default.Save();
        }


        #region  -- Constructor --
        public HL7Mappings()
        { }

        public HL7Mappings(DirectoryInfo defaultMappingFolderDi, string mappingFileExtention) : this()
        {
            this.DefaultMappingFolderDi = defaultMappingFolderDi;
            this.MappingFileExtention = mappingFileExtention;
            ImportMappings();
        }

        #endregion  -- Constructor --


        public Hl7Mapping ImportMapping(FileInfo fi)
        {
            Hl7Mapping newMapping = new Hl7Mapping(fi);
            //newMapping.ImportMapping(fi);
            this.Add(newMapping);
            return newMapping;
        }

        public HL7Mappings ImportMappings()
        {
            // -- Importer mapping files ih fullpah for filene i Properties.Settings--
            string[] mappingfiles = Properties.Settings.Default.Mappingfiler.Split(MAPPING_FILE_SEPARATOR);

            foreach (string strFi in mappingfiles)
            {
                if (String.IsNullOrEmpty(strFi))
                {
                    continue;
                }

                FileInfo fi = new System.IO.FileInfo(strFi);
                Hl7Mapping hl7Mapping = new Hl7Mapping(fi);
                //hl7Mapping.ImportMapping(hl7Mapping.FileInfo);

                if (hl7Mapping.FileInfo.Exists)
                {
                    this.Add(hl7Mapping);
                }
                else
                {
                    MessageBox.Show("Mappingfilen ikke funnet\r\n" + fi.FullName, "Importer mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // -- Legger til mappinger som ligger i progamfolderen --
            if (DefaultMappingFolderDi != null)
            {
                if (DefaultMappingFolderDi.Exists)
                {
                    string filenameMask = "*" + MappingFileExtention;
                    FileInfo[] files = DefaultMappingFolderDi.GetFiles(filenameMask, SearchOption.AllDirectories);
                    foreach (FileInfo fi in files)
                    {
                        if (!this.Contains(fi))
                        {
                            Hl7Mapping mapping = new Hl7Mapping(fi);
                            //mapping.ImportMapping(fi);
                            if (!this.Contains(fi))
                            {
                                this.Add(mapping);
                            }
                        }
                    }
                }
            }

            //MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
            //MappingSelected.ImportMapping(MappingFileFi);
            return this;
        }

        public new void Remove(Hl7Mapping mapping)
        {
            if (this.Contains(mapping.FileInfo))
            {
                base.Remove(mapping);
                SaveMappingsPropertyString();
            }
        }

        public new void Add(Hl7Mapping mapping)
        {
            if (!this.Contains(mapping.FileInfo))
            {
                base.Add(mapping);
                try
                {
                    this.Sort();
                }
                catch
                {
                    MessageBox.Show("Internt problem med sortering av mapping. Sjekk at det er lagt inn minst én mappingfil. (Kode 201016)", "Innlesing av mapping", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                SaveMappingsPropertyString();
            }
        }

        public bool Contains(FileInfo fi)
        {
            foreach (Hl7Mapping mapping in this)
            {
                if (mapping.FileInfo.FullName.ToUpper() == fi.FullName.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        public Hl7Mapping Get(FileInfo fi)
        {
            foreach (Hl7Mapping mapping in this)
            {
                if (mapping.FileInfo.FullName.ToUpper() == fi.FullName.ToUpper())
                {
                    return mapping;
                }
            }
            return null;
        }

        public Hl7Mapping Get(string fullPath)
        {
            FileInfo fi;
            if (string.IsNullOrEmpty(fullPath))
            {
                //MessageBox.Show("Intern melding: \r\nKunne ikke finne mapping med filnavn " + fullPath, "Mapping.Get", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            try
            {
                fi = new FileInfo(fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Intern feil: \r\nKunne ikke finne mapping med filnavn " + fullPath, "Mapping.Get", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            if (fi != null)
            {
                return this.Get(fi);
            }
            else
            {
                return null;
            }

        }

        public Hl7Mapping GetBydisplayName(string displayName)
        {
            foreach (Hl7Mapping mapping in this)
            {
                if (mapping.DisplayName.ToUpper() == displayName.ToUpper())
                {
                    return mapping;
                }
            }
            return null;
        }

        public void Remove(string fullPath)
        {
            Hl7Mapping mappingTmp = this.Get(fullPath);
            if (mappingTmp != null)
            {
                this.Remove(mappingTmp);
            }
        }
    }
}
