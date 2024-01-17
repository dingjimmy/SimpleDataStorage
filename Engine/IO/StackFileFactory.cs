using System.IO.Abstractions;

namespace Engine.IO
{
    public class StackFileFactory : IDataFileFactory<StackFile>
    {
        public StackFileFactory(IFileSystem fileSystem)
        {
        }

        StackFile IDataFileFactory<StackFile>.Create(string filePath, int recordlength)
        {
            throw new System.NotImplementedException();
        }

        StackFile IDataFileFactory<StackFile>.Open(string filepath)
        {
            throw new System.NotImplementedException();
        }
    }
}