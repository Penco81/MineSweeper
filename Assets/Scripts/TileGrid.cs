using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileGrid : MonoBehaviour {

    enum State
    {
        REVEAL,
        UNREVEAL,
        FLAG
    }
    public Vector2 pos;
    public int row;
    public int col;
    State state = State.UNREVEAL;
    public int mineNum;
    public bool isMine;
    private SpriteRenderer sr;
    private bool isMouseOver;
    private Text text;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        text = transform.Find("Canvas/Text").GetComponent<Text>();
        text.text = "";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && isMouseOver)
        {
            if (GameManager.Instance.canClick == false)
                return;
            if (state != State.REVEAL)
            {
                if (state == State.UNREVEAL && GameManager.Instance.MineLeft > 0)
                {
                    sr.sprite = GameManager.Instance.sm.flag;
                    GameManager.Instance.MineLeft--;
                    state = State.FLAG;
                }
                else if (state == State.FLAG)
                {
                    sr.sprite = GameManager.Instance.sm.unreveal;
                    GameManager.Instance.MineLeft++;
                    state = State.UNREVEAL;
                }
            }
        }
    }

    public void SetOrigin()
    {
        state = State.UNREVEAL;
        mineNum = 0;
        isMine = false;
        if (text != null)
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }
        if(sr != null)
        {
            sr.sprite = GameManager.Instance.sm.unreveal;
        }
    }

    public void Set(Vector2 pos, int row, int col)
    {
        this.pos = pos;
        this.row = row;
        this.col = col;
        SetOrigin();
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void OnMouseDown()
    {
        if (state == State.FLAG || state == State.REVEAL)
            return;
        if (GameManager.Instance.canClick == false)
            return;
        GameManager.Instance.SetFace(1);
    }

    private void OnMouseUp()
    {
        if (state == State.FLAG || state == State.REVEAL)
            return;
        if (GameManager.Instance.canClick == false)
            return;
        GameManager.Instance.SetFace(0);
        if (!isMouseOver)
        {
            return;
        }
        if (isMine)
        {
            sr.sprite = GameManager.Instance.sm.mine;
            //game over TODO
            GameManager.Instance.GameOver();
        }
        else
        {
            Reveal();
        }
    }

    void Reveal()
    {
        if (state == State.REVEAL)
            return;
        state = State.REVEAL;
        sr.sprite = GameManager.Instance.sm.reveal;
        GameManager.Instance.AddRevealTiles();
        if (mineNum != 0)
        {
            ShowText();
        }
        else
        {
            List<TileGrid> n = GameManager.Instance.GetNearby(row, col);
            foreach (var v in n)
            {
                v.Reveal();
            }
        }
    }

    void ShowText()
    {
        if (mineNum == 0)
            return;
        text.gameObject.SetActive(true);
        text.text = mineNum.ToString();
    }

}
