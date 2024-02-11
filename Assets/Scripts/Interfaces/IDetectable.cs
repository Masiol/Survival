using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectable
{
    void DetectRaycast(bool detect);
    void OnTriggered();
}
