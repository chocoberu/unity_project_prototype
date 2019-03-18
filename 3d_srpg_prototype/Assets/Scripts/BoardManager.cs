using System.Collections;
using System.Collections.Generic;
using System; // 직렬화 사용을 위한 네임스페이스 선언
using System.Collections.Generic; // List 사용 가능
using Random = UnityEngine.Random; // UnityEngine의 랜덤임을 명시
using UnityEngine;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min,int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public int columns = 20; // 보드판의 행과열
    public int rows = 20;
    public Count wallCount = new Count(5, 9); // 보드판에 생성할 벽의 개수의 최대 최소를 나타내는 Count 객체
    public GameObject[] floorTiles; // 바닥 타일
    public GameObject[] wallTiles; // 벽 타일
    public GameObject[] outerWallTiles; // 외벽 타일

    private Transform boardHolder; // 하이어라키를 정리하기 위한 변수, 오브젝트들을 boardHolder의 자식으로 넣음
    private List<Vector3> gridPositions = new List<Vector3>(); // 보드판의 가능한 모든 위치를 추적하기 위한 변수, 오브젝트가 해당 장소에 있는지 없는지 추적

    void InitializeList()
    {
        gridPositions.Clear(); // gridPositions을 초기화 
        for(int x = 1; x < columns - 1; x++)
        {
            for(int z = 1; z < rows -1; z++)
            {
                gridPositions.Add(new Vector3(x, 0f, z)); // 통과 불가능한 경우를 배제하기위해 7*7 만 랜덤으로 생성
            }
        }
    }
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform; // boardHolder를 새로운 게임오브젝트의 Transform과 동일하게
        GameObject instance;
        for(int x = -1; x < columns + 1; x++)
        {
            for(int z = -1; z < rows + 1; z++) // 외부 벽을 만들기 위해 (-1,-1)과 (8,8)까지 포함
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; // 일반 바닥 타일을 랜덤하게 지정
                if(x == -1 || x == columns || z == -1  || z == rows) // 좌표가 외벽이 위치할 좌표라면
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)]; // 외벽을 지정
                    instance = Instantiate(toInstantiate, new Vector3(x, 0f, z), Quaternion.identity); // 인스턴스화 
                }
                
                instance = Instantiate(toInstantiate, new Vector3(x, -0.5f, z),Quaternion.identity); // 인스턴스화 

                instance.transform.SetParent(boardHolder);
            }
        }
    }
    Vector3 RandomPosition() // 랜덤한 좌표를 반환하는 함수
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }
    void LayoutObjectAtRandom(GameObject[] tileArray,int minimum,int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1); // objectCount를 minimum과 maximum +1 사이의 랜덤한 수로 선택, objectCount는 주어진 오브젝트를 얼마나 스폰할지 조정
        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition(); // randomPosition을 gridPosition의 좌표중 하나로 랜덤 선택
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    public void SetUpScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles,wallCount.minimum,wallCount.maximum);

    }
}
