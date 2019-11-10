using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using CadFiler.Infrastructure.LocalDB;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Extensions.FileProviders;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace CadFiler.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDropTarget
    {
        private ICadFileMetadataRepository _cadFile;
        public MainWindowViewModel()
            : this(new CadFiles())
        {

        }
        public MainWindowViewModel(ICadFileMetadataRepository cadFile)
        {
            _cadFile = cadFile;
            foreach (var entity in _cadFile.GetData())
            {
                CadFiles.Add(new MainWindowViewModelCadFile(entity));
            }
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
                var directoryName = Path.GetDirectoryName(file);
                var fileName = Path.GetFileName(file);

                var physicalFileProvider = new PhysicalFileProvider(directoryName);
                CadFiles.Add(
                    new MainWindowViewModelCadFile(
                        new CadFileEntity(
                            physicalFileProvider.GetFileInfo(fileName),
                            GetNewGuid(),
                            CadFiles.Max(x => x.DisplayOrder) + 1,
                            GetDateTime())));
            }
        }
    }
}
