using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using NativeWebSocket;

public class Connection : MonoBehaviour
{
  [SerializeField]
  private WebSocket websocket;
  private GikoboxGame.OnMessageReceived gikoboxSend;
  private GikoboxGame.OnConnectionReady onConnectionReady;

  // Start is called before the first frame update
  public async void Initialize(GikoboxGame.OnMessageReceived onMessageReceived, GikoboxGame.OnConnectionReady onConnectionReady)
  {
    Debug.Log("Init");
    this.gikoboxSend = onMessageReceived;
    this.onConnectionReady = onConnectionReady;
    string address = "ws://localhost:3000";
    if (false){
        address = "ws://gikobox.feor.org";
    }
    websocket = new WebSocket(address);

    websocket.OnOpen += () =>
    {
        Debug.Log("OnOpen");
        onConnectionReady();
    };

    websocket.OnError += (e) =>
    {
      Debug.Log("Error! " + e);
    };

    websocket.OnClose += (e) =>
    {
      Debug.Log("Connection closed!");
    };

    websocket.OnMessage += (bytes) =>
    {
        //Debug.Log("OnMessage!");
        //Debug.Log(bytes);

        // getting the message as a string
        var jsonMessage = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log("OnMessage! " + jsonMessage);

        WsMessage deserializedMessage = JsonConvert.DeserializeObject<WsMessage>(jsonMessage);
        gikoboxSend(deserializedMessage, jsonMessage);

        

    };

    // waiting for messages
    await websocket.Connect();
  }

  void Update()
  {
    if (websocket != null)
    {
      #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
      #endif
    }
  }

  public async void wsSendMessage(string json)
  {
    if (websocket.State == WebSocketState.Open)
    {
      await websocket.SendText(json);
    }
  }

  public async void wsSendBytes(byte[] bytes)
  {
    if (websocket.State == WebSocketState.Open)
    {
      // Sending bytes
      await websocket.Send(bytes);
    }
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }

}