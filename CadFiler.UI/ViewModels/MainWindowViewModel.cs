using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using CadFiler.Infrastructure.Azure.BlobStorage;
using CadFiler.Infrastructure.LocalDB;
using CadFiler.Infrastructure.LocalFile;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CadFiler.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDropTarget
    {
        private ICadFileStorageRepository _cadFileStorage;
        private ICadFileRepository _cadFile;
        private ICadFileMetadataRepository _cadFileMetadata;
        public MainWindowViewModel()
            : this(
                  new CadFileStorage(),
                  new CadFiles(),
                  new CadFileMetadata())
        {

        }
        public MainWindowViewModel(
            ICadFileStorageRepository cadFileStorage,
            ICadFileRepository cadFile,
            ICadFileMetadataRepository cadFileMetadata)
        {
            _cadFileStorage = cadFileStorage;
            _cadFile = cadFile;
            _cadFileMetadata = cadFileMetadata;

            DeleteCommand = new DelegateCommand<Guid?>(Delete);
            DownloadCommand = new DelegateCommand<ValueTuple<string, Guid>?>(Download);

            Update();
        }

        public ObservableCollection<MainWindowViewModelCadFile> CadFiles
        { get; set; } = new ObservableCollection<MainWindowViewModelCadFile>();

        public ICommand DeleteCommand { get; private set; }
        public ICommand DownloadCommand { get; private set; }

        public void DragOver(IDropInfo dropInfo) => dropInfo.Effects = DragDropEffects.Copy;

        public void Drop(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Copy;
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            foreach(var file in dragFileList)
            {
                var fileInfo = _cadFile.GetFileInfo(file);
                var physicalFileName = GetNewGuid();
                _cadFileStorage.Upload(fileInfo, physicalFileName);
                _cadFileMetadata.Save(
                    new CadFileEntity(
                            fileInfo,
                            physicalFileName,
                            CadFiles.Count == 0 ? 1 : CadFiles.Max(x => x.DisplayOrder) + 1,
                            GetDateTime()
                        ));
            }
            Update();
        }

        void Update()
        {
            CadFiles.Clear();
            foreach (var entity in _cadFileMetadata.GetData())
            {
                CadFiles.Add(new MainWindowViewModelCadFile(entity));
            }
        }

        public void Delete(Guid? physicalFileName)
        {
            if (physicalFileName == null) return;

            _cadFileMetadata.Delete(physicalFileName.GetValueOrDefault());
            Update();
        }

        public void Download(ValueTuple<string, Guid>? fileDetail)
        {
            string savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                fileDetail.Value.Item1.ToString());
            var physicalFileName = fileDetail.Value.Item2;

            _cadFileStorage.Download(savePath, physicalFileName);
        }
    }
}
