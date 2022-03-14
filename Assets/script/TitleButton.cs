using UnityEngine;
using System.Collections;

public class TitleButton : MonoBehaviour {

    public void MoveCharSelect()
    {
        //재생 중이던 모든 배경음, 효과음 삭제
        AudioManager.GetInstance().StopSound(AudioType.BGM);
        AudioManager.GetInstance().StopSound(AudioType.Effect);

        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelect");
    }

	public void MoveGame()
    {
        //재생 중이던 모든 배경음, 효과음 삭제
        AudioManager.GetInstance().StopSound(AudioType.BGM);
        AudioManager.GetInstance().StopSound(AudioType.Effect);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Tower"); //씬 전환
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll(); //PlayerPref를 통해 저장한 모든 것 삭제
        PlayerPrefs.Save();//삭제후 세이브 해줘야함

        /*
         PlayerPrefs.DeleteKey("키값"); 
         PlayerPrefs.Save();//삭제후 세이브 해줘야함
         
         
          */
    }
}
