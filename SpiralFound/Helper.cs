using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiralFound
{
    public class Helper
    {

        public static string Trim(object o, int i)
        {
            string input = Convert.ToString(o);
            int stringLengthName = input.Length;
            // Trim Title to 50
            if (stringLengthName > i)
            {
                input = HttpContext.Current.Server.HtmlEncode(input.Substring(0, Math.Min(i, input.Length))) + "...";
            }
            else
            {
                input = HttpContext.Current.Server.HtmlEncode(input);
            }
            return input;
        } 


    }
}