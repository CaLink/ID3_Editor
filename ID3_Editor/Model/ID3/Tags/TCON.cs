using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Editor.Model.ID3.Tags
{
    class TCON : Tag
    {
        readonly static string[] genres = new string[] { "Blues", "Classic Rock", "Country", "Dance", "Disco", "Funk", "Grunge", "Hip-Hop", "Jazz", "Metal", "New Age", "Oldies", "Other", "Pop", "R&B", "Rap", "Reggae", "Rock", "Techno", "Industrial", "Alternative", "Ska", "Death Metal", "Pranks", "Soundtrack", "Euro-Techno", "Ambient", "Trip-Hop", "Vocal", "Jazz+Funk", "Fusion", "Trance", "Classical", "Instrumental", "Acid", "House", "Game", "Sound Clip", "Gospel", "Noise", "AlternRock", "Bass", "Soul", "Punk", "Space", "Meditative", "Instrumental Pop", "Instrumental Rock", "Ethnic", "Gothic", "Darkwave", "Techno-Industrial", "Electronic", "Pop-Folk", "Eurodance", "Dream", "Southern Rock", "Comedy", "Cult", "Gangsta", "Top 40", "Christian Rap", "Pop/Funk", "Jungle", "Native American", "Cabaret", "New Wave", "Psychadelic", "Rave", "Showtunes", "Trailer", "Lo-Fi", "Tribal", "Acid Punk", "Acid Jazz", "Polka", "Retro", "Musical", "Rock & Roll", "Hard Rock", "Folk", "Folk-Rock", "National Folk", "Swing", "Fast Fusion", "Bebob", "Latin", "Revival", "Celtic", "Bluegrass", "Avantgarde", "Gothic Rock", "Progressive Rock", "Psychedelic Rock", "Symphonic Rock", "Slow Rock", "Big Band", "Chorus", "Easy Listening", "Acoustic", "Humour", "Speech", "Chanson", "Opera", "Chamber Music", "Sonata" };

        public TCON(BinaryReader br, byte[] id)
        {
            this.id = id;
            ID = GetContent(id);

            ByteReader(byteSize, br);
            Size = IntMaker(byteSize);

            ByteReader(flags, br);

            fullContent = new byte[Size];
            ByteReader(fullContent, br);
            GetEncoding(fullContent);

            Content = GetGenre(byteContent);

        }

        public string GetGenre(byte[] content)
        {
            string ans = "";


            if ((char)content[0] == '\0' && (char)content[1] == '(')
            {
                int i = 2;
                string temp = "";

                while ((char)content[i] != ')')
                {
                    temp += (char)content[i];
                    i++;
                }

                ans = genres[int.Parse(temp)];
            }
            else
            {

                for (int i = 0; i < content.Length; i++)
                {
                    if (((char)content[i]) == '\0')
                        continue;
                    ans += (char)content[i];
                }

            }
            return ans;
        }

        public TCON()
        { }

        public override List<byte> GetByteTag(string newTag, string from)
        {
            List<byte> ans = new List<byte>();

            id = new byte[4] { 0x54, 0x43, 0x4f, 0x4e };

            int size = GetEbanyTvoyRotSize(int.Parse(newTag) + 2);
            byteSize = BitConverter.GetBytes(size);

            flags = new byte[2] { 0, 0 };

            List<byte> content = new List<byte>();

            content.AddRange(new List<byte> { 0x00, 0x28 });
            content.AddRange(System.Text.Encoding.UTF8.GetBytes(newTag));
            content.Add(0x29);


            id.ToList().ForEach((s) => { ans.Add(s); });

            for (int i = byteSize.Length - 1; i > -1; i--)
            {
                ans.Add(byteSize[i]);
            }

            flags.ToList().ForEach((s) => { ans.Add(s); });

            content.ForEach((s) => { ans.Add(s); });

            // Могущественный Аллах, Пожалуйста, пусть это заработает;

            return ans;
        }
    }
}
