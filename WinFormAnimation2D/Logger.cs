using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormAnimation2D
{
    public enum Level
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
        MAX
    }

    class Logger
    {
        private static string _local_app_data_dir = System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.LocalApplicationData);

        private string[] _map_lvl2prefix = new string[]{ "Debug:  "
                                                      , "Info:   "
                                                      , "Warning:"
                                                      , "Error:  "
        };

        private string _log_file_path;
        public Logger(string file_name)
        {
            _log_file_path = System.IO.Path.Combine(_local_app_data_dir, file_name);
        }

        public void ClearLog()
        {
            System.IO.File.Delete(_log_file_path);
        }

        private void AppendLog(string text)
        {
            System.IO.File.AppendAllText(_log_file_path, string.Format("{0}\r\n", text));
        }

        public void Log(Level level, string message)
        {
            string head = _map_lvl2prefix[(int)level];
            AppendLog(head + message);
        }

        public void Log(string message)
        {
            Log(Level.Info, message);
        }

    }
}
