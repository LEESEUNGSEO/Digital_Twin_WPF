using System.Windows.Controls;

namespace Caps_1_.MVVMView
{
    public partial class Data_Graph : Page
    {
        public Data_Graph()
        {
            InitializeComponent();
            this.DataContext = new MVVMViewModel.Data_GraphVM();
        }
    }
}
