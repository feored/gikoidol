using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glowstick : MonoBehaviour
{   
    [SerializeField]
    private Image glowstick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startFloating(float height, float width, float time){
        LeanTween.moveX(this.gameObject.GetComponent<RectTransform>(), this.gameObject.GetComponent<RectTransform>().anchoredPosition.x + width, 0.5f).setLoopPingPong().setEase(LeanTweenType.easeInOutSine).setOnComplete(deleteSelf);
        LeanTween.moveY(this.gameObject.GetComponent<RectTransform>(), this.gameObject.GetComponent<RectTransform>().anchoredPosition.y + height, time).setEase(LeanTweenType.easeInOutSine).setOnComplete(deleteSelf);
    }

    public void deleteSelf(){
        GameObject.Destroy(this.gameObject);
    }
}
