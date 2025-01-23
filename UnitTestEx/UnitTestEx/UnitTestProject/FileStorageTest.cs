using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for FileStorageTest
    /// </summary>
    [TestClass]
    public class FileStorageTest
    {
        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";

        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = @"D:\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;
        //public FileStorage storage = new FileStorage(NEW_SIZE);

        ///* ПРОВАЙДЕРЫ */

        //static object[] NewFilesData =
        //{
        //    new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
        //    new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) },
        //    new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        //};

        private FileStorage storage;

        [SetUp]
        public void Setup()
        {
            storage = new FileStorage(NEW_SIZE);
        }

        /* Провайдеры данных */
        public static IEnumerable<File> NewFilesData
        {
            get
            {
                yield return new File("testfile1.txt", Convert.ToString(1024)); // Пример файла с именем и размером
                yield return new File("testfile2.txt", Convert.ToString(2048)); // Другой пример файла
            }
        }


        static object[] FilesForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING },
            new object[] { null, TIC_TOC_TOE_STRING }
        };

        static object[] NewExceptionFileData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };

        /* Тестирование записи файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void WriteTest(File file)
        {
            Assert.IsTrue(storage.Write(file));
            storage.DeleteAllFiles();
        }

        /* Тестирование записи дублирующегося файла */
        [Test, TestCaseSource(nameof(NewExceptionFileData))]
        public void WriteExceptionTest(File file)
        {
            bool isException = false;
            try
            {
                storage.Write(file);
                Assert.IsFalse(storage.Write(file));
            }
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            Assert.IsTrue(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void IsExistsTest(File file)
        {
            string name = file.GetFilename();
            Assert.IsFalse(storage.IsExists(name));
            storage.Write(file);
            Assert.IsTrue(storage.IsExists(name));
            storage.DeleteAllFiles();
        }

        /* Тестирование удаления файла */
        [Test, TestCaseSource(nameof(FilesForDeleteData))]
        public void DeleteTest(File file, string fileName)
        {
            //storage.Write(file);
            //Assert.IsTrue(storage.Delete(fileName));

            if (file != null)
            {
                storage.Write(file);
                Assert.IsTrue(storage.Delete(fileName));
            }
            else
            {
                Assert.IsFalse(storage.Delete(fileName));
            }
        }

        /* Тестирование получения файлов */
        [Test]
        public void GetFilesTest()
        {
            foreach (File el in storage.GetFiles())
            {
                Assert.IsNotNull(el);
            }
        }

        /* Тестирование получения файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void GetFileTest(File expectedFile)
        {
            //storage.Write(expectedFile);
            //File actualfile = storage.GetFile(expectedFile.GetFilename());
            //bool difference = actualfile.GetFilename().Equals(expectedFile.GetFilename()) && actualfile.GetSize().Equals(expectedFile.GetSize());
            //Assert.IsFalse(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize()));

            storage.Write(expectedFile);
            File actualFile = storage.GetFile(expectedFile.GetFilename());
            Assert.IsNotNull(actualFile, $"Файлa <{expectedFile.GetFilename()}> нет в хранилище.");//проверяем файл на наличие записей
            bool difference = actualFile.GetFilename() != expectedFile.GetFilename() || actualFile.GetSize() != expectedFile.GetSize();
            Assert.IsFalse(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize()));
        }

        // Тест для проверки записи пустого файла
        [Test]
        public void WriteEmptyFileTest()
        {
            var emptyFile = new File("empty.txt", string.Empty);
            Assert.IsTrue(storage.Write(emptyFile));
            Assert.IsTrue(storage.IsExists(emptyFile.GetFilename()));
            storage.DeleteAllFiles();
        }

        // Тест для получения списка файлов
        [Test]
        public void GetFilesCountTest()
        {
            var file1 = new File("file1.txt", CONTENT_STRING);
            var file2 = new File("file2.txt", CONTENT_STRING);
            storage.Write(file1);
            storage.Write(file2);

            var files = storage.GetFiles();
            Assert.AreEqual(2, files.Count, "Количество файлов в хранилище не совпадает.");

            storage.DeleteAllFiles();
        }

        // Тест для проверки, что файл не существует после его удаления
        [Test]
        public void IsExistsAfterDeleteTest()
        {
            var file = new File("testfile.txt", CONTENT_STRING);
            storage.Write(file);
            Assert.IsTrue(storage.IsExists(file.GetFilename()));

            storage.Delete(file.GetFilename());
            Assert.IsFalse(storage.IsExists(file.GetFilename()), "Файл все еще существует после удаления.");
        }
    }
}
