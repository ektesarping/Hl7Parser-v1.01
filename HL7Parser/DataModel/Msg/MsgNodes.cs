using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel.Msg
{
    public class MsgNodes : List<MsgNode>
    {
        [Obsolete]
        public static int loop_L1 = 0;
        [Obsolete]
        public static int loop_L2 = 0;

        private string reportContent;
        public string ToReport()
        {
            reportContent = String.Empty;

            reportContent = MsgNode.ToReportHeader();
            foreach (MsgNode msgChildNode in this)
            {
                if (!String.IsNullOrEmpty(reportContent))
                {
                    reportContent += HL7.LINEFEED;
                }
                reportContent += msgChildNode.ToReport();
                ToReportRecursively(msgChildNode);

                // --debug--
                loop_L1++;
            }
            return reportContent;
        }

        public void ToReportRecursively(MsgNode msgNode)
        {
            string str2 = String.Empty;
            foreach (MsgNode msgChildNode in msgNode.Children)
            {
                // --debug--
                loop_L2++;
                //Console.WriteLine("L2: " + loop_L2.ToString() + "\tLengde: " + str2.Length.ToString() + "\t" + msgChildNode.ToString());
                //Console.WriteLine("2:" + msgChildNode.ToReport());


                if (!String.IsNullOrEmpty(reportContent))
                {
                    reportContent += HL7.LINEFEED;
                }
                string tmp = msgChildNode.ToReport();


                reportContent += msgChildNode.ToReport();
                // str2 += strNode;
                //str2 += ToReportRecursively(msgChildNode);
            }
            // return str2;
        }
    }
}
