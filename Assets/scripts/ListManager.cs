using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Class for managing a list of stamps onscreen.
 * - Stores the list of objects
 * - manages the GUI for adding to, removing from, and navigating the list.
 * - determines appropriate display sizing for the list and all components.
 */
public class ListManager : MonoBehaviour
{
    [SerializeField]
    private GameObject stampPrefab;
    [SerializeField]
    private GameObject addBtnPrefab;

    public List<GameObject> stamps { get; private set; }
    public float screenWidth { get; private set; }
    private RectTransform mgrCont;
    private RectTransform flexCont;
    private float currentWidthRequired;

    [SerializeField]
    private float previewSizeMultiplier;
    private float previewWidth;

    private AppManager manager;
    void Start()
    {
        //currentWidthRequired = screenWidth;
        mgrCont = transform.GetComponent<RectTransform>();
        flexCont = transform.GetChild(0).GetComponent<RectTransform>();
        stamps = new List<GameObject>();
        screenWidth = transform.parent.transform.GetComponent<RectTransform>().sizeDelta.x;
        manager = GameObject.FindWithTag("Manager").GetComponent<AppManager>();

        previewSizeMultiplier = 0.4f;
        //previewWidth = SetPreviewWidth();
    }

    /*
     * Instantiates new stamp, adds to list, sets texture.
     */
    public void InstantiateStamp(Texture2D texture)
    {
        if (manager.activeStamp != null)
        {
            Destroy(manager.activeStamp.transform.parent.gameObject);
        }
        GameObject newStampObject = Instantiate(stampPrefab, flexCont.transform);

        // AddStamp(newStampObject);

        Stamp newStamp = newStampObject.transform.GetComponentInChildren<Stamp>();
        newStamp.SetTexture(texture);
        //newStamp.SetShader(darkroomPreview);
    }

    private float SetPreviewWidth()
    {
        float width = screenWidth * previewSizeMultiplier;
        stampPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 500);
        return width;
    }

    private float GetContWidth()
    {
        return stamps.Count * previewWidth;
    }


    /*
     * Determines the appropriate width of the container based on the number of stamps in the list.
     */
    private void SetContWidth()
    {
        mgrCont.sizeDelta = new Vector2(GetContWidth(), mgrCont.sizeDelta.y);
    }

    /*
     * Arranges the stamps in line for viewing.
     */
    private void ArrangeStamps()
    {
        float totalXPadding = 0;
        for (int i = 0; i < stamps.Count; i++)
        {
            Vector2 pos = stamps[i].GetComponent<RectTransform>().localPosition;
            
            float x = stamps[i].GetComponent<RectTransform>().sizeDelta.x;
            totalXPadding -= x;
            stamps[i].GetComponent<RectTransform>().localPosition = new Vector2(totalXPadding, pos.y);
            
        }
        //currentWidthRequired = totalXPadding;
    }

    /*
     * Adds a stamp object to the list and updates the list.
     */
    public void AddStamp(GameObject stampObj)
    {
        stamps.Add(stampObj);
        UpdateList();
    }

    /*
     * Removes a stamp object from the list and updates the list.
     */
    public void RemoveStamp(GameObject stampObj)
    {
        stamps.Remove(stampObj);
        UpdateList();
    }

    /*
     * Sets the appropriate container width and arranges the stamps inside.
     */
    public void UpdateList()
    {
        SetContWidth();
        if (GetContWidth() < screenWidth)
        {
            SetCentered();
        }
        else
        {
            SetRight();
        }
        ArrangeStamps();

        

    }

    private void SetCentered()
    {
        ResetAnchorPos();
        flexCont.pivot = new Vector2(0.5f, 1f);
        ResetAnchorPos();
    }

    private void SetRight()
    {
        ResetAnchorPos();
        flexCont.pivot = new Vector2(1f, 1f);
        ResetAnchorPos();

    }

    private void ResetAnchorPos()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);

    }



}
