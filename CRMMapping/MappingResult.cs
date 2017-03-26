namespace CRMMapping
{
    public class MappingResult
    {
        public MappingResult(byte[] serializedMessageBody, string typeHeaderValue)
        {
            SerializedMessageBody = serializedMessageBody;
            TypeHeaderValue = typeHeaderValue;
        }

        public byte[] SerializedMessageBody { get; set; }
        public string TypeHeaderValue { get; set; }
    }
}