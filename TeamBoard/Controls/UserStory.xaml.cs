using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamBoard.Controls
{
    /// <summary>
    /// Interaction logic for UserStory.xaml
    /// </summary>
    public partial class UserStory : UserControl, INotifyPropertyChanged
    {
        public UserStory()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty UserStoryIdProperty = DependencyProperty.Register("UserStoryId", typeof(string), typeof(UserStory), new FrameworkPropertyMetadata("#00-00000"));
        public static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(int), typeof(UserStory), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(UserStory), new FrameworkPropertyMetadata("User story caption"));
        public static readonly DependencyProperty TimeSpentProperty = DependencyProperty.Register("TimeSpent", typeof(int), typeof(UserStory), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty TimeLeftProperty = DependencyProperty.Register("TimeLeft", typeof(int), typeof(UserStory), new FrameworkPropertyMetadata(8));
        public static readonly DependencyProperty PhotoPathProperty = DependencyProperty.Register("PhotoPath", typeof(string), typeof(UserStory), new FrameworkPropertyMetadata(string.Empty));


        public string UserStoryId
        {
            get { return (string)GetValue(UserStoryIdProperty); }
            set { SetValue(UserStoryIdProperty, value); }
        }

        public int Priority
        {
            get { return (int)GetValue(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public int TimeSpent
        {
            get { return (int)GetValue(TimeSpentProperty); }
            set 
            {
                SetValue(TimeSpentProperty, value);
                FirePropertyChanged("Progress");
            }
        }

        public int TimeLeft
        {
            get { return (int)GetValue(TimeLeftProperty); }
            set 
            {
                SetValue(TimeLeftProperty, value);
                FirePropertyChanged("Progress");
            }
        }

        public int Progress
        {
            get {
                if (this.TimeLeft + this.TimeSpent == 0)
                    return 0;
                else
                    return (int)((double)this.TimeSpent / (this.TimeSpent + this.TimeLeft) * 100);
            }
        }

        public string PhotoPath
        {
            get { return (string)GetValue(PhotoPathProperty); }
            set { SetValue(PhotoPathProperty, value); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
