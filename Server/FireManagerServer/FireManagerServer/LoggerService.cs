
namespace FireManagerServer
{
    public class LoggerService<T> : ILoggerService<T>
    {
        public void WillLog(string message)
        {
            //var nameService = $"{typeof(T).Name}";
            //string currentDirectory = Directory.GetCurrentDirectory();
            //var path = Path.Combine(currentDirectory, nameService);
            //var date = DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss");
            //var text = $"[{date}]: {message}";
            //File.AppendAllLines(path+".txt", new string[] {text});
        }
    }
}
