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
    }
}
