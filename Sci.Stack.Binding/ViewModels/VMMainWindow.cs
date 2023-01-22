
using OCST.OpcUa.Subscription.Helper;
using Sci.Stack.Binding.Models;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sci.Stack.Binding.ViewModels
{
    public class VMMainWindow : BaseViewModel
    {
        private ObservableCollection<NumericAxis> _YAxes = new();
        public ObservableCollection<NumericAxis> YAxes { get { return _YAxes; } set { _YAxes = value; OnPropertyChanged(); } }

        private ObservableCollection<IRenderableSeries> _RenderableSeries = new();
        public ObservableCollection<IRenderableSeries> RenderableSeries { get { return _RenderableSeries; } set { _RenderableSeries = value; OnPropertyChanged(); } }

        private ItemsPanelTemplate _YAxesPanelTemplate = new();
        public ItemsPanelTemplate YAxesPanelTemplate { get { return _YAxesPanelTemplate; } set { _YAxesPanelTemplate = value; OnPropertyChanged(); } }

        private ObservableCollection<ObjDataInfo> _DataInfos = new();
        public ObservableCollection<ObjDataInfo> DataInfos { get { return _DataInfos; } set { _DataInfos = value; OnPropertyChanged(); } }

        public ICommand ITagImport { get; set; }
        public ICommand IStartSubscribe { get; set; }
        public ICommand IStopSubscribe { get; set; }

        public VMMainWindow()
        {
            DataInfos.Clear();
            bool IsGraph = false;
            for (int i = 0; i < 10; i++)
            {
                string DataName = $@"Data{i.ToString("000")}";
                ObjDataInfo NewData = new(DataName, IsGraph);
                NewData.IsGraphChanged += () => ChartUpdate();
                DataInfos.Add(NewData);
                IsGraph = !IsGraph;
            }
            ChartUpdate();
        }

        private void ChartUpdate()
        {

            YAxes.Clear();
            RenderableSeries.Clear();
            int CountGraph = 0;
            int TotalIsGraph = DataInfos.Where(x => x.IsGraph).Count();
            YAxesPanelTemplate = HelperSciChartXaml.GetItemsPanelTemplate(TotalIsGraph);
            var YAxisStyle = Application.Current.FindResource("YAxisStyle") as Style;
            foreach (var item in DataInfos)
            {
                if (item.IsGraph)
                {
                    NumericAxis NewYAxix = new()
                    {
                        Id = item.DataName,
                        AxisTitle = item.DataName,
                        Style = YAxisStyle,
                        Name = item.DataName,
                    };
                    Grid.SetRow(NewYAxix, CountGraph);
                    YAxes.Add(NewYAxix);
                    FastLineRenderableSeries NewSeries = new FastLineRenderableSeries()
                    {
                        YAxisId = item.DataName,
                        DataSeries = item.ChannelDataSeries,
                        Stroke = item.Stroke,
                    };
                    RenderableSeries.Add(NewSeries);
                    CountGraph += 2;
                }
            }

        }
    }
}
