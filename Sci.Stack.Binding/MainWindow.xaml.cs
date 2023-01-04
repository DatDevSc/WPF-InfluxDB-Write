using Sci.Stack.Binding.ViewModels;
using System.Windows;

namespace Sci.Stack.Binding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VMMainWindow VMMainWindow { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = VMMainWindow;
        }
    }
}
