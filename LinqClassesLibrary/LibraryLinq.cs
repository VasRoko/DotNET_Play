using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace LinqClassesLibrary
{
    public class LibraryLinq
    {
        public FileInfo[] ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var files = directoryInfo.GetFiles();

            Array.Sort(files, new FileInfoComparer());
            return files;
        }
        
        public FileInfo[] ShowLargeFilesWithLinq(string path)
        {
            var query = from file in new DirectoryInfo(path).GetFiles()
                        orderby file.Length descending
                        select file;
            //var query2 = new DirectoryInfo(path).GetFiles().OrderByDescending(f => f.Length).Take(5);
            return query.Take(5).ToArray();
        } 
        
        public class FileInfoComparer : IComparer<FileInfo>
        {
            public int Compare(FileInfo x, FileInfo y)
            {
                var result = y.Length.CompareTo(x.Length);
                return result;
            }
        }
    

    }
}
