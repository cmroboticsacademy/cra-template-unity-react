using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using cmra;
using System.Runtime.InteropServices;

namespace cmra
{
  public class MessageDispatcher : MonoBehaviour
  {
    public bool sendAwakeCall = true;
    public string awake_message = "unity_awake";

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
      if (sendAwakeCall)
      {
        StartCoroutine(UnityMessengerReady());
      }
    }

    IEnumerator UnityMessengerReady()
    {
      yield return new WaitForEndOfFrame();
      SendData(null, awake_message);
    }
    public void SendData(object message, string topic = "generic")
    {
      MessageOutgoing outgoing;
      outgoing.topic = topic;

      outgoing.message = message;

      #if UNITY_WEBGL
          Debug.Log("SENDING MESSAGE FROM UNITY " + outgoing.ToString());
          MessageUnityOutgoing(JsonConvert.SerializeObject(outgoing));

      #endif
      #if UNITY_EDITOR
            Debug.Log("Will send " + outgoing.topic);
      #endif
    }

    public List<Subscriber> subscribers = new List<Subscriber>();


    // Method that recieves Messages from the browser.
    public void UnityMessengerDispatcher(string message)
    {

      RecieveMessage(message);
    }

    public virtual void RecieveMessage(string objectString)
    {
      // serialize class and then send to subscribers.
      MessageTopic messageTopic = JsonConvert.DeserializeObject<MessageTopic>(objectString);

      // loop through messages and call the subscribers with the matching topics.
      for (int i = 0; i < subscribers.Count; i++)
      {
        if (subscribers[i].topic == messageTopic.topic &&
        subscribers[i].callback.Method.Name == messageTopic.message.method)
        {
          subscribers[i].callback(messageTopic.message.parameters);
        }
      }

    }

    public virtual void addMessageListener(Subscriber subscriber)
    {
      subscribers.Add(subscriber);
    }

    [DllImport("__Internal")]
    private static extern void MessageUnityOutgoing(string str);

  }
  public class Subscriber
  {
    public Subscriber(string topic, Del callback)
    {
      this.topic = topic;
      this.callback = callback;
    }
    public string topic;
    public delegate void Del(object list);
    public Del callback;
  }
}