// Licensed under GPL License

using Streamer;
using System.Windows;
using System.Windows.Controls;

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
            int i = 12;
            while (i > 0)
            {
                i--;
                playerClassList[i] = new PlayerClass();

            }

        }
        PlayerClass[] playerClassList = new PlayerClass[12];
        private void Alert(int i, object sender, string dir = null)
        {
            Button btn = (sender as Button);
         

            if (i == 1)
            {
                MessageBox.Show("Streaming Camera: wait for 10 s");
                btn.IsEnabled = false;
            }
            else if (i == 2)
            {
                if (btn.Content.ToString() == "Stop")
                {
                    MessageBox.Show($"Recording stopped");
                    btn.Content = "Rec";
                }
                else if (btn.Content.ToString() == "Rec")
                {
                   
                    MessageBox.Show($"Recording started at {dir}");
                    btn.Content = "Stop";

                }

            }
        }
        public bool CheckifRecOrStop(object sender)
        {
            Button btn = (sender as Button);
            if (btn.Content.ToString() == "Rec")
            {
                return true;
            }
            else if (btn.Content.ToString() == "Play")
            {
                return false;
            }
            return false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Url.Text))
                return;
            Alert(1, sender);
            PlayerClass playerClass = playerClassList[0];
            playerClass.GetLiveVideoStream(myWindow1, Url.Text);


        }
        private void Button_Click_Rec(object sender, RoutedEventArgs e)
        {
            PlayerClass playerClass = playerClassList[1];
            
            string dir = string.Empty;
            bool rec = CheckifRecOrStop(sender);

            if (!rec)
            {
                playerClass.StopVideoStreamRecording();

            }
            else
            {
                if (string.IsNullOrEmpty(Url.Text))
                    return;

                System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                var result = openFileDlg.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    dir = openFileDlg.SelectedPath;
                }
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                playerClass.RecordLiveVideoStream("cam1", Url.Text, dir);
            }
            Alert(2, sender, dir);
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(Url1.Text))
                return;
            Alert(1, sender);
            PlayerClass playerClass = playerClassList[2]; ;

            playerClass.GetLiveVideoStream(myWindow2, Url1.Text);


        }
        private void Button_Click_Rec1(object sender, RoutedEventArgs e)
        {
            PlayerClass playerClass = playerClassList[3];
            string dir = string.Empty;
            bool rec = CheckifRecOrStop(sender);

            if (!rec)
            {
                playerClass.StopVideoStreamRecording();

            }
            else
            {
                if (string.IsNullOrEmpty(Url1.Text))
                    return;

                System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                var result = openFileDlg.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    dir = openFileDlg.SelectedPath;
                }
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                playerClass.RecordLiveVideoStream("cam2", Url1.Text, dir);

            }
            Alert(2, sender, dir);
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Url2.Text))
                return;
            Alert(1, sender);
            PlayerClass playerClass = playerClassList[4];

            playerClass.GetLiveVideoStream(myWindow3, Url2.Text);


        }
        private void Button_Click_Rec2(object sender, RoutedEventArgs e)
        {
            PlayerClass playerClass = playerClassList[5];
            string dir = string.Empty;
            bool rec = CheckifRecOrStop(sender);

            if (!rec)
            {
                playerClass.StopVideoStreamRecording();

            }
            else
            {
                if (string.IsNullOrEmpty(Url2.Text))
                    return;

                System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                var result = openFileDlg.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    dir = openFileDlg.SelectedPath;
                }
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                playerClass.RecordLiveVideoStream("cam3", Url2.Text, dir);
            }
            Alert(2, sender, dir);
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Url3.Text))
                return;
            Alert(1, sender);
            PlayerClass playerClass = playerClassList[6];

            playerClass.GetLiveVideoStream(myWindow4, Url3.Text);


        }
        private void Button_Click_Rec3(object sender, RoutedEventArgs e)
        {
            PlayerClass playerClass = playerClassList[7];
            string dir = string.Empty;
            bool rec = CheckifRecOrStop(sender);

            if (!rec)
            {
                playerClass.StopVideoStreamRecording();

            }
            else
            {
                if (string.IsNullOrEmpty(Url3.Text))
                    return;

                System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                var result = openFileDlg.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    dir = openFileDlg.SelectedPath;
                }

                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }

                playerClass.RecordLiveVideoStream("cam4", Url3.Text, dir);
            }
            Alert(2, sender, dir);

        }
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Url4.Text))
                return;
            Alert(1, sender);
            PlayerClass playerClass = playerClassList[8];

            playerClass.GetLiveVideoStream(myWindow5, Url4.Text);


        }
        private void Button_Click_Rec4(object sender, RoutedEventArgs e)
        {
            PlayerClass playerClass = playerClassList[9];
            string dir = string.Empty;
            bool rec = CheckifRecOrStop(sender);

            if (!rec)
            {
                playerClass.StopVideoStreamRecording();

            }
            else
            {
                if (string.IsNullOrEmpty(Url4.Text))
                    return;

                System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                var result = openFileDlg.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    dir = openFileDlg.SelectedPath;
                }
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                playerClass.RecordLiveVideoStream("cam5", Url4.Text, dir);

            }
            Alert(2, sender, dir);
        }
        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Url5.Text))
                return;
            Alert(1, sender);
            PlayerClass playerClass = playerClassList[10];

            playerClass.GetLiveVideoStream(myWindow6, Url5.Text);


        }
        private void Button_Click_Rec5(object sender, RoutedEventArgs e)
        {
            PlayerClass playerClass = playerClassList[11];
            string dir = string.Empty;
            bool rec = CheckifRecOrStop(sender);

            if (!rec)
            {
                playerClass.StopVideoStreamRecording();

            }
            else
            {
                if (string.IsNullOrEmpty(Url5.Text))
                    return;

                System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                var result = openFileDlg.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    dir = openFileDlg.SelectedPath;
                }
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                playerClass.RecordLiveVideoStream("cam6", Url5.Text, dir);

            }
            Alert(2, sender, dir);
        }
    }
}
