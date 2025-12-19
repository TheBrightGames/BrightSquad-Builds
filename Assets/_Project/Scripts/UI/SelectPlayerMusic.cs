using UnityEngine;

public class SelectPlayerMusic : MonoBehaviour
{
    void Start()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.OnEnterSelectPlayer();
    }
}
