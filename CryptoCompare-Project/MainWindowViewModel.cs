using System.Windows.Input;
using CryptoCompare_Project;
using CryptoCompare_Project.Views;

namespace MvvmSwitchViews
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ICommand _gotoHomePageCommand;
        private ICommand _gotoCryptoRatesCommand;
        private ICommand _gotoVariationsCommand;
        private ICommand _gotoNotificationsCommand;
        
        private object _currentView;
        private object _HomePageView;
        private object _CryptoRatesView;
        private object _VariationsView;
        private object _NotificationsView;
 
        public MainWindowViewModel()
        {
            _HomePageView = new HomePage();
            _CryptoRatesView = new CryptoRates();
            _VariationsView = new Variations();
            _NotificationsView = new Notifications();
 
            CurrentView = _HomePageView;
        }
 
        public object GotoHomePageCommand
        {
            get
            {
                return _gotoHomePageCommand ?? (_gotoHomePageCommand = new RelayCommand(
                    x =>
                    {
                        GotoHomePageView();
                    }));
            }
        }
 
        public ICommand GotoCryptoRatesCommand
        {
            get
            {
                return _gotoCryptoRatesCommand ?? (_gotoCryptoRatesCommand = new RelayCommand(
                    x =>
                    {
                        GotoCryptoRatesView();
                    }));
            }
        }
        
        public ICommand GotoVariationsCommand
        {
            get
            {
                return _gotoVariationsCommand ?? (_gotoVariationsCommand = new RelayCommand(
                    x =>
                    {
                        GotoVariationsView();
                    }));
            }
        }
        
        public ICommand GotoNotificationsCommand
        {
            get
            {
                return _gotoNotificationsCommand ?? (_gotoNotificationsCommand = new RelayCommand(
                    x =>
                    {
                        GotoNotificationsView();
                    }));
            }
        }
 
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }
 
        private void GotoHomePageView()
        {
            CurrentView = _HomePageView;
        }
 
        private void GotoCryptoRatesView()
        {
            CurrentView =  _CryptoRatesView;
        }
        
        private void GotoVariationsView()
        {
            CurrentView =  _VariationsView;
        }
        
        private void GotoNotificationsView()
        {
            CurrentView =  _NotificationsView;
        }
    }
}