
using UnityEngine;

public enum BossState
{
    None,
    Idle,
    Observe,
    Chase,
    Attack,
    Jump,
    Death,
    Spawn,
    Dash
}
public abstract class BossStateInterface : MonoBehaviour
{
    abstract public void enter(BossMain bossAgent);
    abstract public void update(BossMain bossAgent);
    abstract public void exit(BossMain bossAgent);


}
