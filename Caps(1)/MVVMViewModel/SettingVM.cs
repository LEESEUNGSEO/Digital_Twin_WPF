using Prism.Mvvm;
using Prism.Commands;
using System;
using Caps_1_.MVVMModel;
using System.Collections.ObjectModel;
using System.Windows;
using static Caps_1_.MVVMModel.MyDataModel;

namespace Caps_1_.MVVMViewModel
{
    public class SettingVM : BindableBase
    {
        private readonly MyDataModel _dataModel;
        private DateTime _selectedDate;
        private string _selectedItem;
        private string _selectedStrRel;
        private int _selectedQuantity;
        private DateTime _selectedSetTime;

        public ObservableCollection<string> Items { get; set; }
        public ObservableCollection<string> Str_Rel { get; set; }
        public ObservableCollection<InventoryItem> InventoryItems => _dataModel.InventoryItems;
        public ObservableCollection<InventoryItem> StoreItems { get; set; }
        public ObservableCollection<InventoryItem> ReleaseItems { get; set; }

        public SettingVM()
        {
            _dataModel = MyDataModel.Instance;
            _selectedDate = DateTime.Now;
            DateSelectedCommand = new DelegateCommand<DateTime?>(OnDateSelected);
            AddInventoryItem = new DelegateCommand(ExecuteAddInventoryItem);

            Items = new ObservableCollection<string>
            {
                "- None -",
                "radish",
                "chinese_cabbage",
                "garlic",
                "onion",
                "pepper"
            };

            Str_Rel = new ObservableCollection<string>
            {
                "- None -",
                "입고 예약",
                "출고 예약"
            };

            StoreItems = new ObservableCollection<InventoryItem>();
            ReleaseItems = new ObservableCollection<InventoryItem>();
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    RaisePropertyChanged(nameof(SelectedDay));
                    RaisePropertyChanged(nameof(SelectedMonth));
                    RaisePropertyChanged(nameof(SelectedDayofWeek));
                    LoadInventoryItemsForSelectedDate();
                }
            }
        }

        public int SelectedDay => SelectedDate.Day;
        public int SelectedMonth => SelectedDate.Month;
        public DayOfWeek SelectedDayofWeek => SelectedDate.DayOfWeek;

        public string SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string SelectedStrRel
        {
            get => _selectedStrRel;
            set => SetProperty(ref _selectedStrRel, value);
        }

        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set => SetProperty(ref _selectedQuantity, value);
        }

        public DateTime SelectedSetTime
        {
            get => _selectedSetTime;
            set
            {
                if (SetProperty(ref _selectedSetTime, value))
                {
                    RaisePropertyChanged(nameof(CombinedDateTime));
                }
            }
        }

        public DateTime CombinedDateTime => new DateTime(
            SelectedDate.Year,
            SelectedDate.Month,
            SelectedDate.Day,
            SelectedSetTime.Hour,
            SelectedSetTime.Minute,
            SelectedSetTime.Second
        );

        public DelegateCommand<DateTime?> DateSelectedCommand { get; }
        public DelegateCommand AddInventoryItem { get; }

        private void OnDateSelected(DateTime? date)
        {
            if (date.HasValue)
            {
                SelectedDate = date.Value;
            }
        }

        private void ExecuteAddInventoryItem()
        {
            if (string.IsNullOrEmpty(SelectedItem) || string.IsNullOrEmpty(SelectedStrRel))
            {
                MessageBox.Show("Please select an item, release type, quantity, and set time.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedQuantity == 0)
            {
                MessageBox.Show("Please select quantity", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedSetTime == DateTime.MinValue)
            {
                MessageBox.Show("Please select Set time", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var inventoryItem = new InventoryItem
            {

                Item = SelectedItem,
                StrRel = SelectedStrRel,
                Quantity = SelectedQuantity,
                SetTime = SelectedSetTime,

                LogType = SelectedStrRel,
                LogContent = $"{SelectedItem} {SelectedQuantity}개가 {SelectedStrRel}되었습니다.",
                TargetLocation = "-",
                LogTime = DateTime.Now,
                Notes = "-"
            };

            //log 오브젝트에 데이터 전달
            _dataModel.AddLogItem(inventoryItem);


            if (inventoryItem.StrRel == "입고 예약")
            {
                StoreItems.Add(inventoryItem);
            }
            else if (inventoryItem.StrRel == "출고 예약")
            {
                ReleaseItems.Add(inventoryItem);
            }

            _dataModel.AddInventoryItem(SelectedDate, inventoryItem);

            MessageBox.Show("Item, release type, quantity, and set time added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadInventoryItemsForSelectedDate()
        {
            var inventoryItems = _dataModel.GetInventoryItemsByDate(SelectedDate);

            StoreItems.Clear();
            ReleaseItems.Clear();

            foreach (var item in inventoryItems)
            {
                if (item.StrRel == "입고 예약")
                {
                    StoreItems.Add(item);
                }
                else if (item.StrRel == "출고 예약")
                {
                    ReleaseItems.Add(item);
                }
            }
        }
    }
}
