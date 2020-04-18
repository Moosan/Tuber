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
        //webCamTexture.requestedHeight = 960;
        //webCamTexture.requestedWidth = 1280;
        _renderer.material.mainTexture = webCamTexture;
        webCamTexture.Play();
    }
}