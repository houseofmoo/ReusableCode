
namespace MessageHandler
{


    /// <summary>
    /// A message that can be sent between the pipe classes
    /// </summary>
    public class PipeMessage
    {
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
        public int CurrentProgress { get; set; }
        public int TotalOperations { get; set; }
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

    /// <summary>
    /// Holds constant values used with pipe objects
    /// </summary>
    public static class Name
    {
        public const string PipeName = "GenericPipeName";
        public const int TImeOutTime = 10000;   // 10 seconds
        public const string ServerName = ".";   // this means server is the 
                                                // machine  this process is running on
    }
}
