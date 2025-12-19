using UnityEngine;

public class SocialLinksPessoa : MonoBehaviour
{
    [Header("URLs (deixe vazio se não usar)")]
    public string urlLinkedIn;
    public string urlItchIo;
    public string urlGitHub;
    public string urlInstagram;
    public string urlYouTube;

    void AbrirUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }

    public void AbrirLinkedIn()
    {
        AbrirUrl(urlLinkedIn);
    }

    public void AbrirItchIo()
    {
        AbrirUrl(urlItchIo);
    }

    public void AbrirGitHub()
    {
        AbrirUrl(urlGitHub);
    }

    public void AbrirInstagram()
    {
        AbrirUrl(urlInstagram);
    }

    public void AbrirYouTube()
    {
        AbrirUrl(urlYouTube);
    }
}
