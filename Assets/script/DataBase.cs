using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataBase{ 
    //게임 오브젝트의 컴포넌트로 사용하지 않는다면 모노비헤이비어를 상속받지 않는다

    //아이템 이름 목록을 담을 리스트
    //public List<string> itemNameList = new List<string>();
    //리스트 자료구조 : 배열이랑 비슷, 다른 점은 딕셔너리 처럼 추가 삭제가 가능, 단점은 배열보다 느리다

    public Dictionary<string, tagItemList> itemNameList = new Dictionary<string, tagItemList>();

    //게임에서 사용하는 데이터들을 보관하는 클래스
    static DataBase instance = null;

    public static DataBase GetInstance()
    {
        if(instance == null)
        {
            instance = new DataBase(); // GameObject.FindObjectOfType<DataBase>();로 생성할 필요없음
            //이 클래스는 모노비헤이비어를 상속받지 않으므로 그냥 new로 생성함
        }
        return instance;
    }

    public struct tagItem
    {
        public string itemname; //아이템 이름
        public int row, col; //아이템 위치
        public GameObject itemObj; //아이템 게임오브젝트

        public tagItem(string _itemname, int _row, int _col, GameObject _obj)
        {
            itemname = _itemname;
            row = _row;
            col = _col;
            itemObj = _obj;
        }
    }

    //아이템 이름 목록을 담을 리스트
   public struct tagItemList
    {
        public string itemCode; //아이템 코드
        public string itemName; //아이템 이름(한글 이름)
        //나중에 또 추가

        public tagItemList(string _code, string _name)
        {
            itemCode = _code;
            itemName = _name;
        }
    }

    private DataBase() { } //외부에서 데이터베이스를 못 만들게 할려고 private 형의 DataBase 생성자를 선언한다.
    //모노비헤이비어를 상속받지 않는다면 생성자를 선언해야 한다.

    public void SetItemList()
    {
        //아이템 데이터베이스를 불러오는 함수
        TextAsset textasset = Resources.Load("Text/item") as TextAsset;
        StringReader reader = new StringReader(textasset.text);

        string line;
        while (true)
        {
            line = reader.ReadLine(); //한 줄을 읽어 들인다
            if (line == null) break;

            //apple,사과
            string[] arr = line.Split(',');//꼼마를 기준으로 분리하여 배열에 저장

            itemNameList.Add(arr[0], new tagItemList(arr[0], arr[1]));
            //불러온 텍스트를 한 줄 씩 itemNameList에 담는다.
        }
    }
}
