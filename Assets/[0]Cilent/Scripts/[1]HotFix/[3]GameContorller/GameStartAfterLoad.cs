using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
public class GameStartAfterLoad : MonoBehaviour
{
    // Start is called before the first frame update
    float StartDelayTime = 3;
    void Start()
    {
#if UNITY_EDITOR
        StartDelayTime = 0.1f;
#endif
        gameObject.GetComponent<SupplementaryMetadata>().onSupplementaryMetadata += GameLoadedInit;
    }
    async void GameLoadedInit()
    {
        Debug.Log("GameHotedInit4");
        await WaitLoadAnimation(this.GetCancellationTokenOnDestroy());
        Debug.Log("加载数据完成");
        await UniTask.Delay(TimeSpan.FromSeconds(StartDelayTime));
        // var HotUI = await LoadYooAssetsTool.LoadAsset<GameObject>("LoadNewGameVesion");
        // Instantiate(HotUI, FindObjectOfType<Canvas>().transform);
        var serialportManager = await LoadYooAssetsTool.LoadAsset<GameObject>("SerialportManager");
        Instantiate(serialportManager, Vector3.zero, Quaternion.identity);
        LoadYooAssetsTool.LoadSceneAsync("DrumScene").Forget();
    }
    async UniTask WaitLoadAnimation(CancellationToken cancellationToken)
    {
        var progress = FindObjectOfType<HotFixAssetsProgress>();
        while (progress._tweenerQueue.Count > 0)
        {
            await UniTask.Delay(100, cancellationToken: cancellationToken);
        }
    }
}
