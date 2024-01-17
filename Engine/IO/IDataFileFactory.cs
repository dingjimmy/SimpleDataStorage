namespace Engine.IO
{
    public interface IDataFileFactory<T> where T : IDataFile
    {
        public T Create(string filePath, int recordlength);

        public T Open(string filepath);
    }
}