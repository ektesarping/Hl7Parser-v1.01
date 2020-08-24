using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel
{
    public class SectionIndexPair : IComparable
    {
        public string Name { get; set; }
        public int startPos { get; set; }

        public string SourceString { get; set; }

        public SectionIndexPair(string name, int startpos)
        {
            this.Name = name;
            this.startPos = startpos;
        }

        public int CompareTo(object obj)
        {
            SectionIndexPair p2 = (SectionIndexPair)obj;
            return this.startPos.CompareTo(p2.startPos);
        }

        public override string ToString()
        {
            return this.Name + " / " + this.startPos.ToString() + "  /  " + this.SourceString;
        }
    }
}
