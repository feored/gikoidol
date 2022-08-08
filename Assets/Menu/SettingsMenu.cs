using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class SettingsMenu : MonoBehaviour
{

    private FMOD.Studio.Bus musicBus;
    private FMOD.Studio.Bus effectsBus;

    [SerializeField]
    private TextMeshProUGUI specsTimerValue;

    [SerializeField]
    private Slider specsTimerSlider;

    [SerializeField]
    private TextMeshProUGUI drawTimerValue;

    [SerializeField]
    private Slider drawTimerSlider;

    [SerializeField]
    private TextMeshProUGUI voteTimerValue;

    [SerializeField]
    private Slider voteTimerSlider;

    [SerializeField]
    private TextMeshProUGUI musicVolumeValue;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI effectsVolumeValue;

    [SerializeField]
    private Slider effectsVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI showDurationValue;

    [SerializeField]
    private Slider showDurationSlider;



    public static void loadMainScreen(){
        SceneManager.LoadScene("MainScreen");
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get music + fx bus

        this.musicBus    = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        this.effectsBus  = FMODUnity.RuntimeManager.GetBus("bus:/Effects");

        // Set all sliders & values to initial value
        specsTimerValue.text = ((int)Settings.TIMER_SPECS).ToString();
        specsTimerSlider.value = Settings.TIMER_SPECS;

        drawTimerValue.text = ((int)Settings.TIMER_DRAW).ToString();
        drawTimerSlider.value = Settings.TIMER_DRAW;

        voteTimerValue.text = ((int)Settings.TIMER_VOTE).ToString();
        voteTimerSlider.value = Settings.TIMER_VOTE;

        showDurationValue.text = ((int)Settings.SHOW_DURATION).ToString();
        showDurationSlider.value = Settings.SHOW_DURATION;

        int musicVolume = (int)(Settings.VOLUME_MUSIC*100);
        musicVolumeValue.text = musicVolume.ToString();
        musicVolumeSlider.value = musicVolume;

        int effectsVolume = (int)(Settings.VOLUME_FX*100);
        effectsVolumeValue.text = effectsVolume.ToString();
        effectsVolumeSlider.value = effectsVolume;
        
    }

    public void specsTimerChanged(){
        Settings.TIMER_SPECS = specsTimerSlider.value;
        specsTimerValue.text = specsTimerSlider.value.ToString();
        saveSettings();
    }

    public void drawTimerChanged(){
        Settings.TIMER_DRAW = drawTimerSlider.value;
        drawTimerValue.text = drawTimerSlider.value.ToString();
        saveSettings();
    }

    public void voteTimerChanged(){
        Settings.TIMER_VOTE = voteTimerSlider.value;
        voteTimerValue.text = voteTimerSlider.value.ToString();
        saveSettings();
    }

    public void showDurationChanged(){
        Settings.SHOW_DURATION = showDurationSlider.value;
        showDurationValue.text = showDurationSlider.value.ToString();
        saveSettings();
    }

    public void musicVolumeChanged(){
        float newVolume = ((float)(musicVolumeSlider.value))/100f;
        Settings.VOLUME_MUSIC = newVolume;
        musicVolumeValue.text = musicVolumeSlider.value.ToString();

        this.musicBus.setVolume(newVolume);
        saveSettings();
    }

    public void effectsVolumeChanged(){
        float newVolume = ((float)(effectsVolumeSlider.value))/100f;
        Settings.VOLUME_FX = newVolume;
        effectsVolumeValue.text = effectsVolumeSlider.value.ToString();

        this.effectsBus.setVolume(newVolume);
        saveSettings();
    }

    public static void applySettings(){
        FMOD.Studio.Bus musicBus    = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        FMOD.Studio.Bus effectsBus  = FMODUnity.RuntimeManager.GetBus("bus:/Effects");
        effectsBus.setVolume(Settings.VOLUME_FX);
        musicBus.setVolume(Settings.VOLUME_MUSIC);
    }

    public static string getPath(){
        return Application.persistentDataPath + Path.AltDirectorySeparatorChar + "settings.json";
    }

    public void saveSettings(){
        SettingsSave settings = new SettingsSave(Settings.TIMER_SPECS, Settings.TIMER_DRAW, Settings.TIMER_VOTE, Settings.SHOW_DURATION, Settings.VOLUME_MUSIC, Settings.VOLUME_FX);
        string serializedData = JsonUtility.ToJson(settings);
        using StreamWriter writer = new StreamWriter(getPath());
        writer.Write(serializedData);
    }

    public void sliderReleased(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/btn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
