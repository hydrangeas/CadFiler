using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using CadFiler.Infrastructure.Azure.BlobStorage;
using CadFiler.Infrastructure.LocalDB;
using CadFiler.Infrastructure.LocalFile;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
            Update();
        }

        public ObservableCollection<MainWindowViewModelCadFile> CadFiles
        { get; set; } = new ObservableCollection<MainWindowViewModelCadFile>();

        public void DragOver(IDropInfo dropInfo) => dropInfo.Effects = DragDropEffects.Copy;

        public void Drop(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Copy;
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            foreach(var file in dragFileList)
            {
                var fileInfo = _cadFile.GetFileInfo(file);
                _cadFileStorage.Upload(fileInfo);
                _cadFileMetadata.Save(
                    new CadFileEntity(
                            fileInfo,
                            GetNewGuid(),
                            CadFiles.Max(x => x.DisplayOrder) + 1,
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
    }
}
