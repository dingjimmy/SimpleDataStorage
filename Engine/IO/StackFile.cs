using System;
using System.IO;

namespace Database.IO
{
    /// <summary>
    /// Represents a collection of fixed-size records that are accessed in a "last-in first-out" manner. Records can be pushed (written) and popped (read) from the top of the stack.
    /// </summary>
    class StackFile
    {
        private FileStream fs;
        //private BinaryReader br;
        //private BinaryWriter bw;

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
        /// Factory method to creates a new, empty stack file.
        /// </summary>
        public static StackFile Create(string filepath, int recordlength)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Factory method to open an existing stack file.
        /// </summary>
        public static StackFile Open(string filepath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Flushes all data in io buffers to disk, closes the underlying OS file stream and releases any resources.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Writes a record (in the form of a byte array) to the end of the file.
        /// </summary>
        public void Push(byte[] data)
        {
        }

        /// <summary>
        /// Reads the record at the end of the file and then deletes it.
        /// </summary>
        public byte[] Pop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the record at the end of the file, without deleting it.
        /// </summary>
        public byte[] Peek(int position)
        {
            throw new NotImplementedException();
        }

    }
}
