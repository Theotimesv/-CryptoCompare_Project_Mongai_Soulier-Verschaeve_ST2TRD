using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace CryptoCompare_Project.Views
{
    public partial class CryptoRates : UserControl
    {
        private readonly CryptoRatesDataScrapper _crDataScrapper;
        private string _crypto1Link;
        private string _crypto2Link;

        private Boolean _isNotRate = true;
        //private Semaphore sem = new Semaphore(1, 1);

        public CryptoRates()
        {
            InitializeComponent();
            _crDataScrapper = new CryptoRatesDataScrapper();
            _crypto1Link = "";
            _crypto2Link = "";
            Crypto1.SelectedIndex = 1;
            Crypto2.SelectedIndex = 1;
            _isNotRate = false;
            
            Thread updateDataThread = new Thread(UpdateData);
            Thread updatePlotThread = new Thread(UpdatePlot);
            updateDataThread.Start();
            updatePlotThread.Start();
            
        }
        
        private async Task GetHistoricalData()
        {
            //sem.WaitOne();
            var crypto1 = Crypto1.Text;
            var crypto2 = Crypto2.Text;
            var period = Period.Text;
            if (crypto1 == "")
            {
                crypto1 = "BTC";
            }
            if (crypto2 == "")
            {
                crypto2 = "ETH";
            }
            if (period == "")
            {
                period = "Daily";
            }

            if (crypto2 == "USD")
            {
                _isNotRate = true;
            }
            
            int limit = 0;
           
            switch (period)
            {
                case "Daily":
                    limit = 1440;
                    _crypto1Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym="+crypto1+"&tsym=EUR&limit="+limit;
                    _crypto2Link = "https://min-api.cryptocompare.com/data/v2/histominute?fsym="+crypto2+"&tsym=EUR&limit="+limit;
                    break;
                case "Weekly":
                    limit = 168;
                    _crypto1Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym="+crypto1+"&tsym=EUR&limit="+limit;
                    _crypto2Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym="+crypto2+"&tsym=EUR&limit="+limit;
                    break;
                case "Monthly":
                    limit = 720;
                    _crypto1Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym="+crypto1+"&tsym=EUR&limit="+limit;
                    _crypto2Link = "https://min-api.cryptocompare.com/data/v2/histohour?fsym="+crypto2+"&tsym=EUR&limit="+limit;
                    break;
                case "Yearly":
                    limit = 364;
                    _crypto1Link = "https://min-api.cryptocompare.com/data/v2/histoday?fsym="+crypto1+"&tsym=EUR&limit="+limit;
                    _crypto2Link = "https://min-api.cryptocompare.com/data/v2/histoday?fsym="+crypto2+"&tsym=EUR&limit="+limit;
                    break;
                default:
                    //A FAIRE
                    break;
            }

            //sem.Release();
        }

        private void UpdateData()
        {
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    //sem.WaitOne();
                    await GetHistoricalData();
                    await _crDataScrapper.ScrapDataCrypto1Function(_crypto1Link);
                    await _crDataScrapper.ScrapDataCrypto2Function(_crypto2Link);
                    //sem.Release();
                }));
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
        
        private void UpdatePlot()
        {
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action( async () =>
                {
                    PlotingFunction();
                }));
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        private void PlotingFunction()
        {
            if (_isNotRate)
            {
                double[] axis = _crDataScrapper.crypto1Dates.Select(x => x.ToOADate()).ToArray();
                WpfPlot1.Plot.Clear();
                WpfPlot1.Plot.Title("Rate evolution");
                if (_crDataScrapper.crypto1ClosePrices[_crDataScrapper.crypto1ClosePrices.Count - 1] - _crDataScrapper.crypto1ClosePrices[_crDataScrapper.crypto1ClosePrices.Count - 2] > 0)
                {
                    WpfPlot1.Plot.PlotScatter(axis, _crDataScrapper.crypto1ClosePrices.ToArray(), color: Color.LimeGreen, markerSize: 0);
                }
                else
                {
                    WpfPlot1.Plot.PlotScatter(axis, _crDataScrapper.crypto1ClosePrices.ToArray(), color: Color.Red, markerSize: 0);
                }
            
                WpfPlot1.Plot.XAxis.DateTimeFormat(true);
                WpfPlot1.Plot.XLabel("Time (" + Period.Text + ")");
                WpfPlot1.Plot.YLabel(Crypto1.Text+" / "+Crypto2.Text);
                WpfPlot1.Refresh();
            }
            else
            {
                //sem.WaitOne();
                double[] rate = new double[_crDataScrapper.crypto1ClosePrices.Count];
                for (int valueIndex = 0; valueIndex < _crDataScrapper.crypto1ClosePrices.Count; valueIndex++)
                {
                    rate[valueIndex] = _crDataScrapper.crypto1ClosePrices[valueIndex] /
                                       _crDataScrapper.crypto2ClosePrices[valueIndex];
                }

                double[] axis = _crDataScrapper.crypto1Dates.Select(x => x.ToOADate()).ToArray();

                WpfPlot1.Plot.Clear();
                WpfPlot1.Plot.Title("Rate evolution");
                if (rate[rate.Length - 1] - rate[rate.Length - 2] > 0)
                {
                    WpfPlot1.Plot.PlotScatter(axis, rate, color: Color.LimeGreen, markerSize: 0);
                }
                else
                {
                    WpfPlot1.Plot.PlotScatter(axis, rate, color: Color.Red, markerSize: 0);
                }

                WpfPlot1.Plot.XAxis.DateTimeFormat(true);
                WpfPlot1.Plot.XLabel("Time (" + Period.Text + ")");
                WpfPlot1.Plot.YLabel(Crypto1.Text + " / " + Crypto2.Text);
                WpfPlot1.Refresh();
                //sem.Release();
            }
        }
        
        
        //https://stackoverflow.com/questions/16966264/what-event-handler-to-use-for-combobox-item-selected-selected-item-not-necessar
        private bool crypto1handle = true;
        private bool crypto2handle = true;
        private bool periodhandle = true;
        
        private void Crypto1_DropDownClosed(object sender, EventArgs e) {
            if(crypto1handle)HandleCrypto1();
            crypto1handle = true;
        }
        
        private async void HandleCrypto1() {
            
            await GetHistoricalData();
            await _crDataScrapper.ScrapDataCrypto1Function(_crypto1Link);
            await _crDataScrapper.ScrapDataCrypto2Function(_crypto2Link);
            PlotingFunction();
        }
        
        private void Crypto2_DropDownClosed(object sender, EventArgs e) {
            if(crypto2handle)HandleCrypto2();
            crypto2handle = true;
        }
        
        private async void HandleCrypto2() {
            
            await GetHistoricalData();
            await _crDataScrapper.ScrapDataCrypto1Function(_crypto1Link);
            await _crDataScrapper.ScrapDataCrypto2Function(_crypto2Link);
            PlotingFunction();
        }

        private void Period_DropDownClosed(object sender, EventArgs e) {
            if(periodhandle)HandlePeriod();
            periodhandle = true;
        }
        
        private async void HandlePeriod() {
            
            await GetHistoricalData();
            await _crDataScrapper.ScrapDataCrypto1Function(_crypto1Link);
            await _crDataScrapper.ScrapDataCrypto2Function(_crypto2Link);
            PlotingFunction();
        }
        
    }
}