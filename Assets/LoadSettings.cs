using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class LoadSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.loadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadSettings(){
        SettingsSave settingsSave;
        string path = SettingsMenu.getPath();

        if (File.Exists(path)){
            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            settingsSave = JsonUtility.FromJson<SettingsSave>(json);
        } else {
            settingsSave = new SettingsSave();
        }
        setSettings(settingsSave);
        SettingsMenu.applySettings();
    }

    public void setSettings(SettingsSave settingsSave){
        Settings.SHOW_DURATION = settingsSave.SHOW_DURATION;
        Settings.TIMER_DRAW = settingsSave.TIMER_DRAW;
        Settings.TIMER_SPECS = settingsSave.TIMER_SPECS;
        Settings.TIMER_VOTE = settingsSave.TIMER_VOTE;
        Settings.VOLUME_FX = settingsSave.VOLUME_FX;
        Settings.VOLUME_MUSIC = settingsSave.VOLUME_MUSIC;
    }
}
