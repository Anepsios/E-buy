using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class PayPalLogger
    {
        public static string LogDirectoryPath = Environment.CurrentDirectory;
        public static void Log(String messages)
        {
            try
            {
                StreamWriter strw = new StreamWriter(LogDirectoryPath + "\\PayPalError.log", true);
                strw.WriteLine("{0}--->{1}",DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),messages);
            }
            // VS must be run as Admin (now it works without being run as admin... keeps this for reminder just in case)
            catch (Exception)
            {
                throw;
            }
        }
    }
}