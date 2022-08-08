using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;



public  class MainScreenMenu : MonoBehaviour
{
    // Start is called before the first frame update


    public static void loadGame(){
        SceneManager.LoadScene("GikoIdol");
    }

    public static void loadSettings(){
        SceneManager.LoadScene("Settings");
    }

    public static void loadMainScreen(){
        SceneManager.LoadScene("MainScreen");
    }

}
