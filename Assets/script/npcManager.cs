using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic; //특이한 메모리 구조를 사용하기 위한 기능을 사용

public class npcManager : MonoBehaviour {

    Dictionary<string, string> npcList = new Dictionary<string, string>(); //이름 값이니깐 string
    //string형 키( : 주소값), string형 값 ( : 데이터)을 갖는  npcList라는 딕셔너리를 생성
    //배열과 비슷한 메모리 구조로  키와 데이터 값이 한 쌍을 이루어 변수를 선언하여 원하는 만큼 담을 수 있고 뺄 수 있다.
    //배열 : 연속된 데이터 공간, 한번 만들면 크기 변경 불가, 삽입 추가 불가, 접근은 숫자 인덱스로, 빠르다.
    //딕셔너리 : 색인을 위한 배열,  삽입시 add(키, 값), arr.add("십",10), 인덱스를 정할 수 있음, Dictionary<인덱스,자료형>, 힙에 저장

    Dictionary<string, GameObject> npcObjectList = new Dictionary<string, GameObject>(); //게임오브젝트이니깜 GameObject 값
    //생성된 npc 오브젝트들의 리스트를 가지고 있는 딕셔너리이다

    Dictionary<string, Sprite> npcFaceList = new Dictionary<string, Sprite>();
    //npc들의 얼굴 사진 목록을 갖는 딕셔너리이다
    public GameObject talkObj;
    public UnityEngine.UI.Text talkText;
    public UnityEngine.UI.Image talkFace;

    void Start() {

        talkObj.SetActive(false);
        TextAsset npcTextAsset = Resources.Load("Text/npc") as TextAsset;  // Resources/Text/npc.txt
        StringReader npcreader = new StringReader(npcTextAsset.text); //StringReader : 읽기를 위한 도구, 확장자 즉 형식을 인식할 수 없다.

        string line;
        while (true)
        {
            line = npcreader.ReadLine(); //한줄씩 텍스트를 읽어서 line 변수에 담는다
            if (line == null) break;
            string[] arr = line.Split(','); //파싱 : 특정한 문자로 구분하여 나누는 것을 파싱한다는 것
            //npc0.5.4,left
            //arr[0] => "npc0",
            //arr[1] => "5", 5행
            //arr[2] => "4", 4열
            //arr[3] => "left", 왼쪽 방향
            //arr[4] => 대사
            GameObject prefab = Resources.Load("npc/" + arr[0]) as GameObject; // Resources/npc
            GameObject g = Instantiate(prefab) as GameObject;
            g.GetComponent<npc>().Init(int.Parse(arr[1]), int.Parse(arr[2]), arr[3], arr[4]); // Resources/Text/npc.txt

            npcList.Add(arr[1] + "_" + arr[2], arr[0]); //위치 값을 색인할 수 있도록 저장하고 해당 위치에 누가 있는지(npc 몇번에 있는지 그 이름을) 알아보기 위해서 추가한다
            //딕셔너리에 "5_4"을 키로, "npc0"을 값으로 하는 데이터를 집어 넣는다.

            npcObjectList.Add(arr[1] + "_" + arr[2], g); //게임 오브젝트가 뭔지 알아보기 위해서 추가한다.
            //인스턴스화 한 오브젝트 g를, "5_4" 키로 저장

            npcFaceList.Add(arr[1] + "_" + arr[2], (Sprite)Resources.Load("Face/" + arr[0], typeof(Sprite)));
            //이미지도 여러 형식이 있기 때문에 sprite 형식을 가진 이미지를 불러오면 
            //위와 같은 형식으로 Resources.Load를 사용해야만 한다.
        }
    }

    void Update()
    {
		// 0: 마우스 왼쪽 버튼 0, 오른쪽 1, 휠 2
		if(!Input.GetMouseButtonDown(0) && !Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0)){ 
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Space))
        {
            //아무키나 눌렀는데 그게 스페이스바가 아니라면
            talkObj.SetActive(false);
			if (npcScript != null)
				npcScript.CloseNpcObject ();
        }
      }
	}

    public bool CheckNpc(int _row, int _col)
    {
        //npcList에 받은 행_렬 이라는 키를 가진 데이터가 있는지 확인
        if (npcList.ContainsKey(_row + "_" + _col))
            return true; //있으면 true 반환
        return false; //없으면  false 반환
    }

    //주인공이 스페이스바를 누르면 이 함수를 호출해서 talkobj를 활성화 시킨다.
	npc npcScript = null; //내가 지금 만나고 있는 npc를 저장
    public void Talk(int _row, int _col)
        {
			//원하는 방향에 npc가 있다면
            if(npcList.ContainsKey(_row + "_"+_col))
             {
			//대화창을 활성화
                 talkObj.SetActive(true);
                 talkText.text = npcObjectList[_row + "_" + _col].GetComponent<npc>().talk;
                 talkFace.sprite = npcFaceList[_row + "_" + _col];
				 npcObjectList[_row + "_" + _col].GetComponent<npc>().OpenNpcObject();
				 npcScript = npcObjectList [_row + "_" + _col].GetComponent<npc> ();
				
             }
        }

}
