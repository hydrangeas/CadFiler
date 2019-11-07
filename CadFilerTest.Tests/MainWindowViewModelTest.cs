using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace CadFilerTest.Tests
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cadFileMock = new Mock<ICadFileRepository>();
            var entities = new List<CadFileEntity>();
            entities.Add(
                new CadFileEntity(
                    "test.stl",
                    new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"),
                    1024,
                    1,
                    Convert.ToDateTime("2019/11/07 23:45"),
                    Convert.ToDateTime("2019/11/07 23:45")
                ));
            var viewModel = new MainWindowViewModel(cadFileMock.Object);
            viewModel.CadFiles.Count.Is(2);
        }
    }
}
