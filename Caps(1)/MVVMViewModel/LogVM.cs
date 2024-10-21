using Caps_1_.MVVMModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Caps_1_.MVVMModel.MyDataModel;

namespace Caps_1_.MVVMViewModel
{
    public class LogVM : BindableBase
    {
        public LogVM()
        {
            // MyDataModel의 LogItems에 바인딩
            LogItems = MyDataModel.Instance.LogItems;
        }

        private ObservableCollection<MyDataModel.InventoryItem> _logItems;
        public ObservableCollection<MyDataModel.InventoryItem> LogItems
        {
            get => _logItems;
            set => SetProperty(ref _logItems, value);
        }
    }
}