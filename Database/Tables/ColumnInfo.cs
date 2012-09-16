using System;

namespace Database.Tables
{
    class ColumnInfo
    {
        public string Name { get; private set; }
        public DataType Type { get; private set; }
        public int Bytes { get; private set; }
    }
}
