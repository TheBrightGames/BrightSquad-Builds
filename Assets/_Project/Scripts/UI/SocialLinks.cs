using UnityEngine;

public class SocialLinks : MonoBehaviour
{
    [Header("URLs das redes")]
    public string urlLinkedIn = "https://www.linkedin.com/in/seu-perfil";
    public string urlItchIo = "https://seuusuario.itch.io/bright-squad";
    public string urlGitHub = "https://github.com/SeuUsuario/SeuRepositorio";
    public string urlInstagram = "https://www.instagram.com/seuusuario";
    public string urlYouTube = "https://www.youtube.com/@seucanal";

    public void AbrirLinkedIn()
    {
        Application.OpenURL(urlLinkedIn);
    }

    public void AbrirItchIo()
    {
        Application.OpenURL(urlItchIo);
    }

    public void AbrirGitHub()
    {
        Application.OpenURL(urlGitHub);
    }

    public void AbrirInstagram()
    {
        Application.OpenURL(urlInstagram);
    }

    public void AbrirYouTube()
    {
        Application.OpenURL(urlYouTube);
    }
}
