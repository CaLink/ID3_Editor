using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ID3_Editor.Model.ID3;

namespace ID3_Editor.View
{
    /// <summary>
    /// Логика взаимодействия для TagEditor.xaml
    /// </summary>
    public partial class TagEditor : Window
    {
        ID3v23 edited;
        string way;

        public TagEditor(string way)
        {
            InitializeComponent();
            this.way = way;

            if (ID3v23.ID3_Info(way))
            {
                edited = new ID3v23(way);

                Way.Text = way;
                if (!string.IsNullOrEmpty(edited.TIT2.Content))
                    Title.Text = edited.TIT2.Content;
                if (!string.IsNullOrEmpty(edited.TPE1.Content))
                    Artist.Text = edited.TPE1.Content;
                if (!string.IsNullOrEmpty(edited.TALB.Content))
                    Album.Text = edited.TALB.Content;
                if (!string.IsNullOrEmpty(edited.TYER.Content))
                    Year.Text = edited.TYER.Content;
                if (!string.IsNullOrEmpty(edited.TCON.Content))
                    Genre.SelectedItem = edited.TCON.Content;




                // Заполняем TextBox
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Тут просто бахнуть другой конструктор ID3v23
        }
    }
}
