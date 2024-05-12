using System.Windows;
using School.MainProgram.ViewModel;

namespace School.MainProgram.View
{
    /// <summary>
    /// Interaction logic for AddAssessmentWindow.xaml
    /// </summary>
    public partial class AddAssessmentWindow : Window
    {
        public AddAssessmentWindow(SchoolViewModel schoolViewModel)
        {
            InitializeComponent();

            DataContext = schoolViewModel;
        }
    }
}
