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
             
        }
    }
}
