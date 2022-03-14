using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDll;
using MyDll2;

public class TestDll : MonoBehaviour {

	
	void Start () {
        Class1 c = new Class1();
        int a = c.Add(10, 20);

        Debug.Log("계산한 값 : " + a);

        this.gameObject.AddComponent<Class1>();
    }
	
	
	
}
