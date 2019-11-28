using System;
using System.Globalization;

namespace Test2Xml.XSLT
{

	public class Formatting
	{
		// Used by NumberFormatInfo.XXXXXXGroupSizes
		private int[] _objZeroGroupSizes = { 0, 0, 0, 0, 0, 0, 0};
		private int[] _objGroupSizes     = { 3, 3, 3, 3, 3, 3, 3};

        // Bool to signal that request came from the portal
        public bool FromPortal = false;

		public Formatting()
		{ 

		}

		public string formatString(string strValue, string strType, string strFormat)
		{
			// Default return value
			string strRetVal = strValue;

			if (strValue.Trim() != "")
			{
				switch(strType.ToLower().Trim())
				{
					case "date":
                        strRetVal = FormatDate(strValue, strFormat);
                        break;
					case "datetime":
						strRetVal = FormatDateTime(strValue, strFormat);
						break;
					case "number":
						strRetVal = FormatNumber(strValue, strFormat);
						break;
					case "percentage":
						strRetVal = FormatPercentage(strValue, strFormat);
						break;
					case "currency":
						strRetVal = FormatCurrency(strValue, strFormat);
						break;
				}            
			}

			return strRetVal;
		}

		public string formatString(string strValue, string strCurrency, string strType, string strFormat)
		{
			// Default return value
			string strRetVal = strValue;

			if (strValue.Trim() != "" && strType.ToLower().Trim().Equals("number"))
			{
				int intPrecision;
				int intNumDigits, intGrouping, intNegOrder, intPosOrder;

				NumberFormatInfo objFormatInfo = new NumberFormatInfo();
                
                intPrecision = 2;

                // Parse the format string, and return useful values
				GetNumericFormatInfo(strFormat, out intNumDigits, out intGrouping, out intNegOrder, out intPosOrder);
				// 
				// If the out parameters are valid, use it! Otherwise, use defaults that make sense.
				//
				objFormatInfo.NumberDecimalDigits   = intNumDigits >= 0 ? intNumDigits : intPrecision;                    
				objFormatInfo.NumberNegativePattern = intNegOrder  >= 0 ? intNegOrder  : 1;
				objFormatInfo.NumberGroupSizes      = intGrouping  == 0 ? this._objZeroGroupSizes : this._objGroupSizes;
    
				// Apply the format to the value
				double dblValue = double.Parse(strValue, objFormatInfo);
				strRetVal = dblValue.ToString("N", objFormatInfo);
			}

			return strRetVal;
		}

		private string FormatDate(string strValue, string strFormat)
		{
			CultureInfo objCultureInfo = null;

			// Default return value
			string strRetVal = strValue;           
			string strTemp;

			DateTime objDate = new DateTime();
			objDate = DateTime.Parse(strValue);

			// Get culture info and create a culture-info object, if any
			if ((strTemp = GetFormatValue("locale", strFormat)) != null)
			{
				objCultureInfo = new CultureInfo(int.Parse(strTemp));
			}

			// Get the date format from the string
			if ((strTemp = GetFormatValue("dateFormat", strFormat)) != null)
			{
				// Convert patterns to .NET format
				switch(strTemp)
				{
					case "Short Date": 
						strTemp = "d";
						break;
					case "Long Date":
						strTemp = "D";
						break;
					case "Year Month":
						strTemp = "y";
						break;
				}

				if (objCultureInfo != null)
				{
					// Culture was provided, use it
					strRetVal = objDate.ToString(strTemp, objCultureInfo);    
				}
				else
				{
					// No date formatting specified
					strRetVal = objDate.ToString(strTemp);
				}
			}
			else
			{
				// Otherwise, use the default Spectrum date
				strRetVal = objDate.ToString("MMM dd, yyyy");    
			}

			return strRetVal;
		}

        private string FormatDateTime(string strValue, string strFormat)
        {
            CultureInfo objCultureInfo = null;

            // Default return value
            string strRetVal = strValue;           
            string strTemp;

            DateTime objDate = new DateTime();
            objDate = DateTime.Parse(strValue);

            // Get culture info and create a culture-info object, if any
            if ((strTemp = GetFormatValue("locale", strFormat)) != null)
            {
                objCultureInfo = new CultureInfo(int.Parse(strTemp));
            }

            // Format the Date part first
            strRetVal = FormatDate(strValue, strFormat) + " ";
            
            // Get the time format from the string
            if ((strTemp = GetFormatValue("timeFormat", strFormat)) != null)
            {
                if (strTemp != "none")
                {
                    if (objCultureInfo != null)
                    {
                        // Culture was provided, use it
                        strRetVal += objDate.ToString(strTemp, objCultureInfo);    
                    }
                    else
                    {
                        // No date formatting specified
                        strRetVal += objDate.ToString(strTemp);
                    }
                }
                else
                {
                    // Do not display time
                    strRetVal = strRetVal.Trim();
                }
            }
            else
            {
                // Otherwise, use the default Spectrum time
                strRetVal += objDate.TimeOfDay.ToString();    
            }

            return strRetVal;
        }


