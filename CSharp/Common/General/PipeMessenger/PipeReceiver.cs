using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MessageHandler
{
    /// <summary>
    /// This pipe exclusively recieves information
    /// </summary>
    public class PipeReceiver<T> where T : new()
    {
        #region private fields
        private byte[] _messageBytes;
        private XmlSerializer _xmlSerializer;
        private NamedPipeServerStream _pipe;
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type"></param>
        public PipeReceiver(Type type)
        {
            this._xmlSerializer = new XmlSerializer(type);
            this._pipe = new NamedPipeServerStream(Name.PipeName, PipeDirection.In, 1, PipeTransmissionMode.Message);
            
            // wait for connection
            this._pipe.WaitForConnection();
        }

        /// <summary>
        /// Receives and returns a message from pipe.
        /// If a message has not been received, waits until a message is received
        /// </summary>
        /// <returns></returns>
        public T ReceiveMessage()
        {
            // if we're not connected return blank object
            if (this._pipe == null || !this._pipe.IsConnected)
            {
                // log error - not connected
                return new T();
            }

            // read message
            var builder = new StringBuilder();
            this._messageBytes = new byte[1];
            do
            {
                this._pipe.Read(this._messageBytes, 0, this._messageBytes.Length);
                var msgChunk = Encoding.UTF8.GetString(this._messageBytes);
                builder.Append(msgChunk);
            }
            while (!this._pipe.IsMessageComplete);

            // deserialize message back to object
            using (var stringReader = new StringReader(builder.ToString()))
            {
                try
                {
                    var reader = new StringReader(builder.ToString());
                    var xmlReader = XmlReader.Create(reader);
                    T message = (T)this._xmlSerializer.Deserialize(xmlReader);

                    // return message
                    return message;
                }
                catch (Exception e)
                {
                    // log error(e) - deserialization failed
                    return new T();
                }
            }
        }
    }
}
