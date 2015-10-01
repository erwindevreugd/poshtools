﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerShellTools.Common;

namespace PowerShellTools.Explorer
{
    internal class PSParameterEditorViewModel : ViewModel, IHostWindowContent
    {
        private readonly IHostWindow _hostWindow;
        private readonly IDataProvider _dataProvider;
        private readonly IExceptionHandler _exceptionHandler;

        private IPowerShellCommand _command;
        private CommandModel _commandModel;
        private PSParameterEditorOptionSet _options = new PSParameterEditorOptionSet();
        private string _commandPreview = string.Empty;
        private int _selectedIndex = 0;
        private string _selectedItem = string.Empty;
        private bool _isBusy;

        public PSParameterEditorViewModel(IHostWindow hostWindow, IDataProvider dataProvider, IExceptionHandler exceptionHandler)
        {
            _hostWindow = hostWindow;
            _dataProvider = dataProvider;
            _exceptionHandler = exceptionHandler;

            _options.Options.ForEach(x => x.PropertyChanged += OnOptionChanged);

            ViewDetailsCommand = new ViewModelCommand(this, ViewDetails);
            CopyCommand = new ViewModelCommand(this, Copy);
            CancelCommand = new ViewModelCommand(this, Cancel);
        }

        public ViewModelCommand ViewDetailsCommand { get; set; }
        public ViewModelCommand CopyCommand { get; set; }
        public ViewModelCommand CancelCommand { get; set; }

        public List<OptionModel> Options
        {
            get
            {
                return _options.Options;
            }
        }

        public void LoadCommand(IPowerShellCommand command)
        {
            _isBusy = true;
            _command = command;
            _dataProvider.GetCommandMetaData(_command, GetCommandMetadataCallback);
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IPowerShellCommand Info
        {
            get
            {
                return _command;
            }

            set
            {
                if (_command != value)
                {
                    _command = value;
                    RaisePropertyChanged();
                }
            }
        }

        public CommandModel Model
        {
            get
            {
                return _commandModel;
            }

            set
            {
                if (_commandModel != value)
                {
                    _commandModel = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CommandPreview
        {
            get
            {
                return _commandPreview;
            }

            set
            {
                if (_commandPreview != value)
                {
                    _commandPreview = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                _selectedItem = value;
                this.Model.SelectParameterSetByName(_selectedItem);
                UpdateCommandPreview();
            }
        }

        private void GetCommandMetadataCallback(IPowerShellCommandMetadata result)
        {
            Model = CommandModelFactory.GenerateCommandModel(result);

            if (Model != null)
            {
                _hostWindow.SetCaption(Model.Name);
                UpdateCommandPreview();
                Model.PropertyChanged += OnCommandModelPropertyChanged;
            }

            IsBusy = false;
        }

        private void OnCommandModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCommandPreview();
        }

        private void OnOptionChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCommandPreview();
        }

        private void UpdateCommandPreview()
        {
            try
            {
                CommandFormatterOptions options = new CommandFormatterOptions()
                {
                    FormateStyle = _options.FormatAsHashTable ? CommandFormatStyle.HashTable : CommandFormatStyle.Inline,
                    ParameterSet = _selectedItem
                };

                CommandPreview = CommandFormatter.FormatCommandModel(Model, options);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }

        private void ViewDetails()
        {
            var window = new PSCommandDetails(_dataProvider, _command);
            window.Show();
        }

        private void Copy()
        {
            try
            {
                // TODO: Get the value to copy directly from the command preview box
                CommandFormatterOptions options = new CommandFormatterOptions()
                {
                    FormateStyle = _options.FormatAsHashTable ? CommandFormatStyle.HashTable : CommandFormatStyle.Inline,
                    ParameterSet = _selectedItem
                };

                var command = CommandFormatter.FormatCommandModel(Model, options);

                ClipboardHelper.SetText(command);
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex);
            }
        }

        private void Cancel()
        {
            _hostWindow.ShowCommandExplorer();
        }

        internal void Activated()
        {
        }

        void IHostWindowContent.Activated()
        {
        }
    }
}
