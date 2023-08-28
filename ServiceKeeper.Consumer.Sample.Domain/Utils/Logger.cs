namespace ServiceKeeper.Consumer.Sample.Domain.Utils
{
    public class Logger
    {
        private static Logger instance = null!;
        private static ReaderWriterLockSlim logWriteLock = null!;
        private readonly string _logdictory;
        private readonly string publicfile;
        private readonly string errorfile;
        private readonly static object locker = new();
        public static Logger GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance ??= new Logger();
                }
            }
            return instance;
        }
        private Logger()
        {
            logWriteLock = new ReaderWriterLockSlim();
            try
            {
                _logdictory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!System.IO.Directory.Exists(_logdictory))
                    Directory.CreateDirectory(_logdictory);
                publicfile = _logdictory;
                errorfile = _logdictory;
            }
            catch (Exception ex)
            {
                Exception e = new("Logger class failed", ex);
                throw (e);
            }
        }
        public void Log_public(string str)
        {
            try
            {
                logWriteLock.EnterWriteLock();
                if (!System.IO.File.Exists(publicfile + @"\public-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"))
                    System.IO.File.Create(publicfile + @"\public-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt").Close();//创建该文件 
                var now = DateTime.Now;
                var logContent = string.Format($"{now.ToLongDateString()} {now.ToLongTimeString()}.{now.Millisecond}\t{str}\r\n");
                File.AppendAllText(publicfile + @"\public-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", logContent);
            }
            catch (Exception e)
            {
                Console.WriteLine("Logs function failed:\r\n" + e.Message);
            }
            finally
            {
                logWriteLock.ExitWriteLock();
            }
        }
        public void Log_error(string str)
        {
            try
            {
                logWriteLock.EnterWriteLock();
                if (!System.IO.File.Exists(errorfile + @"\error-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"))
                    System.IO.File.Create(errorfile + @"\error-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt").Close();//创建该文件 
                var now = DateTime.Now;
                var logContent = string.Format($"{now.ToLongDateString()} {now.ToLongTimeString()}.{now.Millisecond}\t{str}\r\n");
                File.AppendAllText(errorfile + @"\error-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", logContent);
            }
            catch (Exception e)
            {
                Console.WriteLine("Logs function failed:\r\n" + e.Message);
            }
            finally
            {
                logWriteLock.ExitWriteLock();
            }
        }
    }
}
