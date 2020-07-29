/*using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using cmra;
public class UnityComms : MonoBehaviour
{
  public string awakeAPI = "game_awake";

  // public Robot robot; Need to make Generic on core...
  [HideInInspector]
  public MessageDispatcher messageDispatcher;

  private VirtualWorldsAPI.GameSettingProps game;
  private UnitySceneLoader sceneLoader;



  /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake()
  {

    // ...DESTROTY IF ALREADY IN EXISTANCE
    var objects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "UnityComms").ToArray();
    if (objects.Length > 1)
    {
      // Destroy self
      Destroy(this.gameObject);
    }

    sceneLoader = GetComponent<UnitySceneLoader>();
    SceneManager.sceneLoaded += NewSceneLoaded;
    messageDispatcher = GetComponent<MessageDispatcher>();
    messageDispatcher.addMessageListener(new Subscriber("game_settings", OnIncomingGameSettings));
    messageDispatcher.addMessageListener(new Subscriber("keyboard", OnIncomingKeyboardToggle));
#if !UNITY_EDITOR && UNITY_WEBGL
    WebGLInput.captureAllKeyboardInput = false;
#endif

    DontDestroyOnLoad(this.gameObject);
  }

  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  void Start()
  {
    StartCoroutine(FirstFrameMessage());

  }
  void NewSceneLoaded(Scene scene, LoadSceneMode mode)
  {

    // if there are game settings...send a message to react to let them know it is ready.
    if (!string.IsNullOrEmpty(game.game_settings.scene_name))
    {
      StartCoroutine(FirstFrameMessage());
    }
  }

  IEnumerator FirstFrameMessage()
  {
    yield return new WaitForEndOfFrame();
    SendData(null, awakeAPI);

  }
  private bool isUploading = false;

  private List<PostableObject> dataQueue = new List<PostableObject>();

  #region FrontEndMessage
  private bool once = false;
  public void SendData(object message, string topic = "generic")
  {

#if UNITY_WEBGL && !UNITY_EDITOR
    Message outgoing = new Message();
    outgoing.topic = topic;
    outgoing.message = message;
    SendMessageFromUnity(JsonConvert.SerializeObject(outgoing));
#endif
#if UNITY_EDITOR
    if (topic == awakeAPI && sceneLoader.isInMainMenu && !once) {
      once = true;
      // Send a message back with fakse settings..
      Message rc = new Message();
      rc.topic = "game_settings";
      VirtualWorldsAPI.MessageCommand messageOBJ;
      messageOBJ.api = "load_scene";
      VirtualWorldsAPI.GameSettingProps game;
      VirtualWorldsAPI.GameSettings settings;
      settings.difficulty = "level2";
      settings.scene_name = "default";
      game.game_settings = settings;
      messageOBJ.param = game;
      rc.message = messageOBJ;
      var outgoing = JsonConvert.SerializeObject(rc);
     // Debug.Log(outgoing);
      messageDispatcher.RecieveMessage(outgoing);
    }
#endif

  }
  
  public string getCurrentDifficulty()
  {
    if (game.game_settings.difficulty == null)
    {
      return "";
    }
    else
    {
      return game.game_settings.difficulty;
    }
  }
  private void OnIncomingGameSettings(object message)
  {
    VirtualWorldsAPI.MessageCommand command = GetCommand(message.ToString());
    // This is only one api...Look in the params.
    game = JsonConvert.DeserializeObject<VirtualWorldsAPI.GameSettingProps>(command.param.ToString());
    // Debug.Log(game);
    if (game.game_settings.scene_name == "default")
    {
      // Debug.Log("default");
      sceneLoader.LoadLevelByIndex(1); // Load the second scene
    }
    else
    {
      sceneLoader.LoadLevelByName(game.game_settings.scene_name);
    }
  }

  private void OnIncomingKeyboardToggle(object message)
  {

    VirtualWorldsAPI.MessageCommand command = GetCommand(message.ToString());
    VirtualWorldsAPI.CaptureAllKeyboard keyboardSetting = JsonConvert.DeserializeObject<VirtualWorldsAPI.CaptureAllKeyboard>(command.param.ToString());
    if (keyboardSetting.enable_capture)
    {
      WebGLInput.captureAllKeyboardInput = true;
    }
    else
    {
      WebGLInput.captureAllKeyboardInput = false;
    }
  }
  #endregion

  #region WebPost
  private void PostComplete()
  {
    // Debug.Log("POST Request Complete");

  }

  public void StartUpload(string data, string URL, PostableObject.Del callback)
  {
    PostableObject po = new PostableObject();
    po.data = data;
    po.url = URL;
    po.callback = callback;
    StartCoroutine(PostData(po));
  }
  IEnumerator PostData(PostableObject data)
  {

    //Debug.Log("Posting......");
    //    Debug.Log(data.data);
    byte[] bytePostData = Encoding.UTF8.GetBytes(data.data);
    UnityWebRequest request = UnityWebRequest.Put(data.url, bytePostData); //use PUT method to send simple stream of bytes
    request.method = "POST"; //hack to send POST to server instead of PUT
    request.SetRequestHeader("Content-Type", "application/json");
    // Debug.Log(request.ToString());
    yield return request.SendWebRequest();
    if (request.isNetworkError || request.isHttpError)
    {
      Debug.Log(request.error);
    }
    else
    {
      data.callback(request.downloadHandler.text);
    }
    PostComplete();
  }
  protected static byte[] GetBytes(string str)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(str);
    return bytes;
  }
  #endregion

  private VirtualWorldsAPI.MessageCommand GetCommand(string message)
  {
    return JsonConvert.DeserializeObject<VirtualWorldsAPI.MessageCommand>(message.ToString());
  }
}



public class PostableObject
{
  public string data;
  public string url;
  public delegate void Del(string dataSet);
  public Del callback;
}


public class RecievedMessage
{
  public string message;
  public string units;
}*/