		private string FormatNumber(string strValue, string strFormat)
		{
			// Default return value
			string strRetVal = strValue;
			int intNumDigits, intGrouping, intNegOrder, intPosOrder;

			NumberFormatInfo objFormatInfo = new NumberFormatInfo();

			// Parse the format string, and return useful values
			GetNumericFormatInfo(strFormat, out intNumDigits, out intGrouping, out intNegOrder, out intPosOrder);
			// 
			// If the out parameters are valid, use it! Otherwise, use defaults that make sense.
			//
			objFormatInfo.NumberDecimalDigits   = intNumDigits >= 0 ? intNumDigits : 2;
			objFormatInfo.NumberNegativePattern = intNegOrder  >= 0 ? intNegOrder  : 1;
			objFormatInfo.NumberGroupSizes      = intGrouping  == 0 ? this._objZeroGroupSizes : this._objGroupSizes;
            
			// Apply the format to the value
			double dblValue = double.Parse(strValue, objFormatInfo);
			strRetVal = dblValue.ToString("N", objFormatInfo);

			return strRetVal;
		}

		private string FormatPercentage(string strValue, string strFormat)
		{
			// Default return value
			string strRetVal = strValue;
			int intNumDigits, intGrouping, intNegOrder, intPosOrder;

			NumberFormatInfo objFormatInfo = new NumberFormatInfo();

			// Parse the format string, and return useful values
			GetNumericFormatInfo(strFormat, out intNumDigits, out intGrouping, out intNegOrder, out intPosOrder);
			// 
			// If the out parameters are valid, use it! Otherwise, use defaults that make sense.
			//
			objFormatInfo.PercentDecimalDigits   = intNumDigits >= 0 ? intNumDigits : 2;  
			objFormatInfo.PercentNegativePattern = intNegOrder  >= 0 ? intNegOrder  : 1;
			objFormatInfo.PercentGroupSizes      = intGrouping  == 0 ? this._objZeroGroupSizes : this._objGroupSizes;

			// Apply the format to the value
			double dblValue = double.Parse(strValue,objFormatInfo) * 0.01;
			strRetVal = dblValue.ToString("P", objFormatInfo);

			return strRetVal;
		}

		private string FormatCurrency(string strValue, string strFormat)
		{           
			// Default return value
			string strRetVal = strValue;
			string strTemp;

			int intNumDigits, intGrouping, intNegOrder, intPosOrder;
         
			NumberFormatInfo objFormatInfo = null;

			// Get culture info and create a culture-info object, if any
			if ((strTemp = GetFormatValue("currencyLocale", strFormat)) != null)
			{
				// Get NumberFormatInfo object based on culture info
				CultureInfo objCultureInfo = new CultureInfo(int.Parse(strTemp));
				objFormatInfo = objCultureInfo.NumberFormat;
			}
			else
			{
				objFormatInfo = new NumberFormatInfo();
			}

			// Parse the format string, and return useful values
			GetNumericFormatInfo(strFormat, out intNumDigits, out intGrouping, out intNegOrder, out intPosOrder);
			// 
			// If the out parameters are valid, use it! Otherwise, use defaults that make sense.
			//
			objFormatInfo.CurrencyDecimalDigits   = intNumDigits >= 0 ? intNumDigits : 2;
			objFormatInfo.CurrencyNegativePattern = intNegOrder  >= 0 ? intNegOrder  : 1;
			objFormatInfo.CurrencyPositivePattern = intPosOrder  >= 0 ? intPosOrder  : 0;
			objFormatInfo.CurrencyGroupSizes      = intGrouping  == 0 ? this._objZeroGroupSizes : this._objGroupSizes;

			// Apply the format to the value
			double dblValue = double.Parse(strValue,objFormatInfo);
			strRetVal = dblValue.ToString("C", objFormatInfo);
            
			return strRetVal;
		}

		private string GetFormatValue(string strItem, string strFormat)
		{
			string strValue = null;

			// Check if the item exist in the format string
			int intPos = strFormat.IndexOf(strItem);

			if (intPos >= 0)
			{
				// Yep, look for the colon
				int intColonPos = strFormat.IndexOf(":", intPos);

				if (intColonPos >= 0)
				{
					// Colon found, now look for the semicolon
					int intSemiColonPos = strFormat.IndexOf(";", ++intColonPos);

					if (intSemiColonPos >= 0)
					{
						// Semi-colon found!!!!! Now get the value!!!!
						strValue = strFormat.Substring(intColonPos, intSemiColonPos - intColonPos);
					}
				}
			}

			return strValue;
		}

        private void GetNumericFormatInfo(string strFormat, out int intNumDigits, out int intGrouping, out int intNegOrder, out int intPosOrder)
		{
			string strTemp;

			// Default values (invalid)
			intNumDigits = -1;
			intGrouping  = -1;
			intNegOrder  = -1;
			intPosOrder  = -1;

			// Get number of digits, if specified
			if ((strTemp = GetFormatValue("numDigits", strFormat)) != null)
			{
				if (!strTemp.Equals("auto"))
				{
					intNumDigits = int.Parse(strTemp);
				}
			}

			// Get grouping, if specified
			if ((strTemp = GetFormatValue("grouping", strFormat)) != null)
			{
				intGrouping = int.Parse(strTemp);
			}

			// Get negative order, if specified
			if ((strTemp = GetFormatValue("negativeOrder", strFormat)) != null)
			{
				intNegOrder = int.Parse(strTemp);
			}

			// Get positive order, if specified
			if ((strTemp = GetFormatValue("positiveOrder", strFormat)) != null)
			{
				intPosOrder = int.Parse(strTemp);
			}
		}
	}
}
