using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DetectableObserver : MonoBehaviour
{
    private List<IDetectable> detectables = new List<IDetectable>();

    public void AddDetectable(IDetectable _detectable)
    {
        if(!detectables.Contains(_detectable))
        {
            detectables.Add(_detectable);
        }
    }
    public void RemoveDetectable(IDetectable _detectable)
    {
        if (detectables.Contains(_detectable))
        {
            detectables.Remove(_detectable);
        }
    }

    public void RaiseDetectable()
    {
        foreach (var detectable in detectables)
        {
            detectable.OnTriggered();
        }
    }
}
