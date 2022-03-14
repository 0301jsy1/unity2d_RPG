using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            MySingleton.GetInstance().DebugWrite("이게 되니?");
        }
	}
}
