using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class GikoIdolStage_Show : Stage
{
    [SerializeField]
    private GikoidolText text;

    [SerializeField]
    private GameObject timerBackground;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Image gikoDrawing;

    [SerializeField]
    private List<GikoidolText> traitsText;

    [SerializeField]
    private GikoidolText idolName;

    private float timeLeft;
    private bool timer;

    private bool flipped = false;

    private Vector3 IdolPosition = new Vector3(-15, 35, 0);

    private Player currentPlayer;

    // Start is called before the first frame update
    public override void Start()
    {
    }

    private void startTimer(){
        this.timeLeft = 20.0f;
        this.timerBackground.SetActive(true);
        this.timerBackground.transform.localScale = new Vector3(0f, 0f, 0f);
        LeanTween.scale(this.timerBackground, new Vector3(1,1,1), 2f).setEase(LeanTweenType.easeOutQuart);
        this.timer = true;
    }

    private void updateTimerText(){
        this.timerText.text = ((int)this.timeLeft).ToString();
    }

    private void timerOver(){
        this.game.nextStage();
    }

    IEnumerator scriptwriter()
    {
        this.text.setText("Time for the show!");

        this.text.fadeIn();

        foreach(Player p in this.game.players)
        {
            this.game.playerData[p].avatar.hideScore();
        }


        //LeanTween.scale(this.shii, new Vector3(1,1,1), 3f).setEase(LeanTweenType.easeInBounce);

        this.game.loadNewPage("GikoIdolShowSpectator");

        this.text.fadeOut();
        this.text.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        
        // Main loop iterating through all the players currently in the game,
        // giving each player a chance to show dance/text
        foreach(Player p in this.game.players)
        {
            this.highlightCorrectNameplate(p);
            showIdol(p);
            yield return new WaitForSeconds(1.0f);
            int i = 0;
            foreach (string trait in this.game.playerData[p].traits){
                setTrait(trait, i);
                i++;
                yield return new WaitForSeconds(0.5f);
            }
            this.currentPlayer = p;
            yield return new WaitForSeconds(Constants.SHOW_DURATION);
            hideIdol();
            yield return new WaitForSeconds(1.5f);
        }

        this.game.loadNewPage("empty");
        this.game.nextStage();
        
    }

    public void highlightCorrectNameplate(Player p){
        foreach(Player pp in this.game.players){
            var highlight = false;
            if (pp == p){
                highlight = true;
                this.game.loadNewPage("GikoIdolShow");
            } else {
                this.game.loadNewPage("GikoIdolShowSpectator");
            }
            this.game.playerData[pp].avatar.highlight(highlight);
        }
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


    public void showIdol(Player p){
        this.flipped = false;
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage( this.game.playerData[p].canvas);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        this.gikoDrawing.sprite = sprite;
        this.gikoDrawing.transform.position = new Vector3(-1080, 35, 0);
        LeanTween.moveX(this.gikoDrawing.GetComponent<RectTransform>(), IdolPosition.x, 2f).setEase(LeanTweenType.easeOutQuart);
        this.idolName.setText(this.game.playerData[p].idolName);
        this.idolName.fadeIn();
    }

    public void setTrait(string trait, int index){
        this.traitsText[index].setText(trait);
        this.traitsText[index].fadeIn();
    }

    public void removeTraits(){
        for (int i = 0; i < this.traitsText.Count; i++){
            this.traitsText[i].setText("");
            this.traitsText[i].fadeOut();
        }
    }

    public void gikoFlip(){
        LeanTween.cancel(this.gikoDrawing.gameObject);
        LeanTween.rotateY(this.gikoDrawing.gameObject, flipped ? 0 : 180, 1f).setEase(LeanTweenType.easeOutQuart);
        flipped = !flipped;
    }

    public void gikoMove(string direction){
        LeanTween.cancel(this.gikoDrawing.gameObject);
        float curRotation = this.gikoDrawing.GetComponent<RectTransform>().eulerAngles.y;
        LeanTween.moveX(this.gikoDrawing.GetComponent<RectTransform>(), this.gikoDrawing.GetComponent<RectTransform>().anchoredPosition.x + (direction == "left" ? -50 : 50), 0.5f).setEase(LeanTweenType.easeOutQuart);
    }

    public void hideIdol(){
        LeanTween.moveX(this.gikoDrawing.GetComponent<RectTransform>(), 1080f, 3f).setEase(LeanTweenType.easeOutQuart);
        this.removeTraits();
        this.idolName.fadeOut();
    }

    public override void Init(){
        this.currentPlayer = null;
        this.timer = false;
        foreach(Player p in this.game.players){
            this.game.playerData[p].avatar.highlight(false);
        }
    }

    public override void hide(){
        this.gameObject.SetActive(false);
    }

    public override void show(){
        this.gameObject.SetActive(true);
        StartCoroutine(scriptwriter());
    }

    public override void handleMessage(WsMessage message, string json){
        Debug.Log("Show stage received message:");
        dynamic d = JObject.Parse(json);
        string command = d.message.command;

        // Handle main player commands
        
        // Check that commands are coming from main player currently

        if (currentPlayer != null && message.key == currentPlayer.key){
            switch(command){
                case "flip":
                    gikoFlip();
                    break;
                case "move":
                    string direction = d.message.direction;
                    gikoMove(direction);
                    break;
            }
        }

        // Handle spectator commands
    }

}
