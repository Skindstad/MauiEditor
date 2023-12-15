using MauiEditor.View;

namespace MauiEditor
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            // Jesper
            Routing.RegisterRoute(nameof(UpdateDataView), typeof(UpdateDataView));
            Routing.RegisterRoute(nameof(InsertDataView), typeof(InsertDataView));
            Routing.RegisterRoute(nameof(KommuneManagerView), typeof(KommuneManagerView));


        }
    }
}
