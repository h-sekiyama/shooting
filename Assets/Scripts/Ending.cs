using UnityEngine;
using System.Collections;

/// エンディング管理
public class Ending : MonoBehaviour
{
  private GameObject staffRole;
  private float nowYPos;
  static bool firstFlag = true;

  /// 開始
  void Start()
  {
    staffRole = GameObject.Find("StaffRole");
    nowYPos = staffRole.transform.position.y;
  }

  void Update()
  {
    if(firstFlag == true) {
      Sound.PlayBgm("bgm8");
      firstFlag = false;
    }
    MoveStaffRole();
  }

  void MoveStaffRole() {
    if(staffRole.transform.position.y <= 41.9f) {
      staffRole.transform.position = new Vector3(-2.6f, nowYPos, 0);
      nowYPos += 0.01f;
    } else {
      if (Util.SmartphoneCheck()) {
        //タッチがあるかどうか？
        if (Input.touchCount > 0){
          Touch _touch = Input.GetTouch(0);
          if(_touch.phase == TouchPhase.Began){
            Application.LoadLevel("Title");
            GameMgr.difficulty = 0;
            Sound.StopBgm();
          }
        }
      } else {
        if(Input.GetMouseButtonDown(0)) {
            Application.LoadLevel("Title");
            GameMgr.difficulty = 0;
            Sound.StopBgm();
        }
      }
    }
  }
}
