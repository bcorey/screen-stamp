using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampCont : MonoBehaviour
{
    
    private void OnDestroy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i));
        }
    }

}
