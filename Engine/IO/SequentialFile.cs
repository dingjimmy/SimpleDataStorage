using System;
using System.IO;
using System.IO.Abstractions;

namespace Engine.IO
{
    /// <summary>
    /// Represents a collection of fixed-size records that can be written and read from anywhere in the file using a sequentially issued record number. If the record-number is not 
    /// known when attempting to read a record, the file must be scanned sequentially and every record tested to find the desired one.
    /// </summary>
    public class SequentialFile
    {
        private Stream fileStream;
        private IFileSystem fileSystem;

        /// <summary>
        /// The fully qualified path to the file.
        /// </summary>
        public String FilePath { get; private set; }

        /// <summary>
        /// A flag which indicates weather this file is currently open.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// The length of each record.
        /// </summary>
        public int RecordLength { get; private set; }

        /// <summary>
        /// The total number of active records in the file.
        /// </summary>
        public int RecordCount { get; private set; }

        /// <summary>
        /// Creates a new instance of the SequentialFile class.
        /// </summary>
        public SequentialFile(IFileSystem fileSystem)
        {
            if (fileSystem == null) throw new ArgumentNullException("fileSystem");

            this.fileSystem = fileSystem;

            IsOpen = false;
            RecordLength = 0;
            RecordCount = 0;
        }

        /// <summary>
        /// Creates a new sequential access file to disk.
        /// </summary>
        public void Create(string filePath, int recordlength)
        {
            if (IsOpen) throw new InvalidOperationException("Can't create file. Another file has already been opened by this instance.");
            if (this.fileSystem.File.Exists(filePath)) throw new IOException("Can't create file. It already exists.");

            this.fileStream = this.fileSystem.File.Open(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

            FilePath = filePath;
            RecordLength = recordlength;
            IsOpen = true;

            WriteHeader();
        }

        /// <summary>
        /// Open an existing sequential access file from disk.
        /// </summary>
        public void Open(string filePath)
        {
            if (IsOpen) throw new InvalidOperationException("Can't open file. Another file has already been opened by this instance.");
            if (!this.fileSystem.File.Exists(filePath)) throw new IOException("Can't open file. It doesn't exist");

            this.fileStream = this.fileSystem.File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);          

            ReadHeader();

            FilePath = filePath;
            IsOpen = true;
        }

        /// <summary>
        /// Flushes all data in io buffers to disk, closes the underlying OS file stream and releases any resources.
        /// </summary>
        public void Close()
        {
            
            //flush buffers and close file
            if (this.fileStream != null)
            {
                //this.fileStream.Flush(true);
                this.fileStream.Close();
                this.fileStream.Dispose();

                //set flag to false
                IsOpen = false;
            }
        }

        /// <summary>
        /// Updates the file header with relevent meta data, such as the current record-length.
        /// </summary>
        public void WriteHeader()
        {
            //convert integer to array of 4 bytes
            Byte[] bytes = BitConverter.GetBytes(RecordLength);
            
            //write record length at position 0 of the file.
            fileStream.Position = 0;
            fileStream.Write(bytes, 0, bytes.Length);
            
        }

        /// <summary>
        /// Reads the contents of the file header into memory.
        /// </summary>
        public void ReadHeader()
        {
            Byte[] bytes = null;         

            //read the first four bytes of the file. this is our 32bit integer that designates the length of each record in the file
            this.fileStream.Position = 0;
            this.fileStream.Read(bytes, 0, 4);

            RecordLength = BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Writes a record (in the form of a byte array) to the file at the position specified by the record number.
        /// </summary>
        public void WriteRecord(byte[] data, int recordNumber)
        {
            //check data is correct length
            if (data.Length != RecordLength) throw new ArgumentOutOfRangeException("The length of the provided Byte-Array does not match record length.");

            //calculate offset-position from start of stream where we will write record to
            int offset = (recordNumber * RecordLength) + 4;

            //move to offset postion, write the record and then flush the buffer to ensure data has been written to file
            fileStream.Position = offset;
            fileStream.Write(data,0, RecordLength);
            fileStream.Flush();
        }

        /// <summary>
        /// Reads the contents of the specified record from file into memory.
        /// </summary>
        public byte[] ReadRecord(int recordNumber)
        {
            byte[] data = null;

            //calculate offset-position from start of stream where to where the record starts
            int offset = (recordNumber * RecordLength) + 4;

            //move to offset postion and read the record
            fileStream.Position = offset;
            fileStream.Read(data, 0, RecordLength);
            
            //return record to caller
            return data;
        }

    }
}
