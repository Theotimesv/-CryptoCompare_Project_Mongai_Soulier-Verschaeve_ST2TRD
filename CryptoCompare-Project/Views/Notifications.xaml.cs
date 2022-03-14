using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CryptoCompare_Project.Views
{
    public partial class Notifications : UserControl
    {
        private List<Notification> _notifList;
        private List<Notification> _updatedNotifList;
        public static int NotifCounter = 0;
        private readonly object _notifListLock = new object();
        
        public Notifications()
        {
            InitializeComponent(); ;
            _notifList = new List<Notification>();
            _updatedNotifList = new List<Notification>();
            Thread checkNotifThread = new Thread(CheckNotifs);
            //Thread dataUpdateThread = new Thread(DataUpdate);
            checkNotifThread.Start();
            //dataUpdateThread.Start();
        }
        
        private void CheckNotifs()
        {
            while (true)
            {
                while (_notifList.Count != 0)
                {
                    lock (_notifListLock)
                    {
                        _notifList = _updatedNotifList;
                        Thread.Sleep(TimeSpan.FromSeconds(8));
                        this.Dispatcher.BeginInvoke(new Action(async () =>
                        {
                            var notifListSize = _notifList.Count;
                            for (int notifIndex = 0; notifIndex < notifListSize; notifIndex++)
                            {
                                _notifList[notifIndex].GetHistoricalData();
                                await _notifList[notifIndex].CheckDelta();

                                if (_notifList[notifIndex].IsDeltaReached)
                                {
                                    Thread.Sleep(TimeSpan.FromSeconds(5));
                                    MessageBox.Show("Delta: " + _notifList[notifIndex].Delta + " for the ratio: " +
                                                    _notifList[notifIndex].Crypto1 + "/" +
                                                    _notifList[notifIndex].Crypto2 + " on period: " +
                                                    _notifList[notifIndex].Period + " has been reached.");
                                    _updatedNotifList.Remove(_notifList[notifIndex]);
                                    ActiveNotifications.ItemsSource = null;
                                    ActiveNotifications.ItemsSource = _updatedNotifList;
                                }
                            }
                        }));
                        //Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));
            
        }
        
        private void AddNotifClick(object sender, RoutedEventArgs e)
        {
            
            var crypto1 = Crypto1.Text;
            var crypto2 = Crypto2.Text;
            bool typeCheck = Double.TryParse(Delta.Text, out var delta);

            if (typeCheck == true)
            {
                var period = Period.Text;
                Notification notif = new Notification(NotifCounter, Crypto1.Text, Crypto2.Text, Period.Text,
                    Double.Parse(Delta.Text));
                lock (_notifListLock)
                {
                    _notifList.Add(notif);
                    _updatedNotifList = _notifList;
                }

                ActiveNotifications.ItemsSource = null;
                ActiveNotifications.ItemsSource = _notifList;
            }
            else
            {
                MessageBox.Show("Please, enter a value of type: double.");
            }
            
        }
        
        private void DeleteNotifClick(object sender, RoutedEventArgs e)
        {
            lock (_notifListLock)
            {
                Notification obj = ((FrameworkElement)sender).DataContext as Notification;
                if (ActiveNotifications.Items.Contains(obj))
                {
                    _updatedNotifList.Remove(obj);
                    ActiveNotifications.ItemsSource = null;
                    ActiveNotifications.ItemsSource = _updatedNotifList;
                }
            }
        }
    }
}