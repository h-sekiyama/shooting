using UnityEngine;
using System.Collections;

/// タイトル画面管理
public class TitleMgr : MonoBehaviour
{
  /// Press to startの表示フラグ
  bool _bDrawPressStart = false;

  IEnumerator Start()
  {
    while (true)
    {
      // 0.6秒で点滅する
      _bDrawPressStart = !_bDrawPressStart;
      yield return new WaitForSeconds(0.6f);
    }
  }

  void Update()
  {
    if (Util.SmartphoneCheck()) {
      //タッチがあるかどうか？
      if (Input.touchCount > 0){
        Touch _touch = Input.GetTouch(0);
        if(_touch.phase == TouchPhase.Began){
          Application.LoadLevel("Main");
        }
      }
    } else {
      if(Input.GetMouseButtonDown(0)) {
        Application.LoadLevel("Main");
      }
    }
  }

  void OnGUI()
  {
    if (_bDrawPressStart)
    {
      // ゲーム開始メッセージの描画
      // フォントサイズ設定
      Util.SetFontSize(32);
      // 中央揃え
      Util.SetFontAlignment(TextAnchor.LowerCenter);
      Util.SetFontColor(Color.white);

      // フォントの位置
      float w = 64; // 幅
      float h = 16; // 高さ
      float px = Screen.width / 2 - w / 2;
      float py = Screen.height - h - 10;

      // フォント描画
      Util.GUILabel(px, py, w, h, "画面タップでゲーム開始");
    }
  }
}
