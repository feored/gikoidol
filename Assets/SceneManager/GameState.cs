using System.Collections.Generic;

public static class GameState {

    public static GameSceneCollection MAIN_SCREEN = new GameSceneCollection("Title Screen", new List<string>(){
        GameSceneName.MAIN_SCREEN
    });

    public static GameSceneCollection BUILDAGIKO_SCREEN = new GameSceneCollection("Build A Giko Game", new List<string>(){
        GameSceneName.BUILDAGIKO_SCREEN
    }); 

}

public class GameSceneCollection {
    public string name;
    public List<string> scenes;
    public bool active;
    public GameSceneCollection(string name, List<string> scenes){
        this.name = name;
        this.scenes = scenes;
        this.active = false;
        }
    }

public static class GameSceneName {
    public static string MAIN_SCREEN = "MainScreen";
    public static string BUILDAGIKO_SCREEN = "BuildAGiko";
}
