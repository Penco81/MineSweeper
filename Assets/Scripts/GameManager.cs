using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float StartR;
    public float StartC;
    private const int width = 10;
    public float gap = 1.0f;
    public GameObject tile;
    [HideInInspector]
    public List<TileGrid> tiles;
    [HideInInspector]
    public List<TileGrid> mines;
    [HideInInspector]
    public SpriteManager sm;
    public Image face;
    private int mineLeft = 10;
    [HideInInspector]
    private int revealTiles = 0;
    public Text flagText;
    public Text timeText;
    private float time = 180;
    public bool canClick = true;
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (GameManager)FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public int MineLeft
    {
        get
        {
            return mineLeft;
        }

        set
        {
            mineLeft = value;
            ChangeFlagNum();
        }
    }

    private void Awake()
    {
        sm = GetComponent<SpriteManager>();
    }

    void Start () {
        tiles = new List<TileGrid>();
        mines = new List<TileGrid>();
        //generate tile
		for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                Vector3 v = new Vector3(StartR + j * gap, StartC - i * gap, 0);
                GameObject go = Instantiate(tile, v, Quaternion.identity);
                go.transform.SetParent(this.transform);
                TileGrid tg = go.GetComponent<TileGrid>();
                tg.Set(v, i, j);
                tiles.Add(tg);
            }
        }
        GenerateMine();
	}

    private void Update()
    {
        if (canClick)
        {
            time -= Time.deltaTime;
        }
        if (time <= 0)
        {
            GameOver();
            return;
        }
        timeText.text = ((int)time).ToString();
    }

    private void GenerateMine()
    {
        for (int i = 0; i < 10; i++)
        {
            TileGrid tg = null;
            int row;
            int col;
            do
            {
                int x = Random.Range(0, 99);
                row = x / 10;
                col = x % 10;
                tg = tiles[row * 10 + col];
            } while (tg.isMine == true);
            tg.isMine = true;
            mines.Add(tg);
            List<TileGrid> nearby = GetNearby(row, col);
            foreach (var v in nearby)
            {
                v.mineNum++;
            }
        }
    }

    public List<TileGrid> GetNearby(int row, int col)
    {
        List<TileGrid> nearby = new List<TileGrid>();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                int nr = row + i;
                int nc = col + j;
                if (nr < 0 || nr >= 10 || nc < 0 || nc >= 10)
                    continue;
                nearby.Add(tiles[nr * 10 + nc]);
            }
        }
        return nearby;
    }

    // 0 ok 1 shock 2 dead 3 victory
    public void SetFace(int state)
    {
        switch(state)
        {
            case 0:
                face.sprite = sm.ok;
                break;
            case 1:
                face.sprite = sm.shock;
                break;
            case 2:
                face.sprite = sm.dead;
                break;
            default:
                face.sprite = sm.victory;
                break;
        }
    }

    public void AddRevealTiles()
    {
        revealTiles++;
        IsVector();
    }

    void IsVector()
    {
        if (revealTiles == 90 && mineLeft == 0)
        {
            Victory();
        }
    }

    private void ChangeFlagNum()
    {
        flagText.text = MineLeft.ToString();
        IsVector();
    }

    private void Victory()
    {
        canClick = false;
        SetFace(3);
    }

    public void GameOver()
    {
        canClick = false;
        SetFace(2);
    }

    public void SetOrigin()
    {
        foreach(var v in tiles)
        {
            v.SetOrigin();
        }
        GenerateMine();
        time = 180;
        canClick = true;
        revealTiles = 0;
        MineLeft = 10;
    }
}
