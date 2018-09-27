using System;
using System.IO;

namespace DatabaseAccessLayer
{
    class Logger
    {
        static string directoryName = AppDomain.CurrentDomain.BaseDirectory + "Log";
        static string fileName = directoryName + @"\ApplicationLog.log";

        public Logger()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static void Log(string logInformation)
        {
            if (Directory.Exists(directoryName))
            {
                if (File.Exists(fileName))
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fs))
                        {
                            string date = System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year;
                            string time = System.DateTime.Now.TimeOfDay.Hours + ":" + System.DateTime.Now.TimeOfDay.Minutes + ":" + System.DateTime.Now.TimeOfDay.Seconds;
                            streamWriter.WriteLine("(" + date + " " + time + ") " + logInformation);
                            streamWriter.Close();
                        }
                        fs.Close();
                    }
                }
                else
                {

                    using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fs))
                        {
                            string date = System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year;
                            string time = System.DateTime.Now.TimeOfDay.Hours + ":" + System.DateTime.Now.TimeOfDay.Minutes + ":" + System.DateTime.Now.TimeOfDay.Seconds;
                            streamWriter.WriteLine("(" + date + " " + time + ") " + logInformation);
                            streamWriter.Close();
                        }
                        fs.Close();
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(directoryName);

                using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fs))
                    {
                        string date = System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year;
                        string time = System.DateTime.Now.TimeOfDay.Hours + ":" + System.DateTime.Now.TimeOfDay.Minutes + ":" + System.DateTime.Now.TimeOfDay.Seconds;
                        streamWriter.WriteLine("(" + date + " " + time + ") " + logInformation);
                        streamWriter.Close();
                    }
                    fs.Close();
                }

            }
        }//Log()


        /// <summary>
        /// Logs the given information in the log file but DOES NOT include the time stamp of logging
        /// </summary>
        /// <param name="logInformation">Text to log</param>
        public static void LogWithoutTimeStamp(string logInformation)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine(logInformation);
                    streamWriter.Close();
                }
                fs.Close();
            }
        }//LogWithoutTimeStamp()

        /// <summary>
        /// Inserts a seperator in the log file
        /// </summary>
        public static void InsertSeperator()
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine("-------------------------");
                    streamWriter.Close();
                }
                fs.Close();
            }
        }//InsertSeperator()


        /// <summary>
        /// Deletes the log file
        /// </summary>
        public static void ClearLogFile()
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            catch (Exception e)
            {
                Log("Error Deleting log file : " + System.IO.Path.GetFullPath(fileName) + " Error : " + e.Message);
            }
        } //ClearLogFile()
    }
}
