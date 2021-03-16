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
        }

        private async void InitializeUpdateDB()
        {
            try
            {
                Dictionary<string, Datum> ItemsFromWarehouse = ConnectionWithRemonline.GetItemByArticle(await ConnectionWithRemonline.GetCollectionOfItems(), Repository.GetAllArticlesOfItemWhatWeNeed());
                Repository.Update(ItemsFromWarehouse);
                UpdateButton.Background = Brushes.Green;
                WhenUpdated.Text = $"{DateTime.Now}";
                await Task.Delay(10000);
                UpdateButton.Background = Brushes.White;
            }
            catch (Exception ex)
            {
                UpdateButton.Background = Brushes.Red;
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Обновление не удалось");
                Console.WriteLine("Updating is fail");
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

        private async void CreateDB(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, Datum> ItemsFromWarehouse = ConnectionWithRemonline.GetItemByArticle(await ConnectionWithRemonline.GetCollectionOfItems(), Repository.GetAllArticlesOfItemWhatWeNeed());
                Repository.Add(ItemsFromWarehouse);
                CreateButton.Background = Brushes.Green;
                await Task.Delay(10000);
                CreateButton.Background = Brushes.White;

            }
            catch (Exception ex)
            {
                CreateButton.Background = Brushes.Red;
                await Task.Delay(10000);
                CreateButton.Background = Brushes.White;
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Обновление не удалось");
                Console.WriteLine("Updating is fail");
            }
        }

        private async void UpdateDB(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, Datum> ItemsFromWarehouse = ConnectionWithRemonline.GetItemByArticle(await ConnectionWithRemonline.GetCollectionOfItems(), Repository.GetAllArticlesOfItemWhatWeNeed());
                Repository.Update(ItemsFromWarehouse);
                UpdateButton.Background = Brushes.Green;
                WhenUpdated.Text = $"{DateTime.Now}";

                await Task.Delay(5000);
                UpdateButton.Background = Brushes.White;
            }
            catch (Exception ex)
            {
                UpdateButton.Background = Brushes.Red;
                await Task.Delay(5000);
                UpdateButton.Background = Brushes.White;
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Обновление не удалось");
                Console.WriteLine("Updating is fail");
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

        
    }
}

