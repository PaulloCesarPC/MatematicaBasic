using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class necklaceText : MonoBehaviour {
    public GameObject obj;  
    void LateUpdate() {
        if(obj != null)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint
           (Camera.main, obj.transform.position);
            transform.position = screenPoint;

        }
    }
}
