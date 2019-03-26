using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//时间设置
public class Done_Countdown : MonoBehaviour {

    //暂停
    Text text;
    bool paused = true;

    float time;

    void Start() {
        
    }

    void Update() {
        if (paused) return;
        time += Time.deltaTime;
        UpdateText();
    }
    /// <summary>
    /// 更新时间
    /// </summary>
    void UpdateText() {
        text = GetComponent<Text>();
        text.text = ((int)time).ToString();
        while (text.text.Length < 3) text.text = "0" + text.text;
    }
    /// <summary>
    /// 终止计时
    /// </summary>
    public void PauseTimer() {
        paused = true;
    }
    /// <summary>
    /// 开始计时
    /// </summary>
    public void StartTimer() {
        paused = false;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reset() {
        time = 0;
        UpdateText();
    }
}
