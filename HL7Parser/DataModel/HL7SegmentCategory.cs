﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7Parser.DataModel
{
    public class HL7SegmentCategory
    {
        public string CategoryName { get; set; }

        public HL7Segments _HL7Segments { get; set; } = new HL7Segments();

        public TreeNode Treenode { get; set; }

        public override string ToString()
        {
            return this.CategoryName;
        }
    }
}