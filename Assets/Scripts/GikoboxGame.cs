using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public abstract class GikoboxGame : MonoBehaviour
{
    public delegate void OnMessageReceived(WsMessage message, string jsonMessage);
    public delegate void OnConnectionReady();

    public int minPlayers;
    public int maxPlayers;

    public string key;
    public string room;
    public List<Player> players;

    protected Stage currentStage;

    [SerializeField]
    protected List<Stage> stages;

    [SerializeField]
    public Connection connection;

    [SerializeField]
    protected TextMeshProUGUI roomCode;


    public virtual void Start()
    {
        this.players = new List<Player>();
        connection.Initialize(onMessageReceived, onConnectionReady);
        this.currentStage = stages[0];
        for(int i = 1; i < this.stages.Count; i++){
            this.stages[i].hide();
        }
        this.currentStage.Init();
        this.currentStage.show();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void changeStage(Stage newStage){
        this.currentStage.hide();
        this.currentStage = newStage;
        this.currentStage.show();
    }

    public void nextStage(){
        Stage oldStage = this.currentStage;
        int nextStageIndex = this.stages.IndexOf(this.currentStage) + 1;
        if (nextStageIndex < this.stages.Count){
            this.currentStage = this.stages[nextStageIndex];
            this.currentStage.Init();
            this.currentStage.show();
            oldStage.hide();
        }
    }

    public virtual void onConnectionReady(){
        this.createRoom();
    }

    public virtual void createRoom(){
        WsSimpleMessage createRoomMessage = new WsSimpleMessage("", WsMessage.CREATEROOM, "", "", "");
        connection.wsSendMessage(JsonConvert.SerializeObject(createRoomMessage));
    }

    public virtual void onPlayerJoin(WsMessage playerJoinMessage){
        this.players.Add(new Player(playerJoinMessage.player, playerJoinMessage.key));
        if (this.players.Count == 1){
            this.players[0].vip = true;
        }
        Debug.Log("Player added: " + playerJoinMessage.player);
        if (this.players.Count >= this.minPlayers){
            this.makeGameStartable();
        }
    }

    public virtual void makeGameStartable(){
        WsSimpleMessage startableMessage = new WsSimpleMessage(this.key, WsMessage.GAMESTARTABLE, this.room, "GM", "");
        connection.wsSendMessage(JsonConvert.SerializeObject(startableMessage));
    }

    public virtual void onRoomCreated(WsMessage roomCreatedMessage){
        this.roomCode.text = roomCreatedMessage.room;
        this.room = roomCreatedMessage.room;
        this.key = roomCreatedMessage.key;
    }

    public abstract void onGameMessageReceived(WsMessage deserializedMessage, string jsonMessage);
        // Let the game decide how it wants to parse the json
        // Main Game Logic Here

    public virtual void onGameStartReceived(WsMessage message){
        if (this.players.Count < 1){
            return;
            
        }
        Player vip_player = this.players[0];
        foreach (Player p in this.players){
            if (p.vip){
                vip_player = p;
            }
        }

        if (vip_player != null && message.key == vip_player.key){
            this.startGame();
        }
    }

    public abstract void startGame();

    public virtual void onMessageReceived(WsMessage message, string json){
        Debug.Log("New message received of type " + message.type);
        switch(message.type){
            case WsMessage.CREATEROOM:
                onRoomCreated(message);
                break;
            case WsMessage.PLAYERJOIN:
                onPlayerJoin(message);
                break;
            case WsMessage.GAMESTART:
                onGameStartReceived(message);
                break;
            case WsMessage.GAMEMESSAGE:
                onGameMessageReceived(message, json);
                break;
        }
    }

    public virtual void loadNewPage(string newPage){
        WsSimpleMessage sendNewPage = new WsSimpleMessage(this.key, WsMessage.LOADPAGE, this.room, "GM", newPage);
        Debug.Log("Sent new stage message : " + newPage);
        connection.wsSendMessage(JsonConvert.SerializeObject(sendNewPage));
    }

    public virtual void loadNewPageSpecificPlayer(string newPage, Player p){
        WsSimpleMessage sendNewPage = new WsSimpleMessage(this.key, WsMessage.LOADPAGESPECIFIC, this.room, p.key, newPage);
        connection.wsSendMessage(JsonConvert.SerializeObject(sendNewPage));
    }

    public virtual void sendGameMessage(){

    }

    public Player getPlayer(string key){
        foreach (Player p in this.players){
            if (p.key == key){
                return p;
            }
        }
        return null;
    }

}
