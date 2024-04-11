using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Fables> wildFables;

    public Fables GetRandomWildFables()
    {
        var wildFable =  wildFables[Random.Range(0, wildFables.Count)];
        wildFable.Init();
        return wildFable;
    }
}
