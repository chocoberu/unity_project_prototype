using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public BoardManager boardScript; // 
    private int level = 1;
	// Use this for initialization
	void Awake () {
        boardScript = GetComponent<BoardManager>(); // boardScirpt에 BoardManager 컴포넌트들을 레퍼런스로 지정
        InitGame();
	}
	
    void InitGame()
    {
        boardScript.SetUpScene(level);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
