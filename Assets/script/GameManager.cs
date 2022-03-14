using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    static GameManager instance = null; //싱글톤 만들기

    public static GameManager GetInstance()
    {
        if(instance == null)
        {
            instance = GameObject.FindObjectOfType<GameManager>();
        }
        return instance;
    }

    public GameObject inventoryObj; //인벤토리 오브젝트
    UnityEngine.UI.Image[] invenItem;//인벤토리 아이템 이미지들
    UnityEngine.UI.Text[] invenName; //인벤토리 이름 텍스트들
    UnityEngine.UI.Text[] invenNumber; //인벤토리 갯수 텍스트들
   

	List<PlayDataManager.tagHeroInventory> heroInven; //히어로 한테 인벤토리 받아서 값을 넣어줄것임
    public Hero heroScript; //히어로 스크립트

    public Dictionary<string, Sprite> imageList = new Dictionary<string, Sprite>();
    //리소스 폴더에서 아이템 이미지들을 갖고 옴

	public UnityEngine.EventSystems.EventSystem eventSystem;
    
    void Start()
    {
        //for (int i = 0; i < DataBase.GetInstance().itemNameList.Count; i++)
        //{
        //    imageList.Add(DataBase.GetInstance().itemNameList[i], (Sprite)Resources.Load("itemImage/" + DataBase.GetInstance().itemNameList[i], typeof(Sprite)));
        //}

        //주인공 생성 스크립트
        GameObject heroPrefab = Resources.Load("Hero/" + PlayDataManager.GetInstance().GetHero()) as GameObject;

        GameObject g = Instantiate(heroPrefab); //히어로 오브젝트 생성
        heroScript = g.GetComponent<Hero>();//히어로 스크립트 등록

		// 이벤트 시스템 등록
		eventSystem = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        //foreach(임시변수 in 순회할 리스트 or 딕셔너리 or ...)
        foreach (KeyValuePair<string, DataBase.tagItemList> k in DataBase.GetInstance().itemNameList)
        {
            imageList.Add(k.Key, (Sprite)Resources.Load("itemImage/" + k.Value.itemCode, typeof(Sprite)));
        }

        inventoryObj.SetActive(false);
        invenItem = new UnityEngine.UI.Image[Myconst.MAX_INVEN];
        invenName = new UnityEngine.UI.Text[Myconst.MAX_INVEN];
        invenNumber = new UnityEngine.UI.Text[Myconst.MAX_INVEN];

        for (int i = 0; i < Myconst.MAX_INVEN; i++)
        {
            invenItem[i] = inventoryObj.transform.Find("inven" + i).GetComponent<UnityEngine.UI.Image>();
            //inventoryObj의 자식 오브젝트 중 inven0~9 이름을 가진 걸 찾아서 이미지 컴포넌트를 가져온다.

            invenName[i] = inventoryObj.transform.Find("name" + i).GetComponent<UnityEngine.UI.Text>();

            invenNumber[i] = inventoryObj.transform.Find("number" + i).GetComponent<UnityEngine.UI.Text>();

            invenItem[i].gameObject.SetActive(false); //비활성화
            invenName[i].gameObject.SetActive(false); //비활성화
            invenNumber[i].gameObject.SetActive(false); //비활성화
        }
    }

    void OnDestroy()
    {
        //이 함수는 오브젝트가 파괴될 때 불린다
        instance = null;
        //오브젝트가 파괴될 때 instance를 해주면 다른 씬에서 GetInstace()를 호출했을때 그 씬에 있는 GameManager의 컴포넌트가 불리게 된다
        Debug.Log("destroy GameManager");
    }

	void Update () {
	    if(Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryObj.activeSelf)//인벤토리가 켜져있으면
                inventoryObj.SetActive(false);//닫는다
            else // 인벤토리가 꺼져 있으면 
            {
                inventoryObj.SetActive(true);//킨다. 
                InvenRefresh();              
            }
        }

		if (Input.GetKeyDown(KeyCode.K)) {
			PlayDataManager.GetInstance().AddMoney(300, InvenRefresh);
		}
       
	}

    public void InvenRefresh()
    {
		//내소지금 출력
		inventoryObj.transform.Find ("label_money").GetComponent<UnityEngine.UI.Text> ().text 
					= "내가가진 돈 : " + PlayDataManager.GetInstance().GetMoney();

		heroInven = PlayDataManager.GetInstance().inventory; //히어로의 인벤토리 받아와서
        for (int i = 0; i < heroInven.Count; i++)
        {
            // Debug.Log(heroInven[i]); //잘 받았는지 한번 출력해 봄
            invenItem[i].gameObject.SetActive(true);
            invenItem[i].sprite = imageList[heroInven[i].itemName];
            //heroInven[i] 에는 아이템의 이름이 들어있다
            //그이름에 맞는 이미지를 찾아서 inven0~9의 스프라이트를 바꿔준다.

            invenName[i].gameObject.SetActive(true);
            invenName[i].text = DataBase.GetInstance().itemNameList[heroInven[i].itemName].itemName;

            invenNumber[i].gameObject.SetActive(true);
            invenNumber[i].text = heroInven[i].itemCount + "개";
        }

        for(int i = heroInven.Count; i<invenItem.Length; i++)
        {
            //가방에 든 아이템 개수번째 인덱스부터 인벤토리 끝까지 안쓰는 번호는 끄자.
            invenItem[i].gameObject.SetActive(false);
            invenName[i].gameObject.SetActive(false);
            invenNumber[i].gameObject.SetActive(false);
        }
    }
}
