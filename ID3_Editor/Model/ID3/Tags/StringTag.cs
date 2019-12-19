using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Editor.Model.ID3.Tags
{
    class StringTag : Tag
    {
        public StringTag(BinaryReader br, byte[] id)
        {
            this.id = id;
            Position = br.BaseStream.Position;
            ID = GetContent(id);

            ByteReader(byteSize, br);
            Size = IntMaker(byteSize);

            ByteReader(flags, br);

            fullContent = new byte[Size];
            ByteReader(fullContent, br);
            GetEncoding(fullContent);

            Content = GetContent(byteContent);
            
        }

        public StringTag()
        { }

        public override List<byte> GetByteTag(string newTag, string from)
        {
            List<byte> ans = new List<byte>();

            
            id = System.Text.Encoding.UTF8.GetBytes(from.ToCharArray());

            int size = GetEbanyTvoyRotSize(newTag.Length*2+3);
            byteSize = BitConverter.GetBytes(size);

            flags = new byte[2] { 0, 0 };

            List<byte> content = new List<byte>();

            content.AddRange(new List<byte> { 0x01, 0xff, 0xfe });
            content.AddRange(System.Text.Encoding.Unicode.GetBytes(newTag));



            id.ToList().ForEach((s) => { ans.Add(s); });

            for (int i = byteSize.Length - 1; i > -1; i--)
            {
                ans.Add(byteSize[i]);
            }

            flags.ToList().ForEach((s) => { ans.Add(s); });

            content.ForEach((s) => { ans.Add(s); });

            return ans;
        }
    }
}
