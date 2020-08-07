# Unity-React-Template

## Installing
1) Install [npx](https://www.npmjs.com/package/npx) 
2) Open a terminal and go to a folder where you want to download the template.
3) Enter the command: `npx create-react-app my-unity-react-app --template @carnegie-mellon-robotics-academy/cra-template-unity-react`
* "my-unity-react-app" is your new apps name. It must be lowercase with no spaces.

### Project Environment
This template installs a Unity Project along side of a react application. The root of the project is meant to be a workspace for your IDE.

## Getting Started
### Building Unity
1) Open the unity project "My_Unity_Project" (located in "your app's folder"/dist/). You may need to upgrade or downgrade, that is fine.
2) Open the Build Settings menu.
3) Click build. Name the game "unity_game" and save it in the dist folder (located in "your app's folder"/dist/)
### Building the app
1) cd into the root of the project.
2) run `npm build` or `yarn build`  (if the build fails try removing node_models and re-install)
3) Once, complete, run `npm start` or `yarn start`

### Subscribing to messages
JavaScript
```javascript
// When component mounts, listen for any message with a topic of unity_awake
const unityAwakeSubscriber = { topic: "unity_awake", callback: handleTestMessage }

  useEffect(() => {
    // addTopicListener imported from unity_api.js
    addTopicListener(unityAwakeSubscriber);
  }, [])
``` 
C#
```c#
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
```

### Sending Messages
JavaScript
```javascript
const handleTest = async () => {
    const msg = { topic: "test", message: { method: "startTest", parameters: { i_testTime: 3, resolver: "endTest" } } }
    setTesting(true);
    // Waits until Unity sends a message back with a topic equal to endTest.
    // This method is imported from unity_api.js
    await sendUnityMessageAsync(msg, "endTest").then((message) => {
      setTesting(false);
    });
  }
```
C#
```c#
// Sends a message to the browser with a topic of the resolver variable. This message does not need to send an object as parameters. "messageDispatcher" is assined on start.
messageDispatcher.SendData(null, resolver);
```
