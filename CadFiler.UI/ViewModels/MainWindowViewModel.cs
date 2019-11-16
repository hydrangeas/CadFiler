using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using CadFiler.Infrastructure.Azure.BlobStorage;
using CadFiler.Infrastructure.LocalDB;
using CadFiler.Infrastructure.LocalFile;
using CadFiler.UI.Views;
using GongSolutions.Wpf.DragDrop;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

            DeleteCommand = new DelegateCommand<Guid?>(DeleteAsync);
            DownloadCommand = new DelegateCommand<ValueTuple<string, Guid>?>(Download);

            Update();
        }

        public bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
            }
        }
        public string errorMessage { get; set; } = string.Empty;

        public ObservableCollection<MainWindowViewModelCadFile> CadFiles
        { get; set; } = new ObservableCollection<MainWindowViewModelCadFile>();

        public ICommand DeleteCommand { get; private set; }
        public ICommand DownloadCommand { get; private set; }

        public void DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            foreach (var file in dragFileList)
            {
                var fileAttributes = File.GetAttributes(file);
                if (fileAttributes.HasFlag(FileAttributes.Directory))
                {
                    dropInfo.Effects = DragDropEffects.None;
                    return;
                }
            }
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public async void Drop(IDropInfo dropInfo)
        {
            IsBusy = true;
            try
            {
                dropInfo.Effects = DragDropEffects.All;
                var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
                foreach (var file in dragFileList)
                {
                    var fileInfo = _cadFile.GetFileInfo(file);
                    var physicalFileName = GetNewGuid();
                    await _cadFileStorage.Upload(fileInfo, physicalFileName);
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
            catch(Exception ex)
            {
                await ShowErrorDialog(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        void Update()
        {
            CadFiles.Clear();
            foreach (var entity in _cadFileMetadata.GetData())
            {
                CadFiles.Add(new MainWindowViewModelCadFile(entity));
            }
        }

        async Task ShowErrorDialog(Exception ex)
        {
            errorMessage = ex.Message;
            await DialogHost.Show(new OkDialog());
            errorMessage = string.Empty;

        }

        public async void DeleteAsync(Guid? physicalFileName)
        {
            if (physicalFileName == null) return;

            IsBusy = true;
            try
            {
                _cadFileMetadata.Delete(physicalFileName.GetValueOrDefault());
                Update();
            }
            catch (Exception ex)
            {
                await ShowErrorDialog(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void Download(ValueTuple<string, Guid>? fileDetail)
        {
            IsBusy = true;
            try
            {
                string savePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    fileDetail.Value.Item1.ToString());
                var physicalFileName = fileDetail.Value.Item2;

                await _cadFileStorage.Download(savePath, physicalFileName);
            }
            catch (Exception ex)
            {
                await ShowErrorDialog(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
