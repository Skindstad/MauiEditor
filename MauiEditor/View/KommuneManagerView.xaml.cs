using MauiEditor.ViewModel;

namespace MauiEditor.View;

public partial class KommuneManagerView : ContentPage
{
    private KommuneViewModel model = new KommuneViewModel();

    public KommuneManagerView(KommuneViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}