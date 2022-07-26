using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WsMessage
{

    public const string CREATEROOM = "create-room";
    public const string PLAYERJOIN = "player-join";
    public const string GAMEMESSAGE = "game-message";
    public const string GAMESTART = "game-start";
    public const string GAMESTARTABLE = "GAMESTARTABLE";
    public const string LOADPAGE = "LOADPAGE";
    public const string TARGETEDGAMEMESSAGE = "TARGETEDGAMEMESSAGE";
    public const string DELETEROOM = "DELETEROOM";

    public string type;
    public string room;
    public string player;
    public string key;

    public WsMessage(){
        type = "";
        room = "";
        player = "";
        key = "";
    }

    public WsMessage(string key, string type, string room, string player){
        this.key = key;
        this.type = type;
        this.room = room;
        this.player = player;
    }
}
