using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using cmra;
// This test script is triggered by the browser. It will send a resolving message back to the browser when complete. 
public class MessageTesting : MonoBehaviour
{
  
  // Message Structure for Start Test Parameters.
  private struct StartTestParameters
  {
    public int i_testTime;
    public string resolver;

  }
  private cmra.MessageDispatcher messageDispatcher;

  void Start()
  {
    // Find the MessageDispatcher script
    messageDispatcher = GetComponent<cmra.MessageDispatcher>();
    // Add an event listener. This subscribes to the topic test, with a method name of startTest which is also a function below.
    messageDispatcher.addMessageListener(new Subscriber("test", startTest));
  }

  private void startTest(object parameters)
  {
    // Deserialize the parameters again. TODO: Come up with a solution around having to do this. Simply casting did not work out. 
    StartTestParameters testParams = JsonConvert.DeserializeObject<StartTestParameters>(parameters.ToString());
    // Pass the parameters to the test coroutine.
    StartCoroutine(TestCoroutine(testParams));

  }
  IEnumerator TestCoroutine(StartTestParameters parameters)
  {
    // This test simply waits three seconds. It expects a resolver, but does not need one.
    yield return new WaitForSeconds(parameters.i_testTime);
    Debug.Log("END COROUTINE. Has resolver? " + parameters.resolver);
    if (parameters.resolver != null) {
      ResolveMessage(parameters.resolver);
    }
  }
  private void ResolveMessage(string resolver)
  {
    // Sends a message to the browser with a topic of the resolver variable. This message does not need to send an object as parameters.
    messageDispatcher.SendData(null, resolver);
  }

}

