using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCompare_Project
{
    public class HomePageDataScrapper
    {
        public List<USD> BestCryptoList { get; set; }

        public HomePageDataScrapper()
        {
            BestCryptoList = new List<USD>();
        }

        public async Task scrapDataFunction()
        {
            string url_best_20_cryptos = "https://min-api.cryptocompare.com/data/top/mktcapfull?limit=20&tsym=USD";
            string key = "0edc1384280b50ac53679b94991868fb11fca894abeaf40290cbe2548199599f";

            using (var web1 = new HttpClient())
            {
                web1.DefaultRequestHeaders.Add("Apikey", key);

                var response = web1.GetAsync(url_best_20_cryptos).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<BestCryptoData>(jsonString);
                    for (int i = 0; i< result.Data.Count; i++)
                    {
                        USD usd = result.Data[i].RAW.USD;
                        BestCryptoList.Add(usd);
                    }
                }
            }
        }
    }

    public class CryptoRatesDataScrapper
    {
        public List<double> crypto1ClosePrices { get; set; }
        public List<DateTime> crypto1Dates { get; set; }
        public List<double> crypto2ClosePrices { get; set; }
        public List<DateTime> crypto2Dates { get; set; }
        
        public CryptoRatesDataScrapper()
        {
            crypto1ClosePrices = new List<double>();
            crypto1Dates = new List<DateTime>();
            crypto2ClosePrices = new List<double>();
            crypto2Dates = new List<DateTime>();
        }
        
        public async Task ScrapDataCrypto1Function(string cryptoLink)
        {
            crypto1ClosePrices = new List<double>();
            crypto1Dates = new List<DateTime>();
            string key = "0edc1384280b50ac53679b94991868fb11fca894abeaf40290cbe2548199599f";
            
            using (var web2 = new HttpClient())
            {
                web2.DefaultRequestHeaders.Add("Apikey", key);
                var response = web2.GetAsync(cryptoLink).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    
                    var result = JsonConvert.DeserializeObject<CryptoHistDataRoot>(jsonString);
                    for (int i = 0; i < result.Data.Data.Count; i++)
                    {
                        crypto1ClosePrices.Add(result.Data.Data[i].close);
                        var dateTime = ConvertFromUnixTimestamp(result.Data.Data[i].time);
                        crypto1Dates.Add(dateTime);
                    }
                }
            }
        }

        public async Task ScrapDataCrypto2Function(string cryptoLink)
        {
            crypto2ClosePrices = new List<double>();
            crypto2Dates = new List<DateTime>();
            string key = "0edc1384280b50ac53679b94991868fb11fca894abeaf40290cbe2548199599f";

            using (var web3 = new HttpClient())
            {
                web3.DefaultRequestHeaders.Add("Apikey", key);
                var response = web3.GetAsync(cryptoLink).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<CryptoHistDataRoot>(jsonString);
                    for (int i = 0; i < result.Data.Data.Count; i++)
                    {
                        crypto2ClosePrices.Add(result.Data.Data[i].close);
                        var dateTime = ConvertFromUnixTimestamp(result.Data.Data[i].time);
                        crypto2Dates.Add(dateTime);
                    }
                }
            }
        }
        
        static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            // Format our new DateTime object to start at the UNIX Epoch
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            // Add the timestamp (number of seconds since the Epoch) to be converted
            dateTime = dateTime.AddSeconds(timestamp);

            return dateTime;
        }
    }
    
    public class CryptoDifferencePriceDataScrapper
    {
        public double crypto1CurrentPrice { get; set; }
        public double crypto2CurrentPrice { get; set; }
        public double crypto1OpenPrice { get; set; }
        public double crypto2OpenPrice { get; set; }

        public async Task scrappingCrypto1CurrentPrice(string cryptoLink)
        {
            string key = "0edc1384280b50ac53679b94991868fb11fca894abeaf40290cbe2548199599f";

            using (var web = new HttpClient())
            {
                web.DefaultRequestHeaders.Add("Apikey", key);

                var response = web.GetAsync(cryptoLink).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<CurrentPriceUSD>(jsonString);
                    crypto1CurrentPrice = result.USD;
                }
            }
        }

        public async Task scrappingCrypto2CurrentPrice(string cryptoLink)
        {
            string key = "0edc1384280b50ac53679b94991868fb11fca894abeaf40290cbe2548199599f";

            using (var web = new HttpClient())
            {
                web.DefaultRequestHeaders.Add("Apikey", key);

                var response = web.GetAsync(cryptoLink).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<CurrentPriceUSD>(jsonString);
                    crypto2CurrentPrice = result.USD;
                }
            }
        }

        public async Task scrappingCrypto1InitialPrice(string cryptoLink)
        {
            string key = "0edc1384280b50ac53679b94991868fb11fca894abeaf40290cbe2548199599f";

            using (var web = new HttpClient())
            {
                web.DefaultRequestHeaders.Add("Apikey", key);

                var response = web.GetAsync(cryptoLink).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<CryptoHistDataRoot>(jsonString);

                    List<double> listOpen = new List<double>();
                    for (int i = 0; i < result.Data.Data.Count; i++)
                    {
                        var openPrice = result.Data.Data[i].open;
                        listOpen.Add(openPrice);
                    }

                    crypto1OpenPrice = listOpen.Last();
                }
            }
        }

        public async Task scrappingCrypto2InitialPrice(string cryptoLink)
        {
            string key = "0edc1384280b50ac53679b94991868fb11fca894abeaf40290cbe2548199599f";

            using (var web = new HttpClient())
            {
                web.DefaultRequestHeaders.Add("Apikey", key);

                var response = web.GetAsync(cryptoLink).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<CryptoHistDataRoot>(jsonString);

                    List<double> listOpen = new List<double>();
                    for (int i = 0; i < result.Data.Data.Count; i++)
                    {
                        var openPrice = result.Data.Data[i].open;
                        listOpen.Add(openPrice);
                    }

                    crypto2OpenPrice = listOpen.Last();
                }
            }
        }
    }
}