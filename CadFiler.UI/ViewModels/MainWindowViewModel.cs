using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using CadFiler.Infrastructure.Azure.BlobStorage;
using CadFiler.Infrastructure.LocalDB;
using CadFiler.Infrastructure.LocalFile;
using CadFiler.Infrastructure.log4net;
using CadFiler.UI.Views;
using GongSolutions.Wpf.DragDrop;
using log4net;
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
        private ILoggerRepository _logger;
        public MainWindowViewModel()
            : this(
                  new CadFileStorage(),
                  new CadFiles(),
                  new CadFileMetadata(),
                  new Logger())
        {

        }
        public MainWindowViewModel(
            ICadFileStorageRepository cadFileStorage,
            ICadFileRepository cadFile,
            ICadFileMetadataRepository cadFileMetadata,
            ILoggerRepository logger)
        {
            _cadFileStorage = cadFileStorage;
            _cadFile = cadFile;
            _cadFileMetadata = cadFileMetadata;
            _logger = logger;

            DeleteCommand = new DelegateCommand<Guid?>(DeleteAsync);
            DownloadCommand = new DelegateCommand<ValueTuple<string, Guid>?>(DownloadAsync);
            UpdateCommand = new DelegateCommand(Update);

            UpdateCommand.Execute(null);
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
        public ICommand UpdateCommand { get; private set; }

        public void DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            foreach (var file in dragFileList)
            {
                var fileAttributes = File.GetAttributes(file);
                if (fileAttributes.HasFlag(FileAttributes.Directory))
                {
                    _logger.GetLogger().Info($"[DragOver] {file} (Directory)");
                    dropInfo.Effects = DragDropEffects.None;
                    return;
                }
                _logger.GetLogger().Info($"[DragOver] {file} (File)");
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
                    _logger.GetLogger().Info($"[Drop] {file} is processing..");
                    var physicalFileName = GetNewGuid();
                    await _cadFileStorage.Upload(fileInfo, physicalFileName);
                    _logger.GetLogger().Info($"[Drop] {file} is uploaded");
                    _cadFileMetadata.Save(
                        new CadFileEntity(
                                fileInfo,
                                physicalFileName,
                                CadFiles.Count == 0 ? 1 : CadFiles.Max(x => x.DisplayOrder) + 1,
                                GetDateTime()
                            ));
                    _logger.GetLogger().Info($"[Drop] {file} is registered");
                }
                Update();
            }
            catch(Exception ex)
            {
                _logger.GetLogger().Error($"[Drop] {ex.StackTrace}");
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
                _logger.GetLogger().Info($"[DeleteAsync] {physicalFileName.GetValueOrDefault()} is deleted");
                Update();
            }
            catch (Exception ex)
            {
                _logger.GetLogger().Error($"[DeleteAsync] {ex.StackTrace}");
                await ShowErrorDialog(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void DownloadAsync(ValueTuple<string, Guid>? fileDetail)
        {
            IsBusy = true;
            try
            {
                string savePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    fileDetail.Value.Item1.ToString());
                var physicalFileName = fileDetail.Value.Item2;

                await _cadFileStorage.Download(savePath, physicalFileName);
                _logger.GetLogger().Info($"[DeleteAsync] {physicalFileName} is downloaded");
            }
            catch (Exception ex)
            {
                _logger.GetLogger().Error($"[Download] {ex.StackTrace}");
                await ShowErrorDialog(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
