using HEXDumpViewer.Extensions;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HEXDumpViewer
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string text;
        public string Text {
            get { return text; }
            set {
                text = value;
                this.OnPropertyChanged("Text");
            }
        }

        private string texthexa;

        public string TextHexa {
            get { return texthexa; }
            set { texthexa = value;
                this.OnPropertyChanged("TextHexa");
            }
        }

        private string filename;
        public string Filename {
            get { return filename; }
            set { filename = value;
                this.OnPropertyChanged("Filename");
            }
        }

        private string filelength;
        public string Filelength {
            get { return filelength; }
            set { filelength = value;
                this.OnPropertyChanged("Filelength");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void TextBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var textToSync = (sender == TextboxText) ? TextboxHexa : TextboxText;
            textToSync.ScrollToVerticalOffset(e.VerticalOffset);
            textToSync.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            if (openFileDlg.ShowDialog() == true) {
                string filename = openFileDlg.FileName;
                this.Filename = openFileDlg.SafeFileName;

                long length = (new FileInfo(filename)).Length;
                if (length > 1000000)
                    this.Filelength = Math.Round(length / 1000000.0, 3) + " mo";
                else if (length > 1000)
                    this.Filelength = Math.Round(length / 1000.0, 3) + " ko";
                else
                    this.Filelength = length + " o";

                byte[] filecontent = File.ReadAllBytes(filename);

                StringBuilder sbtext = new StringBuilder();
                StringBuilder sbhexa = new StringBuilder();
                for (int i = 0; i < filecontent.Length/* && i < 3000*/; i++) {
                    sbtext.Append(filecontent[i] == 0 ? '.' : ((char)filecontent[i]).RemoveSpecialCharacters());

                    string hex = filecontent[i].ToString("X");
                    sbhexa.Append((hex.Length == 1 ? "0" : "") + hex);

                    if (((i + 1) % 32) == 0) {
                        sbtext.Append("\n");
                        sbhexa.Append("\n");
                    }
                    else if (((i + 1) % 8) == 0) {
                        sbhexa.Append(" - ");
                    }
                    else if (((i + 1) % 4) == 0) {
                        sbhexa.Append(".");
                    }
                    else {
                        sbtext.Append(" ");
                        sbhexa.Append(" ");
                    }
                }

                this.Text = sbtext.ToString();
                this.TextHexa = sbhexa.ToString();
            }
        }
    }
}
