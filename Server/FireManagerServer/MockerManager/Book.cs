using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockerManager
{
    public class Book
    {
        private static List<string> books = new List<string>() { "Toan", "Van", "Anh","Lich su", "Dia ly" };
        public async Task<List<string>> GetBook ()
        {
            Thread.Sleep(3000);

            return await Task.FromResult(books);

        }
    }
}
