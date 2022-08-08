using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interactiveButton : MonoBehaviour
{
    public void enlarge(){
        LeanTween.cancel(this.gameObject);
        LeanTween.scale(this.gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.2f).setEase(LeanTweenType.easeOutQuart);
    }

    public void normalSize(){
        LeanTween.cancel(this.gameObject);
        LeanTween.scale(this.gameObject, new Vector3(1f, 1f, 1f), 0.1f).setEase(LeanTweenType.easeOutQuart);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => { 
            FMODUnity.RuntimeManager.PlayOneShot("event:/btn");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
