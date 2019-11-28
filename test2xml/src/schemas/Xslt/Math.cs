
using System;
using System.Xml.XPath;

namespace Test2Xml.XSLT
{
    public class Math
    {
        public Math()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public XPathNodeIterator Nz(XPathNodeIterator objIterator)
        {
            //
            // TODO: Implement Nz() in the same manner as the xdMath implementation in InfoPath.
            //
            return objIterator;
        }

        public double Avg(XPathNodeIterator objIterator)
        {
            double dblSum = 0;
            int intCount = 0;

            while(objIterator.MoveNext())
            {
                try
                {
                    dblSum += double.Parse(objIterator.Current.Value);
                    intCount++;
                }
                catch
                {
                }
            }

            return SafeDivide(dblSum, intCount);
        }

        private double SafeDivide(double sum, int count)
        {
            if (count != 0.0)
            {
                return sum/count;
            }

            return Double.NaN;
        }

        public double Max(XPathNodeIterator objIterator)
        {
            double dblMax = double.MinValue;
            while(objIterator.MoveNext())
            {
                double dblTest = double.Parse(objIterator.Current.Value);
                if (dblTest > dblMax)
                {
                    dblMax = dblTest;
                }
            }

            return dblMax;
        }

        public double Min(XPathNodeIterator objIterator)
        {
            double dblMin = double.MaxValue;
            while(objIterator.MoveNext())
            {
                double dblTest = double.Parse(objIterator.Current.Value);
                if (dblTest < dblMin)
                {
                    dblMin = dblTest;
                }
            }

            return dblMin;
        }
    }
}
