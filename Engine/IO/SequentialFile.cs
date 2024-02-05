using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Engine.IO
{
    /// <summary>
    /// Represents a collection of fixed-size records that can be written and read from anywhere in the file using a sequentially issued record number. If the record-number is not 
    /// known when attempting to read a record, the file must be scanned sequentially and every record tested to find the desired one.
    /// </summary>
    public class SequentialFile : IDataFile
    {
        private readonly FileSystemStream _FileStream;

        /// <summary>
        /// The fully qualified path to the file.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// The records header data
        /// </summary>
        public SequentialFileHeader Header { get; private set; }

        /// <summary>
        /// Creates a new instance of the SequentialFile class.
        /// </summary>
        public SequentialFile(FileSystemStream fileStream)
        {
            _FileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));

            FilePath = fileStream.Name;
        }

        /// <summary>
        /// Flushes all data in io buffers to disk, closes the underlying OS file stream and releases any resources.
        /// </summary>
        public void Close()
        {
            //flush buffers and close file
            if (_FileStream != null)
            {
                _FileStream.Flush(true);
                _FileStream.Close();
                _FileStream.Dispose();
            }
        }

        /// <summary>
        /// Returns the number of records in the file.
        /// </summary>
        public long RecordCount()
        {
            return _FileStream.Length / Header.RecordLength;
        }

        /// <summary>
        /// Updates the file header with relevent meta data, such as the current record-length.
        /// </summary>
        public void WriteHeader(SequentialFileHeader header)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));

            //convert integer to array of 4 bytes
            Byte[] bytes = BitConverter.GetBytes(header.RecordLength);
            
            //write record length at position 0 of the file.
            _FileStream.Position = 0;
            _FileStream.Write(bytes, 0, bytes.Length);            
        }

        /// <summary>
        /// Reads the contents of the file header into memory.
        /// </summary>
        public void ReadHeader()
        {
            byte[] bytes = new byte[4];         

            //read the first four bytes of the file. this is our 32bit integer that designates the length of each record in the file
            _FileStream.Position = 0;
            _FileStream.Read(bytes, 0, 4);

            var recordLength = BitConverter.ToInt32(bytes, 0);

            Header = new SequentialFileHeader(recordLength);
        }

        /// <summary>
        /// Writes a record (in the form of a byte array) to the file at the position specified by the record number.
        /// </summary>
        public void WriteRecord(byte[] data, int recordNumber)
        {
            //check data is correct length
            if (data.Length != Header.RecordLength) throw new ArgumentOutOfRangeException("The length of the provided Byte-Array does not match record length.");

            //calculate offset-position from start of stream where we will write record to
            int offset = (recordNumber * Header.RecordLength) + 4;

            //move to offset postion, write the record and then flush the buffer to ensure data has been written to file
            _FileStream.Position = offset;
            _FileStream.Write(data,0, Header.RecordLength);
            _FileStream.Flush();
        }

        /// <summary>
        /// Reads the contents of the specified record from file into memory.
        /// </summary>
        public byte[] ReadRecord(int recordNumber)
        {
            byte[] data = null;

            //calculate offset-position from start of stream where to where the record starts
            int offset = (recordNumber * Header.RecordLength) + 4;

            //move to offset postion and read the record
            _FileStream.Position = offset;
            _FileStream.Read(data, 0, Header.RecordLength);
            
            //return record to caller
            return data;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SequentialFileHeader
    {
        /// <summary>
        /// The length of each record.
        /// </summary>
        public int RecordLength { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordLength"></param>
        public SequentialFileHeader(int recordLength)
        {
            RecordLength = recordLength;
        }
    }
}
