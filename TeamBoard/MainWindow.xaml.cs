using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamBoard.Commands;
using TeamBoard.TeamBoardService;

namespace Team_board
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;
        private Point start = new Point();
        private FrameworkElement draggable = null;
        internal TeamBoardServiceClient service;


        public static readonly DependencyProperty ScaleFactorProperty =  DependencyProperty.Register("ScaleFactor", typeof(double),
     typeof(MainWindow), new FrameworkPropertyMetadata((double)1));

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void MainWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            //ITeamBoardServiceCallback callback = new CallbackHandler(this);
            //this.service = new TeamBoardServiceClient(new InstanceContext(callback));
            //this.service.Ping();
            
            //TeamBoardService.
        }

        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        private void Image_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.startPoint = e.GetPosition(null);
            this.draggable = e.Source as FrameworkElement;
            start.X = NoNaN((double)this.draggable.GetValue(Canvas.LeftProperty));
            start.Y = NoNaN((double)this.draggable.GetValue(Canvas.TopProperty));
        }

        private void Image_PreviewMouseMove_1(object sender, MouseEventArgs e)
        {
            if (this.draggable == null)
                return;

            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = mousePos - startPoint;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Move the element on the canvas 

                this.draggable.SetValue(Canvas.LeftProperty, start.X + diff.X / ScaleFactor);
                this.draggable.SetValue(Canvas.TopProperty, start.Y + diff.Y / ScaleFactor);
            } 

        }

        private static double NoNaN(double x)
        {
            return double.IsNaN(x) ? 0 : x;
        }

        private void Canvas_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            this.draggable = null;
        }

        private void MainWindow_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            this.ScaleFactor = this.ActualWidth / 1920;
        }

        private void Label_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            this.WindowStyle = this.WindowStyle == System.Windows.WindowStyle.None ? WindowStyle.SingleBorderWindow : System.Windows.WindowStyle.None;
        }

        private void MainWindow_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            this.ScaleFactor = this.ScaleFactor + (double)e.Delta / 3000;
        }

        private void MainWindow_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemPlus && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                this.ScaleFactor *= 1.1;
            if (e.Key == Key.OemMinus && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                this.ScaleFactor /= 1.1;
        }
    }

    public class CallbackHandler : ITeamBoardServiceCallback
    {
        MainWindow w;

        public CallbackHandler(MainWindow win)
        {
            this.w = win;
        }

        public void HandleEvent(object @event)
        {
            w.Title = @event.ToString();
        }

        public void HandleHistory(object[] events)
        {
        }


        public void PingBack()
        {
            //this.w.service.ProcessCommand(new CreateUserStory(Guid.NewGuid(), "New User Story", "123"));
        }
    }

}
