using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Tables
{
    public class Record
    {

        public bool IsDeleted { get; private set; }
        public object[] Data { get; private set; }

        public static Record FromBytes(byte[] bytes, FieldInfo[] fields)
        {

            //init
            int byteOffset = 0;
            Record record = new Record();
            record.Data = new object[fields.Length];

            //get IsDeleted byte
            record.IsDeleted = BitConverter.ToBoolean(bytes, byteOffset);
            byteOffset += 1;

            //get data for all fields
            for(int i=0; i < fields.Length; i++)
            {
                //DataType type = fields[i].Type;
                //int byteLen = fields[i].ByteLength;
                //int Len = fields[i].Length;

                //if (type == DataType.Boolean)
                //{
                //    record.Data[i] = BitConverter.ToBoolean(bytes, byteOffset);
                //    byteOffset += byteLen;                
                //                    }
                //else if (type == DataType.Integer)
                //{
                //    record.Data[i] = BitConverter.ToInt32(bytes, byteOffset);
                //    byteOffset += byteLen;  
                //}
                //else if (type == DataType.Text)
                //{
                //    StringBuilder sb = new StringBuilder(Len);
                //    Encoding.UTF32.

                //        = BitConverter.ToBoolean(bytes, byteOffset);
                //    byteOffset += byteLen;  
                //}
                //else
                //{
                //}
            }

            //return record to caller
            return record;
        }

        public Byte[] ToBytes()
        {
            throw new NotImplementedException();
        }
    }
}
