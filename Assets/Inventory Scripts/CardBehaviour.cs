using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBehaviour : ScriptableObject {
    public abstract bool canUse();
    public abstract void Activate();
    public abstract void Regenerate();
}
