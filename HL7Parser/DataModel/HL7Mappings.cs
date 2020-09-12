using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class HL7Mappings : List<Hl7Mapping>
    {
        public DirectoryInfo DefaultMappingFolderDi { get; set; }

        public string MapiingFileExtention { get; set; }


        private string propertyString = String.Empty;
        private char[] MAPPING_FILE_SEPARATOR = new char[] { ';' };

        public string PropertyString
        {
            get
            {
                return Properties.Settings.Default.Mappingfiler;
            }
            set
            {
                string tmp = String.Empty;
                foreach (Hl7Mapping mapping in this)
                {
                    tmp += mapping.FileInfo.FullName + MAPPING_FILE_SEPARATOR;
                    Properties.Settings.Default.Mappingfiler.Split(MAPPING_FILE_SEPARATOR);
                    Properties.Settings.Default.Save();
                }
            }
        }

        public HL7Mappings ImportMappings()
        {
            string[] mappingfiles = PropertyString.Split(MAPPING_FILE_SEPARATOR);

            foreach (string fi in mappingfiles)
            {
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
                    string ext = string.Empty;
                    if (String.IsNullOrEmpty(MapiingFileExtention))
                    {
                        ext = MapiingFileExtention;
                    }
                    FileInfo[] files = DefaultMappingFolderDi.GetFiles(ext, SearchOption.AllDirectories);
                    foreach (FileInfo fi in files)
                    {
                        if (!this.Contains(fi))
                        {
                            Hl7Mapping mapping = new Hl7Mapping(fi);
                            mapping.ImportMapping(fi);
                        }
                    }
                }
            }


            //MappingFileFi = new FileInfo(Path.Combine(executableFi.DirectoryName, "Datamodel", MappingFileName));
            //MappingSelected.ImportMapping(MappingFileFi);



            return this;
        }

        private bool Contains(FileInfo fi)
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
    }
}
