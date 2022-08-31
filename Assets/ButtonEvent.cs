using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    private List<Joycon> joycons;
    private Joycon jLeft;
    private Joycon jRight;
    private GameObject digit3Button;
    private Button button;
    private GameObject PlusConBubble;
    private GameObject HomeConBubble;
    private GameObject SquatBubble;
    private GameObject TurnBubble;

    // Start is called before the first frame update
    void Start()
    {
        PlusConBubble = GameObject.Find("PlusConBubble");
        HomeConBubble = GameObject.Find("HomeConBubble");
        SquatBubble = GameObject.Find("SquatBubble");
        TurnBubble = GameObject.Find("TurnBubble");

        PlusConBubble.SetActive(false);
        SquatBubble.SetActive(false);
        TurnBubble.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        joycons = JoyconManager.Instance.j;
        if (joycons[0].isLeft)
        {
            jLeft = joycons[0];
            jRight = joycons[1];
        }
        else
        {
            jRight = joycons[0];
            jLeft = joycons[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jRight.GetButtonDown(Joycon.Button.HOME))
        {
            digit3Button = GameObject.Find("Digit3");
            digit3Button.SetActive(true);
            digit3Button.GetComponent<Button>().Select();
            HomeConBubble.SetActive(false);
            PlusConBubble.SetActive(true);
        }
        if (jRight.GetButtonDown(Joycon.Button.PLUS))
        {
            PlusConBubble.SetActive(false);
            SquatBubble.SetActive(true);
            TurnBubble.SetActive(true);
        }
    }


}
