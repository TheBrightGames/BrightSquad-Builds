using UnityEngine;
using UnityEngine.UI;   // <‑ isso é obrigatório para usar Image

public class LoadingRandomCharacter : MonoBehaviour
{
    public Sprite[] randomSprites;
    public Image imgRandom;

    public void ChangeRandomImage()
    {
        if (randomSprites == null || randomSprites.Length == 0) return;
        int index = Random.Range(0, randomSprites.Length);
        imgRandom.sprite = randomSprites[index];
    }
}
