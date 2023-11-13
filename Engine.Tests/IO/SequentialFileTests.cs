using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.IO;
using Moq;
using System.Collections.Generic;

namespace Engine.Tests
{
    [TestClass]
    public class SequentialFileTests
    {
        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        public void Create_OnFileDoesNotExist_CreatesNewFile()
        {
            // Arrange
            var sys = new MockFileSystem();      
            var file = new SequentialFile(sys);

            // Action
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.IsTrue(sys.FileExists(@"C:\foo\bar.dat"));
        }

        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        public void Create_OnFileOpened_SetsIsOpenFlag()
        {
            // Arrange
            var sys = new MockFileSystem();      
            var file = new SequentialFile(sys);

            // Action
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.IsTrue(file.IsOpen);
        }

        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        public void Create_OnFileOpened_SetsRecordLength()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);

            // Action
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert
            Assert.AreEqual(1024, file.RecordLength);
        }

        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        [ExpectedException(typeof(IOException))]
        public void Create_OnFileAlreadyExists_ThrowsIOException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 10));

            // Action
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert - not required; expecting an exception
        }

        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_OnAlreadyOpen_ThrowsInvalidOperationException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);

            // Action - the second call of Create() should throw the exception
            file.Create(@"C:\foo\bar.dat", 1024);
            file.Create(@"C:\foo\bar.dat", 1024);

            // Assert - not required; expecting an exception
        }

        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        public void Open_OnFileOpened_SetsIsOpenFlag()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));

            // Action
            file.Open(@"C:\foo\bar.dat");

            // Assert
            Assert.IsTrue(file.IsOpen);
        }


        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        public void Open_OnFileOpened_SetsRecordLength()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));

            // Action
            file.Open(@"C:\foo\bar.dat");

            // Assert
            Assert.AreEqual(1024, file.RecordLength);
        }

        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        [ExpectedException(typeof(IOException))]
        public void Open_OnFileDoesNotExist_ThrowsIOException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);

            // Action
            file.Open(@"C:\foo\bar.dat");

            // Assert - not required; expecting an exception
        }


        [TestMethod]
        [TestCategory("Engine.IO.SequentialFile")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Open_OnAlreadyOpen_ThrowsInvalidOperationException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.AddFile(@"C:\foo\bar.dat", CreateSequentialFileData(1024, 100));

            // Action - the second call of Open() should throw the exception
            file.Open(@"C:\foo\bar.dat");
            file.Open(@"C:\foo\bar.dat");

            // Assert - not required; expecting an exception
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
