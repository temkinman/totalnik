using System.IO;
using System.Windows.Media;

namespace WpfTotalnik
{
    public class FolderCmpItem
    {
        public string firstName { get; set; }
        public string firstType { get; set; }
        public string firstSize { get; set; }
        public string firstDate { get; set; }
        public string сmpIcon { get; set; }
        public string secondName { get; set; }
        public string secondType { get; set; }
        public string secondSize { get; set; }
        public string secondDate { get; set; }
        public string status { get; set; }
        public string parentDir { get; set; }
        public string pathWhereNeedCopy { get; set; }
        public string color { get; set; }

        public FolderCmpItem createCmpItem(FileInfo file, FileInfo secondFile, string imagePath, string statusCmp, string parentDir, string pathWhereNeedCopy = null, string color = null)
        {
            return new FolderCmpItem()
            {
                firstName = file == null ? "" : file.FullName,
                firstSize = file == null ? "" : MyFile.GetFileSize(file),
                firstDate = file == null ? "" : MyFile.GetFileDate(file),
                сmpIcon = imagePath == null ? null : imagePath,
                secondDate = secondFile == null ? "" : MyFile.GetFileDate(secondFile),
                secondSize = secondFile == null ? "" : MyFile.GetFileSize(secondFile),
                secondName = secondFile == null ? "" : secondFile.FullName,
                status = statusCmp,
                parentDir = parentDir,
                pathWhereNeedCopy = pathWhereNeedCopy,
                color = color
            };
        }

        public FolderCmpItem createCmpItem(string dirName)
        {
            return new FolderCmpItem()
            {
                firstName = dirName,
                firstSize = "",
                firstDate = "",
                сmpIcon = null,
                secondDate = "",
                secondSize = "",
                secondName = "",
                status = ""
            };
        }
    }
}
