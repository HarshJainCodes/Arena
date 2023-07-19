using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBossObservePlayerState : IAiBossState
{
    public AiBossStateType GetStateType()
    {
	    return AiBossStateType.ObservePlayer;
    }

    public void Enter(AiBossAgent bossAgent)
    {
	   
    }

    public void Update(AiBossAgent bossAgent)
    {
	    throw new System.NotImplementedException();
    }

    public void Exit(AiBossAgent bossAgent)
    {
	    throw new System.NotImplementedException();
    }
}
