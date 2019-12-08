using ID3_Editor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ID3_Editor.ViewModel
{
    class ViewMainWindow : NotifyModel
    {
        public ObservableCollection<FileInfo> File { get; set; }
        public CustomCommand<string> Add { get; set; }
        public CustomCommand<Collection<Object>> RemoveSelected { get; set; }
        public CustomCommand<string> Remove { get; set; }


        List<FileInfo> _selectedFile;
        public List<FileInfo> SelecttedFile { get { return _selectedFile; } set {_selectedFile = value; } }

        public ViewMainWindow()
        {
            File = new ObservableCollection<FileInfo>();
            Data.File = File;

            Add = new CustomCommand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case "file":
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Multiselect = true;
                            ofd.Filter = "MP3 (.mp3)|*.mp3";
                            if(ofd.ShowDialog() == DialogResult.OK)
                            {
                                foreach (string item in ofd.FileNames)
                                {
                                    File.Add(new FileInfo(item));
                                }
                            }
                            break;

                        case "dir" :
                            FolderBrowserDialog fbd = new FolderBrowserDialog();
                            if (fbd.ShowDialog() == DialogResult.OK)
                            {
                                 string[] temp = Directory.GetFiles(fbd.SelectedPath, "*.mp3", SearchOption.AllDirectories);

                                foreach (string item in temp)
                                {
                                    File.Add(new FileInfo(item));
                                }
                            }
                            break;
                    }
                });

            RemoveSelected = new CustomCommand<Collection<Object>>(
                (s) =>
                {
                    /*
                     * SAS
                     * Вот этот пацан тоже не работает
                    List<FileInfo> temp = s.Cast<FileInfo>.ToList();
                     * кто бы сомневался 
                     * SAS
                    */
                });


            Remove = new CustomCommand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case "clear":
                            File.Clear();
                            break;
                            

                        case "duplicate":
                            // SAS почему-то не работает
                            // SAS А чего я ещё ожидал
                            File = new ObservableCollection<FileInfo>(File.Distinct().ToList());
                            Data.File = File;
                            break;
                            
                    }
                });
        }
    }
}
