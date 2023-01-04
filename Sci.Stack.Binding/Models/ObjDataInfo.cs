using Sci.Stack.Binding.ViewModels;
using SciChart.Charting.Model.DataSeries;
using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sci.Stack.Binding.Models
{
    public class ObjDataInfo : BaseViewModel
    {
        public string DataName { get; set; }

        private bool _IsGraph = false;
        public bool IsGraph { get { return _IsGraph; } set { _IsGraph = value; OnPropertyChanged(); IsGraphChanged?.Invoke(); } }

        private double _Value = 0;
        public double Value { get { return _Value; } set { _Value = value; OnPropertyChanged(); } }

        private Color _Stroke = Colors.White;
        public Color Stroke { get { return _Stroke; } set { _Stroke = value; OnPropertyChanged(); } }

        private XyDataSeries<DateTime, double>? _ChannelDataSeries;
        public XyDataSeries<DateTime, double>? ChannelDataSeries { get { return _ChannelDataSeries; } set { _ChannelDataSeries = value; OnPropertyChanged(); } }

        private DispatcherTimer ValueTimer = new();
        private int Count = 0;
        public Action? IsGraphChanged;

        public ObjDataInfo(string DataName_, bool IsGraph_)
        {
            DataName = DataName_;
            IsGraph = IsGraph_;
            ChannelDataSeries = new XyDataSeries<DateTime, double> { FifoCapacity = 100, AcceptsUnsortedData = true };
            ValueTimer.Tick += ValueTimer_Tick;
            ValueTimer.Interval = TimeSpan.FromMilliseconds(10);
            ValueTimer.Start();
        }

        private void ValueTimer_Tick(object? sender, EventArgs e)
        {
            Value = Sinewave[Count];
            ChannelDataSeries?.Append(DateTime.UtcNow, Value);
            Count++;
            if (Count >= Sinewave.Count) Count = 0;
        }
    }
}
