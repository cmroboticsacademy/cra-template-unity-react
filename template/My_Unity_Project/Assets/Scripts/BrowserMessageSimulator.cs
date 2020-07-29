using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using cmra;
public class BrowserMessageSimulator : MonoBehaviour
{
  private cmra.MessageDispatcher messageDispatcher;
  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  void Start()
  {
   messageDispatcher = GetComponent<cmra.MessageDispatcher>();   
   messageDispatcher.addMessageListener(new Subscriber("test", startTest));
   messageDispatcher.addMessageListener(new Subscriber("test", endTest));
  }


  private void startTest(object parameters) {
    bogusParameter bg = JsonConvert.DeserializeObject<bogusParameter>(parameters.ToString());
    Debug.Log("startTest Called " + bg.message);
    Debug.Log("Value " + bg.valueInt);
  }
  private void endTest(object _parameters) {
    Debug.Log("No Params Needed");
  }

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update()
  {
      if (Input.GetKeyDown(KeyCode.E)) {
        MessageTopic topicMessage;
        topicMessage.topic = "test";
        MessagePacket methodMessage;
        methodMessage.method = "startTest";
        bogusParameter bp;
        bp.message = "high";
        bp.valueInt = 30;
        methodMessage.parameters = bp;
        topicMessage.message = methodMessage;
        var outgoing = JsonConvert.SerializeObject(topicMessage);
        Debug.Log("outgoing message -> " + outgoing);
        messageDispatcher.UnityMessengerDispatcher(outgoing);
      }
      if (Input.GetKeyDown(KeyCode.T)) {
        MessageTopic topicMessage;
        topicMessage.topic = "test";
        MessagePacket methodMessage;
        methodMessage.method = "endTest";
        //bogusParameter bp;
        //bp.message = "high";
        //bp.valueInt = 30;
        //methodMessage.parameters = bp;
        methodMessage.parameters = null;
        topicMessage.message = methodMessage;
        var outgoing = JsonConvert.SerializeObject(topicMessage);
        Debug.Log("outgoing message -> " + outgoing);
        messageDispatcher.UnityMessengerDispatcher("{\"topic\":\"test\",\"message\":{\"method\":\"endTest\"}}");
      }
  }
  struct bogusParameter {
    public string message;
    public int valueInt;
  }
}

