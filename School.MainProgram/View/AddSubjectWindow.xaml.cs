using School.MainProgram.ViewModel;
using System.Windows;

namespace School.MainProgram.View
{
    /// <summary>
    /// Interaction logic for AddSubjectWindow.xaml
    /// </summary>
    public partial class AddSubjectWindow : Window
    {
        public AddSubjectWindow(SchoolViewModel schoolViewModel)
        {
            InitializeComponent();

            DataContext = schoolViewModel;
        }
    }
}
