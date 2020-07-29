mergeInto(LibraryManager.library, {
  MessageUnityOutgoing: function (str) {
    // Send string to ReactUnityWebGL...
    ReactUnityWebGL.MessageFromUnity(Pointer_stringify(str));
  },
});
