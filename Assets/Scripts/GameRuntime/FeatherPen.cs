using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherPen : MonoBehaviour
{
    PlayerRuntime _collisionPlayer;
    private void Awake()
    {
        GameRuntimeManager.Instance.nowaGameMode.runtimeFeatherPenCount++;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerRuntime>(out _collisionPlayer))
        {
            _collisionPlayer.PlayerData.runtime_rewrite_ink += 100;
            if (_collisionPlayer.PlayerData.runtime_rewrite_ink > _collisionPlayer.PlayerData.runtime_rewrite_ink_Max) _collisionPlayer.PlayerData.runtime_rewrite_ink = _collisionPlayer.PlayerData.runtime_rewrite_ink_Max;
            GameRuntimeManager.Instance.nowaGameMode.runtimeFeatherPenCount--;
            GameRuntimeManager.Instance.GameRuntimeRemove(this.gameObject);
        }
    }
}
