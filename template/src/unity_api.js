



export const registerEvents = (unityContent) => {
  console.log(unityContent);
  unityContent.on("MessageUnityOutgoing", message => {
    console.log("message", message);
    parseMessage(message);
  })
};

const parseMessage = (message) => {
  console.log("message", JSON.parse(message));
}

export const selectObject = ({ name, rotation }) => {
  sendUnityMessage(
    {
      topic: 'label_view',
      message: {
        api: 'change_object',
        param: {
          name,
          rotation,
        },
      },
    },
    unity()
  );
};

export const takeSnapshot = (id, label) => {
  sendUnityMessageAsync(
    {
      topic: 'label_view',
      message: {
        api: 'get_image_view',
      },
    },
    unity(), "img_raw"
  ).then((response) => {
    const binary = response.message.value;
    addExample(binary, label);
    store.dispatch(addImage(id, binary));
  });
};

export const keyboardOverride = (keyevent) => {
  sendUnityMessage(
    {
      topic: 'keyboard_override',
      message: {
        api: keyevent,
      },
    },
    unity()
  );
};
