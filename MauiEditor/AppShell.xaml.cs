using MauiEditor.View;

namespace MauiEditor
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(InsertDataView), typeof(InsertDataView));
            Routing.RegisterRoute(nameof(KommuneManagerView), typeof(KommuneManagerView));
        }
    }
}
