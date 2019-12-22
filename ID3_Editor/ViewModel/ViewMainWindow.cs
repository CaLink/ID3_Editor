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
        public CustomCommand<FileInfo> OpenSmt{ get; set; }

        FileInfo selectedFile;
        public FileInfo SellllectedFile{ get { return selectedFile; } set { selectedFile = value;} }


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
                                //Тут косячок, что нужно убрать системные папки
                                //https://ru.stackoverflow.com/questions/437404/%D0%9E%D1%82%D0%BA%D0%B0%D0%B7%D0%B0%D0%BD%D0%BE-%D0%B2-%D0%B4%D0%BE%D1%81%D1%82%D1%83%D0%BF%D0%B5-%D0%BF%D1%80%D0%B8-%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B8-directory-getfiles
                                // ХеХе

                                string[] temp = SecretPool(fbd.SelectedPath).ToArray();

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
                    // Говно-Жопа
                    if (SellllectedFile == null)
                        return;

                    File.Remove(SellllectedFile);
                    RaiseEvent(nameof(File));
                    // Мне не нравится
                    // А, я не заметил комент выше
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

            OpenSmt = new CustomCommand<FileInfo>(
                (s) => 
                {
                    if (SellllectedFile == null)
                        return;
                    var costi = SellllectedFile;
                    Data.Travel = SellllectedFile.FullName;
                    new View.TagEditor(SellllectedFile.FullName).ShowDialog();
                    if (Data.Costil)
                    {
                        File[File.IndexOf(costi)] = new FileInfo(Data.NewPath);
                        Data.Costil = false;
                    }

                    RaiseEvent(nameof(File));
                });

        }

        List<string> SecretPool(string way)
        {
            var files = new List<string>();

            try
            {
                files.AddRange(Directory.GetFiles(way, "*.mp3", SearchOption.TopDirectoryOnly));
                foreach (var directory in Directory.GetDirectories(way))
                    files.AddRange(SecretPool(directory));
            }
            catch (UnauthorizedAccessException) { }

            return files;

            
        }


    }
}
