using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.PipedMessenger
{
    /// <summary>
    /// This pipe exclusively recieves information
    /// </summary>
    public class PipeReceiver<T> where T : new()
    {
        #region private fields
        private byte[] _messageBytes;
        private StringBuilder _stringBuilder;
        private StringReader _stringReader;
        private XmlSerializer _xmlSerializer;
        private NamedPipeServerStream _pipe;
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type"></param>
        public PipeReceiver(Type type)
        {
            this._stringBuilder = new StringBuilder();
            this._xmlSerializer = new XmlSerializer(type);
            this._pipe = new NamedPipeServerStream(Name.PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message);

            // wait for connection
            this._pipe.WaitForConnection();
        }

        /// <summary>
        /// Receives and returns a message from pipe
        /// </summary>
        /// <returns></returns>
        public T ReceiveMessage()
        {
            this._messageBytes = new byte[1];
            do
            {
                this._pipe.Read(this._messageBytes, 0, this._messageBytes.Length);
                var msgChunk = Encoding.UTF8.GetString(this._messageBytes);
                this._stringBuilder.Append(msgChunk);
            }
            while (!this._pipe.IsMessageComplete);

            this._stringReader = new StringReader(this._stringBuilder.ToString());
            var xmlReader = XmlReader.Create(this._stringReader);
            T message = (T)this._xmlSerializer.Deserialize(xmlReader);
            return message;
        }
    }
}
