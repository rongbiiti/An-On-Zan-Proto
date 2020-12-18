using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCamera_Transform : MonoBehaviour
{
    Vector3 pos;
    Quaternion rot;
    private int cameracount;

    // Start is called before the first frame update
    void Start()
    {
        pos = gameObject.transform.position;
        rot = gameObject.transform.rotation;
        cameracount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(cameracount < 3)
            {
                cameracount++;
            }else{
                cameracount = 0;
            }
            ChangeCameraPosiyion();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (cameracount > 0)
            {
                cameracount--;
            }
            else{
                cameracount = 3;
            }
            ChangeCameraPosiyion();
        } 
    }

    void ChangeCameraPosiyion()
    {
        switch (cameracount)
        {
            case 0:     //プレイヤー側斜め上視点
                gameObject.transform.position = new Vector3(6, 9.98f, -6);
                gameObject.transform.rotation = Quaternion.Euler(35, -45, 0);
                break;
            case 1:     //正面横
                gameObject.transform.position = new Vector3(7, 5.2f, 0);
                gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case 2:     //相手側斜め上視点
                gameObject.transform.position = new Vector3(-6, 9.98f, 6);
                gameObject.transform.rotation = Quaternion.Euler(35, 135, 0);
                break;
            case 3:     //俯瞰視点
                gameObject.transform.position = new Vector3(0, 19, -0.9f);
                gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
        }
    }
}
