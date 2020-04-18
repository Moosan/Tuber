using UnityEngine;
using UnityEngine.UI;
public class WebCamTest : MonoBehaviour
{

    public MeshRenderer _renderer;

    WebCamTexture webCamTexture;
    WebCamDevice[] webCamDevice;
    

    void Start()
    { 
        webCamDevice = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(webCamDevice[0].name);
        _renderer.material.mainTexture = webCamTexture;
        webCamTexture.Play();
    }
}