using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private RectTransform canvasRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        //canvasRectTransform = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 cursorPos = (Input.mousePosition - canvasRectTransform.localPosition);
        transform.position = Input.mousePosition;
    }
}
