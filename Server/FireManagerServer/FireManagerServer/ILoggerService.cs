namespace FireManagerServer
{
    public interface ILoggerService<T>
    {
        void WillLog(string message);
    }
}
