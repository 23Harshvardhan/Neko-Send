using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Microsoft.Win32;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WhatsAppBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] names;
        string uploadFileLocation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnUsernames_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog usernamepath = new Microsoft.Win32.OpenFileDialog();
            usernamepath.Filter = "TXT file with usernames (*.txt)|*.txt";
            Nullable<bool> result = usernamepath.ShowDialog();
            if (result == true)
            {
                lblUsernamePath.Content = "Usernames Location: " + usernamepath.FileName;
                string userPath = usernamepath.FileName;
                names = File.ReadAllLines(userPath);
                cbxUsernames.ItemsSource = names;
            }
        }

        private void btnfile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileLocation = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = fileLocation.ShowDialog();
            if (result == true)
            {
                lblFilePathInfo.Content = "File Location: " + fileLocation.FileName;
                uploadFileLocation = fileLocation.FileName;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (names == null)
            {
                MessageBox.Show("The mentioned names file seems invalid.\n Please Check and proceed.", "Names file empty!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (uploadFileLocation == null)
                {
                    MessageBox.Show("The path of the file to be uploaded seems invalid.\n Please check and proceed.", "Invalid file path!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Process[] processcheck = Process.GetProcessesByName("Chrome");
                    if (processcheck.Length > 0)
                    {
                        IWebDriver chromedriver = new ChromeDriver("driver");
                        chromedriver.Navigate().GoToUrl("https://web.whatsapp.com/");
                        Thread.Sleep(9000);
                        foreach (var n in names)
                        {
                            string currentName = n;
                            chromedriver.FindElement(By.XPath("//span[@title = " + "'" + n + "'" + "]")).Click();
                            Thread.Sleep(1000);
                            chromedriver.FindElement(By.XPath("//div[@title = 'Attach']")).Click();
                            Thread.Sleep(1000);
                            chromedriver.FindElement(By.XPath("//input[@accept='image/*,video/mp4,video/3gpp,video/quicktime']")).SendKeys(uploadFileLocation);
                            Thread.Sleep(1000);
                            chromedriver.FindElement(By.XPath("//span[@data-icon='send']")).Click();
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Chrome is not detected in the background. Uploading cannot proceed.", "Chrome not found!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}