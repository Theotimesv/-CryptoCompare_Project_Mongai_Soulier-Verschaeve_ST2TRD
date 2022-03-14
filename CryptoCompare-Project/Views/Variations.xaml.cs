using System;
using System.Threading;
using System.Windows.Controls;
using Brushes = System.Windows.Media.Brushes;

namespace CryptoCompare_Project.Views
{
    public partial class Variations : UserControl
    {
        public CryptoDifferencePriceDataScrapper _cryptoDifferencePriceDataScrapper;
        private string _currentPriceCrypto1Link;
        private string _currentPriceCrypto2Link;
        private string _initialPriceCrypto1Link;
        private string _initialPriceCrypto2Link;

        public Variations()
        {
            InitializeComponent();
            _cryptoDifferencePriceDataScrapper = new CryptoDifferencePriceDataScrapper();
            _currentPriceCrypto1Link = "";
            _currentPriceCrypto2Link = "";
            _initialPriceCrypto1Link = "";
            _initialPriceCrypto2Link = "";
            Thread updateDataThread = new Thread(AutoUpdateVariations);
            updateDataThread.Start();
        }
        
        public void GetHistoricalData()
        {
            var crypto1 = Crypto1.Text;
            var crypto2 = Crypto2.Text;
            var period = Period.Text;
            var date = DateTime.Now;
            long unixTime = ((DateTimeOffset)date).ToUnixTimeSeconds();
            string toTs = "";

            _currentPriceCrypto1Link = "https://min-api.cryptocompare.com/data/price?fsym=" + crypto1 + "&tsyms=USD";
            _currentPriceCrypto2Link = "https://min-api.cryptocompare.com/data/price?fsym=" + crypto2 + "&tsyms=USD";

            switch (period)
            {
                case "Today":
                    _initialPriceCrypto1Link = "https://min-api.cryptocompare.com/data/v2/histoday?fsym=" + crypto1 + "&tsym=USD&limit=1"; 
                    _initialPriceCrypto2Link = "https://min-api.cryptocompare.com/data/v2/histoday?fsym=" + crypto2 + "&tsym=USD&limit=1";
                    break;
                
                case "1 hour":
                    toTs = (unixTime - 3600).ToString();
                    _initialPriceCrypto1Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym=" + crypto1 + "&tsym=USD&limit=1&toTs=" + toTs;
                    _initialPriceCrypto2Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym=" + crypto2 + "&tsym=USD&limit=1&toTs=" + toTs;
                    break;
                
                case "24 hours":
                    toTs = (unixTime - 86400).ToString();
                    _initialPriceCrypto1Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym=" + crypto1 + "&tsym=USD&limit=1&toTs=" + toTs;
                    _initialPriceCrypto2Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym=" + crypto2 + "&tsym=USD&limit=1&toTs=" + toTs;
                    break;
                
                case "7 days": //7 days - 2 minutes exactly
                    toTs = (unixTime - 604680).ToString();
                    _initialPriceCrypto1Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym=" + crypto1 + "&tsym=USD&limit=1&toTs=" + toTs;
                    _initialPriceCrypto2Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym=" + crypto2 + "&tsym=USD&limit=1&toTs=" + toTs;
                    break;
                
                //En cours
                case "30 days":
                    toTs = (unixTime - 2592000).ToString();
                    _initialPriceCrypto1Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym=" + crypto1 + "&tsym=USD&limit=1&toTs=" + toTs;
                    _initialPriceCrypto2Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym=" + crypto2 + "&tsym=USD&limit=1&toTs=" + toTs;
                    break;

                case "90 days":
                    toTs = (unixTime - 7776000).ToString();
                    _initialPriceCrypto1Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym=" + crypto1 + "&tsym=USD&limit=1&toTs=" + toTs;
                    _initialPriceCrypto2Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym=" + crypto2 + "&tsym=USD&limit=1&toTs=" + toTs;
                    break;

                case "1 year":
                    toTs = (unixTime - 31536000).ToString();
                    _initialPriceCrypto1Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym=" + crypto1 + "&tsym=USD&limit=1&toTs=" + toTs;
                    _initialPriceCrypto2Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym=" + crypto2 + "&tsym=USD&limit=1&toTs=" + toTs;

                    break;
                default:
                    break;
            }
        }
        
