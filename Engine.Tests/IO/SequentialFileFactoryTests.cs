using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Engine.IO;
using Xunit;

namespace Engine.Tests
{
    public class SequentialFileFactoryTests
    {
        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Creating_a_sequential_file_adds_file_to_filesystem()
        {
            // Arrange
            var sys = new MockFileSystem();
            var factory = new SequentialFileFactory(sys);

            // Action
            factory.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.True(sys.FileExists(@"C:\foo\bar.dat"));
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Creating_a_sequential_file_adds_valid_header()
        {
            // Arrange
            var recordLength = 1024;
            var sys = new MockFileSystem();
            var factory = new SequentialFileFactory(sys);

            // Action
            factory.Create(@"C:\foo\bar.dat", recordLength);

            // Assert header is valid
            var file = sys.GetFile(@"C:\foo\bar.dat");
            var header = ReadSequentialFileHeaderData(file);
            Assert.Equal(recordLength, header.RecordLength);
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Creating_a_sequential_file_fails_if_file_already_exists()
        {
            // Arrange
            var sys = new MockFileSystem();
            var factory = new SequentialFileFactory(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 10));

            // Act & Assert
            Assert.Throws<IOException>(() => factory.Create(@"C:\foo\bar.dat", 1024));
        }      

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Opening_a_sequential_file_loads_a_valid_header()
        {
            // Arrange
            var sys = new MockFileSystem();
            var factory = new SequentialFileFactory(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));

            // Action
            var file = factory.Open(@"C:\foo\bar.dat");

            // Assert
            Assert.Equal(1024, file.Header.RecordLength);
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Opening_a_sequential_file_fails_if_file_does_not_exist()
        {
            // Arrange
            var sys = new MockFileSystem();
            var factory = new SequentialFileFactory(sys);

            // Act & Assert
            Assert.Throws<IOException>(() => factory.Open(@"C:\foo\bar.dat"));
        }


        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Opening_a_sequential_file_fails_if_file_is_already_open()
        {
            // Arrange
            var sys = new MockFileSystem();
            var factory = new SequentialFileFactory(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));            
            factory.Open(@"C:\foo\bar.dat");  

            // Act & Assert - this second call of Open() should throw the exception
            Assert.Throws<InvalidOperationException>(() => factory.Open(@"C:\foo\bar.dat"));
        }


        #region Helpers

        public MockFileData CreateSequentialFileData(int recordLength, int recordCount)
        {
            int headerSize = 4;
            int dataSize = recordCount * recordLength;
            int totalSize = headerSize + dataSize;

            byte[] data = new byte[totalSize];

            // set header
            BitConverter.GetBytes(recordLength).CopyTo(data, 0);

            // set data to empty value
            byte[] emptyData = new byte[dataSize];
            Array.Fill<byte>(emptyData, 1);

            return new MockFileData(data);
        }

        public SequentialFileHeader ReadSequentialFileHeaderData(MockFileData data)
        {
            var recordLength = BitConverter.ToInt32(data.Contents,0);
            
            return new SequentialFileHeader(recordLength);
        }

        #endregion
    }
}
