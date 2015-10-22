using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookKeylogger.Base
{
    public class StreamExtension : Stream
    {
        private Stream stream;
        private static readonly byte[] POLLING_BYTE_ARRAY = new byte[0];

        public StreamExtension(Stream s)
        {
            stream = s;
        }

        public bool IsConnected()
        {
            try
            {
                Console.WriteLine("Testing Stream");
                stream.Write(POLLING_BYTE_ARRAY, 0, POLLING_BYTE_ARRAY.Length);
                stream.Write(POLLING_BYTE_ARRAY, 0, POLLING_BYTE_ARRAY.Length);
                Console.WriteLine("Stream OK.");
                return true;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
        }
        public override bool CanRead
        {
            get
            {
                return stream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return stream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return stream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return stream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return stream.Position;
            }

            set
            {
                stream.Position = value;
            }
        }

        public override void Flush()
        {
            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);
        }
    }
}
