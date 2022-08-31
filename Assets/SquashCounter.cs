using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class SquashCounter : MonoBehaviour{
    
    private List<Joycon> joycons;

    // Values made available via Unity

    private int jc_ind = 0;
    public Quaternion orientation;
    private Text text;
    private int currentText = 0;
    private bool isChange = false;
    private Quaternion yAxis = new Quaternion(0, 1, 0, 0);
    public float angle = 0;
    private bool recentered = false;
    private GameObject parent;
    private Button button;


    // Start is called before the first frame update
    void Start()
    {
        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
        {
            Destroy(gameObject);
        }
        text = gameObject.GetComponent<Text>();
        text.text = "" + currentText;
        parent = gameObject.transform.parent.gameObject;
        button = parent.GetComponent<Button>();
        button.Select();
    }

    // Update is called once per frame

    [Obsolete]
    private void Update()
    {
        if (joycons.Count > 0)
        {
            Joycon j = joycons[jc_ind];
            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 pressed");
                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
                j.Recenter();
                recentered = true;
                //Debug.Log("ori: " + orientation.w + ";" + orientation.x + ";" + orientation.y + ";" + orientation.z);
                
            }
            orientation = j.GetVector();
            orientation = saveDecimalBit(orientation, 2);
            angle = Quaternion.Angle(orientation, yAxis);


            if (isChange == true && recentered == true)
            {
                if (angle < 10)
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

    float PositionChange(float acc)
    {
        float deltaTime = (float)Time.deltaTime;
        float res = (float)(0.5 * PlayerPrefs.GetFloat("ACCY") * deltaTime * deltaTime);
        return res;
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


