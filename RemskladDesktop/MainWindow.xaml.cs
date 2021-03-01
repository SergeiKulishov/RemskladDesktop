using System;
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

        }

        private void Button_Accums(object sender, RoutedEventArgs e)
        {
            var art = Repository.articlesOfAccums;
            List<Datum> data = Repository.FetchData();
            List<Datum> filtered = new List<Datum>();
            foreach (var article in art)
            {
                foreach (var item in data)
                {
                    if (item.article == article)
                    {
                        filtered.Add(item);
                    }
                }
            }
            ItemList.ItemsSource = filtered.Reverse<Datum>();

        }

        private void Button_DispOrig(object sender, RoutedEventArgs e)
        {
            var art = Repository.articlesOfDisplayOrig;
            List<Datum> data = Repository.FetchData();
            List<Datum> filtered = new List<Datum>();
            foreach (var article in art)
            {
                foreach (var item in data)
                {
                    if (item.article == article)
                    {
                        filtered.Add(item);
                    }
                }
            }
            ItemList.ItemsSource = filtered.Reverse<Datum>();

        }

        private void Button_DispCopy(object sender, RoutedEventArgs e)
        {

        }

        private async void UpdateDB(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, Datum> ItemsFromWarehouse = ConnectionWithRemonline.GetItemByArticle(await ConnectionWithRemonline.GetCollectionOfItems(), Repository.GetAllArticlesOfItemWhatWeNeed());
                Repository.Update(ItemsFromWarehouse);
                UpdateButton.Background = Brushes.Green;
                await Task.Delay(10000);
                UpdateButton.Background = Brushes.White;
            }
            catch (Exception ex)
            {
                UpdateButton.Background = Brushes.Red;
                await Task.Delay(10000);
                UpdateButton.Background = Brushes.White;
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Обновление не удалось");
                Console.WriteLine("Updating is fail");
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
    }
}

