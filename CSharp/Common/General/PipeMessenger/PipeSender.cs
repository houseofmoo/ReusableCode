using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Xml.Serialization;

namespace Common.PipedMessenger
{
    /// <summary>
    /// This pipe exclusively sends information
    /// </summary>
    public class PipeSender<T> where T : new()
    {
        #region private fields
        private XmlSerializer _xmlSerializer;
        private StringWriter _stringWriter;
        private NamedPipeClientStream _pipe;
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type"></param>
        public PipeSender(Type type)
        {
            this._xmlSerializer = new XmlSerializer(type);
            this._stringWriter = new StringWriter();
            this._pipe = new NamedPipeClientStream(PipeMessage.PipeName);

            // attempt to connect for 10 seconds
            this._pipe.Connect(PipeMessage.TimeOutTime);
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="messageObject"></param>
        public void Send(T messageObject)
        {
            this._xmlSerializer.Serialize(this._stringWriter, messageObject);
            var message = Encoding.UTF8.GetBytes(this._stringWriter.ToString());
            this._pipe.Write(message, 0, message.Length);
        }
    }
}
