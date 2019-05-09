using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineStateListener : MonoBehaviour
{

    public float TimeNotificationVisible = 3f;

    bool HasExpireTime = false;
    float ExpireTime;
    int LastEngineFlag = 0;

    public void UpdateState(int engineFlag)
    {
        GetComponent<Text>().text = "Engine Flag: " + engineFlag;
        if (engineFlag != LastEngineFlag)
        {
            GetComponent<Text>().enabled = true;
            LastEngineFlag = engineFlag;
            HasExpireTime = true;
            ExpireTime = Time.time + TimeNotificationVisible;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasExpireTime && Time.time > ExpireTime)
        {
            HasExpireTime = false;
            GetComponent<Text>().enabled = false;
        }
    }
}
