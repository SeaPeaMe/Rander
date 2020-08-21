using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Rander.BaseComponents
{
    class FolderCompressor
    {
        public static void Compress(string InputDirectory, string OutputFile, CompressionLevel Compression = CompressionLevel.Optimal, bool Encrypt = false, bool DeleteFolder = false)
        {
            if (File.Exists(OutputFile)) File.Delete(OutputFile);
            ZipFile.CreateFromDirectory(InputDirectory, OutputFile, Compression, false);

            if (Encrypt)
            {
                byte IncAmt = byte.Parse(Rand.RandomInt(1, 9).ToString());
                FileStream fileStream = new FileStream(OutputFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                for (int i = 0; i < fileStream.Length; i++)
                {
                    byte b;
                    if (!byte.TryParse(fileStream.ReadByte().ToString(), out b)) { break; }

                    fileStream.Position -= 1;
                    b += IncAmt;
                    fileStream.WriteByte(b);
                    fileStream.Position += 1;
                }

                List<byte> encbytes = new List<byte>();
                encbytes.Add(IncAmt);
                encbytes.Add(Encoding.Default.GetBytes("®")[0]);

                fileStream.Write(encbytes.ToArray(), 0, encbytes.Count);

                fileStream.Close();
            }

            if (DeleteFolder)
            {
                Directory.Delete(InputDirectory, true);
            }
        }

        public static void Decompress(string InputFile, string OutputDirectory, bool DeleteFile = false)
        {
            FileStream fileStream = new FileStream(InputFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            fileStream.Position = fileStream.Length - 1;

            if (byte.Parse(fileStream.ReadByte().ToString()) == Encoding.Default.GetBytes("®")[0])
            {
                fileStream.Close();

                // Copies the file for byte changing
                File.Copy(InputFile, InputFile + ".temp");

                fileStream = new FileStream(InputFile + ".temp", FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                fileStream.Position = fileStream.Length - 2;
                byte IncAmt = byte.Parse(fileStream.ReadByte().ToString());

                fileStream.Position = 0;
                for (int i = 0; i < fileStream.Length - 2; i++)
                {
                    byte b;
                    if (!byte.TryParse(fileStream.ReadByte().ToString(), out b)) break;

                    fileStream.Position -= 1;
                    b -= IncAmt;
                    fileStream.WriteByte(b);
                    fileStream.Position += 1;
                }

                fileStream.Position = fileStream.Length - 2;
            }

            fileStream.Close();

            if (!Directory.Exists(OutputDirectory)) Directory.CreateDirectory(OutputDirectory);
            ZipFile.ExtractToDirectory(File.Exists(InputFile + ".temp") ? InputFile + ".temp" : InputFile, OutputDirectory);

            File.Delete(InputFile + ".temp");

            if (DeleteFile)
            {
                File.Delete(InputFile);
            }
        }
    }
}
