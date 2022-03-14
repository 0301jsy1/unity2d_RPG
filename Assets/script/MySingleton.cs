using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySingleton : MonoBehaviour {

    //싱클톤 클래스
/*
* 디자인 패턴

-싱글톤 

1. 단 하나의 객체만 만드는 것

2. 자기 자신의 인스턴스를 가짐

3. 생성자를 private(유니티에서는 생성자를 안씀)

4. 관리자급만 사용하라
*/
    static MySingleton instance = null; 
    //자기 자신을 static 으로 선언

    public static MySingleton GetInstance()
    {
        //남이 가장 처음 이 함수르 호출했을 때 instance가 생성됨
        if(instance == null)
        {
            instance = GameObject.FindObjectOfType<MySingleton>();
            //instance = new MySingleton(); c#의 경우
            //씬에서 마이싱글톤 클래스를 컴포넌트로 가진 오브젝트를 찾아서 그 컴포넌트를 가져온다
            //때문에 마이 싱글톤 컴포넌트를 가진 오브젝트가 여러개 있으면 안되고 무조건 하나의 오브젝트에 하나의 컴포넌트만 붙여야 한다           
        }
        return instance;//인스턴스 리턴
        //이 GetInstance 함수로 다른 곳에서 마이싱글톤에 접근할 수 있다
    }

    //private MySingleton() { } c#의 경우

    public void DebugWrite(string s)
    {
        Debug.Log(s);
        //그냥 로그 출력하는 함수
    }
}
