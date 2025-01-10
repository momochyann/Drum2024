using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

public class AOTGenericReferences1 : MonoBehaviour
{
    void AOTGenericsInitialization()
    {
        // 完全匹配你的使用场景
        var enumerable = UniTaskAsyncEnumerable.Create<float>(async (writer, token) =>
        {
            await writer.YieldAsync(1);
        });

        enumerable.ForEachAwaitAsync(_ =>
        {
            return UniTask.CompletedTask;
        }, default(CancellationToken));

        // 其他相关的泛型实例化
        new UniTask<float>();
        new UniTask<float>().GetAwaiter();
        
        // 取消令牌相关
        CancellationTokenSource.CreateLinkedTokenSource(default, default);
    }
} 