using MPHPresenter.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace MPHPresenter
{
    /// <summary>
    /// Interaction logic for ProjectionWindow.xaml
    /// </summary>
    public partial class ProjectionWindow : Window
    {
        public ProjectionViewModel ProjectionViewModel { get; set; }

        public ProjectionWindow()
        {
            InitializeComponent();
            ProjectionViewModel = new ProjectionViewModel();
            DataContext = ProjectionViewModel;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.Right:
                case Key.Space:
                    ProjectionViewModel.NextSlide();
                    break;
                case Key.Left:
                    ProjectionViewModel.PreviousSlide();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}