using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using cmra;
using System.Runtime.InteropServices;

namespace cmra
{

  // A subscriber is used to add an event listner.
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

  public class MessageDispatcher : MonoBehaviour
  {
    [Tooltip("If true, Unity will send a unity_awake after the first frame")]
    public bool sendAwakeCall = true;
    [Tooltip("The topic of the message that can be sent on the first frame")]
    public string awake_message = "unity_awake";

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
      MessageUnityOutgoing(JsonConvert.SerializeObject(outgoing));
#endif
#if UNITY_EDITOR
      Debug.Log("No browser support for this message: " + outgoing.topic);
#endif
    }

    public List<Subscriber> subscribers = new List<Subscriber>();

    // This method gets called from the browser
    public void UnityMessengerDispatcher(string message)
    {
      MessageTopic messageTopic = JsonConvert.DeserializeObject<MessageTopic>(message);
      // loop through messages and call the subscribers with the matching topics.
      for (int i = 0; i < subscribers.Count; i++)
      {
        // if the topics and the methods name match, call the method...
        if (subscribers[i].topic == messageTopic.topic &&
        subscribers[i].callback.Method.Name == messageTopic.message.method)
        {
          subscribers[i].callback(messageTopic.message.parameters);
        }
      }
    }

    public void addMessageListener(Subscriber subscriber)
    {
      subscribers.Add(subscriber);
    }

    [DllImport("__Internal")]
    private static extern void MessageUnityOutgoing(string str);

  }

}