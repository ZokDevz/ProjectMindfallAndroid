using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Fables _fables;

    public void SetData(Fables fable)
    {
        _fables = fable; 
        nameText.text = fable.Base.FableName;
        levelText.text = "Lvl " + fable.Level;
        hpBar.SetHP((float)fable.HP / fable.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_fables.HP / _fables.MaxHp);
    }
}
