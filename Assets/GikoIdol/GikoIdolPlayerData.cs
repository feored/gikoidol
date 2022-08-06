using System.Collections;
using System.Collections.Generic;

public class GikoIdolPlayerData
{
    public List<string> traits;
    public byte[] canvas;
    public string idolName;
    public GikoPlayer avatar;
    public string vote;
    public GikoIdolPlayerData(){
        this.traits = new List<string>();
    }
}
