using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour {

	void Start () {
        //2d 카메라 사이즈 조절
        //이 스크립트는 카메라의 컴포넌트로 붙여야 한다

        Camera thisCam; // 자기 카메라 갖고옴
        thisCam = this.GetComponent<Camera>();

        float value =
            (Screen.height / (Screen.width / 16.0f)) / 9.0f;
        //화면의 가로에 맞춰서 카메라를 확대하거나 축소
        //현재 기기의 해상도를 16:9 비율로 조절하면 몇 %가 나오는지 계산

        thisCam.orthographicSize *= value;
        //기본 오쏘그래픽 사이즈에다가 위에서 구한 %를 곱하면 카메라가 확대되거나 축소됨
	}		
}
