using SOS.Phone.Utilites.UtilityClasses;
using System;
using System.Windows;

namespace SOS.Phone
{
    public static class ExceptionHandler
    {
        public static WPClogger logger = new WPClogger(LogLevel.error);

        public static void ShowExceptionMessageAndLog(Exception exception, bool isLoggingRequired)
        {
            try
            {
                if (isLoggingRequired) logger.log(LogLevel.error, exception);
#if(!DEBUG)
                MessageBox.Show("Technical error has occured. Kindly retry or send error details through Settings -> Logger for resolution");
#else
                MessageBox.Show("Error: " + exception.ToString());
#endif

            }
            catch { }
        }
    }
}
