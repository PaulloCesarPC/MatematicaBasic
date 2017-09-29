using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBlocks : MonoBehaviour {
    Rigidbody2D rb;
    public GameObject obj;
    bool hasRope = true;
    Vector3 screenPoint;
    Vector2 pos;
    bool mousePressed = false;
    float posX;
    float posY;
    CtrlGeneration ctrl;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        ctrl = FindObjectOfType(typeof(CtrlGeneration)) as CtrlGeneration;
    }

    private void FixedUpdate()
    {
        if (ctrl.gameOver)
            transform.Translate(Vector2.left * 2 * Time.deltaTime);
    }
    public void MouseDown(){      
        if (!hasRope && !ctrl.gameOver){         
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            posX = Input.mousePosition.x - screenPoint.x;
            posY = Input.mousePosition.y - screenPoint.y;
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
        }         
    }

    public void MouseDrag()
    {
        if (!hasRope && !ctrl.gameOver)
        {
            Vector3 posMouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - posX
            , Input.mousePosition.y - posY, screenPoint.z));
            transform.position = posMouse;
        }
    }

    public void MouseUp(){
        if (!hasRope && !ctrl.gameOver)
        {
            mousePressed = false;
            rb.gravityScale = 1;
            FindObjectOfType<CtrlGeneration>().AtivarVerificador();
        }
    }

    public void Cutter()
    {
        if (hasRope && !ctrl.gameOver)
        {
            Destroy(obj);
            hasRope = false;
            rb.gravityScale = 1;
        }
    }

    void OnBecameInvisible()
    {

        Destroy(gameObject);
    }
}
