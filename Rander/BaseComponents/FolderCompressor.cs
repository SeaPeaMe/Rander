using System.IO;
using System.IO.Compression;

namespace Rander
{
    class FolderCompressor
    {
        public static void Compress(string InputDirectory, string OutputFile, CompressionLevel Compression = CompressionLevel.Optimal, bool DeleteFolder = false)
        {
            if (File.Exists(OutputFile)) File.Delete(OutputFile);
            ZipFile.CreateFromDirectory(InputDirectory, OutputFile, Compression, false);

            if (DeleteFolder)
            {
                Directory.Delete(InputDirectory, true);
            }
        }

        public static void Decompress(string InputFile, string OutputDirectory, bool DeleteFile = false)
        {
            FileStream fileStream = new FileStream(InputFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            fileStream.Position = fileStream.Length - 1;

            fileStream.Close();

            if (!Directory.Exists(OutputDirectory)) Directory.CreateDirectory(OutputDirectory);
            ZipFile.ExtractToDirectory(InputFile, OutputDirectory);

            if (DeleteFile)
            {
                File.Delete(InputFile);
            }
        }
    }
}
