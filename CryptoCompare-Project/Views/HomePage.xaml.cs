using System.Linq;
using System.Windows.Controls;

namespace CryptoCompare_Project.Views
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            HomePageDataScrapper hPdataScrapper = new HomePageDataScrapper();
            hPdataScrapper.scrapDataFunction().ConfigureAwait(true); ;
            CryptoInfo1.ItemsSource = from element in hPdataScrapper.BestCryptoList select new {element.FROMSYMBOL, element.PRICE, element.HIGHDAY, element.LOWDAY, element.LASTVOLUME};
        }
    }
}