using SciChart.Charting.Visuals;
using System.Windows;

namespace Sci.Stack.Binding
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SciChartSurface.SetRuntimeLicenseKey("");
        }
    }
}
