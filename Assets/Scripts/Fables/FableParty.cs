using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FableParty : MonoBehaviour
{
    [SerializeField] List<Fables> fables;

    public List<Fables> Fables
    {
        get { return fables; }
    }

    private void Start()
    {
        foreach (var fable in fables)
        {
            fable.Init();
        }
    }

    public void SwitchFable(int index, Fables newFable)
    {
        if (index >= 0 && index < fables.Count)
        {
            fables[index] = newFable;
        }
        else
        {
            Debug.LogError("Invalid fable index for switching.");
        }
    }
    public Fables GetHealthyFable()
    {
        return fables.FirstOrDefault(x => x.HP > 0);
    }
}
