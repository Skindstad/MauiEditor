using MauiEditor.ViewModel;

namespace MauiEditor.View;

public partial class InsertDataView : ContentPage
{
	public InsertDataView(InsertViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}