using System;

namespace Database.Tables
{
    public struct FieldInfo
    {
        public string Name { get; private set; }
        public DataType Type { get; private set; }
        public int Length { get; private set; }
        public int ByteLength { get; private set; }

        public FieldInfo (string fieldName, DataType fieldType, int fieldLength): this()
        {
                       
            //set fieldname and type
            Name = fieldName;
            Type = fieldType;

            //set length depending on the type
            if (Type == DataType.Boolean || Type == DataType.Integer)
                Length = 1;
            else
                Length = fieldLength;

            //set bytelength as the fieldlength mulitplied by the no of bytes for the type. add four additional bytes for TEXT fields to accomodate the string-length value
            if (Type == DataType.Text)
                ByteLength = (Length * Type.Bytes) + 4;
            else
                ByteLength = Length * Type.Bytes;
            
        }
    }
}
