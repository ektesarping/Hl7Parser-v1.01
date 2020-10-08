using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class Hl7MappingSections : List<Hl7MappingSection>
    {
        public Hl7MappingSection Get(string name)
        {
            foreach (Hl7MappingSection section in this)
            {
                if (section.Name == name)
                {
                    return section;
                }
            }
            return null;
        }

        public bool Contains(string name)
        {
            Hl7MappingSection section = this.Get(name);

            if (section != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public new void Add(Hl7MappingSection section)
        {
            if (!this.Contains(section.Name))
            {
                base.Add(section);
            }
        }

        public void Add(string name)
        {
            Hl7MappingSection section = new Hl7MappingSection(name);
            this.Add(section);
        }
        public string ToReport()
        {
            string str = String.Empty;
            foreach (Hl7MappingSection section in this)
            {
                if (!String.IsNullOrWhiteSpace(str))
                {
                    str += "\r\n;";
                }
                str += section.ToReport();
            }

            return str;
        }
    }
}
