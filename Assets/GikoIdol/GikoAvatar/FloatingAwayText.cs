using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingAwayText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textAsset;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void startFloating(float height, float time){
        LeanTween.moveY(this.gameObject.GetComponent<RectTransform>(), this.gameObject.GetComponent<RectTransform>().anchoredPosition.y + height, time).setEase(LeanTweenType.easeInOutSine).setOnComplete(deleteSelf);
        this.fadeOut(time);
    }

    public void fadeOut(float time){
        var fadeoutcolor = this.textAsset.faceColor;
        fadeoutcolor.a = 0;
        LeanTween.value(gameObject, updateValueExampleCallback, this.textAsset.faceColor, fadeoutcolor, time).setEase(LeanTweenType.easeInOutQuad);
    }

    void updateValueExampleCallback(Color val)
    {
        this.textAsset.faceColor = val;
    }

    public void setText(string newText){
        this.textAsset.text = newText;
    }

    public void setFontSize(int fontSize){
        this.textAsset.fontSize = fontSize;
    }
    
    public void deleteSelf(){
        GameObject.Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
