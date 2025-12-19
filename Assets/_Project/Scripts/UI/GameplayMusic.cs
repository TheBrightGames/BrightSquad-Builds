using UnityEngine;

public class GameplayMusic : MonoBehaviour
{
    void Start()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.OnEnterGameplay();
    }

}
