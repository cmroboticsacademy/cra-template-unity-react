


let subscribers = new Array();
let unity = null;



const registerCommunicationHandler = (unityContent) => {
  unityContent.on("MessageFromUnity", message => {
    console.log("recived message", message);
    distributeMessage(JSON.parse(message));
  })
  unity = unityContent;
};


const distributeMessage = (message) => {
  subscribers.map(sub => {
    if (sub.topic == message.topic) {
      sub.callback(message);
    }
  })
}

const addTopicListener = (subscriber) => {
  subscribers.push(subscriber);
}

const removeTopicListener = (subscriber) => {
  subscribers = subscribers.filter(sub => sub == subscriber)
}

const sendUnityMessageAsync = (msg, resolver) =>
  new Promise((resolve) => {
    const responseSubscriber = {
      topic: resolver, 
      callback: (message) => {
        removeTopicListener(responseSubscriber);
        resolve(message);
      }
    }
    addTopicListener(responseSubscriber);
    sendUnityMessage(msg, unity);
  });

const sendUnityMessage = (msg, unity) => {
  const message = JSON.stringify(msg);
  unity.send('UnityDispatcher', 'UnityMessengerDispatcher', message);
};
export { removeTopicListener, addTopicListener, registerCommunicationHandler, sendUnityMessageAsync, sendUnityMessage }
