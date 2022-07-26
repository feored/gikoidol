using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    [SerializeField]
    protected GikoIdol game;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void Init();

    public abstract void show();

    public abstract void hide();

    public abstract void handleMessage(WsMessage message, string json);

}
