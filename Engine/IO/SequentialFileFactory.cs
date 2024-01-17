using System.IO;
using System.IO.Abstractions;

namespace Engine.IO
{
    public class SequentialFileFactory : IDataFileFactory<SequentialFile>
    {
        private readonly IFileSystem _FileSystem;

        public SequentialFileFactory(IFileSystem fileSystem)
        {
            _FileSystem = fileSystem ?? throw new System.ArgumentNullException(nameof(fileSystem));
        }

        /// <summary>
        /// Creates a new sequential access file on the file system.
        /// </summary>
        public SequentialFile Create(string filePath, int recordlength)
        {
            if (_FileSystem.File.Exists(filePath)) throw new IOException("Can't create file. It already exists.");

            var fileStream = _FileSystem.File.Open(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

            var file = new SequentialFile(fileStream);
            var fileHeader = new SequentialFileHeader(recordlength);

            file.WriteHeader(fileHeader);

            return file;
        }

        /// <summary>
        /// Open an existing sequential access file from the file system.
        /// </summary>
        public SequentialFile Open(string filePath)
        {
            if (!_FileSystem.File.Exists(filePath)) throw new IOException("Can't open file. It doesn't exist");

            var fileStream = _FileSystem.File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            var file = new SequentialFile(fileStream);

            file.ReadHeader();

            return file;
        }
    }
}