using System;
using System.Collections;
using System.Collections.Generic;
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
using RemskladMailer;
using RemskladDesktop.Cashbox;
using RemskladDesktop.Orders;
using Newtonsoft.Json;

namespace RemskladDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateLoop();
            //ReportLoop();
        }

        private async void InitializeUpdateDB()
        {
            try
            {
                Dictionary<string, Datum> ItemsFromWarehouse = ConnectionWithRemonline.GetItemByArticle(await ConnectionWithRemonline.GetCollectionOfItems(), Repository.GetAllArticlesOfItemWhatWeNeed());
                List<Order> ordersFromRemonline = await ConnectionWithRemonline.GetListOfOrders();
                Repository.Update(ItemsFromWarehouse);
                Repository.UpdateOrders(ordersFromRemonline);
                Brush oldcolor = UpdateButton.Background;
                UpdateButton.Background = Brushes.Green;
                WhenUpdated.Content = $"Обновлено в:\n{DateTime.Now}";
                UpdateCashBoxes();

                await Task.Delay(10000);
                UpdateButton.Background = oldcolor;
            }
            catch (Exception ex)
            {
                Brush oldColor = UpdateButton.Background;
                UpdateButton.Background = Brushes.Red;
                Console.WriteLine(ex.StackTrace);
                await Task.Delay(10000);
                UpdateButton.Background = oldColor;
            }
        }

        private async void UpdateLoop()
        {
            while (true)
            {
                InitializeUpdateDB();
                await Task.Delay(1200000);
            }
        }
        
        public async void ReportLoop()
        {
            while (true)
            {
                if (DateTime.Now.Hour.ToString() == "16" )
                {
                    await SendReportOnMail();
                    await Task.Delay(new TimeSpan(0, 30, 0));
                }
                await Task.Delay(new TimeSpan(0,30,0));
            }
        }

        private async void CreateDB(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, Datum> ItemsFromWarehouse = ConnectionWithRemonline.GetItemByArticle(await ConnectionWithRemonline.GetCollectionOfItems(), Repository.GetAllArticlesOfItemWhatWeNeed());
                List<Order> ordersFromRemonline = await ConnectionWithRemonline.GetListOfOrders();
                Repository.Add(ItemsFromWarehouse);
                Repository.AddOrders(ordersFromRemonline);
                Brush oldColor = CreateButton.Background;
                CreateButton.Background = Brushes.Green;
                await Task.Delay(10000);
                CreateButton.Background = oldColor;

            }
            catch (Exception)
            {
                Brush oldColor = CreateButton.Background;
                CreateButton.Background = Brushes.Red;
                await Task.Delay(10000);
                CreateButton.Background = oldColor;
                
            }
        }

        private async void UpdateDB(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, Datum> ItemsFromWarehouse = ConnectionWithRemonline.GetItemByArticle(await ConnectionWithRemonline.GetCollectionOfItems(), Repository.GetAllArticlesOfItemWhatWeNeed());
                Repository.Update(ItemsFromWarehouse);
                Brush oldColor = UpdateButton.Background;
                UpdateButton.Background = Brushes.Green;
                WhenUpdated.Content = $"Обновлено в:\n{DateTime.Now}";
                UpdateCashBoxes();
                await Task.Delay(5000);
                UpdateButton.Background = oldColor;
            }
            catch (Exception ex)
            {
                Brush oldColor = UpdateButton.Background;
                UpdateButton.Background = Brushes.Red;
                await Task.Delay(5000);
                UpdateButton.Background = oldColor;
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void Button_Accums(object sender, RoutedEventArgs e)
        {
            var filtered = Repository.FetchAccumulatorData();
            ItemList.ItemsSource = filtered.Reverse<Datum>();

        }

        private void Button_DispOrig(object sender, RoutedEventArgs e)
        {
            var filtered = Repository.FetchOrigDisplayData();
            ItemList.ItemsSource = filtered.Reverse<Datum>();

        }

        private void Button_DispCopy(object sender, RoutedEventArgs e)
        {
            var filtered = Repository.FetchCopyDisplayData();
            ItemList.ItemsSource = filtered.Reverse<Datum>();

        }

        private void Button_MainCameras(object sender, RoutedEventArgs e)
        {
            var filtered = Repository.FetchMainCamerasData();
            ItemList.ItemsSource = filtered.Reverse<Datum>();
        }

        private async void Button_SendReportOnMail(object sender, RoutedEventArgs e)
        {
          await SendReportOnMail();
        }

        public async Task SendReportOnMail()
        {
            //Reporter.Reporter.DeleteCSVReportAfterSentAsync();
            Reporter.Reporter.DeleteHTMLReportAfterSentAsync();
            await Task.Delay(2000);
            // await Reporter.Reporter.WriteReportInCSVAsync(Reporter.Reporter.CreateReportCSV());
            await Reporter.Reporter.WriteReportInHTMLAsync(Reporter.Reporter.CreateReportHTML());
            //Mailer.SendEmailWithCSVReportAsync(subject: DateTime.Now.ToString()).GetAwaiter();
            Mailer.SendEmailWithHTMLReportAsync(subject: DateTime.Now.ToString()).GetAwaiter();
        }

        private async void UpdateCashBoxes()
        {
            var CashInfo = await ConnectionWithRemonline.GetCashboxInfo();
            string CurrentCash = String.Format("{0:C}", CashInfo[28384].balance);
            Cash.Content = $"  {CurrentCash}";

            string CurrentTerminal = String.Format("{0:C}", CashInfo[28895].balance);
            Terminal.Content = $"  {CurrentTerminal}";
        }

        private void Warehouse_Click(object sender, RoutedEventArgs e)
        {
            WarehousePanel.Visibility = Visibility.Visible;
            ScrollItemList.Visibility = Visibility.Visible;
            OrdersPanel.Visibility = Visibility.Hidden;
            ScrollOrderList.Visibility = Visibility.Hidden;

        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            WarehousePanel.Visibility = Visibility.Hidden;
            ScrollItemList.Visibility = Visibility.Hidden;
            OrdersPanel.Visibility = Visibility.Visible;
            ScrollOrderList.Visibility = Visibility.Visible;
            
        }

        private async void NewOrdersButton_Click(object sender, RoutedEventArgs e)
        {

            var ord = await ConnectionWithRemonline.GetOrders();
            var orderRoot = JsonConvert.DeserializeObject<Orders.Root>(ord);
            List<Order> listorder = orderRoot.data;
            OrderList.ItemsSource = listorder.Where(x => x.status.group == 1);

        }

        private async void ExecutionOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ord = await ConnectionWithRemonline.GetOrders();
            var orderRoot = JsonConvert.DeserializeObject<Orders.Root>(ord);
            List<Order> listorder = orderRoot.data;
            OrderList.ItemsSource = listorder.Where(x => x.status.group == 2);
        }

        private async void DelayedOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ord = await ConnectionWithRemonline.GetOrders();
            var orderRoot = JsonConvert.DeserializeObject<Orders.Root>(ord);
            List<Order> listorder = orderRoot.data;
            OrderList.ItemsSource = listorder.Where(x => x.status.group == 3);
        }

        private async void FinishedOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ord = await ConnectionWithRemonline.GetOrders();
            var orderRoot = JsonConvert.DeserializeObject<Orders.Root>(ord);
            List<Order> listorder = orderRoot.data;
            OrderList.ItemsSource = listorder.Where(x => x.status.group == 4);
        }
    }
}

