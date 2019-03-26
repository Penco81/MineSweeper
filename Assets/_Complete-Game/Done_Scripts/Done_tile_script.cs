using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 单个方块的管理
/// </summary>
public class Done_tile_script : MonoBehaviour {

    //方块的三种状态,未显示，显示，标旗
    public enum State {
        Unrevealed, Revealed, Flag
    };
    //方块的初始状态是未显示状态
    State state = State.Unrevealed;

    public bool mine;
    //周围地雷数
    public int nearbyMines;
    public int x, y;

    //导入字体
    public Transform TEXT_PREFAB;
    SpriteRenderer renderPhoto;

    bool mouseOver;
    Text text;
    static Done_tile_script mousedScript;
    static Done_face_controller faceHolder;
    static bool lost;
    //需要展示的无雷方块
    public static int tilesLeftToReveal;
   

    //每一个方块都初始化
    public void Reset() {
        state = State.Unrevealed;
        mine = false;
        mouseOver = false;
        lost = false;
        nearbyMines = 0; 

        if (renderPhoto != null) {
            renderPhoto.sprite = Done_Photo.get().unrevealed;
        }
        
        if (text != null) {
            text.text = "";
        }
    }

    //方块初始化，获取图片和笑脸控制，定义字体位置在方块中心
    void Start() {
        if (faceHolder == null) {
            faceHolder = FindObjectOfType<Done_face_controller>();
        }

        renderPhoto = GetComponent<SpriteRenderer>();
        renderPhoto.sprite = Done_Photo.get().unrevealed;

        //将字体显示在方块的正中间，且需在上方
        Transform textObject = (Transform)(Instantiate(TEXT_PREFAB, new Vector3(transform.position.x+16, transform.position.y+16, -3), Quaternion.identity));
        textObject.SetParent(transform);
        //textObject.parent = transform; 
        text = textObject.GetComponent<Text>();
    }

    /// <summary>
    /// 时时获取当前游戏状态
    /// </summary>
    void Update() {
        if (lost) return;
        if (mouseOver && Input.GetMouseButtonDown(1)) {
            MakeFlag();
        }
    }

    //分两种情况，一是未被点击方块标旗，二是已标旗的方块取消标记，同时左边显示器显示数量变化
    void MakeFlag() {
        print("putflag");

        if (state == State.Unrevealed) {
            FindObjectOfType<Done_MinesLeftScript>().removeMine();
            //将方块状态更改为标旗状态，显示图片
            state = State.Flag;
            renderPhoto.sprite = Done_Photo.get().flag;
        }
        else if(state == State.Flag) {
            FindObjectOfType<Done_MinesLeftScript>().addMine();
            //将方块状态更改为未点击状态，显示图片
            state = State.Unrevealed;
            renderPhoto.sprite = Done_Photo.get().unrevealed;
        }
    }

    /// <summary>
    /// 获取当前方块在二维数组中的位置
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetPosition(int x, int y) {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 设置炸弹，把它周围的符合定义方块上的数字均+1
    /// </summary>
    public void MakeMine() {
        mine = true;
        foreach (Done_tile_script t in GetAdjacentTiles()) {
            t.nearbyMines++;
        }
    }

    /// <summary>
    ///显示函数
    ///如果方块不是未点击状态，则操作无效
    ///执行后为点击状态
    ///点击到地雷，游戏失败
    ///点击一次，记录需要点击的方块数-1，如果需要点击的雷数为零，游戏胜利
    ///点击无地雷，检测nearbymine的值，为零则递归遍历周围的方块，反之显示数字
    /// </summary>
    public void reveal() {
        if (lost || state == State.Revealed || state == State.Flag) return;
        state = State.Revealed;
        FindObjectOfType<Done_Countdown>().StartTimer();

        if (mine) {
            youDead();
            return;
        }
        tilesLeftToReveal--;
        if (tilesLeftToReveal == 0) {
            victory();
        }
        renderPhoto.sprite = Done_Photo.get().revealed;

        if (nearbyMines != 0) {
            text.text = nearbyMines.ToString();
        }
        if (nearbyMines == 0) {
            foreach (Done_tile_script t in GetAdjacentTiles()) {
                t.reveal();
            }
        }
    }

    //胜利，停止计时
    void victory() {
        print("youwin");
        faceHolder.setState(Done_face_controller.face_state.wow);
        FindObjectOfType<Done_Countdown>().PauseTimer();
    }

    /// <summary>
    /// 失败
    /// </summary>
    private void youDead() {
        print("youlose");
        //加载所有地雷
        foreach (Done_tile_script t in GetMines()) {
            t.renderPhoto.sprite = Done_Photo.get().mine;
        }

        //renderPhoto.sprite = Photo.get().mine;
        faceHolder.setState(Done_face_controller.face_state.dead);
        
        FindObjectOfType<Done_Countdown>().PauseTimer();
        lost = true;
    }

    //鼠标监听
    void OnMouseEnter() {
        mouseOver = true;
    }

    void OnMouseExit() {
        mouseOver = false;
    }

   
    void GetMouseButtonDown() {

    }

    //鼠标按下，如果此时已经是输掉的状态则操作无效
    //如果方块未被点击，笑脸变换
    void OnMouseDown()
    {
        if (lost) return;
        mousedScript = this;
        if (state == State.Unrevealed)
        {
            faceHolder.setState(Done_face_controller.face_state.ooh);
            renderPhoto.sprite = Done_Photo.get().ooh;
        }
    }

    //鼠标放开，如果此时鼠标还在原来的方块上，调用显示函数，笑脸变化
    void OnMouseUp() {
        if(state == State.Unrevealed) {
            renderPhoto.sprite = Done_Photo.get().unrevealed;
        }
        faceHolder.setState(Done_face_controller.face_state.ok);
        if (mouseOver) {
            reveal();
        }
    }

   
    /// <summary>
    /// 获取周围方块列表
    /// </summary>
    /// <returns></returns>
    List<Done_tile_script> GetAdjacentTiles() {
        Done_init_script script = FindObjectOfType<Done_init_script>();
        List<Done_tile_script> result = script.GetAdjacentTiles(this);
        return result;
    }
    /// <summary>
    /// 获取所有地雷列表
    /// </summary>
    /// <returns></returns>
    List<Done_tile_script> GetMines()
    {
        Done_init_script script = FindObjectOfType<Done_init_script>();
        List<Done_tile_script> result = script.Mines;
        return result;
    }
}
