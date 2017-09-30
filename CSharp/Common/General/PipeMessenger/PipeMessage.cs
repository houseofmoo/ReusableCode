
namespace Common.PipedMessenger
{


    /// <summary>
    /// A message that can be sent between the pipe classes
    /// </summary>
    public class PipeMessage
    {
        // name of the pipe created
        public const string PipeName = "RoccMessagePipe";
        public const int TimeOutTime = 10000; // 10 seconds

        public MessageType  MessageType     { get; set; }
        public string       Message         { get; set; }
        public int          CurrentProgress { get; set; }
        public int          TotalOperations { get; set; }
    }

    /// <summary>
    /// Message types
    /// </summary>
    public enum MessageType
    {
        Begin,
        Progress,
        End,
        Error,
    }
}
