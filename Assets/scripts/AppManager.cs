using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class AppManager : MonoBehaviour
{

    public Button imgBtn;
    public Button startBtn;
    public Slider timeSetter;

    public Image background;
    public Canvas panel;
    public Shader darkroomPreview;
    public Shader lightExposure;

    public GameObject stampPrefab;
    public Stamp activeStamp;

    public GameObject listMgrObj;
    private ListManager listMgr;


    //dummy image for editor testing
    public Texture2D editorImg;

    // Start is called before the first frame update
    void Start()
    {
        imgBtn.onClick.AddListener(ShowMediaPicker);
        startBtn.onClick.AddListener(RunStamper);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        listMgr = listMgrObj.GetComponent<ListManager>();
    }


    public void RunStamper()
    {
       if (activeStamp == null)
        {
            return;
        }
       Debug.Log("started"); 
       StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        // starts in DR preview
        panel.enabled = false;
        yield return new WaitForSeconds(2);

        activeStamp.SetShader(lightExposure);
        background.color = Color.black;
        yield return new WaitForSeconds(timeSetter.value);

        activeStamp.SetShader(darkroomPreview);
        background.color = Color.red;                
        panel.enabled = true;
    }


    public void ShowMediaPicker()
    {
        Debug.Log("showing media picker");

        if (Application.isEditor)
        {
            // Do something else, since the plugin does not work inside the editor
            Debug.Log("hello editor");
            //RenderImage();
            listMgr.InstantiateStamp(editorImg);
        }
        else
        {   
            NativeGallery.GetImageFromGallery((path) =>
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                } else
                {
                    //RenderImage();
                    listMgr.InstantiateStamp(texture);
                }
                
            });
        }        
    }
    

}
