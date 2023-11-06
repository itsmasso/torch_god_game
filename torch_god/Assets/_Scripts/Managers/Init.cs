using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{

    private void Start()
    {      
        GameManager.Instance.UpdateScene(0);
    }


}
