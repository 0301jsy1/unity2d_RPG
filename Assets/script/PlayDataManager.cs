using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDataManager : MonoBehaviour
{ //게임오브젝트의 컴포넌트로 사용하지 않는다면 모노비헤이비어를 상속받아야 함

    public struct tagHeroInventory
	{
		public string itemName;
		public int itemCount;

		public tagHeroInventory(string _name, int _count)
		{
			itemName = _name;
			itemCount = _count;
		}
	}

	void Awake()
	{	
		DontDestroyOnLoad (this.gameObject);
	}
	static PlayDataManager instance = null;

	public static PlayDataManager GetInstance()
	{
		if (instance == null) {
			instance = GameObject.FindObjectOfType<PlayDataManager> ();
		}
		return instance;
	}

	public List<PlayDataManager.tagHeroInventory> inventory = new List<PlayDataManager.tagHeroInventory>();//캐릭터 인벤토리

	//주인공의 인벤토리를 저장하는 함수
	public void AddInven(string _itemname, int _itemcount)
	{
		// 1. 인벤토리에 있는지 검사
		bool isInven = false;
		for (int aa = 0; aa < inventory.Count; aa++)
		{
			if (inventory[aa].itemName == _itemname)
			{
				// 2. 인벤토리에 있으면 수량 추가
				isInven = true;
				PlayDataManager.tagHeroInventory tempInven = inventory[aa];
				tempInven.itemCount++;
				inventory[aa] = tempInven;
				// 구조체 값을 변경할 때는 
				// inventory[aa].itemCount++; 가 아니라
				// 임시 변수에 구조체를 담은 후 임시변수를 바꾸고
				// 그 값을 다시 구조체에 넣어주는 방식으로 해야 함.
				break;
			}
		}
		if (isInven == false)
		{
			// 3. 인벤토리에 없으면 새롭게 1개를 추가 
			inventory.Add(new PlayDataManager.tagHeroInventory(_itemname, _itemcount));
		}
        // 3. 아이템을 로컬에 저장한다. 
        SaveInven();
		GameManager.GetInstance().InvenRefresh();
	}

    void SaveInven()
    {
        
        string itemStr = "";
        for (int i = 0; i < inventory.Count; i++)
        {
            itemStr += inventory[i].itemName + "_" + inventory[i].itemCount + "|";
            // 인벤토리에 있는 아이템들을 모두 문자열 화 시킴.
            // EX) 사과2개, 고기3개, 레몬1개 이 있다면
            // "사과_2|고기_3|레몬_1|" 의 형태로 저장해 놓고
            // 나중에 |로 문자열을 구분하여 다시 인벤토리에 집어넣는다.
        }
        PlayerPrefs.SetString("inventory", itemStr);
        PlayerPrefs.Save();
    }

    public void DeleteInven(string name)
    {
        bool life = false;
        tagHeroInventory tempInven;
        int invenindex = -1;
        for(int i=0; i<inventory.Count; i++)
        {
            if(inventory[i].itemName == name)
            {
                //아이템이 인벤토리에 존재한다
                life = true;
                invenindex = i;//해당 아이템의 인덱스를 기억한다
                break;
            }
        }
        if(life == true)//아이템이 있으면
        {
            tempInven = inventory[invenindex];
            tempInven.itemCount--;

            if(tempInven.itemCount <= 0)
            {
                //수량이 0이하이면 가방에서 삭제해야 한다
                inventory.RemoveAt(invenindex);
                //RemoveAt는 특정 인덱스를 삭제하는 함수
                //Remove는 특정 아이템 자체를 삭제하는 함수
                //구조체의 경우 모든 값이 다 같아야 함
            }
            else
            {
                inventory[invenindex] = tempInven;
            }

            SaveInven();
        }
        else //아이템이 없으면
        {
            BGManager.GetInstance().OpenPopup("오류", "내 가방에 없는 아이템 입니다.");
        }
    }

	public void SettingInven()
	{
		inventory.Clear (); //리트 & 딕셔너리 등에서 내용물을 모두 삭제
		//저장된 인벤토리 목록을 불러와서 다시 셋팅한다
		string saveInvenList = PlayerPrefs.GetString ("inventory");
		string[] arr = saveInvenList.Split ('|');
		for (int i = 0; i < arr.Length; i++) {
			if (arr [i] != "") {//"고기|고기|사과|레몬|"
				string[] arr2 = arr [i].Split ('_');
				inventory.Add (new PlayDataManager.tagHeroInventory (arr2 [0], int.Parse (arr2 [1])));
			}
		}
	}

	public int GetMoney()
	{
		return PlayerPrefs.GetInt ("money", 5000);
		//자기 돈을 반환
		//돈이 없으면 5천원 반환(기본 금액이 5천원)
	}

	public delegate void VoidFunction();
	//델리게이트 : 함수를 담을 수 있는 사용자 정의 자료형
	//ex) public delegate 반환형 이름(매개변수);
	//Furc f; -> f();

	public void AddMoney(int _add, VoidFunction _v)
	{	
		//돈을 추가시켜 주는 함수
		PlayerPrefs.SetInt ("money",GetMoney() + _add);
		PlayerPrefs.Save ();	//PlayerPrefs : 디바이스(컴퓨터, 핸드폰)에 저장시켜주는 함수

		if (_v != null) {
			_v ();
		}			
	}

    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        //모든 데이터를 지우는 함수
    }

    public void SetHero(string _name)
    {
        //새로운 주인공을 설정하는 함수
        PlayerPrefs.SetString("hero", _name);
        PlayerPrefs.Save();
    }

    public string GetHero()
    {
        //선택한 주인공이 무엇인지 반환해 주는 함수
        return PlayerPrefs.GetString("hero", "");
        //선택한 히어로가 없다면 ""반환
    }
}
