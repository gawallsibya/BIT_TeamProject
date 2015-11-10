using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserWallPaper.Xml;

namespace UserWallPaper
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TimeWindow : Window
    {
        ArrayList TimeRow = new ArrayList();
        public TimeWindow()
        {
            InitializeComponent();

            foreach (UIElement element in time.Children)
            {
                TimeRow.Add(((Label)element).Content.ToString());
            }

            ArrayList timelist = TimeXml.LoadAll();

            LoadDay(timelist);
        }

        private void LoadDay(ArrayList timelist)
        {
            foreach (string[] temp in timelist)
            {
                switch (temp[0])
                {
                    case "Mon": SetDay(Mon, temp[1], temp[2], temp[3]); break;
                    case "Tue": SetDay(Tue, temp[1], temp[2], temp[3]); break;
                    case "Wed": SetDay(Wed, temp[1], temp[2], temp[3]); break;
                    case "Thu": SetDay(Thu, temp[1], temp[2], temp[3]); break;
                    case "Fri": SetDay(Fri, temp[1], temp[2], temp[3]); break;
                    case "Sat": SetDay(Sat, temp[1], temp[2], temp[3]); break;
                }
            }
        }

        private void SetDay(Grid Day,string sub,string start,string end)
        {
            int startidx = TimeRow.IndexOf(start);
            int endidx = TimeRow.IndexOf(end) - startidx;

            CreateRowDefinition(Day);

            Label l = new Label();
            l.Style = (Style)this.Resources["CommonLabel"];

            TextBlock tb = new TextBlock();
            tb.TextWrapping = TextWrapping.WrapWithOverflow;
            tb.Text = sub;

            l.Content = tb;

            l.SetValue(Grid.RowProperty, startidx);
            l.SetValue(Grid.RowSpanProperty, endidx + 1);

            l.ContextMenu = (ContextMenu)this.Resources["context"];

            Day.Children.Add(l);
        }

        private void Mon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)e.Source;

            if (e.ClickCount == 2 && element is Grid)
            {
                Grid Day = sender as Grid;

                Setting set = new Setting();
                set.SetData(Day.Name, TimeRow);
                if (set.ShowDialog() == true)
                {
                    string start = set.StartTime;
                    string end = set.EndTime;

                    int startidx = TimeRow.IndexOf(start);
                    int endidx = TimeRow.IndexOf(end) - startidx;

                    CreateRowDefinition(Day);
                    
                    Label l = new Label();
                    l.Style = (Style)this.Resources["CommonLabel"];
                    
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.WrapWithOverflow;
                    tb.Text = set.Sub;
                    tb.Margin = new Thickness(0, 0, 0, 0);

                    l.Content = tb;

                    l.SetValue(Grid.RowProperty, startidx);
                    l.SetValue(Grid.RowSpanProperty, endidx+1);

                    l.ContextMenu = (ContextMenu)this.Resources["context"];

                    if (!CheckReCreate(Day, startidx, endidx))
                        return;
                    

                    Day.Children.Add(l);

                    TimeXml.Save(Day.Name,set.Sub,start,end,set.Book);
                }              
            }
        }

        private bool CheckReCreate(Grid Day, int startidx, int endidx)
        {
            foreach (Label temp in Day.Children)
            {
                int st = (int)temp.GetValue(Grid.RowProperty);
                int ed = st + (int)temp.GetValue(Grid.RowSpanProperty) - 1;
                endidx = endidx + startidx;

                if (st == startidx || ed == endidx)
                    return false;
                else if ((st <= startidx && ed >= startidx) || (st <= endidx && ed >= endidx))
                    return false;
                else if ((st >= startidx && ed <= startidx) || (st >= endidx && ed <= endidx))
                    return false;
            }
            return true;
        }

        private void CreateRowDefinition(Grid Day)
        {
            while (true)
            {
                if (Day.RowDefinitions.Count == TimeRow.Count)
                    break;

                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = new GridLength(50, GridUnitType.Star);
                Day.RowDefinitions.Add(rowdef);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu context = ((MenuItem)e.OriginalSource).Parent as ContextMenu;

            Label l = (Label)context.PlacementTarget;
            Grid Day = l.Parent as Grid;
            string sub = ((TextBlock)l.Content).Text;

            Day.Children.Remove(l);
            TimeXml.Remove(sub);
        }        
    }
}
