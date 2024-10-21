using Caps_1_.MVVMModel;
using System.Windows.Controls;

namespace Caps_1_.MVVMView
{
    public partial class Stimulation : Page
    {
        public Stimulation()
        {
            InitializeComponent();
        }

        // ToggleButton이 체크되었을 때 호출되는 이벤트 핸들러
        private void Change_Manual_Mode_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            // ToggleButton이 체크된 상태일 때 WriteRegister를 호출
            MyDataModel.Instance.WriteRegister(1, 0);
        }

        // ToggleButton이 체크 해제되었을 때 호출되는 이벤트 핸들러
        private void Change_Manual_Mode_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            // ToggleButton이 체크 해제된 상태일 때 WriteRegister를 호출
            MyDataModel.Instance.WriteRegister(1, 2); //
        }


        // 시뮬 입고
        private void incoming_radish_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(6, 1);
        }
        private void incoming_chinesecabbage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(6, 2);
        }

        private void incoming_garlic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(6, 3);
        }

        private void incoming_onion_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(6, 4);
        }

        private void incoming_pepper_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(6, 5);
        }



        // 시뮬 출고
        private void outgoing_radish_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(7, 1);
        }

        private void outgoing_chinesecabbage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(7, 2);
        }



        private void outgoing_garlic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(7, 3);
        }

        private void outgoing_onion_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(7, 4);
        }

        private void outgoing_pepper_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(7, 5);
        }
    }
        
}
