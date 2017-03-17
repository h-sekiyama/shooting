using UnityEngine;
using System.Collections;

/// サウンド管理
public class SoundMgr : MonoBehaviour
{
  /// 開始
  void Start()
  {
    // サウンドをロード
    // "bgm01"をロード。キーは"bgm"とする
    Sound.LoadBgm("bgm1", "bgm01");
    // "damage"をロード。キーは"damage"とする
    Sound.LoadSe("damage", "damage");
    // "destroy"をロード。キーは"destroy"とする
    Sound.LoadSe("destroy", "destroy");
    Sound.LoadBgm("bgm2", "bgm02");
    Sound.LoadBgm("bgm3", "bgm03");
    Sound.LoadBgm("bgm4", "bgm04");
    Sound.LoadBgm("bgm5", "bgm05");
    Sound.LoadBgm("bgm6", "bgm06");
    Sound.LoadBgm("bgm7", "bgm07");
    Sound.LoadBgm("bgm8", "bgm08");
  }
}
