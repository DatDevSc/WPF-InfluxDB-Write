using InfluxDB.Client;
using InfluxDB.Client.Writes;
using System;

namespace Influx.Write
{
    public class Influxdata
    {
        private static Influxdata? _Instance;
        public static Influxdata Instance { get { if (_Instance == null) { _Instance = new Influxdata(); } return _Instance; } set { _Instance = value; } }

        private InfluxDBClient IfClient;
        private WriteOptions IfOption;
        private WriteApi IfWriteApi;
        string Host = "http://127.0.0.1:8086";
        string Username = "admin";
        string Password = "Sbwrm2580*";
        string Org = "OCST";
        string Bucket = "OPCUA";

        public Influxdata()
        {
            IfClient = new InfluxDBClient(Host, Username, Password);
            IfOption = new()
            {
                BatchSize = 200000,
                FlushInterval = 10000,
                RetryInterval = 2000,
                MaxRetries = 3
            };
            IfWriteApi = IfClient.GetWriteApi(IfOption);
            IfWriteApi.EventHandler += (sender, eventArgs) =>
            {
                switch (eventArgs)
                {
                    // success response from server
                    case WriteSuccessEvent successEvent:
                        Console.WriteLine("WriteSuccessEvent: point was successfully written to InfluxDB");
                        Console.WriteLine($"  - {successEvent.LineProtocol}");
                        Console.WriteLine(
                            "-----------------------------------------------------------------------");
                        break;

                    // unhandled exception from server
                    case WriteErrorEvent errorEvent:
                        Console.WriteLine($"WriteErrorEvent: {errorEvent.Exception.Message}");
                        Console.WriteLine($"  - {errorEvent.LineProtocol}");
                        Console.WriteLine(
                            "-----------------------------------------------------------------------");
                        break;

                    // retrievable error from server
                    case WriteRetriableErrorEvent error:
                        Console.WriteLine($"WriteErrorEvent: {error.Exception.Message}");
                        Console.WriteLine($"  - {error.LineProtocol}");
                        Console.WriteLine(
                            "-----------------------------------------------------------------------");
                        break;

                    // runtime exception in background batch processing
                    case WriteRuntimeExceptionEvent error:
                        Console.WriteLine($"WriteRuntimeExceptionEvent: {error.Exception.Message}");
                        Console.WriteLine(
                            "-----------------------------------------------------------------------");
                        break;
                }
            };
        }

        public void FakeInit() { }

        public void Flush() { IfWriteApi.Flush(); }

        public void WriteBatchPoint(string SourceName, string? Group1, string? Group2, string DisplayName, DateTime EventTime, object Value)
        {
            PointData Point_ = PointData.Measurement(SourceName).Field(DisplayName, Value).Timestamp(EventTime, InfluxDB.Client.Api.Domain.WritePrecision.Ns);
            if (!string.IsNullOrEmpty(Group1))
                Point_ = Point_.Tag("GROUP1", Group1);
            if (!string.IsNullOrEmpty(Group2))
                Point_ = Point_.Tag("GROUP2", Group2);
            IfWriteApi.WritePoint(point: Point_, bucket: Bucket, org: Org);
        }
    }
}
