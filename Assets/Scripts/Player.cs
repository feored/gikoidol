using System.Collections;
using System.Collections.Generic;

public class Player
{
    public bool vip;
    public string name;
    public string key;
    
    public Player(){
        name = "";
        key = "";
        vip = false;
    }

    public Player(string name, string key){
        this.name = name;
        this.key = key;
        vip = false;
    }
}
