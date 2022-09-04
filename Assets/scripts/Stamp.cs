using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamp : MonoBehaviour
{
    private Image uiImage;
    private Material material;
    private AppManager manager;
    private float maxWidth;
    private float previewWidth;
    private ListManager listMgr;
    [SerializeField]
    private Button btn;
    private float padding;

    private void Awake()
    {
        btn.onClick.AddListener(Delete);

        listMgr = transform.GetComponentInParent<ListManager>();

        uiImage = GetComponent<Image>();
        material = uiImage.material;
        manager = GameObject.FindWithTag("Manager").GetComponent<AppManager>();
        MakeFocus();
        //gameObject.GetComponent<Button>().onClick.AddListener(MakeFocus);

        // get width of the scroll view object to set max width of image.
        maxWidth = listMgr.screenWidth;
        // preview width of the image is the width of its parent
        //previewWidth = transform.parent.GetComponent<RectTransform>().sizeDelta.x;

        //padding = 0.05f;
        //SetPreviewScale();
    }

    private void SetPreviewScale()
    {
        float sf = previewWidth / maxWidth;
        sf = sf - padding;
        transform.GetComponent<RectTransform>().localScale = new Vector3(sf, sf ,sf);
    }


    private void Delete()
    {
        listMgr.RemoveStamp(transform.parent.gameObject);
        Debug.Log("Deleting stamp");
        Destroy(transform.parent.gameObject);
    }

    private void MakeFocus()
    {
        Debug.Log("this object is focused");
        manager.activeStamp = this;
    }

    public void SetShader(Shader shader)
    {
        material.shader = shader;
        uiImage.enabled = false;
        uiImage.enabled = true;
    }

    
    // TODO: autorotate texture to show it in the largest orientation
    public void SetTexture(Texture2D texture)
    {
        // set display size of UI image to fit onscreen w/o distortion
        Vector2 displaySize = new Vector2(texture.width, texture.height);

        if (texture.width > maxWidth)
        {
            displaySize = new Vector2(maxWidth, (float)(texture.height * (maxWidth / texture.width)));
            Debug.Log(displaySize.x + " : " + displaySize.y);
        }
        uiImage.GetComponent<RectTransform>().sizeDelta = displaySize;

        material.SetTexture("_MainTex", texture);
        //toggle image to refresh texture
        uiImage.enabled = false;
        uiImage.enabled = true;
    }

}
