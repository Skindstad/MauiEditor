using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiEditor.Model;
using MauiEditor.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiEditor.ViewModel
{
   public partial class MainViewModel: ObservableObject
    {

       public RelayCommand InsertDataCammand { get; private set; }
        public RelayCommand KommuneCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }

        public static DataRepository repository = new DataRepository();

        public MainViewModel()
        {
            BindData = new ObservableCollection<Data>(repository);
        }

        [ObservableProperty]
        ObservableCollection<Data> bindData;

        /*    [ObservableProperty]
           string text;

          [ICommand] 
           void Add()
           {
               if (string.IsNullOrEmpty(Text)) return;


               items.Add(text);
               Text = string.Empty;
           }*/
            // Entry
            [ObservableProperty]
             string kommune;

             [ObservableProperty]
             string gruppe;

             [ObservableProperty]
             string year;


        // Labels
            [ObservableProperty]
            string komNr;

            [ObservableProperty]
            string dataID;

            [ObservableProperty]
            string num;

            [ObservableProperty]
            string error;

        public ICommand SearchCommand =>
                 new Command(SearchData);

             private void SearchData(object obj)
             {
            if (string.IsNullOrWhiteSpace(Kommune) && string.IsNullOrWhiteSpace(Gruppe) && string.IsNullOrWhiteSpace(Year)) return;
            try
            {
                repository.Search(Kommune, Gruppe, Year);
                BindData = new ObservableCollection<Data>(repository);
            } catch (Exception ex)
            {
                Error = ex.ToString();
            }

                 Kommune = string.Empty;
                 Gruppe = string.Empty;
                 Year = string.Empty;
             }

    }
}
