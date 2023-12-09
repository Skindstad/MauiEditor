using MauiEditor.Model;
using MauiEditor.ViewModel;

namespace MauiEditor.View;

public partial class UpdateDataView : ContentPage
{
    private UpdateDataViewModel viewModel = new UpdateDataViewModel();
    /*
	public UpdateDataView(UpdateDataViewModel contact)
	{
        // referance UpdateDataViewModel here
        InitializeComponent();
        // BindingContext = contact;
        BindingContext = viewModel;
	}
    */
    public UpdateDataView()
    {
        // referance UpdateDataViewModel here
        InitializeComponent();
        // BindingContext = contact;
        BindingContext = viewModel;
    }

    private void OnKomIdTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void OnCityTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void OnGruppeTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void OnAarstalTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void OnTalTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void UpdateButtonClicked(object sender, EventArgs e)
    {

    }

    private void BackButtonClicked(object sender, EventArgs e)
    {
        // viewModel.OnClose();
    }
}