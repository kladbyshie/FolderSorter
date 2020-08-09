using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderSorter
{
    public partial class Form1 : Form
    {
        static bool Status { get; set; }
        static string DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";

        //figure out how to cram extensions into settings

        static string csvpath = DirectoryPath + "extensions.csv";
 
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Directory.Length > 0)
            {
                DirectoryPath = Properties.Settings.Default.Directory;
            }

            DirectoryTextBox.Text = DirectoryPath;
            StatusSwitcher(false);

            if (Properties.Settings.Default.StartupBoolean == true)
            {
                InitializeSorter();
                checkBox1.Checked = true;
            }

        }


        void StatusSwitcher(bool input)
        {
            if (input == true)
            {
                Status = true;
                OnOffLabel.Text = "On";
            }
            if (input == false)
            {
                Status = false;
                OnOffLabel.Text = "Off";
            }
        }

        static List<string[]> CSVLoad(string csvpath)
        {
            string[] ExtensionList = File.ReadAllLines(csvpath);
            //third: check items and move them if need be

            List<string[]> NewExtensions = new List<string[]>();
            //creates a list of lists (each list is a line (which is another list of lines))
            foreach (string x in ExtensionList)
            {
                string[] splitstring = x.Split(',');
                NewExtensions.Add(splitstring);
            }
            return NewExtensions;
        }

        static Hashtable HashLoad(List<string[]> NewExtensions)
        {
            Hashtable ExtensionsTable = new Hashtable();
            for (int z = 0; z < NewExtensions.Count; z++)
            {
                string[] LineItem = NewExtensions[z];
                string FileType = LineItem[0];
                List<string> ExtensionsList = new List<string>();

                for (int x = 1; x < LineItem.Length; x++)
                {
                    ExtensionsList.Add(LineItem[x]);
                }

                ExtensionsTable.Add(ExtensionsList, FileType);
            }
            return ExtensionsTable;
        }

        static void Mover(Hashtable ExtensionsTable)
        {
            if (Status == true)
            {
                DirectoryInfo LocalDownloadDir = new DirectoryInfo(DirectoryPath);
                FileInfo[] LocalFileInfo = LocalDownloadDir.GetFiles();

                foreach (FileInfo file in LocalFileInfo)
                {
                    string foldername = FolderFinder(file.Extension, ExtensionsTable);
                    if (foldername.Length > 0)
                    {
                        try
                        {
                            string path = DirectoryPath + foldername;
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            file.MoveTo(DirectoryPath + foldername + @"\" + file.Name, true);
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                }
            }
        }

        static string FolderFinder(string input, Hashtable ExtensionsTable)
        {
            foreach (List<string> x in ExtensionsTable.Keys)
            {
                if (x.Contains(input.ToLower()))
                {
                    return (ExtensionsTable[x].ToString());
                }
            }
            return ("");
        }

        
        public void InitializeSorter()
        {
            StatusSwitcher(true);

            if (!File.Exists(csvpath))
            {
                WriteCSV();
            }

            List<string[]> Extensions = CSVLoad(csvpath);
            Hashtable ExtensionsTable = HashLoad(Extensions);

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Mover(ExtensionsTable);
                    Thread.Sleep(15000);
                }
            });

            thread.IsBackground = true;
            thread.Start();

        }

        private void On_Click(object sender, EventArgs e)
        {
            StatusSwitcher(true);
            InitializeSorter();
        }

        private void Off_Click(object sender, EventArgs e)
        {
            StatusSwitcher(false); 
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            notifyIcon.Visible = true;
        }

        private void DirectorySelect_Click(object sender, EventArgs e)
        {
            var directory = new FolderBrowserDialog();
            directory.ShowDialog();
            DirectoryPath = directory.SelectedPath + @"\";
            DirectoryTextBox.Text = DirectoryPath;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartupBoolean = checkBox1.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Directory = DirectoryPath;
            Properties.Settings.Default.Save();
        }

        private void WriteCSV()
        {
            using (var stream = File.CreateText(csvpath))
            {
                stream.WriteLine("Archives,.zip,.7z,.rar");
                stream.WriteLine("Images,.png,.jpeg,.jpg,.gif");
                stream.WriteLine("Music,.mp3,.wav");
                stream.WriteLine("Videos,.mp4,.webm");
                stream.WriteLine("Installers and Executables,.exe,.jar");
                stream.WriteLine("Torrents,.torrent");
            }
        }
    }
}
