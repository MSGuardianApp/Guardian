using System;
using System.IO;

namespace Tools
{
    class OPsLogger
    {
        private static OPsLogger opsLogger = null;
        public static string FileName = string.Empty;

        public static OPsLogger GetInstance(string fileName)
        {
            if (opsLogger==null)
            {
                opsLogger = new OPsLogger();                
                FileName = Environment.CurrentDirectory +@"\" +fileName;
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

            }
            return opsLogger;
        }

        public static void UpdateLastRun(string fileName,string nextRunTime)
        {
            string file = Environment.CurrentDirectory + @"\" + fileName;
            if (File.Exists(file))
            {
                File.Delete(file);
                TextWriter tw = File.CreateText(file);
                tw.WriteLine("LastRunTime=" + nextRunTime);
                tw.Close();
            }                       
        }

        public static string ReadLastRun(string fileName)
        {
            string file = Environment.CurrentDirectory + @"\" + fileName;
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(file);

            //Read the first line of text
            string line = sr.ReadLine();

            sr.Close();

            return line.Split('=')[1];           
                       
        }

        public void WriteLog(string Message)
        {
            lock (this)
            {
                FileStream fs = null;
                if (!File.Exists(FileName))
                {
                    try
                    {
                        fs = new FileStream(FileName, FileMode.CreateNew);
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(Environment.NewLine + "-----------------------------------------------------------" + Environment.NewLine);
                            writer.Write(Message);
                            writer.Write(Environment.NewLine + "-----------------------------------------------------------" + Environment.NewLine);
                        }
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Dispose();
                    }
                }
                else
                {

                    try
                    {
                        fs = new FileStream(FileName, FileMode.Append);
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(Environment.NewLine + "-----------------------------------------------------------" + Environment.NewLine);
                            writer.Write(Message);
                            writer.Write(Environment.NewLine + "-----------------------------------------------------------" + Environment.NewLine);
                        }
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Dispose();
                    }

                }//End Else

            }

        }


    }
}
