using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Fable/Create new Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] new string name; // Use 'new' keyword to hide inherited member
    [TextArea]
    [SerializeField] string description;
    [SerializeField] fableType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public fableType Type { get { return type; } }
    public int Power { get { return power; } }
    public int Accuracy { get { return accuracy; } }
    public int PP { get { return pp; } }

    public bool IsSpecial
    {
        get
        {
            if (type == fableType.Decomposers || type == fableType.Carnivore || type == fableType.Omnivore)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void UpdatePP(int newPP)
    {
        pp = newPP;
    }
}
