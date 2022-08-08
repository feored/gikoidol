public class SettingsSave
{
    public float TIMER_SPECS            = 1.0f;
    public float TIMER_DRAW             = 90.0f;
    public float TIMER_VOTE             = 15.0f;
    public float SHOW_DURATION          = 30.0f;
    public float VOLUME_MUSIC           = 1.0f;
    public float VOLUME_FX              = 1.0f;

    public SettingsSave(float timerSpecs, float timerDraw, float timerVote, float showDuration, float volumeMusic, float volumeFX){
        this.TIMER_SPECS = timerSpecs;
        this.TIMER_DRAW = timerDraw;
        this.TIMER_VOTE = timerVote;
        this.SHOW_DURATION = showDuration;
        this.VOLUME_MUSIC = volumeMusic;
        this.VOLUME_FX = volumeFX;
    }

    public SettingsSave(){

    }

}
