namespace cmra
{
  #region main structure

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

  public struct MessageOutgoing
  {
    public string topic;
    public object message;
  }


  #endregion


}