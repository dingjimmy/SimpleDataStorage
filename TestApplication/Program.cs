using System;
using Database;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            SequentialFile file = SequentialFile.Create("file.dat", 4);
            //SequentialFile file = SequentialFile.Open("file.dat");
           


            file.Close();

        }
    }
}
