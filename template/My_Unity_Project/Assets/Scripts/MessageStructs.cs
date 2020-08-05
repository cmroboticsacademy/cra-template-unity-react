namespace cmra
{

  // These structs are the skelleton of how to send messages back and forth.
  #region Incoming Message
  public struct MessageTopic
  {
    public string topic;
    public MessagePacket message;
  }
  public struct MessagePacket
  {
    public string method;
    public object parameters;
  }
  #endregion
  #region Outgoing Message
  public struct MessageOutgoing
  {
    public string topic;
    public object message;
  }
  #endregion



}