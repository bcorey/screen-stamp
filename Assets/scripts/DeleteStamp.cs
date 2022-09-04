using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteStamp : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(Delete);
    }

    private void Delete()
    {
        Destroy(transform.parent);
        
    }
}
