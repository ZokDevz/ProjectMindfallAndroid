using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Color highlightedColor;

    Fables _fable;
    bool isSelected;
    Color initialTextColor;

    void Start()
    {
        initialTextColor = nameText.color;
    }

    public void SetData(Fables fables)
    {
        _fable = fables;
        Debug.Log("Fable Name: " + _fable.Base.FableName);
        nameText.text = _fable.Base.FableName;
        levelText.text = "Lvl " + _fable.Level;
        hpBar.SetHP((float)_fable.HP / _fable.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            Debug.Log($"{nameText.text} selected");
        else
            Debug.Log($"{nameText.text} deselected");

        if (selected)
            nameText.color = highlightedColor;
        else
            nameText.color = Color.white;
    }
}
