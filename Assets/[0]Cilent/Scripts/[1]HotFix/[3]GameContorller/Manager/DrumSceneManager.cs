using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using QFramework;
using System;
using System.Linq;
public class DrumSceneManager : MonoSingleton<DrumSceneManager>
{
    // Start is called before the first frame update
    CreatPoint createPoint;
    [SerializeField] AudioClip playMusic;
    [SerializeField] AudioClip[] audioEffects;
    [Header("生成音乐点")]
    [SerializeField] List<float> spawnTimes;
    public float musicPointMoveStartWaitTime;

    [Header("检测相关")]
    [SerializeField] Transform musicPointCheckPoint;
    [SerializeField] float musicPointCheckRadius;
    [SerializeField] float musicPointDetectableRadius;
    GameObject currentMusicPoint;
    Queue<GameObject> musicPointQueue = new Queue<GameObject>();
    void Start()
    {
        createPoint = FindObjectOfType<CreatPoint>();
        StartGame().Forget();
    }
    public void RegisterMusicPoint(GameObject musicPoint)
    {
        musicPointQueue.Enqueue(musicPoint);
    }
    public void UnRegisterMusicPoint(GameObject musicPoint)
    {
        // musicPointQueue.Dequeue();
        if (currentMusicPoint == musicPoint)
        {
            currentMusicPoint = null;
        }

        if (musicPointQueue.Contains(musicPoint))
        {
            if (musicPointQueue.Peek() == musicPoint)
            {
                musicPointQueue.Dequeue();
                return;
            }
            var tempList = musicPointQueue.ToList();
            tempList.Remove(musicPoint);
            musicPointQueue = new Queue<GameObject>(tempList);
        }

    }
    // Update is called once per frame
    async UniTaskVoid StartGame()
    {
        Debug.Log("StartGame");
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: token);
        createPoint.StartSpawnSequence(spawnTimes);
        await UniTask.Delay(TimeSpan.FromSeconds(musicPointMoveStartWaitTime), cancellationToken: token);
        AudioSource.PlayClipAtPoint(playMusic, Vector3.zero);
        Debug.Log("播放音乐");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckMusicPoint(0);
        }
    }
    public void CheckMusicPoint(int index)
    {
        Debug.Log("CheckMusicPoint");
        if (currentMusicPoint == null)
        {
            if (musicPointQueue.Count == 0)
                return;
            currentMusicPoint = musicPointQueue.Dequeue();
        }
        if (Vector3.Distance(currentMusicPoint.transform.position, musicPointCheckPoint.position) > musicPointDetectableRadius)
        {
            Debug.Log("未到达检测点");
            return;
        }
        CheckMusicPointHandel(Vector3.Distance(currentMusicPoint.transform.position, musicPointCheckPoint.position) < musicPointCheckRadius, index);
        currentMusicPoint = null;
    }
    void CheckMusicPointHandel(bool isArrive, int anserIndex)
    {
        if (currentMusicPoint == null)
            return;
        // Debug.Log(isArrive ? "检测到音乐点" : "未检测到音乐点");
        bool isRight = currentMusicPoint.GetComponent<MusicPoint>().AnserHandel(anserIndex);
        AudioSource.PlayClipAtPoint(audioEffects[isRight ? 0 : 1], Vector3.zero);
    }
}

