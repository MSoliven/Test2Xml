using System;
using System.Globalization;

namespace Test2Xml.XSLT
{
    public class Date
    {
        public Date()
        {
        }

        public string Today()
        {
            return DateTime.Today.ToString(CultureInfo.InvariantCulture);
        }

        public string Now()
        {
            return DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

    }
}
