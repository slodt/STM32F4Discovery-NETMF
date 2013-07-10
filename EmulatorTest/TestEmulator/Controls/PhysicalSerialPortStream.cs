using System.IO;
using System.IO.Ports;

namespace TestEmulator.Controls
{
    public class PhysicalSerialPortStream : Stream
    {
        readonly SerialPort _port;

        public PhysicalSerialPortStream(SerialPort port)
        {
            _port = port;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _port.Write(buffer, offset, count);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _port.Read(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            _port.Dispose();
            base.Dispose(disposing);
        }

        public override void SetLength(long value)
        {
            _port.BaseStream.SetLength(value);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _port.BaseStream.Seek(offset, origin);
        }

        public override long Position
        {
            get
            {
                return _port.BaseStream.Position;
            }
            set
            {
                _port.BaseStream.Position = value;
            }
        }

        public override long Length
        {
            get { return _port.BaseStream.Length; }
        }

        public override void Flush()
        {
            _port.BaseStream.Flush();
        }

        public override bool CanWrite
        {
            get { return _port.BaseStream.CanWrite; }
        }
        public override bool CanSeek
        {
            get { return _port.BaseStream.CanSeek; }
        }
        public override bool CanRead
        {
            get { return _port.BaseStream.CanRead; }
        }
    }
}