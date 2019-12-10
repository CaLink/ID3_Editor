using ID3_Editor.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ID3_Editor.ViewModel
{
    class ViewMainWindow : NotifyModel
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);

        public ObservableCollection<FileInfo> File { get; set; }
        public CustomCommand<string> Add { get; set; }
        public CustomCommand<FileInfo> RemoveSelected { get; set; }
        public CustomCommand<string> Remove { get; set; }
        public CustomCommand<string> Sort { get; set; }



        List<FileInfo> _selectedFile;
        public List<FileInfo> SelecttedFile { get { return _selectedFile; } set { _selectedFile = value; } }

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
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                foreach (string item in ofd.FileNames)
                                {
                                    File.Add(new FileInfo(item));
                                }
                            }
                            break;

                        case "dir":
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
                    RaiseEvent(nameof(File));

                });

            // Я хз как это все сделать верно
            // Поэтому пользователю будет больно
            // Будет удалять по файлу
            // ГЫ
            RemoveSelected = new CustomCommand<FileInfo>(
                (s) =>
                {
                    File.Remove(s);
                    RaiseEvent(nameof(File));


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

                            var temp = File.ToList();
                            List<string> ass = new List<string>();


                            temp.ForEach(((x) => ass.Add(x.FullName)));
                            ass = ass.Distinct().ToList();

                            File = new ObservableCollection<FileInfo>();
                            ass.ForEach(((x) =>File.Add(new FileInfo(x))));
                            Data.File = File;
                            break;

                    }
                    RaiseEvent(nameof(File));

                });

            Sort = new CustomCommand<string>(
                (s) =>
                {
                    var temp = File.ToList();

                    switch (s)
                    {
                        case "dir":
                            temp.Sort(
                                (x, y) =>
                                {
                                    return StrCmpLogicalW(x.DirectoryName.ToString(), y.DirectoryName.ToString());
                                });
                            break;

                        case "file":
                            temp.Sort(
                                (x, y) =>
                                {
                                    return StrCmpLogicalW(x.Name.ToString(), y.Name.ToString());
                                });
                            break;
                    }

                    File = new ObservableCollection<FileInfo>(temp);
                    Data.File = File;
                    RaiseEvent(nameof(File));




                });



        }

        


    }
}
