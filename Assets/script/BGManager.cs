using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour
{ //게임오브젝트의 컴포넌트로 사용하기 위해서는 모노비헤이비어를 상속받아야 함

    //싱글톤
    static BGManager instance = null;//자기 자신을 static형 변수로 선언

    public static BGManager GetInstance()
    {
        if(instance == null)
        {
            instance = GameObject.FindObjectOfType<BGManager>();
        }
        return instance;
    }

	public GameObject alertObj;
	public UnityEngine.UI.Text text_alerttext; //알럿 텍스트
    public GameObject popupObj;//팝업창ui
    public UnityEngine.UI.Text text_title, text_info; //텍스트들

    private void Awake()
    {
        
            DontDestroyOnLoad(this.gameObject);
            //씬이 바뀌어도 인자로 들어간 오브젝트가 삭제되지 않게 하는 함수
            //this.gameObject를 넣으면 자기자신을 삭제하지 않는다

            popupObj.SetActive(false); //팝업창은 일단 꺼두기
			alertObj.SetActive(false);
        
    }

    void Start()
    {
        DataBase.GetInstance().SetItemList();
    }

    public void OpenPopup(string _title, string _info)
    {
        text_title.text = _title;
        text_info.text = _info;
        //인자로 받은 내용들로 ui 내용을 변경

        popupObj.SetActive(true);
    }

    public void ClosePopup()
    {
        popupObj.SetActive(false);
    }

	//몇 초 동안만 보여지다가 사라지는 팝업창
	public void OpenAlert(string str, float time)
	{
		CancelInvoke ("CloseAlert"); //해당 함수로 등록된 Invoke를 모두 취소
		alertObj.SetActive (true);
		text_alerttext.text = str;

		Invoke ("CloseAlert", time);
		// time 초 뒤에 closeAlert를 실행하는 Invoke를 등록
		// Time.timesclae 영향을 받음
	}

	public void CloseAlert()
	{
		alertObj.SetActive(false);
	}
}
