using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMove : MonoBehaviour
{

    /// <summary>
    /// Step #1
    /// We need a simple reference of joystick in the script
    /// that we need add it.
    /// </summary>
	[SerializeField] private bl_Joystick Joystick;//Joystick reference for assign in inspector

    [SerializeField] private float Speed = 5;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindObjectOfType<GameManager>().hasGameStarted.Value)
        {
            var diff = transform.position - Camera.main.transform.position;

            //Step #2
            //Change Input.GetAxis (or the input that you using) to Joystick.Vertical or Joystick.Horizontal
            float v = Joystick.Vertical; //get the vertical value of joystick
            float h = Joystick.Horizontal;//get the horizontal value of joystick

            //in case you using keys instead of axis (due keys are bool and not float) you can do this:
            //bool isKeyPressed = (Joystick.Horizontal > 0) ? true : false;

            //ready!, you not need more.
            Vector3 translate = new Vector3(h, 0, v);

            translate = Camera.main.transform.TransformDirection(translate);
            translate.y = 0.0f;
            translate.Normalize();

            var moveDir = (translate * Time.deltaTime) * Speed;
            transform.Translate(moveDir);
        }
    }
}
