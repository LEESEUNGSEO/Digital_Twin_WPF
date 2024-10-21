using Caps_1_.MVVMViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Caps_1_.MVVMView
{
    /// <summary>
    /// Setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Setting : Page
    {
        public Setting()
        {
            InitializeComponent();
        }
        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;
            var viewModel = DataContext as SettingVM;

            if (calendar != null && viewModel != null)
            {
                viewModel.DateSelectedCommand.Execute(calendar.SelectedDate);
            }
        }
        private void In_Note_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextNote.Focus();
        }

        private void TextNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // TextNote.Text가 null이거나 빈 문자열인지 확인합니다.
            if (!string.IsNullOrEmpty(TextNote.Text))
            {
                // TextNote.Text가 null이 아니고 빈 문자열이 아니라면, In_Note의 가시성을 Collapsed로 설정합니다.
                In_Note.Visibility = Visibility.Collapsed;
            }
            else
            {
                // TextNote.Text가 null이거나 빈 문자열이면, In_Note의 가시성을 Visible로 설정합니다.
                In_Note.Visibility = Visibility.Visible;
            }
        }

        private void In_Time_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextTime.Focus();
        }

        private void TextTime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
            if (!string.IsNullOrEmpty(TextTime.Text))
            {
                In_Time.Visibility = Visibility.Collapsed;
            }
            else
            {
                In_Time.Visibility = Visibility.Visible;
            }
        }

        private void SR_Changed(object sender, SelectionChangedEventArgs e)
        {
            if(Str_Rel.SelectedIndex == 0) 
            {
                Str_Rel.Text= string.Empty;     /*None 고르면 없어짐*/
            }
        }

        private void Select_item(object sender, SelectionChangedEventArgs e)
        {
            if (choose_item.SelectedIndex == 0)
            {
                choose_item.Text = string.Empty;     /*None 고르면 없어짐*/
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
