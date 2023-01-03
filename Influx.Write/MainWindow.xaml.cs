using System.Windows;

namespace Influx.Write
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainViewModel;
        }
    }
}
