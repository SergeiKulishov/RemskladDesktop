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
                Repository.Update(ItemsFromWarehouse);
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
                Repository.Add(ItemsFromWarehouse);
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
        
    }
}

