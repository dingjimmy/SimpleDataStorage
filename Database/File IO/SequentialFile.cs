using System;
using System.IO;
using System.Text;

namespace Database.IO
{
    public class SequentialFile
    {
        public String FilePath { get; private set; }
        public bool IsOpen { get; private set; }
        public int RecordLength { get; private set; }
        public int RecordCount { get; private set; }

        private FileStream fs;
        //private BinaryReader br;
        //private BinaryWriter bw;

        public SequentialFile()
        {
            IsOpen = false;
            RecordLength = 0;
            RecordCount = 0;
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

            //open file & init stream
            file.fs = new FileStream(file.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, file.RecordLength);

            //set isopen flag to true
            file.IsOpen = true;

            //return file to caller
            return file;
        }

        public void Close()
        {
            
            //flush buffers and close file
            if (fs != null)
            {
                fs.Flush(true);
                fs.Close();
                fs.Dispose();

                //set isopen flag to false
                IsOpen = false;
            }
        }

        private void WriteHeader()
        {
            //convert integer to array of 4 bytes
            Byte[] bytes = BitConverter.GetBytes(RecordLength);
            
            //write record length at position 0 of the file.
            fs.Position = 0;
            fs.Write(bytes, 0, bytes.Length);
            
        }

        private void ReadHeader()
        {
            Byte[] bytes = null;

            //open up file for reading to get metadata located in the header
            FileStream tempFs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4);
            

            //read the first four bytes of the file. this is our 32bit integer that designates the length of each record in the file
            tempFs.Position = 0;
            fs.Read(bytes, 0, 4);

            //set recordlength property
            RecordLength = BitConverter.ToInt32(bytes, 0);

            //close file
            tempFs.Close();
            tempFs.Dispose();
        }

        public void WriteRecord(byte[] data, int recordNumber)
        {
            //check data is correct length
            if (data.Length != RecordLength) throw new ArgumentOutOfRangeException("The length of the provided Byte-Array does not match record length.");

            //calculate offset-position from start of stream where we will write record to
            int offset = (recordNumber * RecordLength) + 4;

            //move to offset postion, write the record and then flush the buffer to ensure data has been written to file
            fs.Position = offset;
            fs.Write(data,0, RecordLength);
            fs.Flush();

        }

        public byte[] ReadRecord(int recordNumber)
        {
            byte[] data = null;

            //calculate offset-position from start of stream where to where the record starts
            int offset = (recordNumber * RecordLength) + 4;

            //move to offset postion and read the record
            fs.Position = offset;
            fs.Read(data, 0, RecordLength);
            
            //return record to caller
            return data;
        }




    }
}
