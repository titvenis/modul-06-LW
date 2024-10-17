using System;
using System.IO;
using System.Threading;

public enum LogLevel
{
    INFO,
    WARNING,
    ERROR
}

public class Logger
{
    private static Logger instance = null;
    private static readonly object lockObj = new object();
    private LogLevel currentLogLevel = LogLevel.INFO;
    private string logFilePath = "log.txt";

    private Logger()
    {
    }

    public static Logger GetInstance()
    {
        if (instance == null)
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
            }
        }
        return instance;
    }

    public void SetLogLevel(LogLevel level)
    {
        lock (lockObj)
        {
            currentLogLevel = level;
        }
    }

    public void SetLogFilePath(string path)
    {
        lock (lockObj)
        {
            logFilePath = path;
        }
    }

    public void Log(string message, LogLevel level)
    {
        if (level < currentLogLevel)
            return;

        lock (lockObj)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now} [{level}] {message}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Logger logger = Logger.GetInstance();
        logger.SetLogLevel(LogLevel.INFO);

        Thread t1 = new Thread(() =>
        {
            logger.Log("Info message from thread 1", LogLevel.INFO);
            logger.Log("Warning message from thread 1", LogLevel.WARNING);
            logger.Log("Error message from thread 1", LogLevel.ERROR);
        });

        Thread t2 = new Thread(() =>
        {
            logger.SetLogLevel(LogLevel.WARNING);
            logger.Log("Info message from thread 2", LogLevel.INFO);  // This will not be logged
            logger.Log("Warning message from thread 2", LogLevel.WARNING);
            logger.Log("Error message from thread 2", LogLevel.ERROR);
        });

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine("Logging complete.");
    }
}
