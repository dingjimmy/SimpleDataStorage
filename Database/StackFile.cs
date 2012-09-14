using System;

namespace Database
{
    class StackFile
    {
        public String FilePath { get; private set; }
        public bool IsOpen { get; private set; }
        public int RecordLength { get; private set; }

        public static StackFile Create(string filepath, int recordlength)
        {
            throw new NotImplementedException();
        }

        public static StackFile Open(string filepath)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
        }

        public void Push(byte[] data, int position)
        {
        }

        public byte[] Pop(int position)
        {
            throw new NotImplementedException();
        }

    }
}
