using System.Collections;
using System.Collections.Generic;

public class WsSimpleMessage : WsMessage
{
    public string message;

    public WsSimpleMessage() : base(){
        this.message = "";
    }

    public WsSimpleMessage(string key, string type, string room, string player, string message) : base (key, type, room, player){
        this.message = message;
    }
}
 