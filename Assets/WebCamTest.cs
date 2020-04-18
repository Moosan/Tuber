using UnityEngine;
using UnityEngine.UI;
public class WebCamTest : MonoBehaviour
{

    public Image _image;

    WebCamTexture webCamTexture;
    WebCamDevice[] webCamDevice;
    

    void Start()
    { 
        webCamDevice = WebCamTexture.devices;
        webCamTexture = new WebCamTexture(webCamDevice[0].name);
        _image.material.mainTexture = webCamTexture;
        webCamTexture.Play();
    }
}