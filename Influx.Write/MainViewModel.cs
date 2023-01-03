using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Influx.Write
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<ObjSine> _SineFields = new();
        public ObservableCollection<ObjSine> SineFields { get { return _SineFields; } set { _SineFields = value; OnPropertyChanged(); } }

        private bool _IsRunning = false;
        public bool IsRunning { get { return _IsRunning; } set { _IsRunning = value; OnPropertyChanged(); } }

        private bool _WriteEnable = true;
        public bool WriteEnable { get { return _WriteEnable; } set { _WriteEnable = value; OnPropertyChanged(); } }

        private int _FieldCount = 1000;
        public int FieldCount { get { return _FieldCount; } set { _FieldCount = value; OnPropertyChanged(); } }

        public ICommand IGenerate { get; set; }
        public ICommand IClear { get; set; }
        public ICommand IStart { get; set; }
        public ICommand IStop { get; set; }


        private CancellationTokenSource CancelSource = new();

        public MainViewModel()
        {
            IGenerate = new RelayCommand(() => Generate());
            IClear = new RelayCommand(() => Clear());
            IStart = new RelayCommand(() => Start(), p => !IsRunning && SineFields.Count > 0);
            IStop = new RelayCommand(() => Stop(), p => IsRunning);
            CancelSource = new();

            Influxdata.Instance.FakeInit();
        }

        private void Generate()
        {
            SineFields.Clear();
            for (int i = 0; i < FieldCount; i++)
            {
                string FieldName = $@"Sine{i.ToString("00000")}";
                SineFields.Add(new(FieldName));
            }
        }

        private void Clear()
        {
            Stop();
            SineFields.Clear();
        }

        private void Start()
        {
            CancelSource = new();
            IsRunning = true;
            foreach (var item in SineFields)
            {
                Task.Run(() => item.WriteLoop(CancelSource.Token, WriteEnable));
            }
        }

        private void Stop()
        {
            CancelSource.Cancel();
            IsRunning = false;
            Influxdata.Instance.Flush();
        }
    }
}
