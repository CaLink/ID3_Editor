using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Editor.Model.ID3
{
    abstract class Tag
    {
        public byte[] id = new byte[4];
        public byte[] byteSize = new byte[4];
        public byte[] flags = new byte[2];
        public byte[] fullContent;
        public byte[] encoding;
        public byte[] byteContent;

        public long Position { get; set; }
        public string ID { get; set; }
        public int Size { get; set; }
        public string Encoding { get; set; }
        public string Content { get; set; }

        public void ByteReader(byte[] array, BinaryReader br)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = br.ReadByte();

        }

        public int IntMaker(byte[] size)
        {
            return size[3] + (size[2] << 7) + (size[1] << 14) + (size[0] << 21);
        }

        public string GetContent(byte[] content)
        {
            string ass = "";
            if (Encoding != null)
                ass = System.Text.Encoding.GetEncoding(Encoding).GetString(content);
            else
                for (int i = 0; i < content.Length; i++)
                {
                    if (((char)content[i]) == '\0')
                        continue;
                    ass += ((char)content[i]).ToString();
                }
            return ass;
        }

        public void GetEncoding(byte[] all)
        {
            if (all[1] == 0xff)
            {
                encoding = new byte[3];
                encoding[0] = all[0];
                encoding[1] = all[1];
                encoding[2] = all[2];

                byteContent = new byte[Size - 3];
                for (int i = 3; i < all.Length; i++)
                    byteContent[i - 3] = all[i];
                Encoding = "UTF-16";

            }
            else if (all[1] == 0xfe)
            {
                encoding = new byte[3];
                encoding[0] = all[0];
                encoding[1] = all[1];
                encoding[2] = all[2];

                byteContent = new byte[Size - 3];
                for (int i = 3; i < all.Length; i++)
                    byteContent[i - 3] = all[i];
                Encoding = "UTF-16";

            }
            else if (all[1] == 0xef)
            {
                encoding = new byte[4];
                encoding[0] = all[0];
                encoding[1] = all[1];
                encoding[2] = all[2];
                encoding[3] = all[3];

                byteContent = new byte[Size - 4];
                for (int i = 4; i < all.Length; i++)
                    byteContent[i - 4] = all[i];
                Encoding = "UTF-8";


            }
            else
            {

                byteContent = new byte[Size];
                for (int i = 0; i < all.Length; i++)
                    byteContent[i] = all[i];

            }


        }

        public int GetEbanyTvoyRotSize(int ebanySize)
        {
            List<char> jepa = new List<char>();
            BitArray ass = new BitArray(BitConverter.GetBytes(ebanySize));

            for (int i = 0; i < ass.Length-3; i++)
            {
                if (i == 7 || i == 14 || i == 21)
                    jepa.Add('0');
                jepa.Add((bool)ass[i] ? '1' : '0');
            }

            for (int i = 0; i < ass.Length; i++)
            {
                ass[i] = jepa[i] == '1' ? true : false;
            }

            int[] resulr = new int[1];
            ass.CopyTo(resulr, 0);

            return resulr[0];

        }

        public abstract List<byte> GetByteTag(string newTag,string from);

    }
}
