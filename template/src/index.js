import React, { useEffect, useState } from 'react';
import ReactDOM from 'react-dom';
import './style/styles.scss';
import { addTopicListener ,removeTopicListener, sendUnityMessage, sendUnityMessageAsync} from "./unity_api";

import UnityWorld from "./UnityWorld";

const Index = () => {
  
  const [connected, setConnection] = useState(false);

  const handleTestMessage = (message) => {
    setConnection(true);
    removeTopicListener(unityAwakeSubscriber);
  }

  const unityAwakeSubscriber = { topic: "unity_awake", callback: handleTestMessage }

  useEffect(() => {
    addTopicListener(unityAwakeSubscriber);
  }, [])

  const handleTest = async () => {
    const msg = {topic: "test", message: {method: "startTest", parameters:{message: "hello", valueInt: 3}}}
    await sendUnityMessageAsync(msg, "endTest").then ((message) => {
      console.log("Test Ended ", message);
    });
  }
  return (
    <div>
      <UnityWorld />
      {
        connected ?
          <button onClick={handleTest}>Start Communication Test</button> :
          <span>Unity is still connecting...</span>
      }
    </div>);
};
ReactDOM.render(<Index />, document.getElementById('root'));