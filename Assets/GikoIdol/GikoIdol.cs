using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

public class GikoIdol : GikoboxGame
{
    string gameName = "GikoIdol";

    public List<string> traits;

    public List<string> reserveTraits;

    [SerializeField]
    private GameObject gikoPlayerPrefab;

    [SerializeField]
    private GameObject gikoPlayerContainer;

    public Dictionary<Player, GikoIdolPlayerData> playerData;

    // Start is called before the first frame update
    public override void Start()
    {


        this.minPlayers = 2;
        this.maxPlayers = 8;


        this.traits = new List<string>();
        this.reserveTraits = new List<string>(){
            "Message just in case there aren't enough #1",
            "Message just in case there aren't enough #2",
            "Message just in case there aren't enough #3",
            "Message just in case there aren't enough #4",
            "Message just in case there aren't enough #5",
            "Message just in case there aren't enough #6",
            "Message just in case there aren't enough #7",
            "Message just in case there aren't enough #8",
            "Message just in case there aren't enough #9",
        };
        this.playerData = new Dictionary<Player, GikoIdolPlayerData>();
        base.Start();
        
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public override void createRoom(){
        WsSimpleMessage createRoomMessage = new WsSimpleMessage("", WsMessage.CREATEROOM, "", "", "GikoIdol");
        connection.wsSendMessage(JsonConvert.SerializeObject(createRoomMessage));
    }

    public override void startGame(){
        Debug.Log("Game Start!");
        this.nextStage();
    }

    public override void onGameMessageReceived(WsMessage message, string json){
        //Debug.Log(message);
        this.currentStage.handleMessage(message, json);
    }

    public override void onPlayerJoin(WsMessage playerJoinMessage){
        Player newPlayer = new Player(playerJoinMessage.player, playerJoinMessage.key);
        this.players.Add(newPlayer);
        if (this.players.Count == 1){
            this.players[0].vip = true;
        }
        this.playerData[newPlayer] = new GikoIdolPlayerData();
        Debug.Log("Player added: " + playerJoinMessage.player);
        if (this.players.Count >= this.minPlayers){
            this.makeGameStartable();
        }
        StartCoroutine(addPlayerSidebar(newPlayer));
    }

    public IEnumerator addPlayerSidebar(Player p)
    {
        GameObject gikoAvatar = Instantiate(this.gikoPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        gikoAvatar.transform.SetParent(gikoPlayerContainer.transform);
        gikoAvatar.transform.localScale = new Vector3(1,1,1);
        GikoPlayer gp = gikoAvatar.GetComponent<GikoPlayer>();
        gp.hideScore();
        gp.hideCheckmark();
        gp.setName(p.name);
        this.playerData[p].avatar = gp;
        LeanTween.alphaCanvas (gikoAvatar.GetComponent<CanvasGroup>(), 1f, 1f);

        // Send list of avatars
        dynamic avatarMessage = new JObject();
        avatarMessage.key = this.key;
        avatarMessage.room = this.room;
        avatarMessage.type = WsMessage.TARGETEDGAMEMESSAGE;
        avatarMessage.player = p.key;
        avatarMessage.message = new JArray();
        for (int i = 0; i < gp.sprites.Count; i++){
            avatarMessage.message.Add(gp.sprites[i].name);
        }
        this.connection.wsSendMessage(avatarMessage.ToString());

        yield return null;
    }

}
