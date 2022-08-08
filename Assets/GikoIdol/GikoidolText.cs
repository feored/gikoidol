using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GikoidolText : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        this.text = this.GetComponent<TextMeshProUGUI>();
        this.instantFadeOut();
    }

    public void fadeOut(){
        var fadeoutcolor = this.text.faceColor;
        fadeoutcolor.a = 0;
        LeanTween.value(gameObject, updateValueExampleCallback, this.text.faceColor, fadeoutcolor, 0.5f).setEase(LeanTweenType.easeInCirc);
    }

    public void fadeIn(){
        var fadeinColor = this.text.faceColor;
        fadeinColor.a = 255;
        LeanTween.value(gameObject, updateValueExampleCallback, this.text.faceColor, fadeinColor, 0.5f).setEase(LeanTweenType.easeInCirc);
    }

    public void instantFadeOut(){
        this.text.faceColor = new Color(this.text.faceColor.r, this.text.faceColor.g, this.text.faceColor.b, 0);
    }

    public void instantFadeIn(){
        this.text.faceColor = new Color(this.text.faceColor.r, this.text.faceColor.g, this.text.faceColor.b, 0f);
    }

    public void changeText(string newText){
        StartCoroutine(waiter(newText));
    }

    public void setText(string newText){
        if (!this.text){
            this.text = this.GetComponent<TextMeshProUGUI>();
        }
        this.text.text = newText;
    }

    IEnumerator waiter(string newText)
    {
        this.fadeOut();
        yield return new WaitForSeconds(1);

        this.text.text = newText;

        this.fadeIn();
        yield return new WaitForSeconds(1);
    }

    void updateValueExampleCallback(Color val)
    {
        this.text.faceColor = val;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
