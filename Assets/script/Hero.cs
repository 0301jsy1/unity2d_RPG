using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {

    public int row, col; //캐릭터가 있는 좌표
    
    public Sprite[] sprite_up;
    public Sprite[] sprite_down;
    public Sprite[] sprite_left;
    public Sprite[] sprite_right;

    TileMapManager tileMng;
    npcManager npcMng;
    itemManager itemMng;

   



    void Start () {
        

        transform.position = new Vector3((col * Myconst.TILE_COL) + Myconst.START_X, (row * -Myconst.TILE_ROW) + Myconst.START_Y, 0); //케릭터 위치 초기화
        tileMng = GameObject.FindObjectOfType<TileMapManager>();
        //게임 오브젝트 목록에서 TileMapManager을 컴포넌트로 가지고 있는 것을 찾는다.
        //여러개가 있다면 가장 처음에 찾은 오브젝트로 가져옴

        npcMng = GameObject.FindObjectOfType<npcManager>();

        itemMng = GameObject.FindObjectOfType<itemManager>();

		PlayDataManager.GetInstance().SettingInven();
    }

    bool CheckNpc(int _row, int _col)
    {
        if (npcMng)//npc 매니저가 있으면
        {
            if (!npcMng.CheckNpc(_row, _col))//갈수 있는 타일 체크
                return true; //갈 수 있으면 true 반환
        }
        else //npc 매니저가 없으면 true 반환
            return true;

            //만약에 npc매니저가 있고 갈 수 있는 타일을 체크했는데 그 곳에 가지 못한다면 false가 반환됨
            return false;
    }
    void Update () {
       if(isMoving == false)
        {
            if (Input.GetKey(KeyCode.UpArrow) )
            {
                moveState = "up";
                if (tileMng.CheckMapObject(row - 1, col) && CheckNpc(row -1, col)) //갈 수 있는 곳이다
                {
                    row--;
                    StartCoroutine("MoveAnimation"); //코루틴 함수 작동
                }
                else //못 간다면 바라보기만이라도....
                    MoveRotate();
                //바라보기만 해도 바로 대사창이 켜지는 코드
                if (npcMng)
                    npcMng.Talk(row-1, col );
            }

            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveState = "down";
                if (tileMng.CheckMapObject(row + 1, col) && CheckNpc(row + 1, col))//갈 수 있는 곳이다
                {
                    row++;
                    StartCoroutine("MoveAnimation");
                }
                else //못 간다면 바라보기만이라도....
                    MoveRotate();
                //바라보기만 해도 바로 대사창이 켜지는 코드
                if (npcMng)
                    npcMng.Talk(row+1, col );
            }

            else if (Input.GetKey(KeyCode.LeftArrow) )
            {
                moveState = "left";
                if (tileMng.CheckMapObject(row, col - 1) && CheckNpc(row, col -1))//갈 수 있는 곳이다
                {
                    col--;
                    StartCoroutine("MoveAnimation");
                }
                else //못 간다면 바라보기만이라도....
                    MoveRotate();
                //바라보기만 해도 바로 대사창이 켜지는 코드
                if (npcMng)
                    npcMng.Talk(row, col - 1);
            }

            else if (Input.GetKey(KeyCode.RightArrow) )
            {
                moveState = "right";
                if (tileMng.CheckMapObject(row , col +1) && CheckNpc(row , col + 1)) //갈 수 있는 곳이다
                {
                    col++;
                    StartCoroutine("MoveAnimation"); //코루틴 함수 작동
                }
                else //못 간다면 바라보기만이라도....
                    MoveRotate();
                //바라보기만 해도 바로 대사창이 켜지는 코드
                if (npcMng)
                    npcMng.Talk(row, col + 1);
            }
        }

       if(Input.GetKeyDown(KeyCode.Space) && npcMng)
        {
            switch(moveState)
            {
                //바라보는 방향이
                case "up": npcMng.Talk(row - 1, col); break; //위쪽에  npc가 있다면 talkobj가 켜질 것이다
                case "down": npcMng.Talk(row + 1, col); break;
                case "left": npcMng.Talk(row , col-1); break;
                case "right": npcMng.Talk(row , col+1); break;

            }
        }

        if (Input.GetKeyDown(KeyCode.Comma)) // 컴마 , 를 누르면...?
        {
            // 갖고 있는 아이템의 개수가 최대치보다 작으면
            // 새로운 아이템을 먹을 수 있다.
			if (PlayDataManager.GetInstance().inventory.Count < Myconst.MAX_INVEN)
            {
                // 해당 위치에 아이템이 있는지 파악하고
                if (itemMng) // 아이템 매니저가 있으면
                {
                    DataBase.tagItem tempItem = itemMng.FindItem(row, col);
                    // 아이템매니저에서 캐릭터의 위치에 존재하는 아이템을 찾는다
                    // 만약 캐릭터 위치와 겹치는 아이템이 없다면
                    // 이름이 "-" 인 쓰레기구조체가 들어올것이다.

                    if (tempItem.itemname != "_")
                    {
                        // 아이템이름이 -가 아니면 실제로 아이템이 무엇인가 있는 것임.

						// 아이템을 먹자
						PlayDataManager.GetInstance().AddInven(tempItem.itemname, 1);

                        // 2. 맵에 있는 아이템 오브젝트를 삭제한다.
                        itemMng.DeleteItem(tempItem);


                    }
                }
            }
            else
            {
                //Debug.Log("배가 부릅니다..");
				BGManager.GetInstance().OpenPopup("경고", "배불러!");
            }
        }
    }

	public void BuyItem(string _buyitem, int _price) // 구매한 아이템 코드를 전달받음
	    {
		//내가 가진돈 - 가격이 0보다 크거나 같으면 물건을 살 수 있다. 
		if (PlayDataManager.GetInstance ().GetMoney () - _price >= 0) {
			if (PlayDataManager.GetInstance ().inventory.Count < Myconst.MAX_INVEN) {
				PlayDataManager.GetInstance ().AddInven (_buyitem, 1);
				BGManager.GetInstance ().OpenAlert (DataBase.GetInstance ().itemNameList [_buyitem].itemName +
					"을 샀습니다!" ,1f);
				PlayDataManager.GetInstance ().AddMoney (- _price, GameManager.GetInstance ().InvenRefresh);
			} else {
				BGManager.GetInstance ().OpenAlert ("구매 실패",1f);
			}
		} else {
			BGManager.GetInstance ().OpenAlert ("구매 실패",1f);
		}
	}

	    

    void MoveRotate() // 방향을 바라만 보는 함수
    {
        switch (moveState)
        {
            case "up":
                this.GetComponent<SpriteRenderer>().sprite = sprite_up[1];
                break;
            case "down":
                this.GetComponent<SpriteRenderer>().sprite = sprite_down[1];
                break;
            case "left":
                this.GetComponent<SpriteRenderer>().sprite = sprite_left[1];
                break;
            case "right":
                this.GetComponent<SpriteRenderer>().sprite = sprite_right[1];
                break;
        }
    }
    string moveState = "";
    bool isMoving = false;
    IEnumerator MoveAnimation()
    {
        if (isMoving) yield break;
        isMoving = true;

        int moveindex = 0; // 스프라이트 애니메이션 배열

        while (true)
        {
            switch (moveState)
            {
                case "up":
                    this.GetComponent<SpriteRenderer>().sprite = sprite_up[moveindex];
                    transform.position += new Vector3(0, Myconst.TILE_ROW / 3f, 0);
                    break;
                case "down":
                    this.GetComponent<SpriteRenderer>().sprite = sprite_down[moveindex];
                    transform.position -= new Vector3(0, Myconst.TILE_ROW / 3f, 0);
                    break;
                case "left":
                    this.GetComponent<SpriteRenderer>().sprite = sprite_left[moveindex];
                    transform.position -= new Vector3(Myconst.TILE_COL / 3f, 0, 0);
                    break;
                case "right":
                    this.GetComponent<SpriteRenderer>().sprite = sprite_right[moveindex];
                    transform.position += new Vector3(Myconst.TILE_COL / 3f, 0, 0);
                    break;
            }
            yield return new WaitForSeconds(0.02f);
            moveindex++;

            if (moveindex >= 3)
            {
                transform.position = new Vector3(
                    (col * Myconst.TILE_COL) + Myconst.START_X,
                    (row * -Myconst.TILE_ROW) + Myconst.START_Y, 0);
                // 위치값 보정 (이동값이 0.48/3f 로 실수로 이동하기 때문에
                // 소수점 상실이 있을 수 있음.)

                break;
            }
        }
        isMoving = false;
    }
}
