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
            List<byte> tags = new List<byte>();



            file = File.ReadAllBytes(way).ToList();
            if (ID3_Info(way))
            {
                size = IntMaker(new byte[4] { file[6], file[7], file[8], file[9] });
                file.RemoveRange(0, size+10);
            }

            // sas
            // Я чет забыл. Нужно еще же и ID3_Header сформировать
            // sas


            
            ans.AddRange(new List<byte>() { 0x49, 0x44, 0x33, 0x03, 0x00, 0x00 ,0x00,0x00,0x1f,0x76});

            int sizeOfReload = 0;

            
            
            if (!string.IsNullOrEmpty(album))
            {
                TALB = new StringTag();
                tags.AddRange(TALB.GetByteTag(album, "TALB"));
                sizeOfReload++;
                //sizeOfReload += IntMaker(TALB.byteSize) + 10;

            }
            if (!string.IsNullOrEmpty(genre))
            {
                TCON = new TCON();
                tags.AddRange(TCON.GetByteTag(genre, "TCON"));
                sizeOfReload++;
                //sizeOfReload += IntMaker(TCON.byteSize) + 10;

            }
            if (!string.IsNullOrEmpty(title))
            {
                TIT2 = new StringTag();
                tags.AddRange(TIT2.GetByteTag(title, "TIT2"));
                sizeOfReload++;
                //sizeOfReload += IntMaker(TIT2.byteSize) + 10;
            }
            if (!string.IsNullOrEmpty(artist))
            {
                TPE1 = new StringTag();
                tags.AddRange(TPE1.GetByteTag(artist, "TPE1"));
                sizeOfReload++;
                //sizeOfReload += IntMaker(TPE1.byteSize) + 10;

            }
            if (!string.IsNullOrEmpty(year))
            {
                TYER = new StringTag();
                tags.AddRange(TYER.GetByteTag(year, "TYER"));
                sizeOfReload++;
                //sizeOfReload += IntMaker(TYER.byteSize) + 10;

            }
            

            if (sizeOfReload == 0)
                return;
            else
            {
                //byte[] tempSize = BitConverter.GetBytes(GetEbanyTvoyRotSize(sizeOfReload));
                //byte[] tempSize = BitConverter.GetBytes(GetEbanyTvoyRotSize(4086));


                /*for (int i = (tempSize.Length - 1); i > -1; i--)
                {
                    ans.Add(tempSize[i]);
                }
                */

                ans.AddRange(tags);

                int remain = 4086 - tags.Count;
                for (int i = 0; i < remain; i++)
                {
                    ans.Add(0x00);
                }

                ans.AddRange(file);

                using (FileStream fs = new FileStream(new FileInfo(way).DirectoryName +"\\" +artist + " - " + title +".mp3", FileMode.Create, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(ans.ToArray());

                }

            }

            //
            /*

            ans.AddRange(file);

            using (FileStream fs = new FileStream(new FileInfo(way).DirectoryName + "\\" + artist + " - " + title + ".mp3", FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(ans.ToArray());
            }
            
            */
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

            while ((findAll !=5 && ((size - br.BaseStream.Position)>=10)))
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
