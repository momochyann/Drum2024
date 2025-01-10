using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
public class CreatPoint : MonoBehaviour
{

    private CancellationTokenSource _cts;
    private IUniTaskAsyncEnumerable<float> _spawnSequence;
    private float previousTime = 0;
    GameObject musicPointPfb;
    Transform moveTargetPoint;
    private async void Start()
    {
        musicPointPfb = await LoadYooAssetsTool.LoadAsset<GameObject>("MusicPoint");
        moveTargetPoint = GameObject.Find("MoveTargetPoint").transform;
    }

    public void StartSpawnSequence(List<float> spawnTimes)
    {
        float startWaitTime = Time.time;
        previousTime = Time.time;
        StopSpawnSequence();
        var ct = this.GetCancellationTokenOnDestroy();
        _cts = new CancellationTokenSource();
        var cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, this.GetCancellationTokenOnDestroy());
        _spawnSequence = UniTaskAsyncEnumerable.Create<float>(async (writer, token) =>
        {
            int index = 0;
            foreach (var targetTime in spawnTimes)
            {
                float delayTime = targetTime + startWaitTime - previousTime;
                await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: token);
                await writer.YieldAsync(index);
                previousTime = Time.time;
                index++;
            }
        });
        _spawnSequence
          .ForEachAwaitAsync(async _ =>
          {
              await SpawnMusicPoint(ct);
          }, ct);

    }

    public void StopSpawnSequence()
    {
        _cts?.Cancel();
        _cts = null;
    }

    private async UniTask SpawnMusicPoint(CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();  // 立即检查取消状态
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            if (this == null || !gameObject.activeInHierarchy) return;  // 检查组件是否还有效
            var musicPoint = Instantiate(musicPointPfb, transform.position, Quaternion.identity);
            musicPoint.GetComponent<MusicPoint>().Init(UnityEngine.Random.Range(0, 4), moveTargetPoint);
            Debug.Log("创建音乐点");
        }
        catch (OperationCanceledException)
        {
            // 优雅处理取消操作
        }
    }
    private void Update()
    {

    }
    private void OnDestroy()
    {
        // StopSpawnSequence();
    }
}
