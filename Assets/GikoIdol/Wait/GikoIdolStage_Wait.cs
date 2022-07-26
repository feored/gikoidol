using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

public class GikoIdolStage_Wait : Stage
{

    public override void Start()
    {
    }

    /*
    IEnumerator scriptwriter()
    {

    }
    */

    // Update is called once per frame
    public override void Update()
    {

        
    }

    public override void Init(){
    }

    public override void hide(){
        this.gameObject.SetActive(false);
    }

    public override void show(){
        this.gameObject.SetActive(true);
        //StartCoroutine(scriptwriter());
    }

    public override void handleMessage(WsMessage message, string json){
        Debug.Log("Stage Wait received message:");
        Debug.Log(json);
        dynamic d = JObject.Parse(json);
        Player p = this.game.getPlayer(message.key);
        Debug.Log("Setting new image for player " + message.player + " : " + d.message);
        this.game.playerData[p].avatar.setImage(d.message.ToString());
    }

}
