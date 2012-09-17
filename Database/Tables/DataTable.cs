using System;
using Database.IO;
using System.Collections.Generic;

namespace Database.Tables
{
    public class DataTable
    {
        protected SequentialFile file;
        protected FieldInfo[] Fields;

        #region Static Factory Methods

        public static DataTable Create(string databasePath, string tableName, FieldInfo[] info)
        {
            DataTable table = new DataTable();
            int recordLength = 0;

            //calculate record length
            foreach (FieldInfo field in info)
            {
                recordLength += field.ByteLength;
            }

            //create file to store data in
            table.Fields = info;
            table.file = SequentialFile.Create(string.Format(@"{0}\{1}.dat", databasePath, tableName), recordLength);

            //return reference to new table
            return table;
        }

        public static DataTable Open(string databasePath, string tableName, FieldInfo[] info)
        {
            DataTable table = new DataTable();

            table.Fields = info;
            table.file = SequentialFile.Open(string.Format(@"{0}\{1}.dat", databasePath, tableName));

            return table;
        }

        #endregion

        #region Maintenance Methods

        public virtual void Close()
        {
            if (file.IsOpen) file.Close();
        }

        #endregion

        #region CRUD Methods

        public virtual Record Create(Record data)
        {
            int recPos;
            Byte[] bytes;

            //scan file, looking for next free record
            for (recPos=0; recPos < file.RecordCount; recPos++)
            {
                bytes = file.ReadRecord(recPos);

            }

            //convert record into byte array

            //write byte array to file

        }

        public virtual IEnumerable<Record> Retrieve()
        {
            throw new NotImplementedException();
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Helper Methods


        #endregion

    }

}
