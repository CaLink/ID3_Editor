using ID3_Editor.Model.ID3.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Editor.Model.ID3
{


    class ID3v23 : Tag
    {
        string way;
        public byte[] sign = new byte[3];
        public byte[] version = new byte[2];
        public byte headerFlags;
        public byte[] headerByteSize = new byte[4];
        public int size;
        List<byte> file;


        public StringTag TIT2;
        public StringTag TPE1;
        public StringTag TALB;
        public TCON TCON;
        public StringTag TYER;

        public ID3v23(string way)
        {
            // Почитаем теги
            this.way = way;

            using (FileStream fs = new FileStream(way, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                GetHeader(br);
                GetFrameV3(br);
            }

        }

        public ID3v23(string way, string title, string artist, string album, string year, string genre)
        {
            // Создание или Пересоздание тега
            this.way = way;
            List<byte> ans = new List<byte>();


            file = File.ReadAllBytes(way).ToList();
            if (ID3_Info(way))
            {
                size = IntMaker(new byte[4] { file[6], file[7], file[8], file[9] });
                file.RemoveRange(0, size);
            }

            // sas
            // Я чет забыл. Нужно еще же и ID3_Header сформировать
            // sas



            if (!string.IsNullOrEmpty(title))
                ans.AddRange(new StringTag().GetByteTag(title, "TIT2"));
            if (!string.IsNullOrEmpty(title))
                ans.AddRange(new StringTag().GetByteTag(artist, "TPE1"));
            if (!string.IsNullOrEmpty(title))
                ans.AddRange(new StringTag().GetByteTag(album, "TALB"));
            if (!string.IsNullOrEmpty(title))
                ans.AddRange(new StringTag().GetByteTag(year, "TYER"));
            if (!string.IsNullOrEmpty(title))
                ans.AddRange(new TCON().GetByteTag(title, "TCON"));




        }


        public static bool ID3_Info(string way)
        {
            string ans;

            using (FileStream fs = new FileStream(way, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                ans = ((char)br.ReadByte()).ToString() + ((char)br.ReadByte()).ToString() + ((char)br.ReadByte()).ToString();
                byte ver = br.ReadByte();
                if (ans == "ID3" && ver == 0x03)
                    return true;
                else
                    return false;
            }
        }

        void GetHeader(BinaryReader br)
        {
            ByteReader(sign, br);
            
            ByteReader(version, br);
            headerFlags = br.ReadByte();
            ByteReader(headerByteSize, br);
            size = IntMaker(headerByteSize);

        }

        void GetFrameV3(BinaryReader br)
        {
            int findAll = 0;

            byte[] id = new byte[4];
            ByteReader(id, br);
            string temp = GetContent(id);

            while ((findAll !=5 && ((br.BaseStream.Length - br.BaseStream.Position)>=4)))
            {
                switch (temp)
                {
                    case "TIT2": TIT2 = new StringTag(br, id); findAll++; break;
                    case "TPE1": TPE1 = new StringTag(br, id); findAll++; break;
                    case "TALB": TALB = new StringTag(br, id); findAll++; break;
                    case "TYER": TYER = new StringTag(br, id); findAll++; break;
                    case "TCON": TCON = new TCON   (br, id); findAll++; break;
                }

                id[0] = id[1];
                id[1] = id[2];
                id[2] = id[3];
                id[3] = br.ReadByte();
                temp = GetContent(id);
            }
        }

        public override List<byte> GetByteTag(string newTag, string from)
        {
            // Заглушка

            throw new NotImplementedException();
            
            // Не совсем понял, как правильно тут сделать, т.к. этот метод мне по факту не нужно, но да
        }

    }
}
