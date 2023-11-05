using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject offset;
    public float offsetHeight;

    [Header("Left")]
    public bool leftFist;
    public bool[] leftfingers = new bool[5];
    public Vector3 leftPos;

    [Header("Right")]
    public bool rightFist;
    public bool[] rightfingers = new bool[5];
    public Vector3 rightPos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        print(data);
        string[] handData = data.Split(',');

        // Hand 1 (start at index 0)
        // Hand 2 (start at index 9)
        int start = 0;
        for (int i=0; i < 2; i++)
        {
            bool isLeft = int.Parse(handData[start + 0]) == 0;

            //0        1*3      2*3
            //x1,y1,z1,x2,y2,z2,x3,y3,z3

            float x = float.Parse(handData[start + 2])/ 400;
            float y = offsetHeight + float.Parse(handData[start + 3]) / 400;
            
            if (isLeft)
            {
                leftFist = int.Parse(handData[start + 1]) < 3;
                leftPos = new Vector3(x, y, 0);
                leftHand.transform.localPosition = leftPos;
            }
            else
            {
                rightFist = int.Parse(handData[start + 1]) < 3;
                rightPos = new Vector3(x, y, 0);
                rightHand.transform.localPosition = rightPos;
            }

            for (int j=0; j < 5; j++)
                (isLeft ? leftfingers : rightfingers)[j] = int.Parse(handData[start + 4 + j]) == 1;

            start = 9;
            if (handData.Length <= 9)
                break;
        }
    }
}