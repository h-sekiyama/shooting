using UnityEngine;
using System.Collections;

/// サウンド管理
public class Gauge : MonoBehaviour
{

  /// ボスの最大HP
  public static float maxHp;
  public static float nowHp;
  public static float hpPer;
  public GameObject gauge;

  /// 開始
  void Start()
  {
  }

  /// 更新
  void Update()
  {
    hpPer = (nowHp / maxHp) / 5;
    gauge = GameObject.FindWithTag("gauge");
    gauge.transform.localScale = new Vector3(hpPer, 0.1f, 1f);
  }
}