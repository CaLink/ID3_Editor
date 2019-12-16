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

        }


        public static bool ID3_Info(string way)
        {
            string ans;

            using (FileStream fs = new FileStream(way, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                ans = ((char)br.ReadByte()).ToString() + ((char)br.ReadByte()).ToString() + ((char)br.ReadByte()).ToString();
                if (ans == "ID3")
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


    }
}
