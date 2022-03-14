using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    BGM,
    Effect
}

public enum SoundList_BGM_Title //타이틀 씬의 배경음 리스트
{
    BGM_TITLE
}

public enum SoundList_Effect_Title //타이틀 씬의 효과음 리스트
{
    CLICK1
}

public enum SoundList_BGM_CharSelect //캐릭터 씬의 배경음 리스트
{
    BGM_CHAR
}

public enum SoundList_Effect_CharSelect //캐릭터 씬의 효과음 리스트
{
    CLICK2
}

public class AudioManager : MonoBehaviour {

    // 사운드 매니저

    private static AudioManager instance = null;

    public static AudioManager GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<AudioManager>();
        }
        return instance;
    }

    public List<AudioClip> bgmClips = new List<AudioClip>();//배경음 리스트
    public List<AudioClip> effectClips = new List<AudioClip>();//효과음 리스트

    float pitch = 1; //음악 재생 속도(빠르기, 2 : 두배)

    float effectsound = 1; //효과음의 음량(0~1)
    float bgmsound = 1; //배경음의 음량(0~1)

    bool bgmmute = false, effectmute = false;
    //음소거 여부, true 이면 음소거 됨

    GameObject soundRoot_BGM, soundRoot_Effect;
    //효과음, 배경음이 재생될 때 부모로 등록할 오브젝트

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject); //나 삭제 안함

        soundRoot_BGM = this.transform.Find("BgmSound").gameObject;
        soundRoot_Effect = this.transform.Find("EffectSound").gameObject;

        //효과음의 음량 저장된 값 불러오기
        //배경음의 음량 저장된 값 불러오기

        SceneLoadAudio("Title");

        Play((int)SoundList_BGM_Title.BGM_TITLE, true, AudioType.BGM, 1, Vector3.zero);
    }

    public void SceneLoadAudio(string sceneName)
    {
        bgmClips.Clear();
        effectClips.Clear();
        //로드 시켰었던 bgm, effect 목록들을 지운다

        //씬에 따라 다시 파일들 로드
        switch(sceneName)
        {
            case "Title":
                bgmClips.Add((AudioClip)Resources.Load("Sound/BGM/bgm_title", typeof(AudioClip)));
                effectClips.Add((AudioClip)Resources.Load("Sound/EFFECT/click1", typeof(AudioClip)));
                break;
            case "CharacterSelect":
                bgmClips.Add((AudioClip)Resources.Load("Sound/BGM/bgm_char", typeof(AudioClip)));
                effectClips.Add((AudioClip)Resources.Load("Sound/EFFECT/click2", typeof(AudioClip)));
                break;
            case "Tower":
                bgmClips.Add((AudioClip)Resources.Load("Sound/BGM/bgm_game", typeof(AudioClip)));
                effectClips.Add((AudioClip)Resources.Load("Sound/EFFECT/hit1", typeof(AudioClip)));
                break;
            case "ItemMap":
                bgmClips.Add((AudioClip)Resources.Load("Sound/BGM/bgm_game", typeof(AudioClip)));
                effectClips.Add((AudioClip)Resources.Load("Sound/EFFECT/hit2", typeof(AudioClip)));
                break;
        }
    }

    public void SetPitch(float p)
    {
        //사운드 재생 속도 조절
        pitch = p;

        //재생중인 음악들의 재생 속도 모두 조절
        foreach (Transform t in soundRoot_BGM.transform)
            t.GetComponent<AudioSource>().pitch = p;

        foreach (Transform t in soundRoot_Effect.transform)
            t.GetComponent<AudioSource>().pitch = p;
    }

    public void StopSound(AudioType audio) //tag : 효과음인지 배경음인지 전달
    {
        //사운드 재생 중지
        switch (audio)
        {
            case AudioType.Effect: //재생 중인 효과음 모두 삭제
                foreach (Transform t in soundRoot_Effect.transform)
                    Destroy(t.gameObject);
                break;
            case AudioType.BGM: //재생 중인 비지엠 모두 삭제
                foreach (Transform t in soundRoot_BGM.transform)
                    Destroy(t.gameObject);
                break;
        }
    }

    public void SetVolume(AudioType audio, float soundsize)
    {
        //음악의 음량 조절, tag : 효과음 or 배경음, soundsize : 음량크기
        switch (audio)
        {
            case AudioType.Effect:
                foreach (Transform t in soundRoot_Effect.transform)
                    t.GetComponent<AudioSource>().volume = soundsize;
                break;
            case AudioType.BGM:
                foreach (Transform t in soundRoot_BGM.transform)
                    t.GetComponent<AudioSource>().volume = soundsize;
                break;
        }
    }

    public void ZeroVolume(AudioType audio, bool life)
    {
        //음악을 전부 음소거 하는 기능, tag : 배경음 또는 효과음
        //life : 음소거이면 true, 아니면 false
        switch (audio)
        {
            case AudioType.Effect:
                foreach (Transform t in soundRoot_Effect.transform)
                    t.GetComponent<AudioSource>().mute = life;
                break;
            case AudioType.BGM:
                foreach (Transform t in soundRoot_BGM.transform)
                    t.GetComponent<AudioSource>().mute = life;
                break;
        }
    }

    bool isPlaying; //음악 재생이 가능한지... 한번에 같은 음을 여러개 재생 안하려고
    int childCount; //재생되고 있는 똑같은 음악의 개수

    public AudioSource Play(int clipnum, bool loop, AudioType audio, int maxcount, Vector3 pos)
    {
        //음악을 재생하는 함수
        //clipnum = 리스트에 등록된 음악 번호
        //loop = 반복 재생 할 것인지 여부
        //tag = effect or bgm
        //maxcount = 같은 음악이 동시에 최대 몇 번 나올 수 있는지
        //pos = 사운드의 위치, 2d이면 상관없지만 3d이면 중요함

        isPlaying = true; //재생 가능하다
        childCount = 0;
        switch (audio)
        {
            case AudioType.Effect:
                foreach (Transform t in soundRoot_Effect.transform)
                {
                    if (t.name == clipnum.ToString())
                        childCount++;
                    //똑같은 음악이 지금 몇개 재생되고 있는지 셈
                }
                if (childCount >= maxcount) isPlaying = false;
                break;
            case AudioType.BGM:
                foreach (Transform t in soundRoot_BGM.transform)
                {
                    if (t.name == clipnum.ToString())
                        childCount++;
                }
                if (childCount >= maxcount) isPlaying = false;
                break;
        }
        if (isPlaying == false) return null;

        GameObject g = new GameObject(clipnum.ToString()); //빈게임 obj만듦
        g.transform.position = pos; //자기가 지정한 위치로 보내줌
        AudioSource source = g.AddComponent<AudioSource>();
        //빈겜 오브젝트에 오디오소스 하나 추가하고 그걸  source에 저장

        switch (audio) //입력한 태그에 맞게 음악을 재생하자
        {
            case AudioType.Effect:
                source.clip = effectClips[clipnum];
                g.transform.parent = soundRoot_Effect.transform;
                source.volume = effectsound;
                source.mute = effectmute;
                break;
            case AudioType.BGM:
                source.clip = bgmClips[clipnum];
                g.transform.parent = soundRoot_BGM.transform;
                source.volume = bgmsound;
                source.mute = bgmmute;
                break;
        }

        source.pitch = pitch; //음악 재생 속도 설정
        source.Play(); //음악을 재생
        source.loop = loop; //루프 여부 설정

        if (!loop) //루프 안하는 걸로 설정했으면
            Destroy(g, source.clip.length); //음악 시간 이후 g를 삭제
                                            //source.clip.length : 음악 재생 시간을 알려주는 코드
        return source;

    }
    

    
}
