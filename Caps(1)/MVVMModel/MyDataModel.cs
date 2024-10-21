using Caps_1_.MVVMView;
using NModbus;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Media.Converters;
using System.Windows.Threading;
using System.Windows;
using System.Linq;

namespace Caps_1_.MVVMModel
{
    public class MyDataModel : BindableBase
    {
        private static MyDataModel _instance;
        public static MyDataModel Instance => _instance ?? (_instance = new MyDataModel());




        private DispatcherTimer timer;

        private TcpClient _client;
        private IModbusMaster _master;
        private string _ipAddress = "192.168.0.1";
        //private string _ipAddress = "127.0.0.1";
        private int _port = 502;
        private MyDataModel()
        {
            LogItems = new ObservableCollection<InventoryItem>();
            InventoryItems = new ObservableCollection<InventoryItem>();
            _dateInventoryItems = new Dictionary<DateTime, ObservableCollection<InventoryItem>>();
            StorageContains = new ObservableCollection<Storage_Contain>();


            for (int i = 1; i <= 9; i++)
            {
                StorageContains.Add(new Storage_Contain
                {
                    No = i,
                    Item = null,
                    EntryDate = null,
                    ExpirationDate = null
                });
            }
            InitializeTimer();
        }
        

        public int AddStorage(string item)
        {
            bool itemAdded = false; // 아이템이 추가되었는지 확인하는 플래그

            // 배열을 순회하면서 NULL인 요소를 찾아서 값을 설정합니다.
            for (int i = 0; i < 9; i++)
            {
                if (StorageContains[i].Item == null)
                {
                    StorageContains[i].Item = $"{item}";
                    if(StorageContains[i].Item=="pepper")
                        StorageContains[i].ExpirationDate = DateTime.Now.AddMinutes(1);
                    else
                        StorageContains[i].ExpirationDate = DateTime.Now.AddDays(10);
                    StorageContains[i].EntryDate = DateTime.Now;
                    itemAdded = true; // 아이템이 추가됨
                    //MessageBox.Show($"{StorageContains[i].ExpirationDate}");
                    return i; // 값을 설정한 후 배열 순회를 종료합니다.
                }
                if (!itemAdded & i==8)
                {
                    MessageBox.Show("Storage is full. Unable to add the item.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return -1;
                }
            }
            return -1;
        }



        private void InitializeTimer()
        {  
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // modbus 통신 읽어오는 코드
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(IsToggled) // Auto 모드일 경우 실행
            {
                Mstd1 = ReadRegister(1101,1);
                Mstd2 = ReadRegister(1102,1);
                CYC1 = ReadCoil(1103, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC2 = ReadCoil(1104, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC3 = ReadCoil(1105, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC4 = ReadCoil(1106, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC5 = ReadCoil(1107, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC6 = ReadCoil(1108, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC6 = ReadCoil(1109, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC7 = ReadCoil(1110, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CYC8 = ReadCoil(1110, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                CONV_1 = ReadCoil(1101, 1)? "#FF6A6AE9" : "#FFE6E6F0";
                CONV_2 = ReadCoil(1102, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                STOPPER_1 = ReadCoil(1103, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                STOPPER_2 = ReadCoil(1104, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                OUT_CYL = ReadCoil(1105, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                DISPOSE_CYL = ReadCoil(1106, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                GAN_ARM = ReadCoil(1107, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                GAN_ARM_ROT= ReadCoil(1108, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                GAN_ARM_GIP = ReadCoil(1109, 1) ? "#FF6A6AE9" : "#FFE6E6F0";
                X_AXIS_MOTOR = ReadRegister(1101, 1);
                Y_AXIS_MOTOR = ReadRegister(1102, 1);

                var timenow = DateTime.Now;
                
                //이전 시퀀스 초기화
                if(ReadRegister(4, 1) != 0)
                {
                    WriteRegister(4, 0);
                }

                //출고시퀀스
                foreach (var someting in InventoryItems)
                {
                    if (Math.Abs((timenow - someting.SetTime).TotalSeconds) <= 1 && someting.StrRel == "출고 예약")
                    {
                        foreach (var StorageItem in StorageContains)
                        {
                            if (someting.Item.Equals(StorageItem.Item))
                            {
                                WriteRegister(3, (ushort)StorageItem.No);
                                WriteRegister(4, 2);
                                StorageItem.Item = null;
                                StorageItem.EntryDate = null;
                                StorageItem.ExpirationDate = null;

                                //출고 시퀀스 로그남기기
                                var inventoryItem = new InventoryItem()
                                {
                                    LogType = "출고 시퀀스",
                                    LogContent = "상품이 출고됩니다.",
                                    TargetLocation = $"{StorageItem.No}번 컨테이너",
                                    LogTime = DateTime.Now,
                                    Notes = "-"
                                };
                                MyDataModel.Instance.AddLogItem(inventoryItem);

                            }
                        }
                    }
                    else if (Math.Abs((timenow - someting.SetTime).TotalSeconds) <= 1 && someting.StrRel == "입고 예약")
                    {
                        //입고시퀀스

                        switch (someting.Item)
                        {
                            case "radish":
                                WriteRegister(5, 1);
                                break;
                            case "chinese_cabbage":
                                WriteRegister(5, 2);
                                break;
                            case "garlic":
                                WriteRegister(5, 3);
                                break;
                            case "onion":
                                WriteRegister(5, 4);
                                break;
                            case "pepper":
                                WriteRegister(5, 5);
                                break;
                            default:
                                 // 예외 처리용 기본 값
                                break;
                        }

                        WriteRegister(4, 1); //입고시퀀스 명령
                        

                        
                    }
                }

                foreach (var StorageItem in StorageContains)
                {
                    //폐기시퀀스
                    if (StorageItem.ExpirationDate.HasValue && Math.Abs((timenow - StorageItem.ExpirationDate.Value).TotalSeconds) <= 2)
                    {
                        WriteRegister(3, (ushort)StorageItem.No);
                        WriteRegister(4, 3);
                        StorageItem.Item = null;
                        StorageItem.EntryDate = null;
                        StorageItem.ExpirationDate = null;

                        var inventoryItem = new InventoryItem()
                        {
                            LogType = "폐기 시퀀스",
                            LogContent = "상품이 폐기됩니다.",
                            TargetLocation = $"{StorageItem.No}번 컨테이너",
                            LogTime = DateTime.Now,
                            Notes = "-"
                        };
                        MyDataModel.Instance.AddLogItem(inventoryItem);

                    }
                   
                }
                
            }
        }

        public ushort ReadRegister(ushort startAddress, ushort numRegisters)
        {
            try 
            { 
                ushort[] registers = _master.ReadHoldingRegisters(1, startAddress, numRegisters);
                return registers[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading register: {ex.Message}");
                return 100; // 에러 시 기본값 반환
            }
        }

        public bool WriteRegister(ushort startAddress, ushort value)
        {
            try
            {
                if (_master != null)
                {
                    _master.WriteSingleRegister(1, startAddress, value);
                    return true; // 쓰기 성공
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing register: {ex.Message}");
            }
            return false; // 쓰기 실패
        }

          


        public bool ReadCoil(ushort startAddress, ushort numCoils)
        {
            try
            {
                if (_master != null)
                {
                    bool[] coils = _master.ReadCoils(1, startAddress, numCoils);
                    return coils[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading coil: {ex.Message}");
                
            }
            return false;
        }

        public bool WriteCoil(ushort startAddress, bool value)
        {
            try
            {
                if (_master != null)
                {
                    _master.WriteSingleCoil(1, startAddress, value);
                    return true; // 쓰기 성공
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing coil: {ex.Message}");
            }
            return false; // 쓰기 실패
        }


        private bool _isToggled;
        public bool IsToggled
        {
            get { return _isToggled; }
            set
            {
                if (_isToggled != value)
                {
                    _isToggled = value;
                    if (_isToggled)
                    {
                        _client = new TcpClient(_ipAddress, _port);
                        var factory = new ModbusFactory();
                        _master = factory.CreateMaster(_client);
                        Console.WriteLine("Connected to Modbus server.");
                    }
           
                }
            }
        }



        public ObservableCollection<InventoryItem> InventoryItems { get; set; }
        private Dictionary<DateTime, ObservableCollection<InventoryItem>> _dateInventoryItems;



        public void AddInventoryItem(DateTime date, InventoryItem item)
        {
            if (!_dateInventoryItems.ContainsKey(date))
            {
                _dateInventoryItems[date] = new ObservableCollection<InventoryItem>();
            }
            _dateInventoryItems[date].Add(item);
            InventoryItems.Add(item);
        }

        public ObservableCollection<InventoryItem> GetInventoryItemsByDate(DateTime date)
        {
            DateTime dateOnly = date.Date;

            if (_dateInventoryItems.ContainsKey(dateOnly))
            {
                return _dateInventoryItems[dateOnly];
            }
            return new ObservableCollection<InventoryItem>();
        }

        public ObservableCollection<Storage_Contain> StorageContains { get; set; }

        public void AddStorageContain(Storage_Contain item)
        {
            StorageContains.Add(item);
        }

        public ObservableCollection<Storage_Contain> GetAllStorageContains()
        {
            return StorageContains;
        }




        // 로그 아이템들을 저장할 ObservableCollection
        public ObservableCollection<InventoryItem> LogItems { get; private set; }

        public void AddLogItem(InventoryItem item)
        {
            LogItems.Add(item);
        }


        #region get/set부분
        public class InventoryItem : BindableBase
        {
            private string _logtype;
            public string LogType
            {
                get {  return _logtype; }
                set { SetProperty(ref _logtype, value); }
            }

            private string _logcontent;
            public string LogContent
            {
                get { return _logcontent; }
                set { SetProperty(ref _logcontent, value); }
            }

            private string _targetlocation;
            public string TargetLocation
            {
                get { return _targetlocation; }
                set { SetProperty(ref _targetlocation, value); }
            }


            private string _item;
            public string Item
            {
                get { return _item; }
                set { SetProperty(ref _item, value); }
            }

            private string _strRel;
            public string StrRel
            {
                get { return _strRel; }
                set { SetProperty(ref _strRel, value); }
            }

            private int _quantity;
            public int Quantity
            {
                get { return _quantity; }
                set { SetProperty(ref _quantity, value); }
            }

            private DateTime _setTime;
            public DateTime SetTime
            {
                get { return _setTime; }
                set { SetProperty(ref _setTime, value); }
            }

            private DateTime _logtime;
            public DateTime LogTime
            {
                get { return _logtime; }
                set { SetProperty(ref _logtime, value); }
            }

            private string _notes;
            public string Notes
            {
                get { return _notes; }
                set { SetProperty(ref _notes, value); }
            }
        }

        public class Storage_Contain : BindableBase
        {
            private int _no;
            private string _item;
            private DateTime? _expirationDate;
            private DateTime? _entryDate;

            public int No
            {
                get { return _no; }
                set { SetProperty(ref _no, value); }
            }

            public string Item
            {
                get { return _item; }
                set { SetProperty(ref _item, value); }
            }

            public DateTime? ExpirationDate
            {
                get { return _expirationDate; }
                set { SetProperty(ref _expirationDate, value); }
            }

            public DateTime? EntryDate
            {
                get { return _entryDate; }
                set { SetProperty(ref _entryDate, value); }
            }
        }

        private int _mstd1, _mstd2;
        private string _cyc1, _cyc2, _cyc3, _cyc4, _cyc5, _cyc6, _cyc7, _cyc8, conv_1,conv_2;

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
            set { SetProperty(ref _cyc4, value);}
        }

        public string CYC5
        {
            get { return _cyc5; }
            set { SetProperty(ref _cyc5, value);}
        }
        public string CYC6
        {
            get { return _cyc6; }
            set { SetProperty(ref _cyc6, value);}
        }
        public string CYC7
        {
            get { return _cyc7; }
            set { SetProperty(ref _cyc7, value);}
        }
        public string CYC8
        {
            get { return _cyc8; }
            set { SetProperty(ref _cyc8, value);}
        }


        public string CONV_1
        {
            get { return conv_1; }
            set { SetProperty(ref conv_1, value); }
        }

        public string CONV_2
        {
            get { return conv_2; }
            set { SetProperty(ref conv_2, value); }
        }

        private string _stopper1, _stopper2, _outCyl, _disposeCyl, _ganArm, _ganArmRot, _ganArmGip;
        private int x_motor, y_motor;

        public string STOPPER_1
        {
            get { return _stopper1; }
            set { SetProperty(ref _stopper1, value); }
        }

        public string STOPPER_2
        {
            get { return _stopper2; }
            set { SetProperty(ref _stopper2, value); }
        }

        public string OUT_CYL
        {
            get { return _outCyl; }
            set { SetProperty(ref _outCyl, value); }
        }

        public string DISPOSE_CYL
        {
            get { return _disposeCyl; }
            set { SetProperty(ref _disposeCyl, value); }
        }

        public string GAN_ARM
        {
            get { return _ganArm; }
            set { SetProperty(ref _ganArm, value); }
        }

        public string GAN_ARM_ROT
        {
            get { return _ganArmRot; }
            set { SetProperty(ref _ganArmRot, value); }
        }

        public string GAN_ARM_GIP
        {
            get { return _ganArmGip; }
            set { SetProperty(ref _ganArmGip, value); }
        }
        public int X_AXIS_MOTOR
        {
            get { return x_motor; }
            set { SetProperty(ref x_motor, value); }
        }
        public int Y_AXIS_MOTOR
        {
            get { return y_motor; }
            set { SetProperty(ref y_motor, value); }

        }
        #endregion
    }
}
