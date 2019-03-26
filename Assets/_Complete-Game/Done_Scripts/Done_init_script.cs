using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 场景初始化
/// </summary>
public class Done_init_script : MonoBehaviour {

    //定义方块，方块大小，以及雷区的行列数
    public Transform tile;
    int tileSize = 32;
    const int tilesAcross = 10;
    const int tilesDown = 10;
   
    //建立二维数组，以便管理雷区中的方块
    public Done_tile_script[,] grid = new Done_tile_script[tilesAcross, tilesDown];
    //记录地雷列表
    public List<Done_tile_script> Mines = new List<Done_tile_script>();



    /// <summary>
    /// 布置雷区，实例化方块，并将每一个建立的方块添加到二位数组中
    /// </summary>
    void Start() {
        print("start");

        //数据清理
        PlayerPrefs.DeleteAll();
        
       
        for (int y = 0; y < tilesAcross; y++) {
            for (int x = 0; x < tilesDown; x++) {
                Transform newTile = (Transform)(Instantiate(tile, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity));
                grid[x, y] = newTile.GetComponent<Done_tile_script>();
                grid[x, y].SetPosition(x, y);
                newTile.SetParent(transform);
                //newTile.parent = transform;
            }
        }
        print("over");
        Reset();
    }

    /// <summary>
    /// 给数组中的每一个方块都进行初始化，遍历方块，在没有炸弹的方块上随机放置炸弹
    /// </summary>
    public void Reset() {
        print("reset");
        for (int y = 0; y < tilesAcross; y++) {
            for (int x = 0; x < tilesDown; x++) {
                //对数组中的每一个方块都进行初始化
                grid[x, y].Reset();
            }
        }
        //设置十个地雷数，一个暂时用于存放所有地雷的列表
        int numMines = 10;
        List<Done_tile_script> mines = new List<Done_tile_script>();

        //随机选择二维数组中的方块，判断此处是否有地雷，有则重新选择，没有则继续执行接下来的程序
        for (int i = 0; i < numMines; i++) {
            print("putmines");
            Done_tile_script tile;
            do {
                tile = grid[Random.Range(0, tilesAcross), Random.Range(0, tilesDown)];
            }
            while (tile.mine);
            //添加地雷
            tile.MakeMine();
            //将方块载入列表中
            mines.Add(tile);
            //将暂存地雷的列表存入全局变量中，用于游戏失败时地雷的全部显示
            Mines = mines;
        }

        Done_tile_script.tilesLeftToReveal = tilesAcross * tilesDown - numMines; 

        FindObjectOfType<Done_MinesLeftScript>().setMines(numMines);
        FindObjectOfType<Done_Countdown>().reset();
        FindObjectOfType<Done_face_controller>().Reset();
    }

    /// <summary>
    /// 遍历周围的以点击方块为中心的3*3数组，将在雷区内的方块存入result列表中
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public List<Done_tile_script> GetAdjacentTiles(Done_tile_script origin) {
        //被点击的方块所处于整个雷区这个二维数组的位置
        int startX = origin.x;
        int startY = origin.y;
        //新建一个结果列表用于return周围的具体方块
        List<Done_tile_script> result = new List<Done_tile_script>();
        //对周围八个方块进行遍历，如果被点击的方块处于边缘，则只需遍历五个方块
        //如果处于角落，则只需遍历三个方块
        for (int dx = -1; dx <= 1; dx++) {
            int newX = startX + dx;
            if (newX < 0 || newX >= tilesAcross) {
                continue;
            }

            for (int dy = -1; dy <= 1; dy++) {
                int newY = startY + dy;
                if (newY < 0 || newY >= tilesDown) {
                    continue;
                }
                if (dx == 0 && dy == 0) {
                    continue;
                }

                result.Add(grid[newX, newY]);
            }
        }

        return result;
    }


    void Update() {

    }
}
