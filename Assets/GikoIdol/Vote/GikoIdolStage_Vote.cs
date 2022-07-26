using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class GikoIdolStage_Vote : Stage
{
    [SerializeField]
    private GikoidolText text;

    [SerializeField]
    private GameObject timerBackground;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private float timeLeft;
    private bool timer;

    // Start is called before the first frame update
    public override void Start()
    {
        this.timer = false;
        this.timeLeft = Constants.TIMER_VOTE;
        foreach(Player p in this.game.players){
            this.game.playerData[p].avatar.highlight(false);
        }
    }

    private void startTimer(){
        this.timeLeft = Constants.TIMER_VOTE;
        this.timerBackground.SetActive(true);
        this.timerBackground.transform.localScale = new Vector3(0f, 0f, 0f);
        LeanTween.scale(this.timerBackground, new Vector3(1,1,1), 2f).setEase(LeanTweenType.easeOutQuart);
        this.timer = true;
        this.updateTimerText();
    }

    private void updateTimerText(){
        this.timerText.text = ((int)this.timeLeft).ToString();
    }

    private void timerOver(){
        this.game.nextStage();
    }

    IEnumerator scriptwriter()
    {
        this.game.loadNewPage("GikoIdolVote");
        this.text.setText("Vote for the best idol!");

        this.text.fadeIn();

        foreach(Player p in this.game.players){
            this.game.playerData[p].avatar.hideScore();
        }

        // We wait here to make sure loadpage
        // has been received before sending the vote
        // options
        yield return new WaitForSeconds(1.0f);
        
        this.sendOptions();

        yield return new WaitForSeconds(1.0f);

        this.startTimer();
        
    }

    public void sendOptions(){
        // Send names of all players to allow voting
        // of course, players can't vote for themselves
        foreach(Player p in this.game.players){
            
            dynamic voteMessage = new JObject();
            voteMessage.key = this.game.key;
            voteMessage.room = this.game.room;
            voteMessage.type = WsMessage.TARGETEDGAMEMESSAGE;
            voteMessage.player = p.key;
            voteMessage.message = new JArray();

            foreach(Player pp in this.game.players){
                if (pp != p){
                    // TODO : Use exterior ID when needing to differentiate between players
                    // Use a different ID from key used internally so players can't get each other's keys
                    // For now, use player name
                    JObject idolVote = new JObject();
                    idolVote.Add("idolName", this.game.playerData[pp].idolName);
                    idolVote.Add("player", pp.name);
                    voteMessage.message.Add(idolVote);
                }
            }
            Debug.Log("Sending options message");
            Debug.Log(voteMessage.ToString());
            this.game.connection.wsSendMessage(voteMessage.ToString());
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (this.timer){
            this.timeLeft -= Time.deltaTime;
            this.updateTimerText();
            if(this.timeLeft < 0)
                {
                    timerOver();
                    timer = false;
                }
        }
        
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
        bool allReady = true;
        foreach(Player p in this.game.players){
            if (System.String.IsNullOrEmpty(this.game.playerData[p].vote)){
                allReady = false;
                break;
            }
        }
        return allReady;
    }

    public override void handleMessage(WsMessage message, string json){
        Debug.Log("Vote stage received message:");
        Player p = this.game.getPlayer(message.key);
        dynamic d = JObject.Parse(json);
        string idolVote = d.message.vote.ToString();
        if (this.game.playerData[p].idolName != idolVote){
            Debug.Log("Registered vote: " + p.name + " voted for " + idolVote);
            this.game.playerData[p].vote = idolVote;
        }
        if (checkAllReady()){
            this.timerOver();
        }
    }

}
