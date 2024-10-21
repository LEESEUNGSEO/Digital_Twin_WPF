using Caps_1_.MVVMModel;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Caps_1_.MVVMViewModel
{
    public class Data_GraphVM : BindableBase
    {
        private ObservableCollection<MyDataModel.Storage_Contain> _storageContains;

        public ObservableCollection<MyDataModel.Storage_Contain> StorageContains
        {
            get { return _storageContains; }
            set { SetProperty(ref _storageContains, value); }
        }

        public Data_GraphVM()
        {
            // MyDataModel 인스턴스를 통해 Storage_Contain 데이터를 가져옵니다.
            StorageContains = MyDataModel.Instance.GetAllStorageContains();
        }
    }
}
