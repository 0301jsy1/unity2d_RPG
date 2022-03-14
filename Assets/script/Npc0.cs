using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Npc0 : npc
{

    // 아이템을 판매하는 npc

    GameObject prefab; // 판매하는 아이템 ui의 프리팹이 담긴다.

    Dictionary<string, int> sellList = new Dictionary<string, int>();
    // 이 딕셔너리에는 npc가 판매하는 아이템의 이름과 가격이 담긴다.

    public GameObject sellObj; // npc 판매 스크롤 뷰 오브젝트(껐다켰다 할려고)

    public GameObject thisSellContent;
    // 스크롤 뷰의 콘텐트 오브젝트가 담긴다.
    // 판매하는 아이템 오브젝트들은 이 오브젝트의 자식으로 들어갈 것이다.

    // 이 NPC 초기화 함수
    public override void Init(int _row, int _col, string _dir, string _text)
    {
        base.Init(_row, _col, _dir, _text);
        // 부모의 Init를 불러옴.

        // 부모의 Init 함수가 virtual 이고, 자식이 override로 Init 함수를
        // 재정의하게 되면 부모에 선언된 Init 내용은 아예 사라지게 때문에
        // 부모 함수의 내용도 갖다 쓰기 위해서 base.Init을 호출함

        prefab = Resources.Load("UI/item") as GameObject; // ui 아이템

        // -------------- 텍스트 파일 로드 시작 ----------------- //
        TextAsset t = Resources.Load("TEXT/npc0_list") as TextAsset;
        // 엔피시 판매 목록 텍스트파일 로드

        StringReader reader = new StringReader(t.text);

        string line;
        string[] arr;

        while (true)
        {
            line = reader.ReadLine();
            if (line == null) break;
            arr = line.Split(','); // apple,30 형태로 불러온 것을 ,로 쪼갬
            sellList.Add(arr[0], int.Parse(arr[1])); // 아이템코드, 가격
        }

        // -------------- 텍스트 파일 로드 끝 ----------------- //

        // -------------- 스크롤 뷰에 UI 아이템들 생성 시작 -------------//
        int index = 0; // 위치값을 설정하기 위한 변수
        foreach (KeyValuePair<string, int> k in sellList)
        {
            GameObject g = Instantiate(prefab) as GameObject;
            g.transform.parent = thisSellContent.transform;
            g.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(-220, 110 + (index * -60), 0);

            g.GetComponent<UnityEngine.UI.Image>().sprite =
                GameManager.GetInstance().imageList[k.Key];
            // 게임매니저에서 아이템 이미지를 가져옴.

            g.transform.Find("itemname").GetComponent<UnityEngine.UI.Text>().
                text = DataBase.GetInstance().itemNameList[k.Key].itemName; // 이름 

            g.transform.Find("price").GetComponent<UnityEngine.UI.Text>().
                text = k.Value.ToString(); // 가격 

            g.transform.Find("Button").GetComponent<UnityEngine.UI.Button>().
                onClick.AddListener(InvenAddItem);

            g.transform.Find("Button").gameObject.name = k.Key;
            // 버튼의 이름을 아이템코드 로 바꾼다.

            index++;
        }
        // ------------- 아이템 생성 끝 -------------------//

        sellObj.SetActive(false);
    }

    public void InvenAddItem()
    {
		// 누른 버튼의 이름을 가져옴. 아까 아이템 코드로 바꿨으니까
		// 버튼을 누를때마다 아이템 코드가 출력되게된다.
		string name = GameManager.GetInstance ().eventSystem.currentSelectedGameObject.name;
		GameManager.GetInstance().heroScript.BuyItem(name, sellList[name]);  

    }

    public override void OpenNpcObject()
    {
        // 자기 판매 창을 열음.
        sellObj.SetActive(true);
    }

	public override void CloseNpcObject(){
		//자기 판매 창을 닫음
		sellObj.SetActive(false);
	}
}
