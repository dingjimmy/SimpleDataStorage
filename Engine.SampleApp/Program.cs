using System;
using Engine.Tables;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            FieldInfo[] fieldMasterFields = new FieldInfo[4];
            FieldInfo[] objectMasterFields = new FieldInfo[3];

            fieldMasterFields[0] = new FieldInfo("Table", DataType.Text, 128);
            fieldMasterFields[1] = new FieldInfo("Name", DataType.Text, 128);
            fieldMasterFields[2] = new FieldInfo("Type", DataType.Text, 128);
            fieldMasterFields[3] = new FieldInfo("Length", DataType.Integer, 1);

            objectMasterFields[0] = new FieldInfo("Name", DataType.Text, 128);
            objectMasterFields[1] = new FieldInfo("Type", DataType.Text, 128);
            objectMasterFields[2] = new FieldInfo("Name", DataType.Text, 128);

            MasterTable fieldMaster = MasterTable.Create(@".\data", "fields", fieldMasterFields);
            MasterTable objectMaster = MasterTable.Create(@".\data", "objects", objectMasterFields);

            DataTable heroes = DataTable.Create(@".\data", "heroes", fieldMasterFields);
            DataTable villans = DataTable.Create(@".\data", "villans", objectMasterFields);
        }
    }
}
