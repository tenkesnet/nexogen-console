using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace NexogenLogger
{
    public class Logger
    {
        private readonly int maxMessageSize = 1000;
        private readonly long maxLogSize = 5 * 1024;
        private readonly string logFileName = "log.txt";
        private readonly string logFileNameWithPath="";
        private readonly string logPath="";
        private LogTypeEnum logType;
        private StreamWriter stream;

        public Logger()
        {
            this.stream = new StreamWriter(Console.OpenStandardOutput());
            this.stream.AutoFlush = true;
            logType = LogTypeEnum.Console;
        }

        public Logger(string path)
        {
            if (path == null) throw new ArgumentNullException("Path is null.");
            if (path.Length == 0)
            {
                logPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
            else
            {
                logPath = path;
            }

            logFileNameWithPath = Path.Combine(path, logFileName);
            try
            {
                CreateLogfile(logFileNameWithPath);
            }
            catch (Exception ex)
            {
                throw;
            }
            logType = LogTypeEnum.File;
        }

        public Logger(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("Stream is null.");
            this.stream = new StreamWriter(stream);
            this.stream.AutoFlush = true;
            logType = LogTypeEnum.Stream;
        }


        public async Task error(string message)
        {
            await log(LogLevelEnum.Error, message);
        }

        public async Task info(string message)
        {
            await log(LogLevelEnum.Info, message);
        }

        public async Task debug(string message)
        {
            await log(LogLevelEnum.Debug, message);
        }

        internal async Task log(LogLevelEnum level, string message)
        {
            if (message.Length > maxMessageSize)
            {
                throw new LongLogMessageException("Log message is too long. Maximum character: 1000");
            }
            if (logType == LogTypeEnum.File)
            {
                await logRotate(message.Length);
            }
            if (logType == LogTypeEnum.Console)
            {
                ConsoleColor tempColor = Console.ForegroundColor;
                switch (level)
                {
                    case LogLevelEnum.Debug:
                        Console.ForegroundColor = ConsoleColor.Gray; break;
                    case LogLevelEnum.Info:
                        Console.ForegroundColor = ConsoleColor.Green; break;
                    case LogLevelEnum.Error:
                        Console.ForegroundColor = ConsoleColor.Red; break;
                }
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{level}] - {message}");
                Console.ForegroundColor = tempColor;
            }
            else
            {
                await stream.WriteLineAsync($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{level}] - {message}");
            }
        }
        private async Task logRotate(int messageSize)
        {
            int maxLogrotate = 0;
            FileInfo fi = new FileInfo(logFileNameWithPath);
            IEnumerable<string> logFiles = new List<string>();
            if (fi.Exists && fi.Length + messageSize > maxLogSize)
            {
                try
                {
                    logFiles = Directory.EnumerateFiles(logPath, "log*.txt", SearchOption.AllDirectories).OrderByDescending(filename => filename); ;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (string file in logFiles)
                {
                    string filename = Path.GetFileNameWithoutExtension(file);
                    string pattern = @"^log\.(\d+)";
                    Match m = Regex.Match(filename, pattern, RegexOptions.IgnoreCase);
                    if (m.Success && m.Groups.Values.Count() > 0)
                    {
                        int index = Convert.ToInt16(m.Groups.Values.ToList()[1].Value);
                        IncraseLogrotate(file,Path.Combine(logPath,$"log.{index+1}.txt"));
                        maxLogrotate = maxLogrotate < index ? index : maxLogrotate;
                    }
                }
                if(maxLogrotate > 0)
                {
                    try
                    {
                        stream.Close();
                        IncraseLogrotate(logFileNameWithPath, Path.Combine(logPath, $"log.1.txt"));
                        CreateLogfile(Path.Combine(logPath, "log.txt"));
                    } catch (Exception e) { throw; }
                }
                

            }
        }

        private void SetConsoleLog()
        {
            this.stream = new StreamWriter(Console.OpenStandardOutput());
            this.stream.AutoFlush = true;
            logType = LogTypeEnum.Console;
        }

        private void IncraseLogrotate(string srcFilename,string destFilename)
        {
            if (File.Exists(destFilename))
            {
                File.Delete(destFilename);
            }
            File.Move(srcFilename, destFilename);
        }
        public void CreateLogfile(string filename)
        {
            try
            {
                this.stream = new StreamWriter(filename, true);
                this.stream.AutoFlush = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}