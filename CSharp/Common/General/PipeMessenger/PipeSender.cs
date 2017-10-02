using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Xml.Serialization;

namespace MessageHandler
{
    /// <summary>
    /// This pipe exclusively sends information
    /// </summary>
    public class PipeSender<T> where T : new()
    {
        #region private fields
        private XmlSerializer _xmlSerializer;
        private NamedPipeClientStream _pipe;
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type"></param>
        public PipeSender(Type type)
        {
            this._xmlSerializer = new XmlSerializer(type);
            this._pipe = new NamedPipeClientStream(Name.ServerName, Name.PipeName, PipeDirection.Out);
            
            // attempt to connect for 10 seconds
            this._pipe.Connect();
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="messageObject"></param>
        public void Send(T messageObject)
        {
            // if we're not connected, return
            if (this._pipe == null || !this._pipe.IsConnected)
            {
                // log error - not connected
                return;
            }

            // get message
            var writer = new StringWriter();
            this._xmlSerializer.Serialize(writer, messageObject);
            var message = Encoding.UTF8.GetBytes(writer.ToString());

            // send message
            try
            {
                this._pipe.Write(message, 0, message.Length);
            }
            catch (Exception e)
            {
                // if we throw an exception, we lost connection
                this._pipe.Close();
                // log error(e) - connection closed unexpectedly
            }
        }
    }
}
