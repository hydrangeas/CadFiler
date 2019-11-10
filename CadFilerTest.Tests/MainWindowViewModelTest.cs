using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using CadFiler.UI.ViewModels;
using ChainingAssertion;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CadFilerTest.Tests
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        [TestMethod]
        public void ファイル一覧シナリオ()
        {
            var cadFileMock = new Mock<ICadFileMetadataRepository>();
            var entities = new List<CadFileEntity>();
            entities.Add(
                new CadFileEntity(
                    "test.stl",
                    new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"),
                    1024,
                    1,
                    Convert.ToDateTime("2019/11/07 23:45"),
                    Convert.ToDateTime("2019/11/07 23:46")
                ));
            entities.Add(
                new CadFileEntity(
                    "abc.stl",
                    new Guid("8D3B5BE6-EF75-4FA4-9D9A-FCAA9D8875C1"),
                    512,
                    2,
                    Convert.ToDateTime("2019/11/10 16:24"),
                    Convert.ToDateTime("2019/11/10 16:25")
                ));
            cadFileMock.Setup(x => x.GetData()).Returns(entities);

            var viewModel = new MainWindowViewModel(cadFileMock.Object);
            viewModel.CadFiles.Count.Is(2);

            viewModel.CadFiles[0].LogicalFileName.Is("test.stl");
            viewModel.CadFiles[0].PhysicalFileName.Is(new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"));
            viewModel.CadFiles[0].FileSize.Is(1024);
            viewModel.CadFiles[0].DisplayOrder.Is(1);
            viewModel.CadFiles[0].Created.Is(Convert.ToDateTime("2019/11/07 23:45"));
            viewModel.CadFiles[0].Updated.Is(Convert.ToDateTime("2019/11/07 23:46"));

            viewModel.CadFiles[1].LogicalFileName.Is("abc.stl");
            viewModel.CadFiles[1].PhysicalFileName.Is(new Guid("8D3B5BE6-EF75-4FA4-9D9A-FCAA9D8875C1"));
            viewModel.CadFiles[1].FileSize.Is(512);
            viewModel.CadFiles[1].DisplayOrder.Is(2);
            viewModel.CadFiles[1].Created.Is(Convert.ToDateTime("2019/11/10 16:24"));
            viewModel.CadFiles[1].Updated.Is(Convert.ToDateTime("2019/11/10 16:25"));
        }

        [TestMethod]
        public void ファイルドロップ()
        {
            var cadFileMock = new Mock<ICadFileMetadataRepository>();
            var entities = new List<CadFileEntity>();
            entities.Add(
                new CadFileEntity(
                    "test.stl",
                    new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"),
                    1024,
                    1,
                    Convert.ToDateTime("2019/11/07 23:45"),
                    Convert.ToDateTime("2019/11/07 23:46")
                ));
            cadFileMock.Setup(x => x.GetData()).Returns(entities);

            var viewModelMock = new Mock<MainWindowViewModel>(cadFileMock.Object);
            viewModelMock.Setup(x => x.GetDateTime()).Returns(Convert.ToDateTime("2019/11/10 12:34:56"));
            viewModelMock.Setup(x => x.GetNewGuid()).Returns(new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"));
            var viewModel = viewModelMock.Object;
            viewModel.CadFiles.Count.Is(1);
            viewModel.CadFiles[0].LogicalFileName.Is("test.stl");
            viewModel.CadFiles[0].PhysicalFileName.Is(new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"));
            viewModel.CadFiles[0].FileSize.Is(1024);
            viewModel.CadFiles[0].DisplayOrder.Is(1);
            viewModel.CadFiles[0].Created.Is(Convert.ToDateTime("2019/11/07 23:45"));
            viewModel.CadFiles[0].Updated.Is(Convert.ToDateTime("2019/11/07 23:46"));

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(x => x.Name).Returns("test123.stl");
            fileInfoMock.Setup(x => x.Length).Returns(2048);

            //viewModel.Save();
            viewModel.CadFiles.Add(
                new MainWindowViewModelCadFile(
                    new CadFileEntity(
                        fileInfoMock.Object,
                        new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"),
                        1,
                        Convert.ToDateTime("2019/11/10 12:34:56")
                )));
            viewModel.CadFiles.Count.Is(2);
            viewModel.CadFiles[1].LogicalFileName.Is("test123.stl");
            viewModel.CadFiles[1].PhysicalFileName.Is(new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"));
            viewModel.CadFiles[1].FileSize.Is(2048);
            viewModel.CadFiles[1].DisplayOrder.Is(1);
            viewModel.CadFiles[1].Created.Is(Convert.ToDateTime("2019/11/10 12:34:56"));
            viewModel.CadFiles[1].Updated.Is(Convert.ToDateTime("2019/11/10 12:34:56"));
        }
    }
}
