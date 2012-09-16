using System;

namespace Database.Tables
{
    public struct DataType
    {
        public static DataType Boolean = new DataType() { Description = "A Boolean (True or False/Yes or No/On or Off) value.", Bytes = 1 };
        public static DataType Integer = new DataType() { Description = "A signed 32-bit (4-byte) integer ranging in value from -2,147,483,648 through 2,147,483,647. ", Bytes = 4 };

        public string Description { get; private set; }
        public int Bytes { get; private set; }
    }
}
