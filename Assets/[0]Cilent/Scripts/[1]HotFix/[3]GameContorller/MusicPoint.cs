using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MusicPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Color[] colors;
    SpriteRenderer image;
    [SerializeField] float moveDuration;

    DrumSceneManager manager;
    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        manager = DrumSceneManager.Instance;
    }

    public void Init(int index, Transform moveTargetPoint)
    {
        manager.RegisterMusicPoint(gameObject);
        image.color = colors[index];
        transform.DOMove(moveTargetPoint.position, moveDuration)
        .SetEase(Ease.Linear).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.3f, () =>
            {
                manager.UnRegisterMusicPoint(gameObject);
                manager = null;
            });
            transform.DOMoveX(transform.position.x - 6, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(gameObject, 1f);
            });
        });
    }
    public bool AnserHandel(int index)
    {
        bool isRight = colors[index - 1] == image.color;
        if (isRight)
        {
            transform.DOKill();
            image.DOColor(Color.white, 0.5f).SetEase(Ease.InBack);
            transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
        else
        {
            image.DOColor(Color.black, 0.1f).SetEase(Ease.InBack);
            manager.UnRegisterMusicPoint(gameObject);

            // transform.DOScale(0f, 0.5f).SetEase(Ease.InBack);
        }
        return isRight;
    }
    public void OnDestroy()
    {
        if (manager != null)
        {
            manager.UnRegisterMusicPoint(gameObject);
        }
    }
}
