using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameStartSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameLoad changSceneButton;
    void Start()
    {
        StartInit().Forget();
    }
    async UniTaskVoid StartInit()
    {
        var button = await LoadYooAssetsTool.LoadAsset<GameObject>("ChangeSceneButton", true);
        var button2SceneChange = Instantiate(button, FindObjectOfType<Canvas>().transform).GetComponent<Button2SceneChange>();
        button2SceneChange.sceneName = "DrumScene";
        button2SceneChange.isLocalLoad = false;
    }

}
