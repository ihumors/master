namespace Dms.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class Logger
    {
        protected static string logPath =  System.Configuration.ConfigurationManager.AppSettings["log"] + "";
        public static string LogPath
        {
            get { return string.IsNullOrEmpty(logPath) ? string.Format(CultureInfo.CurrentCulture, @"{0}\log", Environment.CurrentDirectory) : logPath; }
        }
        public static void Error(string message, bool async = true)
        {
            Write("Error", message, async);
        }
        public static void Info(string message, bool async = true)
        {
            Write("Info", message, async);
        }
        public static void Warn(string message, bool async = true)
        {
            Write("Warn", message, async);
        }
        public static void Write(string folderName, string message, bool async = true)
        {
            string path = string.Format(CultureInfo.CurrentCulture, @"{0}\{1}\{2}", LogPath, folderName, DateTime.Now.ToString("yyyyMM"));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string filename = string.Format(CultureInfo.CurrentCulture, @"{0}\{1}.log", path, DateTime.Now.ToString("yyyyMMdd"));
            message = string.Format(CultureInfo.CurrentCulture, "{0} {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);

            if (async)
                Task.Run(async () => await WriteAsync(filename, message));
            else
                WriteSync(filename, message);
        }
        public static async Task WriteAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Default.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
        public static bool WriteSync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Default.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath, 
                FileMode.Append,FileAccess.Write,FileShare.None,
                bufferSize:512,useAsync:false))
            {
                sourceStream.Write(encodedText, 0, encodedText.Length);
            };
            return true;
        }
    }
}
