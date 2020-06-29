using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public Canvas UI;
    public bool toggle;

    public void Activate()
    {
        UI.enabled = toggle;
    }
}
