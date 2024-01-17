using System;
using System.IO;
using System.IO.Abstractions;
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
            var factory = new SequentialFileFactory(sys);

            // Action
            factory.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.True(sys.FileExists(@"C:\foo\bar.dat"));
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void A_newly_created_sequential_file_has_valid_header()
        {
            // Arrange
            var sys = new MockFileSystem();
            var factory = new SequentialFileFactory(sys);
            factory.Create(@"C:\foo\bar.dat", 1024);

            // Action
            var file = factory.Open(@"C:\foo\bar.dat");

            // Assert header is valid
            Assert.Equal(1024, file.Header.RecordLength);
        }

        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Creating_a_sequential_file_fails_if_file_already_exists()
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
            var file = new SequentialFile(sys);

            // Act & Assert
            Assert.Throws<IOException>(() => file.Open(@"C:\foo\bar.dat"));
        }


        [Fact]
        [Trait("Catagory","Engine.IO.SequentialFile")]
        public void Opening_a_sequential_file_fails_if_file_is_already_open()
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

        public object ReadSequentialFileData()
        {
            
        }

        #endregion
    }
}
