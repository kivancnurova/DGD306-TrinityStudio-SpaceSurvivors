using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    void Initialize(Transform self);
    void Tick();
}

