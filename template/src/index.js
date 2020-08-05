import React, { useEffect, useState } from 'react';
import ReactDOM from 'react-dom';
import './style/styles.scss';
import { addTopicListener, removeTopicListener, sendUnityMessage, sendUnityMessageAsync } from "./unity_api";

import UnityWorld from "./UnityWorld";

const Index = () => {

  const [connected, setConnection] = useState(false);

  const [testing, setTesting] = useState(false);

  const handleTestMessage = (message) => {
    setConnection(true);
    removeTopicListener(unityAwakeSubscriber);
  }

  const unityAwakeSubscriber = { topic: "unity_awake", callback: handleTestMessage }

  useEffect(() => {
    addTopicListener(unityAwakeSubscriber);
  }, [])

  
  const handleTest = async () => {
    const msg = { topic: "test", message: { method: "startTest", parameters: { i_testTime: 3, resolver: "endTest" } } }
    setTesting(true);
    await sendUnityMessageAsync(msg, "endTest").then((message) => {
      setTesting(false);
    });
  }
  return (
    <div>
      <UnityWorld />
      {
        connected ?
          <button onClick={handleTest} className={"test-button " + (testing ? "button-locked" : "")}>
            {testing ? "Testing..." : "Start Communication Test"}
          </button> :
          <span>Unity is still connecting...</span>
      }
    </div>);
};
ReactDOM.render(<Index />, document.getElementById('root'));