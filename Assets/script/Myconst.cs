using UnityEngine;
using System.Collections;

public class Myconst {

    public const float TILE_ROW = 0.48f; //행, X값으로 증감
    public const float TILE_COL = 0.48f; //열, Y값으로 증감
    //const로 지정하면 자동으로 static(메모리에 항상 상주)의 상수 형식이 됨

    public static readonly int START_X = -8;
    public static readonly int START_Y = 5;
    //readonly는 읽기 전용
    //static 으로 선언하면 전역이 되기 때문에 Myconst.START_X 처럼 접근 가능

    public const int MAX_INVEN = 10; //최대 인벤토리 개수
}

public struct tagMyMatrix
{
    public int row, col;
    public tagMyMatrix(int _row, int _col)
    {
        row = _row;
        col = _col;
    }
    //이 구조체는 단순히 행과 열을 담기 위해 만든 구조체임
}