        private async void AutoUpdateVariations()
        {
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    UpdateVariations();
                }));
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        private async void UpdateVariations()
        {
            GetHistoricalData();
                    
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1CurrentPrice(_currentPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2CurrentPrice(_currentPriceCrypto2Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1InitialPrice(_initialPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2InitialPrice(_initialPriceCrypto2Link);

            double currentPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1CurrentPrice;
            double currentPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2CurrentPrice;
            double initialPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1OpenPrice;
            double initialPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2OpenPrice;

            var crypto1Variation = (currentPriceCrypto1 - initialPriceCrypto1) / initialPriceCrypto1 * 100;
            var crypto2Variation = (currentPriceCrypto2 - initialPriceCrypto2) / initialPriceCrypto2 * 100;
            var deltaCrypto1Crypto2 = Math.Abs(crypto1Variation - crypto2Variation);

            // Affichage
            if (crypto1Variation >= 0)
            {
                Crypto1Variation.Text = "+ " + crypto1Variation.ToString(".###") + " %";
                Crypto1Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto1Variation.Text = crypto1Variation.ToString(".###") + " %";
                if (crypto1Variation < 0)
                {
                    Crypto1Variation.Background = Brushes.Tomato;
                }
            }

            if (crypto2Variation >= 0)
            {
                Crypto2Variation.Text = "+ " + crypto2Variation.ToString(".###") + " %";
                Crypto2Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto2Variation.Text = crypto2Variation.ToString(".###") + " %";
                if (crypto2Variation < 0)
                {
                    Crypto2Variation.Background = Brushes.Tomato;
                }
            }

            Delta.Text = deltaCrypto1Crypto2.ToString(".###");
            if (deltaCrypto1Crypto2 > 0)
            {
                Delta.Background = Brushes.LimeGreen;
            }

            if (deltaCrypto1Crypto2 < 0)
            {
                Delta.Background = Brushes.Tomato;
            }
        }
        
        private bool crypto1handle = true;
        private bool crypto2handle = true;
        private bool periodhandle = true;
        
        private void Crypto1_DropDownClosed(object sender, EventArgs e) {
            if(crypto1handle)HandleCrypto1();
            crypto1handle = true;
        }
        
        private async void HandleCrypto1()
        {
            GetHistoricalData();
                    
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1CurrentPrice(_currentPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2CurrentPrice(_currentPriceCrypto2Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1InitialPrice(_initialPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2InitialPrice(_initialPriceCrypto2Link);

            double currentPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1CurrentPrice;
            double currentPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2CurrentPrice;
            double initialPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1OpenPrice;
            double initialPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2OpenPrice;

            var crypto1Variation = (currentPriceCrypto1 - initialPriceCrypto1) / initialPriceCrypto1 * 100;
            var crypto2Variation = (currentPriceCrypto2 - initialPriceCrypto2) / initialPriceCrypto2 * 100;
            var deltaCrypto1Crypto2 = Math.Abs(crypto1Variation - crypto2Variation);

            // Affichage
            if (crypto1Variation >= 0)
            {
                Crypto1Variation.Text = "+ " + crypto1Variation.ToString(".###") + " %";
                Crypto1Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto1Variation.Text = crypto1Variation.ToString(".###") + " %";
                if (crypto1Variation < 0)
                {
                    Crypto1Variation.Background = Brushes.Tomato;
                }
            }

            if (crypto2Variation >= 0)
            {
                Crypto2Variation.Text = "+ " + crypto2Variation.ToString(".###") + " %";
                Crypto2Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto2Variation.Text = crypto2Variation.ToString(".###") + " %";
                if (crypto2Variation < 0)
                {
                    Crypto2Variation.Background = Brushes.Tomato;
                }
            }

            Delta.Text = deltaCrypto1Crypto2.ToString(".###");
            if (deltaCrypto1Crypto2 > 0)
            {
                Delta.Background = Brushes.LimeGreen;
            }

            if (deltaCrypto1Crypto2 < 0)
            {
                Delta.Background = Brushes.Tomato;
            }
        }
        
        private void Crypto2_DropDownClosed(object sender, EventArgs e) {
            if(crypto2handle)HandleCrypto2();
            crypto2handle = true;
        }
        
        private async void HandleCrypto2()
        {
            GetHistoricalData();
                    
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1CurrentPrice(_currentPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2CurrentPrice(_currentPriceCrypto2Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1InitialPrice(_initialPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2InitialPrice(_initialPriceCrypto2Link);

            double currentPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1CurrentPrice;
            double currentPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2CurrentPrice;
            double initialPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1OpenPrice;
            double initialPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2OpenPrice;

            var crypto1Variation = (currentPriceCrypto1 - initialPriceCrypto1) / initialPriceCrypto1 * 100;
            var crypto2Variation = (currentPriceCrypto2 - initialPriceCrypto2) / initialPriceCrypto2 * 100;
            var deltaCrypto1Crypto2 = Math.Abs(crypto1Variation - crypto2Variation);

            // Affichage
            if (crypto1Variation >= 0)
            {
                Crypto1Variation.Text = "+ " + crypto1Variation.ToString(".###") + " %";
                Crypto1Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto1Variation.Text = crypto1Variation.ToString(".###") + " %";
                if (crypto1Variation < 0)
                {
                    Crypto1Variation.Background = Brushes.Tomato;
                }
            }

            if (crypto2Variation >= 0)
            {
                Crypto2Variation.Text = "+ " + crypto2Variation.ToString(".###") + " %";
                Crypto2Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto2Variation.Text = crypto2Variation.ToString(".###") + " %";
                if (crypto2Variation < 0)
                {
                    Crypto2Variation.Background = Brushes.Tomato;
                }
            }

            Delta.Text = deltaCrypto1Crypto2.ToString(".###");
            if (deltaCrypto1Crypto2 > 0)
            {
                Delta.Background = Brushes.LimeGreen;
            }

            if (deltaCrypto1Crypto2 < 0)
            {
                Delta.Background = Brushes.Tomato;
            }        }

        private void Period_DropDownClosed(object sender, EventArgs e) {
            if(periodhandle)HandlePeriod();
            periodhandle = true;
        }
        
        private async void HandlePeriod()
        {
            GetHistoricalData();
                    
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1CurrentPrice(_currentPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2CurrentPrice(_currentPriceCrypto2Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto1InitialPrice(_initialPriceCrypto1Link);
            await _cryptoDifferencePriceDataScrapper.scrappingCrypto2InitialPrice(_initialPriceCrypto2Link);

            double currentPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1CurrentPrice;
            double currentPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2CurrentPrice;
            double initialPriceCrypto1 = _cryptoDifferencePriceDataScrapper.crypto1OpenPrice;
            double initialPriceCrypto2 = _cryptoDifferencePriceDataScrapper.crypto2OpenPrice;

            var crypto1Variation = (currentPriceCrypto1 - initialPriceCrypto1) / initialPriceCrypto1 * 100;
            var crypto2Variation = (currentPriceCrypto2 - initialPriceCrypto2) / initialPriceCrypto2 * 100;
            var deltaCrypto1Crypto2 = Math.Abs(crypto1Variation - crypto2Variation);

            // Affichage
            if (crypto1Variation >= 0)
            {
                Crypto1Variation.Text = "+ " + crypto1Variation.ToString(".###") + " %";
                Crypto1Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto1Variation.Text = crypto1Variation.ToString(".###") + " %";
                if (crypto1Variation < 0)
                {
                    Crypto1Variation.Background = Brushes.Tomato;
                }
            }

            if (crypto2Variation >= 0)
            {
                Crypto2Variation.Text = "+ " + crypto2Variation.ToString(".###") + " %";
                Crypto2Variation.Background = Brushes.LimeGreen;
            }
            else
            {
                Crypto2Variation.Text = crypto2Variation.ToString(".###") + " %";
                if (crypto2Variation < 0)
                {
                    Crypto2Variation.Background = Brushes.Tomato;
                }
            }

            Delta.Text = deltaCrypto1Crypto2.ToString(".###");
            if (deltaCrypto1Crypto2 > 0)
            {
                Delta.Background = Brushes.LimeGreen;
            }

            if (deltaCrypto1Crypto2 < 0)
            {
                Delta.Background = Brushes.Tomato;
            }
        }
    }
}