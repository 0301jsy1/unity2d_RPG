using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Npc1 : npc
{
    //아이템을 주인공으로부터 구매해 주는 엔피시
    GameObject prefab; //아이템 ui의 프리팹이 담김
    public GameObject buyObj; //판매 목록 스크롤 뷰
    public GameObject Content; //아이템의 부모가 될 콘텐트 오브젝트

    //npc 초기화 함수
    public override void Init(int _row, int _col, string _dir, string _text)
    {
        base.Init(_row, _col, _dir, _text);

        prefab = Resources.Load("UI/item") as GameObject; // ui 아이템
        buyObj.SetActive(false);//구매 스크롤뷰 비활성화
    }

    public override void OpenNpcObject()
    {
        Debug.Log("open ");

        foreach (Transform t in Content.transform)
        {
            //Content 안에 있는 모든 자식들을 죽인다
            Destroy(t.gameObject);
        }
        Debug.Log("open ");

        int index = 0;

        //창을 열면, 주인공의 인벤토리 내용대로 아이템들이 생긴다
        foreach (PlayDataManager.tagHeroInventory k in PlayDataManager.GetInstance().inventory)
            {
                GameObject g = Instantiate(prefab) as GameObject;
                g.transform.parent = Content.transform;
                g.GetComponent<RectTransform>().anchoredPosition = new Vector3(-220, 110 + (index * -60), 0);
                g.GetComponent<UnityEngine.UI.Image>().sprite = GameManager.GetInstance().imageList[k.itemName]; //이미지
                g.transform.Find("itemname").GetComponent<UnityEngine.UI.Text>().text = DataBase.GetInstance().itemNameList[k.itemName].itemName;
                //아이템의 한글 이름
                g.transform.Find("price").GetComponent<UnityEngine.UI.Text>().text = k.itemCount + "개 있음 / 100원"; 
                g.transform.Find("Button").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(BuyHeroItem);
                //버튼을 누르면 아이템을 사는 함수 호출
                g.transform.Find("Button").gameObject.name = k.itemName;
                //버튼 오브젝트의 이름을 아이템 코드로 변경
                
                index++;
        }
        buyObj.SetActive(true);
        Debug.Log("open ");
    }

    void BuyHeroItem()
    {
        //내가 누른 아이템이 무엇인지 출력해보자
        string name = GameManager.GetInstance().eventSystem.currentSelectedGameObject.name;
        //주인공의 인벤토리에서 아이템을 한 개 삭제한다
        PlayDataManager.GetInstance().DeleteInven(name);
        //주인공의 소지금을 증가시킨다.
        PlayDataManager.GetInstance().AddMoney(100,GameManager.GetInstance().InvenRefresh);

        OpenNpcObject();
    }

    public  override void CloseNpcObject()
    {
        Debug.Log("close ");
        buyObj.SetActive(false);
    }
}
