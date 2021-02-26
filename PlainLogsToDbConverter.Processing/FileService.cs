using System.IO;

namespace PlainLogsToDbConverter.Processing
{
    public interface IFileService
    {
        StreamReader OpenFileTextStream(string filePath);
    }
    public class FileService : IFileService
    {
        public StreamReader OpenFileTextStream(string filePath)
        {
            return File.OpenText(filePath);
        }
    }
}
