using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

public class GikoIdolStage_Specs : Stage
{
    [SerializeField]
    private GikoidolText text;

    [SerializeField]
    private GameObject shii;

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
    }

    private void startTimer(){
        this.timeLeft = Constants.TIMER_SPECS;
        this.timerBackground.SetActive(true);
        this.timerBackground.transform.localScale = new Vector3(0f, 0f, 0f);
        LeanTween.scale(this.timerBackground, new Vector3(1,1,1), 2f).setEase(LeanTweenType.easeOutQuart);
        this.timer = true;
    }

    private void updateTimerText(){
        this.timerText.text = ((int)this.timeLeft).ToString();
    }

    private void timerOver(){
        this.game.loadNewPage("empty");
        this.game.nextStage();
    }

    IEnumerator scriptwriter()
    {
        foreach(Player p in this.game.players){
            this.game.playerData[p].avatar.showScore();
        }

        this.game.loadNewPage("GikoIdolSpecs");

        this.text.fadeIn();

        LeanTween.scale(this.shii, new Vector3(1,1,1), 3f).setEase(LeanTweenType.easeInBounce);

        yield return new WaitForSeconds(2f);

        this.text.changeText("First, let's pick some traits for the next Giko Idol!");

        yield return new WaitForSeconds(2f);

        this.startTimer();
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if (timer){
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

    public override void handleMessage(WsMessage message, string json){
        Debug.Log("Specs stage received message:");
        dynamic d = JObject.Parse(json);
        string receivedTrait = d.message;
        if (receivedTrait.Length < 1){
            return;
        }
        Player p = this.game.getPlayer(message.key);
        this.game.traits.Add(receivedTrait);

        this.game.playerData[p].traitsInput++;
        this.game.playerData[p].avatar.setScore(this.game.playerData[p].traitsInput);
    }

}
