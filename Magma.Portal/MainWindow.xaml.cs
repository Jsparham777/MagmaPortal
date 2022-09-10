using System.Windows;

namespace Magma.Portal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //TODO: define this in the view
            DataContext = new WindowViewModel(this);
        }
    }
}
