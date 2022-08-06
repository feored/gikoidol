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
    private GameObject glowstickSpawn;

    [SerializeField]
    private GameObject glowstickPrefab;

    [SerializeField]
    private GameObject textFloatingPrefab;

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

    private bool jumping = false;

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


        //LeanTween.scale(this.shii, new Vector3(1,1,1), 3f).setEase(LeanTweenType.easeInBounce);

        this.game.loadNewPage("GikoIdolShowSpectator");

        this.text.fadeOut();
        this.text.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        
        // Main loop iterating through all the players currently in the game,
        // giving each player a chance to show dance/text
        foreach(Player p in this.game.players)
        {
            this.currentPlayer = p;
            this.jumping = false;
            this.highlightCorrectNameplate(p);
            showIdol(p);
            yield return new WaitForSeconds(1.0f);
            this.dispatchRightPage(p);
            int i = 0;
            foreach (string trait in this.game.playerData[p].traits){
                setTrait(trait, i);
                i++;
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(Settings.SHOW_DURATION);
            hideIdol();
            yield return new WaitForSeconds(0.5f);
        }

        this.game.loadNewPage("empty");
        this.game.nextStage();
        
    }

    public void dispatchRightPage(Player p){
        for (int i = 0; i < this.game.players.Count; i++){
            if (this.game.players[i] == p){
                this.game.loadNewPageSpecificPlayer("GikoIdolShow", p);
            } else {
                this.game.loadNewPageSpecificPlayer("GikoIdolShowSpectator", this.game.players[i]);
            }
        }
    }

    public void highlightCorrectNameplate(Player p){
        foreach(Player pp in this.game.players){
            var highlight = false;
            if (pp == p){
                highlight = true;
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

    public void startJumping(){
        this.jumping = true;
    }
    public void stopJumping(){
        this.jumping = false;
    }


    public void showIdol(Player p){
        this.jumping = false;
        this.flipped = false;
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage( this.game.playerData[p].canvas);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        this.gikoDrawing.sprite = sprite;
        this.gikoDrawing.transform.position = new Vector3(-1080, 35, 0);
        LeanTween.cancel(this.gikoDrawing.gameObject);
        LeanTween.moveX(this.gikoDrawing.GetComponent<RectTransform>(), IdolPosition.x, 1f).setEase(LeanTweenType.easeOutQuart);
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

    public void gikoTalk(string talk){ 
        GameObject floatingTextGO = Instantiate(this.textFloatingPrefab, this.gikoDrawing.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        floatingTextGO.transform.position = new Vector3(this.gikoDrawing.gameObject.transform.position.x, this.gikoDrawing.gameObject.transform.position.y + 300, this.gikoDrawing.gameObject.transform.position.z);
        floatingTextGO.transform.localScale = new Vector3(1,1,1);
        FloatingAwayText fat = floatingTextGO.GetComponent<FloatingAwayText>();
        fat.setText(talk);
        fat.setFontSize(32);
        fat.startFloating(300f, 4f);
    }

    public void gikoMove(string direction){
        LeanTween.cancel(this.gikoDrawing.gameObject);
        LeanTween.moveX(this.gikoDrawing.GetComponent<RectTransform>(), this.gikoDrawing.GetComponent<RectTransform>().anchoredPosition.x + (direction == "left" ? -50 : 50), 0.5f).setEase(LeanTweenType.easeOutQuart);
    }

    public void gikoJump(){
        LeanTween.cancel(this.gikoDrawing.gameObject);
        LeanTween.moveY(this.gikoDrawing.GetComponent<RectTransform>(), this.gikoDrawing.GetComponent<RectTransform>().anchoredPosition.y + 50, 0.25f).setLoopPingPong(1).setEase(LeanTweenType.easeOutCirc).setOnStart(startJumping).setOnComplete(stopJumping);
    }

    public void hideIdol(){
        LeanTween.cancel(this.gikoDrawing.gameObject);
        LeanTween.moveX(this.gikoDrawing.GetComponent<RectTransform>(), 1080f, 3f).setEase(LeanTweenType.easeOutQuart);
        this.removeTraits();
        this.idolName.fadeOut();
    }

    public void cheer(){
        if (Random.value > 0.66f){
            spawnGlowstick();
        }
    }

    public void spawnGlowstick(){
        GameObject glowstickGO = Instantiate(this.glowstickPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.glowstickSpawn.transform);
        glowstickGO.transform.position = new Vector3( this.glowstickSpawn.transform.position.x + Random.Range(-25f, 25f),  this.glowstickSpawn.transform.position.y + Random.Range(-25f, 25f),  this.glowstickSpawn.transform.position.z);
        glowstickGO.transform.localScale = new Vector3(1,1,1);
        Glowstick glowstick = glowstickGO.GetComponent<Glowstick>();
        glowstick.startFloating(250f+ Random.Range(-25f, 25f), 25f + Random.Range(-10f, 10f), 2f);
    }

    public override void Init(){
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

        if (currentPlayer != null && message.key == currentPlayer.key && !jumping){
            switch(command){
                case "flip":
                    gikoFlip();
                    break;
                case "move":
                    string direction = d.message.direction;
                    gikoMove(direction);
                    break;
                case "talk":
                    string content = d.message.content;
                    gikoTalk(content);
                    break;
                case "jump":
                    gikoJump();
                    break;
                case "cheer":
                    cheer();
                    break;
            }
        }

        // Handle spectator commands
    }

}
