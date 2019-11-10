using CadFile.Domain.Repositories;
using CadFiler.Infrastructure.LocalDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CadFiler.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ICadFileMetadataRepository _cadFile;
        public MainWindowViewModel()
            :this(new CadFiles())
        {

        }
        public MainWindowViewModel(ICadFileMetadataRepository cadFile)
        {
            _cadFile = cadFile;
            foreach(var entity in _cadFile.GetData())
            {
                CadFiles.Add(new MainWindowViewModelCadFile(entity));
            }
        }

        public ObservableCollection<MainWindowViewModelCadFile> CadFiles
        { get; set; } = new ObservableCollection<MainWindowViewModelCadFile>();
    }
}
