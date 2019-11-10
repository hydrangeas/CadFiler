using CadFile.Domain.Repositories;
using CadFiler.Infrastructure.LocalDB;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                System.Diagnostics.Debug.WriteLine($"{file}");
            }
        }
    }
}
