using MauiEditor.ViewModel;

namespace MauiEditor.View;

public partial class KommuneManagerView : ContentPage
{
	public KommuneManagerView(KommuneViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}