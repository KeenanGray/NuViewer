using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Directional_Pad : MonoBehaviour
{
    public static Vector2 d_pad_Input;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GetComponentsInChildren<EventTrigger>().Length; i++)
        {
            EventTrigger et = transform.GetChild(i).GetComponent<EventTrigger>();
            et.triggers[0].callback.RemoveAllListeners();
            switch (i)
            {
                case 0://UP
                    et.triggers[0].callback.AddListener((data) => { Input_Up((PointerEventData)data); });
                    et.triggers[1].callback.AddListener((data) => { Vertical_Up((PointerEventData)data); });
                    break;
                case 1://DOWN
                    et.triggers[0].callback.AddListener((data) => { Input_Down((PointerEventData)data); });
                    et.triggers[1].callback.AddListener((data) => { Vertical_Up((PointerEventData)data); });
                    break;
                case 2://RIGHT
                    et.triggers[0].callback.AddListener((data) => { Input_Right((PointerEventData)data); });
                    et.triggers[1].callback.AddListener((data) => { Horizontal_Up((PointerEventData)data); });
                    break;
                case 3://LEFT
                    et.triggers[0].callback.AddListener((data) => { Input_Left((PointerEventData)data); });
                    et.triggers[1].callback.AddListener((data) => { Horizontal_Up((PointerEventData)data); });
                    break;

            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // d_pad_Input = new Vector3(0, 0);
    }

    public void Input_Up(PointerEventData data)
    {
        d_pad_Input.y = 1;
    }
    public void Input_Down(PointerEventData data)
    {
        d_pad_Input.y = -1;
    }
    public void Input_Left(PointerEventData data)
    {
        d_pad_Input.x = -1;
    }
    public void Input_Right(PointerEventData data)
    {
        d_pad_Input.x = 1;
    }

    public void Horizontal_Up(PointerEventData data)
    {
        d_pad_Input.x = 0;

    }

    public void Vertical_Up(PointerEventData data)
    {
        d_pad_Input.y = 0;
    }
}
