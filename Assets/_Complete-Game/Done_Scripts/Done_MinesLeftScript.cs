using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 左侧地雷数显示管理
/// </summary>
public class Done_MinesLeftScript : MonoBehaviour {
    int minesLeft;
	// Use this for initialization
	void Start () {
	
	}
    /// <summary>
    /// 增加地雷数
    /// </summary>
    public void addMine() {
        minesLeft++;
        updateText();
    }

    /// <summary>
    /// 减少地雷数
    /// </summary>
    public void removeMine() {
        minesLeft--;
        updateText();
    }

    /// <summary>
    /// 显示数字
    /// </summary>
    /// <param name="i"></param>
    public void setMines(int i) {
        minesLeft=i;
        updateText();
    }

    /// <summary>
    /// 更新数字
    /// </summary>
    public void updateText() {
        string s = minesLeft.ToString();
        if (s.Length == 1) s = "0" + s;
        GetComponent<Text>().text = s;
    }

    
    void Update () {
	
	}
}
