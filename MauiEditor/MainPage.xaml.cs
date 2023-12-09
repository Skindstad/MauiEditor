using MauiEditor.View;
using MauiEditor.ViewModel;

namespace MauiEditor
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCityTextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnKomIdTextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void UpdateDataView_Clicked(object sender, EventArgs e) // jesper, goes to updateDataView page
        {
            var navigationParameter = new Dictionary<string, object>()
            {
                {nameof(UpdateDataView), new UpdateDataView() }
            };

            await Shell.Current.GoToAsync(nameof(UpdateDataView), navigationParameter);
        }


        /* private void OnCounterClicked(object sender, EventArgs e)
         {
             count++;

             if (count == 1)
                 CounterBtn.Text = $"Clicked {count} time";
             else
                 CounterBtn.Text = $"Clicked {count} times";

             SemanticScreenReader.Announce(CounterBtn.Text);
         }*/
    }

}
