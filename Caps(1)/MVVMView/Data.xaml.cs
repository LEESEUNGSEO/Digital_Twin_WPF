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
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Media.Effects;

namespace Caps_1_.MVVMView
{
    public partial class Data : Page
    {
        // isDataGraphDisplayed 변수를 클래스 멤버로 선언
        private bool isDataGraphDisplayed = false;

        public Data()
        {
            InitializeComponent();
        }


        private void Button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                                                                                // 클릭하면 그림자 효과를 임시로 제거
                button.Effect = null;
            }
        }

        private void Button_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                                                                                // 클릭을 때면 그림자 효과를 다시 추가
                button.Effect = new DropShadowEffect
                {
                    ShadowDepth = 5,
                    BlurRadius = 10
                };
            }
        }

        private void Go_GraphList(object sender, RoutedEventArgs e)
        {
            if (isDataGraphDisplayed)
            {
                                                                                // 원래 페이지로 돌아감
                List_On_Graph.Content = null;                                   // 기본 페이지로 돌아가기 위해 내용을 비움
                isDataGraphDisplayed = false;
            }
            else
            {
                                                                                // Data_Graph 페이지로 전환
                List_On_Graph.Content = new Data_Graph();
                isDataGraphDisplayed = true;
            }
        }

    }
}
