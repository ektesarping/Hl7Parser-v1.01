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

        //public string PropertyString
        //{
        //    get
        //    {
        //        return Properties.Settings.Default.Mappingfiler;
        //    }
        //    set
        //    {
        //        string tmp = String.Empty;
        //        foreach (Hl7Mapping mapping in this)
        //        {
        //            tmp += mapping.FileInfo.FullName + MAPPING_FILE_SEPARATOR;
        //            Properties.Settings.Default.Mappingfiler.Split(MAPPING_FILE_SEPARATOR);
        //            Properties.Settings.Default.Save();
        //        }
        //    }
        //}

        private void SaveMappingsPropertyString()
        {
            string tmp = String.Empty;
            foreach (Hl7Mapping mapping in this)
            {
                tmp += mapping.FileInfo.FullName + MAPPING_FILE_SEPARATOR[0].ToString();
                Properties.Settings.Default.Mappingfiler.Split(MAPPING_FILE_SEPARATOR);
                Properties.Settings.Default.Save();
            }
        }


        #region  -- Constructor --
        public HL7Mappings(DirectoryInfo defaultMappingFolderDi, string mappingFileExtention)
        {
            this.DefaultMappingFolderDi = defaultMappingFolderDi;
            this.MappingFileExtention = mappingFileExtention;
            ImportMappings();
        }

        #endregion  -- Constructor --


        public HL7Mappings ImportMappings()
        {
            string[] mappingfiles = Properties.Settings.Default.Mappingfiler.Split(MAPPING_FILE_SEPARATOR);

            foreach (string fi in mappingfiles)
            {
                if (String.IsNullOrEmpty(fi))
                {
                    continue;
                }

                Hl7Mapping hl7Mapping = new Hl7Mapping();
                hl7Mapping.FileInfo = new System.IO.FileInfo(fi);
                if (hl7Mapping.FileInfo.Exists)
                {
                    this.Add(hl7Mapping);
                }
            }

            // -- Legger til mappinger som ligger i progamfolderen --
            if (DefaultMappingFolderDi != null)
            {
                if (DefaultMappingFolderDi.Exists)
                {
                    //string ext = string.Empty;
                    //if (String.IsNullOrEmpty(MappingFileExtention))
                    //{
                    //    ext = MappingFileExtention;
                    //}

                    string filenameMask = "*" + MappingFileExtention;
                    FileInfo[] files = DefaultMappingFolderDi.GetFiles(filenameMask, SearchOption.AllDirectories);
                    foreach (FileInfo fi in files)
                    {
                        if (!this.Contains(fi))
                        {
                            Hl7Mapping mapping = new Hl7Mapping(fi);
                            mapping.ImportMapping(fi);
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

        public new void Add(Hl7Mapping mapping)
        {
            if (!this.Contains(mapping.FileInfo))
            {
                base.Add(mapping);
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

    }
}
