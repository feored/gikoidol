using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class GikoIdolStage_Win : Stage
{
    [SerializeField]
    private GikoidolText text;

    [SerializeField]
    private Image gikoDrawing;

    [SerializeField]
    private List<GikoidolText> traitsText;

    [SerializeField]
    private GikoidolText idolName;

    private float timeLeft;

    private Vector3 IdolPosition = new Vector3(-15, 35, 0);

    private Player winner;

    // Start is called before the first frame update
    public override void Start()
    {
    }

    public Player getWinner(){
        Player winner = this.game.players[0];
        
        // Get tally of scores
        Dictionary<string, int> scores = new Dictionary<string, int>();
        
        // Initialize dict
        foreach (Player p in this.game.players){
            scores[p.name] = 0;
        }

        // Sum up scores
        foreach (Player p in this.game.players){
            if (null != this.game.playerData[p].vote){
                string voted = this.game.playerData[p].vote;
                if(scores.ContainsKey(voted)){
                    scores[voted]++;
                }
            }
        }

        foreach (Player p in this.game.players){
            if (scores.ContainsKey(p.name) && scores[p.name] > scores[winner.name]){
                winner = p;
            }
        }

        return winner;
    }

    IEnumerator scriptwriter()
    {

        this.game.loadNewPage("win");
        this.text.setText("Congratulations producer " +  winner.name + "!");
        

        this.text.fadeIn();

        yield return new WaitForSeconds(1.0f);

        showWinnerIdol(this.winner);

        yield return new WaitForSeconds(1.0f);


        // Game over, delete room
        dynamic voteMessage = new JObject();
        voteMessage.key = this.game.key;
        voteMessage.room = this.game.room;
        voteMessage.type = WsMessage.DELETEROOM;
        voteMessage.player = "";
        voteMessage.message = "";
        this.game.connection.wsSendMessage(voteMessage.ToString());
        
    }

    public void showWinnerIdol(Player p){
        
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage( this.game.playerData[p].canvas);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        this.gikoDrawing.sprite = sprite;
        this.gikoDrawing.transform.position = new Vector3(-1080, 35, 0);
        LeanTween.moveX(this.gikoDrawing.GetComponent<RectTransform>(), IdolPosition.x, 2f).setEase(LeanTweenType.easeOutQuart);
        this.idolName.setText(this.game.playerData[p].idolName);
        this.idolName.fadeIn();
        int i = 0;
        foreach (string trait in this.game.playerData[p].traits){
            setTrait(trait, i);
            i++;
        }
    }


    public void setTrait(string trait, int index){
        this.traitsText[index].setText(trait);
        this.traitsText[index].fadeIn();
    }

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
        foreach(Player p in this.game.players){
            this.game.playerData[p].avatar.highlight(false);
        }
        this.winner = this.getWinner();
        StartCoroutine(scriptwriter());
    }

    public override void handleMessage(WsMessage message, string json){
        //Debug.Log("Win stage received message:");
        //Player p = this.game.getPlayer(message.key);
        //dynamic d = JObject.Parse(json);
    }

}
