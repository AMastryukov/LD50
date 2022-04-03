using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityCode;

public class GameEventSystem : UnitySingletonPersistent<GameEventSystem>
{
    public Action OnTimerStart {get; set;}
    public Action<int> OnEvidenceInspected {get; set;}
    public Action OnTimerEnd {get; set;}
}
