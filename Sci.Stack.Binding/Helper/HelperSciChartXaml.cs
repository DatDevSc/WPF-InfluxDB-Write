using System.Windows.Controls;
using System.Windows.Markup;

namespace OCST.OpcUa.Subscription.Helper
{
    public class HelperSciChartXaml
    {
        public static ItemsPanelTemplate GetItemsPanelTemplate(int Count)
        {
            string xaml = @"<ItemsPanelTemplate  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
            <Grid>
                <Grid.RowDefinitions>
";
            if (Count == 1)
            {
                xaml += @"<RowDefinition Height=""*"" />";
            }
            else
            {
                xaml += @"<RowDefinition Height=""*"" />
                    <RowDefinition Height=""10"" />
";
                for (int i = 0; i < Count - 2; i++)
                {
                    xaml += @"<RowDefinition Height=""*"" />
                    <RowDefinition Height=""10"" />";
                }
                xaml += @"<RowDefinition Height=""*"" />
";
            }
            xaml += @"
                </Grid.RowDefinitions>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width=""Auto"" />
                </Grid.ColumnDefinitions>
            </Grid>
        </ItemsPanelTemplate>";
            return XamlReader.Parse(xaml) as ItemsPanelTemplate;
        }
    }
}
