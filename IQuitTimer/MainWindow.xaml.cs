using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace IQuitTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private bool _showSettings = false;
        public bool ShowSettings
        {
            get => _showSettings;
            set
            {
                if (_showSettings != value)
                {
                    _showSettings = value;
                    NotifyPropertyChanged(nameof(ShowSettings));
                }
            }
        }

        private Config _config;

        public ObservableCollection<DayEntry> Days { get; set; } = new ObservableCollection<DayEntry>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            MakeIcon();

            _config = new Config(MimiJson.JsonValue.ParseFile("config.json"));
            for (var d = _config.StartDate; d < _config.EndDate; d = d.AddDays(1))
                if (!_config.ExcludeDates.Contains(d))
                    Days.Add(new DayEntry(d, _config.StartTime, _config.EndTime, _config.Step));
        }

        void HideWindow()
        {
            Hide();
            _showHideMenuItem.Text = "Show";
        }

        void ShowWindow()
        {
            Show();
            var tm = Topmost;
            Topmost = true;
            Topmost = tm;
            _showHideMenuItem.Text = "Hide";
        }

        private void BeforeExit()
        {

        }

        #region notify icon

        System.Windows.Forms.NotifyIcon _notifyIcon;
        System.Windows.Forms.ToolStripMenuItem _showHideMenuItem;
        System.Drawing.Icon _iconStopped;
        System.Drawing.Icon _iconTicking;
        void MakeIcon()
        {
            _iconStopped = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/IQuitTimer;component/hand-finger-icon.ico")).Stream);
            _iconTicking = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/IQuitTimer;component/hand-finger-icon.ico")).Stream);
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Icon = _iconStopped;
            _notifyIcon.Visible = true;
            _notifyIcon.MouseClick += notifyIcon_MouseClick;
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip()
            {
                Size = new System.Drawing.Size(120, 126)
            };
            _notifyIcon.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                new System.Windows.Forms.ToolStripMenuItem() { Size = new System.Drawing.Size(119, 22), Text = "Hide" },
                new System.Windows.Forms.ToolStripMenuItem() { Size = new System.Drawing.Size(119, 22), Text = "Settings" },
                new System.Windows.Forms.ToolStripSeparator() { Size = new System.Drawing.Size(116, 6) },
                new System.Windows.Forms.ToolStripMenuItem() { Size = new System.Drawing.Size(119, 22), Text = "Exit" }
            });
            _notifyIcon.ContextMenuStrip.Items[0].Click += showHideMenu_Click;
            _notifyIcon.ContextMenuStrip.Items[1].Click += settingsMenu_Click;
            _notifyIcon.ContextMenuStrip.Items[3].Click += exitMenu_Click;
            _showHideMenuItem = _notifyIcon.ContextMenuStrip.Items[0] as System.Windows.Forms.ToolStripMenuItem;
        }

        #region notify icon menu buttons

        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                ShowWindow();
        }

        private void showHideMenu_Click(object sender, EventArgs e)
        {
            if (IsVisible)
                HideWindow();
            else
                ShowWindow();
        }

        private void settingsMenu_Click(object sender, EventArgs e)
        {
            ShowWindow();
            ShowSettings = true;
        }

        private void exitMenu_Click(object sender, EventArgs e)
        {
            BeforeExit();
            _trueClosing = true;
            Close();
        }

        #endregion

        private bool _trueClosing = false;
        private readonly bool _hiddenStart = false;
        //private bool _ticking = false;

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_trueClosing)
            {
                e.Cancel = true;
                HideWindow();
            }
            else
                BeforeExit();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_hiddenStart)
                Hide();
        }
        #endregion
    }
}
