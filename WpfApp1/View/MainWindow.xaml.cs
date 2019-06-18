using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using WpfApp1.ViewModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FilesViewModel fvm = new FilesViewModel();
            DataContext = fvm;
        }

        private void getPath_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = folderBrowser.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
               (sender as TextBox).Text = folderBrowser.SelectedPath.ToString();
            }
            
        }

        private void Cbx_time_DropDownClosed(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(lb_time.Text))
                lb_time.Text = DateTime.Now.AddHours(Int32.Parse(cbx_time.Text.Substring(0, 1))).ToString();

            string source = tbx_source.Text.Trim();
            string desti = tbx_destination.Text.Trim();

            string[] sourceFiles = Directory.GetFiles(source);

            foreach (string file in sourceFiles)
            {
                string filename = System.IO.Path.GetFileName(file);

                if (filename.Contains(cbx_fileType.Text))
                {
                    string destFile = System.IO.Path.Combine(desti, filename);
                    countDown(1,TimeSpan.FromMinutes(1), curr => lb_time.Text = curr.ToString().ToString(), file, destFile);
                }
            }
        }

        public int timer(int time)
        {
            if (time == 1)
                return 60;
            else if (time == 2)
                return 120;
            else if (time == 3)
                return 180;
            else if (time == 4)
                return 240;
            else if (time == 5)
                return 300;
            else
                return 360;
        }

        void countDown(int count, TimeSpan interval, Action<int> ts, string file, string destiFile)
        {
            var dt = new System.Windows.Threading.DispatcherTimer();
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                if (count-- == 0)
                {
                    dt.Stop();
                    File.Move(file, destiFile);
                }               
                else
                {
                    ts(count);
                    lb_time.Text = count.ToString();
                }
                  
            };
            ts(count);
            dt.Start();
        }
    }
}
