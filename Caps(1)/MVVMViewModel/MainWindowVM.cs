using System;
using System.Threading;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Caps_1_.MVVMModel;
using System.Windows;
using System.Windows.Threading;
using System.Reflection;
using static Caps_1_.MVVMModel.MyDataModel;
using System.Collections.ObjectModel;

namespace Caps_1_.MVVMViewModel
{
    public class MainWindowVM : BindableBase
    {
        public ObservableCollection<InventoryItem> LogItems { get; set; }
        private MyDataModel _dataModel;
        private bool _isToggleChecked;
        private string _togglemodetext;


        public MainWindowVM()
        {
            _dataModel = MyDataModel.Instance;

            LogItems = _dataModel.LogItems;

            Mstd1 = _dataModel.Mstd1;
            Mstd2 = _dataModel.Mstd2;

            CYC1 = _dataModel.CYC1;
            CYC2 = _dataModel.CYC2;
            CYC3 = _dataModel.CYC3;
            CYC4 = _dataModel.CYC4;
            CYC5 = _dataModel.CYC5;
            CYC6 = _dataModel.CYC6;
            CYC7 = _dataModel.CYC7;
            CYC8 = _dataModel.CYC8;

            IsToggleChecked = true;

            // Subscribe to model property changes
            _dataModel.PropertyChanged += DataModel_PropertyChanged;
        }



        private void DataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
           {
            if (e.PropertyName == nameof(_dataModel.Mstd1))
            {
                Mstd1 = _dataModel.Mstd1;
            }
            else if (e.PropertyName == nameof(_dataModel.Mstd2))
            {
                Mstd2 = _dataModel.Mstd2;
            }
            
            else if (e.PropertyName== nameof(_dataModel.CYC1))
            {
                CYC1 = _dataModel.CYC1;
            }
            else if (e.PropertyName == nameof(_dataModel.CYC2))
            {
                CYC2 = _dataModel.CYC2;
            }
            else if (e.PropertyName == nameof(_dataModel.CYC3))
            {
                CYC3 = _dataModel.CYC3;
            }
            else if (e.PropertyName == nameof(_dataModel.CYC4))
            {
                CYC4 = _dataModel.CYC4;
            }
            else if (e.PropertyName == nameof(_dataModel.CYC5))
            {
                CYC5 = _dataModel.CYC5;
            }
            else if (e.PropertyName == nameof(_dataModel.CYC6))
            {
                CYC6 = _dataModel.CYC6;
            }
            else if (e.PropertyName == nameof(_dataModel.CYC7))
            {
                CYC7 = _dataModel.CYC7;
            }
            else if (e.PropertyName == nameof(_dataModel.CYC8))
            {
                CYC8 = _dataModel.CYC8;
            }
        }

        public bool IsToggleChecked
        {
            get { return _isToggleChecked; }
            set
            {
                if (SetProperty(ref _isToggleChecked, value))
                {
                    ToggleModeText = value ? "Digital Twin Mode" : "Stimulation Mode";
                    _dataModel.IsToggled = value;
                }
            }
        }

        public string ToggleModeText
        {
            get { return _togglemodetext; }
            set { SetProperty(ref _togglemodetext, value); }
        }

        private int _mstd2, _mstd1;
        private string _cyc1, _cyc2, _cyc3, _cyc4, _cyc5, _cyc6, _cyc7, _cyc8;
        public int Mstd1
        {
            get { return _mstd1; }
            set { SetProperty(ref _mstd1, value); }
        }

        public int Mstd2
        {
            get { return _mstd2; }
            set { SetProperty(ref _mstd2, value); }
        }

      

        public string CYC1
        {
            get { return _cyc1; }
            set { SetProperty(ref _cyc1, value); }
        }
        public string CYC2
        {
            get { return _cyc2; }
            set { SetProperty(ref _cyc2, value); }
        }
        public string CYC3
        {
            get { return _cyc3; }
            set { SetProperty(ref _cyc3, value); }
        }

        public string CYC4
        {
            get { return _cyc4; }
            set { SetProperty(ref _cyc4, value); }
        }

        public string CYC5
        {
            get { return _cyc5; }
            set { SetProperty(ref _cyc5, value); }
        }
        public string CYC6
        {
            get { return _cyc6; }
            set { SetProperty(ref _cyc6, value); }
        }
        public string CYC7
        {
            get { return _cyc7; }
            set { SetProperty(ref _cyc7, value); }
        }
        public string CYC8
        {
            get { return _cyc8; }
            set { SetProperty(ref _cyc8, value); }
        }
    }
}
