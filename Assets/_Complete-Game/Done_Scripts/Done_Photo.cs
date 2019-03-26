using UnityEngine;
using System.Collections;

/// <summary>
/// 图像管理
/// </summary>
public class Done_Photo : MonoBehaviour {
    //public int bollocks;
    //砖块的三种状态
    public Sprite unrevealed;
    public Sprite revealed;
    public Sprite flag;
    //地雷
    public Sprite mine;
    //笑脸管理
    public Sprite ooh;
    public Sprite face_ok;
    public Sprite face_ooh;
    public Sprite face_dead;
    public Sprite face_wow;

    private static Done_Photo self;

    public static Done_Photo get() {
        if (self == null) {
            self = (Done_Photo)(FindObjectOfType(typeof(Done_Photo)));
        }
        return self;
    }
}
