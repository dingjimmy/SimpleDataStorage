using System;
using System.IO;
using System.Text;

namespace Database
{
    public class SequentialFile
    {
        public String FilePath { get; private set; }
        public bool IsOpen { get; private set; }
        public int RecordLength { get; private set; }
        public int RecordCount { get; private set; }
        public static Encoding CharacterEncoding { get; private set; }

        private FileStream fs;
        private BinaryReader br;
        private BinaryWriter bw;

        public SequentialFile()
        {
            IsOpen = false;
            RecordLength = 0;
            RecordCount = 0;
            CharacterEncoding = Encoding.Unicode;
        }


        public static SequentialFile Create(string filepath, int recordlength)
        {
            //create new instance
            SequentialFile file = new SequentialFile();

            //set file path & record length
            file.FilePath = filepath;
            file.RecordLength = recordlength;

            //create file, init stream, reader & writer
            file.fs = new FileStream(file.FilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, file.RecordLength);
            file.br = new BinaryReader(file.fs, CharacterEncoding);
            file.bw = new BinaryWriter(file.fs, CharacterEncoding);

            //write metadata to file header
            file.WriteHeader();

            //set isopen flag to true
            file.IsOpen = true;

            //return file to caller
            return file;
        }

        public static SequentialFile Open(string filepath)
        {
            //create new instance
            SequentialFile file = new SequentialFile();

            //set file path
            file.FilePath = filepath;

            //read file header
            file.ReadHeader();

            //open file, init stream, reader & writer
            file.fs = new FileStream(file.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, file.RecordLength);
            file.br = new BinaryReader(file.fs, CharacterEncoding);
            file.bw = new BinaryWriter(file.fs, CharacterEncoding);

            //set isopen flag to true
            file.IsOpen = true;

            //return file to caller
            return file;
        }

        public void Close()
        {
            
            //flush buffers and close file
            fs.Flush(true);
            fs.Close();
            fs.Dispose();
            br.Dispose();
            bw.Dispose();

            //set isopen flag to false
            IsOpen = false;

        }

        private void WriteHeader()
        {
            //write record length at position 0 of the file. takes up first 4 byte's of the file.
            fs.Position = 0;
            bw.Write(RecordLength);
        }

        private void ReadHeader()
        {
            //open up file for reading to get metadata located in the header
            FileStream tempFs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4);
            BinaryReader tempBr = new BinaryReader(tempFs, CharacterEncoding);
            tempFs.Position = 0;

            //read record length, which is 32bit integer at postion 0 in the file. (takes up first four bytes).
            RecordLength = tempBr.ReadInt32();

            //close temp reader
            tempFs.Close();
            tempFs.Dispose();
            tempBr.Dispose();
        }

        public void WriteRecord(byte[] data, int recordNumber)
        {
        }

        public byte[] ReadRecord(int recordNumber)
        {
            throw new NotImplementedException();
        }




    }
}
