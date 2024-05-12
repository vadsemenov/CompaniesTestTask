using School.MainProgram.ViewModel;
using System.Windows;

namespace School.MainProgram.View
{
    /// <summary>
    /// Interaction logic for AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        public AddStudentWindow(SchoolViewModel schoolViewModel)
        {
            InitializeComponent();

            DataContext = schoolViewModel;
        }
    }
}
