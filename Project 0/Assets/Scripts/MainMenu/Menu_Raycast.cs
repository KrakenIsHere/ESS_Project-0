using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Raycast : MonoBehaviour
{
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ~(1 << 8)))
            {
                var Object = hit.collider.gameObject;

                ConsoleProDebug.Watch("Hit object Tag", Object.tag);

                switch (Object.tag)
                {
                    case "Dice":
                        {
                            var dice = Object.GetComponent<Dice_Controller>();

                            dice.mouseUsed = true;
                            break;
                        }
                    case "PlayerButton":
                        {
                            var PB = Object.GetComponent<PlayerButtonController>();

                            PB.isBeingActivated = true;
                            break;
                        }
                }
            }
        }
    }
}
