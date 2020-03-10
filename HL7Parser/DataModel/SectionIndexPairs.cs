using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Parser.DataModel
{
    public class SectionIndexPairs : List<SectionIndexPair>
    {
        public bool Contains(string name)
        {
            return (Get(name) != null); // Return true if this contains pair.name...
        }
        public SectionIndexPair Get(string name)
        {
            foreach (SectionIndexPair pair in this)
            {
                return pair;
            }
            return null;
        }
    }
}
