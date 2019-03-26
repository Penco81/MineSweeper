using UnityEngine;
using System.Collections;

//笑脸操控
public class Done_face_controller : MonoBehaviour {

    //枚举四种笑脸类型
    public enum face_state {
        ok, ooh, dead, wow
    }
    face_state state;
    SpriteRenderer faceSprite;
	// Use this for initialization
	void Start () {
        
	}
    
    //检测当前笑脸状态
    public void setState(face_state state) {
        if(faceSprite == null) {
            faceSprite = GetComponent<SpriteRenderer>();
        }
        if(this.state == face_state.dead) {
            return;
        }
        this.state = state;
        switch (state) {
            case face_state.ok:
                faceSprite.sprite = Done_Photo.get().face_ok;
                break;
            case face_state.ooh:
                faceSprite.sprite = Done_Photo.get().face_ooh;
                break;
            case face_state.dead:
                faceSprite.sprite = Done_Photo.get().face_dead;
                break;
            case face_state.wow:
                faceSprite.sprite = Done_Photo.get().face_wow;
                break;
        }
    }
	
    //重置当前场景
    public void Reset() {
        this.state = face_state.ok;
        setState(face_state.ok);
    }

    //鼠标点击，调用重置函数
    void OnMouseDown() {
        print("clickface");
        FindObjectOfType<Done_init_script>().Reset();
    }
    
	void Update () {
	
	}
}
