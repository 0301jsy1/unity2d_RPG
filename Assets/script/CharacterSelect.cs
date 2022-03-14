using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour {

    public UnityEngine.EventSystems.EventSystem eventSystem;

    string selectHero = "";

    void Awake()
    {
        //캐릭터 씬에서 재생할 음악들 로드
        AudioManager.GetInstance().SceneLoadAudio("CharacterSelect");

        //배경음 플레이       
        StartCoroutine("PlayBgm");
    }

    IEnumerator PlayBgm()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.GetInstance().Play((int)SoundList_BGM_CharSelect.BGM_CHAR, true, AudioType.BGM, 1, Vector3.zero);
    }

    public void HeroSelect()
    {
        string name = eventSystem.currentSelectedGameObject.name;
        Debug.Log("내가 누른 캐릭터 : " + name);
        
        //내가 무슨 캐릭터를 선택했는지 저장
        selectHero = name;
    }

    public void StartGame()
    {
        //캐릭터 선택후 게임 씬으로 이동하는 코드
        if(selectHero == "")
        {
            BGManager.GetInstance().OpenPopup("오류!", "캐릭터를 선택하세요.");
        }
        else
        {
            //기존에 저장된 거 지움
            PlayDataManager.GetInstance().DeleteAllData();
            //캐릭터 저장하는 코드
            PlayDataManager.GetInstance().SetHero(selectHero);
            //씬 이동
            UnityEngine.SceneManagement.SceneManager.LoadScene("Tower");
        }  
    }
}
