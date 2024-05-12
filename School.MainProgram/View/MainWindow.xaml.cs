using System.Linq;
using System.Windows;
using School.DataAccess.Model;
using School.MainProgram.ViewModel;

namespace School.MainProgram.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(SchoolViewModel schoolViewModel)
        {
            InitializeComponent();

            DataContext = schoolViewModel;
        }
    }
}
