using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Engine.IO;
using Xunit;

namespace Engine.Tests
{
    public class SequentialFileTests
    {
        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Creating_a_sequential_file_adds_file_to_filesystem()
        {
            // Arrange
            var sys = new MockFileSystem();      
            var file = new SequentialFile(sys);

            // Action
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.True(sys.FileExists(@"C:\foo\bar.dat"));
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Create_OnFileOpened_SetsIsOpenFlag()
        {
            // Arrange
            var sys = new MockFileSystem();      
            var file = new SequentialFile(sys);

            // Action
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.True(file.IsOpen);
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Create_OnFileOpened_SetsRecordLength()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);

            // Action
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.Equal(1024, file.RecordLength);
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Create_OnFileAlreadyExists_ThrowsIOException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 10));

            // Act & Assert
            Assert.Throws<IOException>(() => file.Create(@"C:\foo\bar.dat", 1024));
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Create_OnAlreadyOpen_ThrowsInvalidOperationException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            file.Create(@"C:\foo\bar.dat", 1024);

            // Act & Assert - this second call of Create() should throw the exception
            Assert.Throws<InvalidOperationException>(() => file.Create(@"C:\foo\bar.dat", 1024));
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Open_OnFileOpened_SetsIsOpenFlag()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));

            // Action
            file.Open(@"C:\foo\bar.dat");

            // Assert
            Assert.True(file.IsOpen);
        }


        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Open_OnFileOpened_SetsRecordLength()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));

            // Action
            file.Open(@"C:\foo\bar.dat");

            // Assert
            Assert.Equal(1024, file.RecordLength);
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Open_OnFileDoesNotExist_ThrowsIOException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);

            // Act & Assert
            Assert.Throws<IOException>(() => file.Open(@"C:\foo\bar.dat"));
        }


        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Open_OnAlreadyOpen_ThrowsInvalidOperationException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));            
            file.Open(@"C:\foo\bar.dat");  

            // Act & Assert - this second call of Open() should throw the exception
            Assert.Throws<InvalidOperationException>(() => file.Open(@"C:\foo\bar.dat"));
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

            //// fill rest of array with zero's.
            //for(int i = headerSize - 1; i < dataSize; i++)
            //{
            //    data[i] = new byte();
            //}

            return new MockFileData(data);
        }

        #endregion
    }
}
