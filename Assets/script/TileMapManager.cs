using UnityEngine;
using System.Collections;
using System.IO; //입출력 기능을 사용하겠다

public class TileMapManager : MonoBehaviour {
    int MAX_ROW, MAX_COL; //타일크기
    //const int START_X = -8, START_Y = 5; //시작점, (0,0) : 중간지점

    int[,] bgmap;
    int[,] bgonmap;

    public string nowScene;//현재 내가 있는 씬
    public string bgmap_path;//배경 파일의 이름
    public string bgonmap_path; //배경 오브젝트 파일의 이름

    void Awake()
    {
        LoadTileMap();
        LoadBackObject();
        //실행 순서 결정을 해주자.
        //모든 오브젝트의 Awake 함수를 호출(뭐가 먼저 호출될지는 모름)
        //모든 오브젝트의 Awake 함수가 호출되면 Start 함수를 호출하기 시작(이것도뭐가 먼저 호출될지 모름)
        //각 오브젝트에서 Start 함수가 호출되면 그 다음부터는 계속 Update 함수가 호출됨
    }

    void LoadBackObject()
    {
        TextAsset textAsset = Resources.Load("Text/" + bgonmap_path) as TextAsset;
        //텍스트 파일 관리 자료형 : TextAsset 
        StringReader reader = new StringReader(textAsset.text);
        //StringReader : 읽기를 위한 도구 클래스로 텍스트의 내용을 읽는다.

        string[] arr;
        string line;

        bgonmap = new int[MAX_ROW, MAX_COL];
        int rowindex = 0;
        while(true)
        {
            line = reader.ReadLine();
            if (line == null) break;
            line = line.Replace(" ", ""); //치환 : " " 을 "" 으로 변환
            arr = line.Split(',');

            for (int i = 0; i < MAX_COL; i++)
            {
                //Debug.Log(arr[i]);
                bgonmap[rowindex, i] = int.Parse(arr[i]);

            }
            rowindex++;
        }

        GameObject g, prefab;
        for(int i = 0; i<MAX_ROW; i++)
        {
            for(int j = 0; j <MAX_COL; j++)
            {
                if (bgonmap[i, j] == -1) continue;
                prefab = Resources.Load("Bgon/bgon" + bgonmap[i, j]) as GameObject;
                g = Instantiate(prefab) as GameObject;
                g.transform.position = new Vector3((j * Myconst.TILE_COL) +Myconst. START_X, (i * -Myconst.TILE_ROW) + Myconst.START_Y, 0);
            }
        }
    }
     void LoadTileMap()
    {
        TextAsset textAsset = Resources.Load("Text/" + bgmap_path) as TextAsset;
        //텍스트 파일 관리 자료형 : TextAsset 
        StringReader reader = new StringReader(textAsset.text);
        //StringReader : 읽기를 위한 도구 클래스로 텍스트의 내용을 읽는다.

        string[] arr;
        string line;
        line = reader.ReadLine();//첫 줄 행, 렬(10,10)을 읽어옴
        arr = line.Split(','); //첫 줄을 읽어서 콤마를 기준으로 나누어서
        MAX_ROW = int.Parse(arr[0]); //10을 배열에 담는다
        MAX_COL = int.Parse(arr[1]); //10을 배열에 담는다
        bgmap = new int[MAX_ROW, MAX_COL];

        int rowindex = 0;
        while (true) //  그 다음 줄을 읽기 위해 무한루프
        {
            line = reader.ReadLine();
            if (line == null) break;
            arr = line.Split(',');
            for (int i = 0; i < MAX_COL; i++)
                bgmap[rowindex, i] = int.Parse(arr[i]);
            rowindex++;
        }

        GameObject g, prefab;
        for (int i = 0; i < MAX_ROW; i++)
        {
            for (int j = 0; j < MAX_COL; j++)
            {
                prefab = Resources.Load("Tile/tile" + bgmap[i, j]) as GameObject;
                //Resourecs 폴더내의 Tile 폴더의 tile이라는 파일을 가져오겠다.
                g = Instantiate(prefab) as GameObject;
                g.transform.position = new Vector3((j * Myconst.TILE_COL) + Myconst.START_X, (i * -Myconst.TILE_ROW) + Myconst.START_Y, 0);
                //타일 이미지의 크기는 48 * 48이며 픽셀 퍼 유닛이 100이다. 
                //100으로 나누어 가져오기 때문에 0.48을 곱하는 것이다.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckMapObject(int _row, int _col)
    {
        if(nowScene == "Tower") //타워 씬이면
        {
            if (_row == 4 && _col == 6) //4행 6열을 밟으면
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("ItemMap");
            }
        }

        if (nowScene == "ItemMap") //아이템 맵 씬이면
        {
            if (_row == 5 && _col == 0) //5행 0열을 밟으면
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Tower");
            }
        }


        if ( _row < 0 || _col < 0 || _row > MAX_ROW  -1 ||  _col > MAX_COL -1 ) //(가려고 하는 위치가 맵을 벗어난 범위이면) 지정 크기의 맵을 벗어나면
            return false; //못 간다.
        //행, 열 값을 받아 해당 위치에 오브젝트가 존재하는지 갈 수 있는지의 여부를 반환
        if (bgonmap[ _row, _col ] == -1) //오브젝트가 없으면
        {
            return true; //갈 수 있다.
        }
        return false; //갈 수 없다.
    }

    public tagMyMatrix GetRandomTile()
    {
        int _row, _col;
        while(true) // 아무것도 업슨ㄴ 랜덤한 타일을 찾을 때 까지 무한 루프
        {
            _row = (int)Random.Range(0, MAX_ROW);
            _col = (int)Random.Range(0, MAX_COL);

            if(bgonmap[_row, _col] == -1)
            {
                //랜덤하게 뽑은 행렬에 아무 배경 오브젝트도 없으면
                //그 위치를 태그매트릭스에 담아서 반환
                return new tagMyMatrix(_row, _col);

                //break 가 없더도 return을 만나면 함수가 끝나므로 자동으로 무한 루프도 같이 종료된다.
            }
        }
    }
}
