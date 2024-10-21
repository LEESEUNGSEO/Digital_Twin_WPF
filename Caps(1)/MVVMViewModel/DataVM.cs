using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Caps_1_.MVVMModel;
using static Caps_1_.MVVMModel.MyDataModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;


namespace Caps_1_.MVVMViewModel
{
    public class DataVM : BindableBase
    {
        Random random = new Random();
        private SeriesCollection _seriesCollection;
        private List<string> _labels;
        private Func<double, string> _values;
        private double _currentProgress;
        private double _currentProgress2;
        private Point _progressPoint;
        private Point _progressPoint2;
        private bool _isLargeArc;
        private bool _isLargeArc2;
        private string _progressText;
        private string _progressText2;
        private DateTime _selectedDate;
        private int todayIndex = (int)DateTime.Today.DayOfWeek;

        public DataVM()
        {
            DataModel = MyDataModel.Instance;

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "입고량",
                    Values = new ChartValues<int>(),
                    Fill = new SolidColorBrush(Color.FromRgb(216,233,255))
                },
                new ColumnSeries
                {
                    Title = "출고량",
                    Values = new ChartValues<int>(),
                    Fill = new SolidColorBrush(Color.FromRgb(61,122,255))
                },
                new ColumnSeries
                {
                    Title = "폐기량",
                    Values = new ChartValues<int>(),
                    Fill = new SolidColorBrush(Color.FromRgb(11,32,122))
                }
            };

            // Set CurrentProgress directly to 80
            CurrentProgress = 10;
            CurrentProgress2 = 20;

            SelectedDate = DateTime.Now;
            UpdateLabels();
        }

        public MyDataModel DataModel { get; set; }

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set => SetProperty(ref _seriesCollection, value);
        }

        public List<string> Labels
        {
            get => _labels;
            set => SetProperty(ref _labels, value);
        }

        public Func<double, string> Values
        {
            get => _values;
            set => SetProperty(ref _values, value);
        }

        public double CurrentProgress
        {
            get => _currentProgress;
            set
            {
                SetProperty(ref _currentProgress, value);
                UpdateProgress();
            }
        }
        public double CurrentProgress2
        {
            get => _currentProgress2;
            set
            {
                SetProperty(ref _currentProgress2, value);
                UpdateProgress();
            }
        }

        public Point ProgressPoint
        {
            get => _progressPoint;
            set => SetProperty(ref _progressPoint, value);
        }
        public Point ProgressPoint2
        {
            get => _progressPoint2;
            set => SetProperty(ref _progressPoint2, value);
        }

        public bool IsLargeArc
        {
            get => _isLargeArc;
            set => SetProperty(ref _isLargeArc, value);
        }
        public bool IsLargeArc2
        {
            get => _isLargeArc2;
            set => SetProperty(ref _isLargeArc2, value);
        }
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }
        public string ProgressText2
        {
            get => _progressText2;
            set => SetProperty(ref _progressText2, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    UpdateLabels();
                }
            }
        }

        private void UpdateProgress()
        {
            double angle = (CurrentProgress / 100) * 360;
            double radians = (Math.PI / 180) * angle;
            double x = 100 + 90 * Math.Sin(radians);
            double y = 100 - 90 * Math.Cos(radians);


            IsLargeArc = angle >= 180;
            ProgressPoint = new Point(x, y);
            ProgressText = $"{CurrentProgress:F2}%";

            double angle2 = (CurrentProgress2 / 100) * 360;
            double radians2 = (Math.PI / 180) * angle2;
            double x2 = 100 + 90 * Math.Sin(radians2);
            double y2 = 100 - 90 * Math.Cos(radians2);

            IsLargeArc2 = angle2 >= 180;
            ProgressPoint2 = new Point(x2, y2);
            ProgressText2 = $"{CurrentProgress2:F2}%";
        }


        private void UpdateLabels()
        {
            
            if (SelectedDate == default)
            {
                SelectedDate = DateTime.Now;
            }

            DateTime startOfWeek = SelectedDate.AddDays(-((int)SelectedDate.DayOfWeek - (int)DayOfWeek.Wednesday));

            var labels = new List<string>();
            var intakeValues = new ChartValues<int>();
            var releaseValues = new ChartValues<int>();
            var disposeValues = new ChartValues<int>();

            for (int i = 0; i < 7; i++)
            {
                DateTime date = startOfWeek.AddDays(i - 3);
                labels.Add(date.ToString("MMM dd"));

                var itemsList = DataModel.GetInventoryItemsByDate(date);
                var items = new ObservableCollection<InventoryItem>(itemsList);

                int intake = 0;
                int release = 0;
                int dispose = random.Next(1, 7) *10;

                foreach (var item in items)
                {
                    if (item.StrRel == "입고 예약")
                    {
                        intake += item.Quantity;
                    }
                    else if (item.StrRel == "출고 예약")
                    {
                        release += item.Quantity;
                    }
                }

                intakeValues.Add(intake);
                releaseValues.Add(release);
                disposeValues.Add(dispose);
            }

            if (releaseValues.Count > todayIndex && releaseValues[todayIndex] != 0)
            {
                CurrentProgress = (intakeValues[todayIndex] / (double)disposeValues[todayIndex]) * 100;

                CurrentProgress2 = (releaseValues[todayIndex] / (double)disposeValues[todayIndex]) * 100;
            }
            else
            {
                CurrentProgress = 0;
                CurrentProgress2 = 0;
            }
            UpdateProgress();

            Labels = labels;
            SeriesCollection[0].Values = intakeValues;
            SeriesCollection[1].Values = releaseValues;
            SeriesCollection[2].Values = disposeValues;
        }

    }
}
