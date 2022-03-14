using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class itemManager : MonoBehaviour {
    List<DataBase.tagItem> itemObjList = new List<DataBase.tagItem>();
    //생성된 아이템들의 목록을 담고 있는 리스트   

    int MAX_RANDOMITEM = 8;//맵에 아이템을 랜덤하게 8개 뿌릴 것이다

    TileMapManager tileMng; //타일 매니저 찾기

    //GameManager gameMng;
	void Start () {
        //gameMng = GameObject.FindObjectOfType<GameManager>(); //게임매니저를 찾아라

        tileMng = GameObject.FindObjectOfType<TileMapManager>();
        //게임오브젝트 중 타일 매니저를 컴포넌트로 가진 애를 찾아서 그 타일 맵매니저를 가져온다.       

        //gameMng.LoadImage(itemNameList);
        //GameManager.GetInstance().LoadImage(itemNameList);//GameManager를 싱글톤으로 만들었기 때문에 이렇게 불러와야 함

        if(tileMng.nowScene == "Tower")
        {
            return;
        }

        ////리스트가 제대로 불러와 졌는지 로그를 띄워 확인해보자
        //for (int i = 0; i < itemNameList.Count; i++)
        //    Debug.Log(itemNameList[i]);

        //List 는 변수명.Count로 안에 몇개의 데이터가 들었는지 확인가능
        //LIST안에 들은 데이터는 변수명[숫자인덱스]로 접근 가능
        //List.Remove(인덱스)로 원하는 인덱스의 데이터도 삭제 가능하다

        //블러온 리스트를 토재로 랜덤하게 아이템을 생성한다
        List<string> tempList = new List<string>();
        foreach(KeyValuePair<string, DataBase.tagItemList> k in DataBase.GetInstance().itemNameList)
        {
            tempList.Add(k.Value.itemCode);
            //아이템 코드들을 임시로 리스트에 집어넣는다
            //이 리스트에서 랜덤한 인덱스로 값을 가져오기 위해서!
        }
        
        for(int i =0; i<MAX_RANDOMITEM; i++)
        {
            //랜덤한 타일을 찾아보자
            tagMyMatrix matrix = tileMng.GetRandomTile();

            int randomItemNum = (int)Random.Range(0, tempList.Count);
            //itemNameList.Count는 총 아이템의 종류의 수
            //랜덤하게 한 아이템을 고른다

            GameObject prefab = Resources.Load("Item/" + tempList[randomItemNum]) as GameObject;
            GameObject g = Instantiate(prefab) as GameObject;

            g.transform.position = new Vector3((matrix.col * Myconst.TILE_COL) + Myconst.START_X, (matrix.row * -Myconst.TILE_ROW) + Myconst.START_Y, 0);
            itemObjList.Add(new DataBase.tagItem(tempList[randomItemNum], matrix.row, matrix.col, g)); //DataBase.GetInstance().itemNameList[randomItemNum] => tempList[randomItemNum]

        }
	}

    public DataBase.tagItem FindItem(int _row, int _col)
    {
        //특정 위치에 아이템이 있는지 확인하고 있으면 그 아이템을 변환해주는 함수

        for(int i = 0; i<itemObjList.Count; i++)
        {
            if(itemObjList[i].row == _row && itemObjList[i].col == _col)
            {
                //검사할 위치에 아이템이 존재하면
                return itemObjList[i];
            }

        }
        //해당 위치에 아이템이 아무것도 없어서 for문을 빠져나온다면?
        return new DataBase.tagItem("_", -1, -1, null);
        //임시로 쓰레기값이 들어가 있는 아이템을 만들어서 반환
    }

    public void DeleteItem(DataBase.tagItem delItem)
    {
        Debug.Log("1. 지금 맵 아이템 리스트에 들어있는 갯수 : " + itemObjList.Count);

        GameObject destrouObj = delItem.itemObj;
        //아이템 게임오브젝트를 저장해놓음
        itemObjList.Remove(delItem); //구조체 리스트에서 해당 구조체 삭제
        //인자로 받은 delItem과 값이 똑같은 구조체를 삭제하게 된다.

        Destroy(destrouObj); //게임 아이템도 씬에서 삭제

        Debug.Log("2, 지금 맵 아이템 리스트에 들어있는 갯수 : " + itemObjList.Count);
    }
}
