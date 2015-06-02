using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.IO;

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
        [ExpectedException(typeof(IOException))]
        public void Create_OnFileAlreadyExists_ThrowsIOException()
        {
            // Arrange
            var sys = new MockFileSystem();
            var file = new SequentialFile(sys);
            sys.File.Create(@"C:\foo\bar.dat");

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
    }
}
