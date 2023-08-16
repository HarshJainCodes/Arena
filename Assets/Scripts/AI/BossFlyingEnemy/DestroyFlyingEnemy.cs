using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFlyingEnemy : MonoBehaviour
{
    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
