using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMusic : MonoBehaviour {

	

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))//배경음 재생
            //AudioManager.GetInstance().Play(0, true, "effect", 1, Vector3.zero);
            AudioManager.GetInstance().Play(0, true, AudioType.BGM, 2, Vector3.zero);
        if (Input.GetKeyDown(KeyCode.P))
            AudioManager.GetInstance().SetPitch(1); //먼저 피치 조절하고 배경음 재생해줘야 작동
        if (Input.GetKeyDown(KeyCode.S))
            AudioManager.GetInstance().SetPitch(2); //먼저 피치 조절하고 배경음 재생해줘야 작동
        if (Input.GetKeyDown(KeyCode.Z))//제로볼륨(음소거) 하기
            AudioManager.GetInstance().ZeroVolume(AudioType.BGM, true);
        if (Input.GetKeyDown(KeyCode.X))//제로볼륨(음소거) 풀기
            AudioManager.GetInstance().ZeroVolume(AudioType.BGM, false);
    }
}
