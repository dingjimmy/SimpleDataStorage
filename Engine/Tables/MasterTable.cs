using System;
using Database.IO;
using System.Collections.Generic;

namespace Database.Tables
{
    public class MasterTable : DataTable
    {
        public static MasterTable Create(string databasePath, string tableName, FieldInfo[] info)
        {
            //MasterTable table = new MasterTable();
            //int recordLength = 0;

            ////calculate record length
            //foreach (FieldInfo field in info)
            //{
            //    recordLength += field.ByteLength;
            //}

            ////create file to store data in
            //table.Fields = info;
            //table.file = SequentialFile.Create(string.Format(@"{0}\{1}.mst", databasePath, tableName), recordLength);

            ////return reference to new table
            //return table;

            throw new NotImplementedException();
        }

        public static MasterTable Open(string databasePath, string tableName, FieldInfo[] info)
        {
            //MasterTable table = new MasterTable();

            //table.Fields = info;
            //table.file = SequentialFile.Open(string.Format(@"{0}\{1}.mst", databasePath, tableName));

            //return table;

            throw new NotImplementedException();
        }
    }
}
