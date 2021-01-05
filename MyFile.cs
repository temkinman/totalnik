using System;
using System.IO;


namespace WpfTotalnik
{
    public static class MyFile
    {
        public static DirectoryInfo[] GetFolders(string path)
        {
            DirectoryInfo DirInfo;
            DirectoryInfo[] dirs = { };
            try
            {
                DirInfo = new DirectoryInfo(path);
                dirs = DirInfo.GetDirectories();
            }
            catch (Exception error)
            {
                Console.WriteLine("The process failed: {0}", error.ToString());
            }

            return dirs;
        }

        public static FileInfo[] GetFiles(string path, string filter = "*.*")
        {
            DirectoryInfo DirInfo;
            FileInfo[] files = { };
            try
            {
                DirInfo = new DirectoryInfo(path);
                files = DirInfo.GetFiles(filter);
            }
            catch (Exception error)
            {
                Console.WriteLine("The process failed: {0}", error.ToString());
            }
            return files;
        }

        public static string GetFileSize(FileInfo file)
        {
            return file.Length <= 1024 ?
                   file.Length.ToString() + " bytes" :
                   Math.Round(file.Length / 1024f).ToString() + " Kb";
        }

        public static string GetFileDate(FileInfo file)
        {
            return file.LastWriteTime.ToString();
            //return file.LastAccessTime.ToShortDateString() + " " + file.LastAccessTime.ToShortTimeString();
        }

        public static void CopyFile(FileInfo file, string path)
        {

        }
    }
}
