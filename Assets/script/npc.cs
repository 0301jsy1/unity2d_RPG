using UnityEngine;
using System.Collections;

public class npc : MonoBehaviour {

    public Sprite sprite_up, sprite_left, sprite_right, sprite_down;
    //각 방향 별 이미지 1번을 담을 변수

    public string talk; //npc가 가지고 있는 대사

    //Init  함수는 생성될 때 불리고
    //인자로 행, 열, 바라보는 방향을 받아서
    //해당 행렬로 npc의 위치를 고정하고 방향대로 이미지를 출력
    public virtual void Init(int _row, int _col, string _dir, string _text)//차례대로 행, 열,, 바라보는 방향,  대사
    {
        talk = _text;
        //대사 담기
        transform.position = new Vector3((_col * Myconst.TILE_COL) + Myconst.START_X, (_row * -Myconst.TILE_ROW) + Myconst.START_Y, 0);
        //열(x) * 0.48, 행(y) * 0.48, z값(깊이)

       
        switch (_dir)
        {
            //바라보는 방향 대로 이미지를 변경함
            case "up": GetComponent<SpriteRenderer>().sprite = sprite_up; break;
            case "down": GetComponent<SpriteRenderer>().sprite = sprite_down; break;
            case "left": GetComponent<SpriteRenderer>().sprite = sprite_left; break;
            case "right": GetComponent<SpriteRenderer>().sprite = sprite_right; break;
        }
    }

    // 자식에서 재정의 할 함수
    // 각자 가지고 있는 어떤 창을 여는 함수이다.
    // 자식이 어떤 창을 갖고 있을지는 자식만 알고 있음. 판매창이든, 퀘스트창이든..
    public virtual void OpenNpcObject() { }
	public virtual void CloseNpcObject() { } //OpenNpcObject 에서 연 창을 닫는 내용

}
