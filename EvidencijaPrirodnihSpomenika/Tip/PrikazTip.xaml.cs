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
using System.Windows.Shapes;

namespace EvidencijaPrirodnihSpomenika.Tip
{
    /// <summary>
    /// Interaction logic for PrikazTip.xaml
    /// </summary>
    public partial class PrikazTip : Window
    {
        public PrikazTip()
        {
            InitializeComponent();
        }
        private void ZatvoriButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
