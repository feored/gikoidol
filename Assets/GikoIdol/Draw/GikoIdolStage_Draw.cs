using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class GikoIdolStage_Draw : Stage
{
    [SerializeField]
    private GikoidolText text;

    [SerializeField]
    private GameObject shii;

    [SerializeField]
    private GameObject timerBackground;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Image gikoDrawing;

    private float timeLeft;
    private bool timer;

    private bool requestedDrawing;

    // Start is called before the first frame update
    public override void Start()
    {
        this.timer = false;
        this.requestedDrawing = false;
    }

    private void startTimer(){
        this.timeLeft = Settings.TIMER_DRAW;
        this.timerBackground.SetActive(true);
        this.timerBackground.transform.localScale = new Vector3(0f, 0f, 0f);
        LeanTween.scale(this.timerBackground, new Vector3(1,1,1), 2f).setEase(LeanTweenType.easeOutQuart);
        this.timer = true;
    }

    private void updateTimerText(){
        this.timerText.text = ((int)this.timeLeft).ToString();
    }

    private void timerOver(){
        foreach (Player p in this.game.players){
            this.game.playerData[p].avatar.highlight(true);
        }
        this.game.loadNewPage("empty");
        this.game.nextStage();
    }

    IEnumerator scriptwriter()
    {
        this.text.fadeIn();

        LeanTween.scale(this.shii, new Vector3(1,1,1), 3f).setEase(LeanTweenType.easeInBounce);

        this.game.loadNewPage("GikoIdolDraw");

        yield return new WaitForSeconds(1f);

        this.text.changeText("Draw an Idol that possesses all the traits picked for her!");

        this.sendTraits();

        this.startTimer();
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if (timer){
            this.timeLeft -= Time.deltaTime;
            this.updateTimerText();

            if (this.timeLeft < 3 && !this.requestedDrawing){
                this.requestSubmitDrawings();
                this.requestedDrawing = true;
            }


            if(this.timeLeft < 0)
                {
                    timerOver();
                    timer = false;
                }
        }
        
    }

    public void sendTraits(){


        // Shuffle reserve list
        var random = new System.Random();
        List<string> usableReserve = new List<string>();
        foreach (string prompt in this.game.reserveTraits){
            usableReserve.Insert(random.Next(0, usableReserve.Count + 1), prompt);
        }


         // Send 3 random traits
        List<string> availableTraits = this.game.traits.ToList();
        if (availableTraits.Count < this.game.players.Count * 3){
            // Add traits from the list we already have
            int missingTraits = (this.game.players.Count * 3) - availableTraits.Count;
            for (int i = 0; i < missingTraits; i++){
                availableTraits.Add(usableReserve[i]);
            }
        }
        for (int i = 0; i < this.game.players.Count; i++){
            
            dynamic traitMessage = new JObject();
            traitMessage.key = this.game.key;
            traitMessage.room = this.game.room;
            traitMessage.type = WsMessage.TARGETEDGAMEMESSAGE;
            traitMessage.player = this.game.players[i].key;
            traitMessage.message = new JArray();

            for (int j = 0; j < 3; j++){
                int index = random.Next(availableTraits.Count);
                this.game.idols[i].traits.Add(availableTraits[index]);
                this.game.playerData[this.game.players[i]].traits.Add(availableTraits[index]);
                traitMessage.message.Add(availableTraits[index]);
                availableTraits.RemoveAt(index);
            }

            this.game.connection.wsSendMessage(traitMessage.ToString());
        }
    }

    public void requestSubmitDrawings(){
        foreach (Player p in this.game.players){
            if (null == this.game.playerData[p].canvas){
                requestSubmitPlayerDrawing(p);
            }
        }
    }

    public void requestSubmitPlayerDrawing(Player p){
        dynamic requestSubmitMessage = new JObject();
        requestSubmitMessage.key = this.game.key;
        requestSubmitMessage.room = this.game.room;
        requestSubmitMessage.type = WsMessage.TARGETEDGAMEMESSAGE;
        requestSubmitMessage.player = p.key;
        requestSubmitMessage.message = "SUBMIT";
        this.game.connection.wsSendMessage(requestSubmitMessage.ToString());
    }

    public override void Init(){
       
    }

    public override void hide(){
        this.gameObject.SetActive(false);
    }

    public override void show(){
        this.gameObject.SetActive(true);
        StartCoroutine(scriptwriter());
    }
    
    public bool checkAllReady(){
        if (this.requestedDrawing){
            // in case time runs short and 
            // someone had to have their drawing autosent
            // because of lack of time,
            // might as well give them the remaining of the time
            // in case they want to finish
            return false;
        }
        bool allReady = true;
        foreach (Player p in this.game.players){
            if (null == this.game.playerData[p].canvas){
                allReady = false;
                break;
            }
        }
        return allReady;
    }

    public override void handleMessage(WsMessage message, string json){
        Debug.Log("Specs stage received message:");
        dynamic d = JObject.Parse(json);
        string base64image = d.message.canvas;
        string idolName = d.message.name;
        string[] b64chunks = base64image.Split(new string[] { "base64," }, System.StringSplitOptions.None);
        if (b64chunks.Length != 2){
            Debug.Log("Wrong format for base64 string, returning.");
            return;
        }
        byte[]  imageBytes = System.Convert.FromBase64String(b64chunks[1]);
        Player p = this.game.getPlayer(message.key);
        this.game.playerData[p].canvas = imageBytes;
        this.game.playerData[p].idolName = idolName;
        this.game.playerData[p].avatar.highlight(true);

        if (checkAllReady()){
            this.timerOver();
        }
    }

}
