using LinqClassesLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace Linq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LibraryLinq library = new LibraryLinq();
            string path = @"C:\windows";
            var files = library.ShowLargeFilesWithoutLinq(path);
            var filesLinq = library.ShowLargeFilesWithLinq(path);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{files[i].Name,-20} : {files[i].Length,10:N0}");
            }

            Console.WriteLine("---------------------------------");
            
            foreach(var file in filesLinq)
            {
                Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            }
        }
    }
}
