using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;
using UnityEngine.UI;


public class SquashCounterAndChanger : MonoBehaviour
{
    private List<Joycon> joycons;

    // Values made available via Unity

    private int jc_ind = 0;
    public Quaternion orientation;
    public Vector3 gyroR = new Vector3(0, 0, 0);
    private Text text;
    private int currentText = 0;
    private bool isChange = false;
    private Quaternion yAxis = new Quaternion(0, 1, 0, 0);
    public float angle = 0;
    private bool recentered = false;
    private GameObject parent;
    private Button button;
    private Joycon jLeft;
    private Joycon jRight;
    private int delayGyro = 0;


    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;

            // get the public Joycon array attached to the JoyconManager in scene
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

            

            if (joycons.Count < jc_ind + 1)
            {
                Destroy(gameObject);
            }
            text = gameObject.GetComponent<Text>();
            text.text = "" + currentText;

            button = parent.GetComponent<Button>();
        



    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (parent == EventSystem.current.currentSelectedGameObject)
        {

            if (jRight.GetButtonDown(Joycon.Button.PLUS))
            {
                Debug.Log("Shoulder button 2 pressed");
                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
                jLeft.Recenter();
                recentered = true;
                
            }



            orientation = jLeft.GetVector();
            orientation = saveDecimalBit(orientation, 2);
            angle = Quaternion.Angle(orientation, yAxis);


            if (isChange == true && recentered == true)
            {
                if (angle < 20)
                {
                    isChange = false;
                }
            }


            if (angle > 60 && isChange == false)
            {
                text.text = "" + currentText;
                if (currentText < 9)
                {
                    currentText++;
                    isChange = true;
                }

            }


        }

    }

    private void FixedUpdate()
    {

            
        
        if (parent == EventSystem.current.currentSelectedGameObject)
        {
            delayGyro++;
            gyroR = jRight.GetGyro();
            float last = PlayerPrefs.GetFloat("LAST_VALUE");
            //Debug.Log("delay: " + delayGyro);
            if (delayGyro > 10 && Math.Abs(gyroR.z - last) < 2)
            {

                float temp = gyroR.z;
                if (gyroR.z > 5)
                {

                    gyroR.x = 0;
                    gyroR.y = 0;
                    gyroR.z = 0;
                    delayGyro = 0;

                    EventSystem.current.SetSelectedGameObject(null);

                    Selectable next = button.FindSelectableOnRight();
                    PlayerPrefs.SetFloat("LAST_VALUE", temp);
                    Debug.Log(temp + " From " + parent.name + " to " + next.name);
                    next.Select();

                }
                if (gyroR.z < -5)
                {
                    gyroR.x = 0;
                    gyroR.y = 0;
                    gyroR.z = 0;
                    delayGyro = 0;
                    EventSystem.current.SetSelectedGameObject(null);

                    Selectable next = button.FindSelectableOnLeft();
                    Debug.Log(temp + " From " + parent.name + " to " + next.name);
                    next.Select();

                }
            }
        }
    }

    public void Reset(Vector3 vector)
    {
        vector.x = 0;
        vector.y = 0;
        vector.z = 0;
    }

    public Vector3 saveDecimalBit(Vector3 original, int bits)
    {
        Vector3 res;
        res.x = (float)System.Math.Round(original.x, bits);
        res.y = (float)System.Math.Round(original.y, bits);
        res.z = (float)System.Math.Round(original.z, bits);
        return res;
    }

    public Quaternion saveDecimalBit(Quaternion original, int bits)
    {
        Quaternion res;
        res.x = (float)System.Math.Round(original.x, bits);
        res.y = (float)System.Math.Round(original.y, bits);
        res.z = (float)System.Math.Round(original.z, bits);
        res.w = (float)System.Math.Round(original.w, bits);
        return res;
    }
}
