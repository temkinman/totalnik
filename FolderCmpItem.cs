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
        public string pathToCopy { get; set; }
        public string color { get; set; }
        public bool isCheck { get; set; }
        public string directory { get; set; }

        public FolderCmpItem createCmpItem(FileInfo file, FileInfo secondFile, string imagePath, string statusCmp, string parentDir, string pathToCopy = null, string color = null, bool isCheck = false)
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
                pathToCopy = pathToCopy,
                color = color,
                isCheck = isCheck
            };
        }

        public FolderCmpItem createCmpItem(string leftDirName, string rightDirName, string imagePath, string directory, string pathToCopy)
        {
            return new FolderCmpItem()
            {
                firstName = leftDirName == "" ? "" : leftDirName,
                firstSize = "",
                firstDate = "",
                сmpIcon = imagePath,
                secondDate = "",
                secondSize = "",
                secondName = rightDirName == "" ? "" : rightDirName,
                status = null,
                directory = directory,
                pathToCopy = pathToCopy == "" ? "" : pathToCopy
            };
        }
    }
}
