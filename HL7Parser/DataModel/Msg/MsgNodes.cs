using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Viewer.DataModel.Msg
{
    public class MsgNodes : List<MsgNode>
    {
        private string reportContent;
        public string ToReport()
        {
            reportContent = String.Empty;

            reportContent = MsgNode.ToReportHeader();
            foreach (MsgNode msgChildNode in this)
            {
                reportContent = AddLineFeed(reportContent);
                reportContent += msgChildNode.ToReport();

                reportContent = AddLineFeed(reportContent);
                reportContent += ToReportRecursively(msgChildNode);
            }
            return reportContent;
        }

        public string ToReportRecursively(MsgNode msgNode)
        {
            string str = String.Empty;
            foreach (MsgNode msgChildNode in msgNode.Children)
            {

                str = AddLineFeed(str);
                string tmpSlettes = msgChildNode.ToReport();

                str += msgChildNode.ToReport();

                str = AddLineFeed(str);
                str += ToReportRecursively(msgChildNode);
            }
            return str;
        }

        /// <summary>
        /// Legger til linefeed til en streng hvis den ikke allerede ender med linefeed.
        /// Ignorerer tomme linjer.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string AddLineFeed(string str)
        {
            bool doAdd = false;
            if (String.IsNullOrEmpty(str))
            {
                doAdd = false;
            }
            else
            {
                if (str.Length < 2)
                {
                    doAdd = true;
                }
                else
                {
                    string strTmp = str.Substring(str.Length - 2, 2);
                    if (strTmp != HL7.LINEFEED)
                    {
                        doAdd = true;
                    }
                }
            }

            if (doAdd)
            {
                str += HL7.LINEFEED;
            }
            return str;
        }

    }
}
