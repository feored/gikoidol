using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GikoPlayer : MonoBehaviour
{
    private Color32 HIGHLIGHTED_BACKGROUND = new Color32(111,21,74,143);
    private Color32 REGULAR_BACKGROUND = new Color32(0,0,0,143);

    [SerializeField]
    private TextMeshProUGUI name;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private GameObject scoreBackground;
    
    [SerializeField]
    private GameObject checkmark;

    [SerializeField]
    private Image avatar;

    [SerializeField]
    public List<Sprite> sprites;

    public Sprite currentSprite;

    public void setName(string newName)
    {
        this.name.text = newName;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.hideScore();
    }

    public void showScore(){
        this.scoreBackground.SetActive(true);
    }

    public void hideScore(){
        this.scoreBackground.SetActive(false);
    }

    public void setScore(int newScore){
        this.scoreText.text = newScore.ToString();
    }

    public void showCheckmark(){
        this.checkmark.SetActive(true);
    }

    public void hideCheckmark(){
        this.checkmark.SetActive(false);
    }

    public void highlight(bool highlight){
        this.GetComponent<Image>().color = highlight ? HIGHLIGHTED_BACKGROUND : REGULAR_BACKGROUND;
    }

    public void setImage(string newImage){
        for (int i = 0; i < this.sprites.Count; i++){
            if (this.sprites[i].name == newImage){
                this.currentSprite = this.sprites[i];
                this.avatar.sprite = this.currentSprite;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
