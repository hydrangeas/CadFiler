using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using CadFiler.UI.ViewModels;
using ChainingAssertion;
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
        public void ファイル一覧シナリオ()
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
            entities.Add(
                new CadFileEntity(
                    "abc.stl",
                    new Guid("8D3B5BE6-EF75-4FA4-9D9A-FCAA9D8875C1"),
                    512,
                    2,
                    Convert.ToDateTime("2019/11/10 16:24"),
                    Convert.ToDateTime("2019/11/10 16:24")
                ));
            cadFileMock.Setup(x => x.GetData()).Returns(entities);

            var viewModel = new MainWindowViewModel(cadFileMock.Object);
            viewModel.CadFiles.Count.Is(2);

            viewModel.CadFiles[0].LogicalFileName.Is("test.stl");
            viewModel.CadFiles[0].PhysicalFileName.Is(new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"));
            viewModel.CadFiles[0].FileSize.Is(1024);
            viewModel.CadFiles[0].DisplayOrder.Is(1);
            viewModel.CadFiles[0].Created.Is(Convert.ToDateTime("2019/11/07 23:45"));
            viewModel.CadFiles[0].Updated.Is(Convert.ToDateTime("2019/11/07 23:45"));
        }
    }
}
