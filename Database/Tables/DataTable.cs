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
            recordLength = CalculateRecordLength(info);

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

        public virtual Record Create(Record newRecord)
        {
            int recNo;
            
            //scan file, looking for next free record
            for (recNo=0; recNo < file.RecordCount; recNo++)
            {
                Byte[] bytes = file.ReadRecord(recNo);
                Record rec = Record.FromBytes(bytes, Fields);
                if (rec.IsDeleted) break;
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

        private static int CalculateRecordLength(FieldInfo[] fields)
        {
            int recLen = 1; //first byte contains IsDeleted flag
            
            foreach (FieldInfo field in fields)
            {
               recLen += field.ByteLength;
            }

            return recLen;
        }

        #endregion

    }

}
