using System;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Main starting...");
        DoWork(); // Đồng bộ, chặn luồng chính
        Console.WriteLine("DoWork completed.");

        Task.Run(async () =>
        {
            await DoWorkAsync(); // Bất đồng bộ, không chặn luồng chính
            Console.WriteLine("DoWorkAsync completed.");
        }).Wait();

       

        Console.WriteLine("Main ending...");
    }

    public static void DoWork()
    {
        // Tác vụ dài hạn đồng bộ
        Thread.Sleep(3000); // Chặn luồng chính trong 3 giây
        Console.WriteLine("Synchronous work done.");
    }

    public static async Task DoWorkAsync()
    {
        // Tác vụ dài hạn bất đồng bộ
        await Task.Delay(3000); // Không chặn luồng chính
        Console.WriteLine("Asynchronous work done.");
    }
}
