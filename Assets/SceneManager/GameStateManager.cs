using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    public static GameStateManager instance;
    private GameSceneCollection active_scene_collection;
    public void Awake()
    {
        instance = this;
        loadSceneCollection(GameState.MAIN_SCREEN);
    }

    public void forceLoadSceneCollection(GameSceneCollection new_collection){
        foreach (string scene_name in this.active_scene_collection.scenes)
            {
                if (scene_name != "PersistentScene"){
                    SceneManager.UnloadSceneAsync(scene_name);
                }
            }

        foreach (string scene_name in new_collection.scenes)
            {
                SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);
            }
        this.active_scene_collection = new_collection;

    }

    public void loadSceneCollection(GameSceneCollection new_collection)
    {
        /* Switch to a new scene collection aka gameplay state, like title screen, single player etc */
        List<string> scenes_already_loaded = new List<string>();
        if (this.active_scene_collection != null)
        {
            if (this.active_scene_collection.name == new_collection.name)
            {
                /* If the scene collection we want to switch is already loaded, we're good to go */
                return;
            }
            /* Otherwise, first unload all scenes in current scene collection that
            are not in the collection we're switching to */
            foreach (string scene_name in this.active_scene_collection.scenes)
            {
                if (scene_name != "PersistentScene"){
                    if (!check_scene_in_collection(scene_name, new_collection))
                    {
                        SceneManager.UnloadSceneAsync(scene_name);
                    } else {
                        scenes_already_loaded.Add(scene_name);
                    }
                }
            }
        }

        /* Then load the ones that aren't already present */
        foreach (string scene_name in new_collection.scenes)
        {
            if (!scenes_already_loaded.Contains(scene_name))
            {
                SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);
            }
        }

        this.active_scene_collection = new_collection;
    }

    private static bool check_scene_in_collection(string scene_name, GameSceneCollection collection)
    {
        foreach (string collection_scene_name in collection.scenes)
        {
            if (scene_name == collection_scene_name)
            {
                return true;
            }
        }
        return false;
    }
}
