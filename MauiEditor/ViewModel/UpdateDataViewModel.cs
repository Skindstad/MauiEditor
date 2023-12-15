using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
// using Java.Time;
using MauiEditor.Model;
using MauiEditor.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiEditor.ViewModel
{
    public partial class UpdateDataViewModel : ObservableObject, INotifyPropertyChanged
    {
        protected static DataRepository repository = [];
        private static string _Aarstal = "";
        private static string _Tal = "";
        private static string _Gruppe = "";
        private static string _City = "";
        private static string _KomNr = "";
        private static string _ID = "";

        public customRelayCommand CloseCommand => new customRelayCommand(execute => OnClose(), canExecute => { return true; });
        public customRelayCommand UpdateCommand => new customRelayCommand(execute => Update());

        public UpdateDataViewModel() 
        {
            // CloseCommand = new RelayCommand(p => { if (CloseHandler != null) CloseHandler(); });
            CityList = getCityList().OrderBy(t => t.cityName).ToList();
        }

        public List<City> CityList { get; set; }
        private List<City> getCityList()
        {
            repository.Search(string.Empty, string.Empty, string.Empty);
            var citys = new List<City>();

            foreach (var city in repository)
            {
                City tempCity = new City { year = city.Year, cityName = city.City};
                tempCity.value = tempCity.year + ", " + tempCity.cityName;

                citys.Add(tempCity);
                //citys.Add(new City { key = KomId, value = repository.});
            }
            /*
            var result = citys.Distinct(new ItemEqualityComparer());
            citys.Clear();
            foreach (var city in result)
            {
                citys.Add(new City { key = city.key, value = city.value });
            }
            */
            return citys;
        }

        private City _selectedCity { get; set; }
        public City SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    repository.Search(_selectedCity.cityName, string.Empty, SelectedCity.year);
                    foreach (var currentCity in repository) // should just be one
                    {
                        City = currentCity.City;
                        Aarstal = currentCity.Year;
                        Gruppe = currentCity.Gruppe;
                        KomId = currentCity.KomNr;
                        Tal = currentCity.Num;
                        _ID = currentCity.DataId;
                    }
                }
            }
        }

        public string Aarstal
        {
            // get; set;
            
            get { return _Aarstal; }
            set
            {
                if (!_Aarstal.Equals(value))
                {
                    _Aarstal = value;
                    OnPropertyChanged("Aarstal");
                }
            }
        }
        public string Tal
        {
            get { return _Tal; }
            set
            {
                if (!_Tal.Equals(value))
                {
                    _Tal = value;
                    OnPropertyChanged("Tal");
                }
            }
        }
        public string Gruppe // problem med ik opdaterer efter ny valg
        {
            get { return _Gruppe; }
            set
            {
                if (!_Gruppe.Equals(value))
                {
                    _Gruppe = value;
                    OnPropertyChanged("Gruppe");
                }
            }
        }
        public string City
        {
            get { return _City; }
            set
            {
                if (!_City.Equals(value))
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }
        public string KomId
        {
            get { return _KomNr; }
            set
            {
                if (!_KomNr.Equals(value))
                {
                    _KomNr = value;
                    OnPropertyChanged("KomId");
                }
            }
        }

        private void OnClose()
        {
            // object sender, EventArgs e
            // returns to previus page
            Shell.Current.GoToAsync("..");
        }

        private void Update()
        {
            // opdater server med nye værdier
            repository.Update(_ID, KomId, City, Gruppe, Aarstal, Tal);
        }

    }

    public class customRelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler? CanExecuteChanged;

        public customRelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }

    public class City()
    {
        public string year { get; set; }
        public string cityName { get; set; }
        public string value { get; set; }
    }
}
