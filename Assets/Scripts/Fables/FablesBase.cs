using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fable", menuName = "Fable/Create new Fable")]
public class FablesBase : ScriptableObject
{
    [SerializeField] string fablename;
    [TextArea][SerializeField] string description;
    [SerializeField] Sprite frontsprite;
    [SerializeField] Sprite backsprite;
    [SerializeField] fableType type1;
    [SerializeField] fableType type2;
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] List<LearnableMove> learnableMoves;

    public string FableName { get { return fablename; } }
    public string Description { get { return description; } }
    public Sprite FrontSpriteName { get { return frontsprite; } }
    public Sprite BackSpriteName { get { return backsprite; } }
    public fableType Type1 { get { return type1; } }
    public fableType Type2 { get { return type2; } }
    public int MaxHp { get { return maxHp; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int SpAttack { get { return spAttack; } }
    public int SpDefense { get { return spDefense; } }
    public int Speed { get { return speed; } }
    public List<LearnableMove> LearnableMoves { get { return learnableMoves; } }
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base { get { return moveBase; } }
    public int Level { get { return level; } }
    public int Count { get; set; }
}

public enum fableType
{
    None,
    Normal,
    Herbivore,
    Producers,
    Decomposers,
    Carnivore,
    Omnivore,
    
}


public class TypeChart
{
    static float[][] chart =
    {
        //                        Nor   Her   Pro  Dec    Car    Omn

       /*Normal*/        new float[] {1f,  1f,   1f,   1f,    1f,    1f},
       /*Herbivore*/     new float[] {1f,  0.5f, 2f,   1f,    0.5f,  0.5f},
       /*Producers*/     new float[] {1f,  0.5f, 0.5f, 2f,    2f,    0.5f},
       /*Decomposers*/   new float[] {1f,  1f,   0.5f, 0.5f,  2f,    2f},
       /*Carnivore*/     new float[] {1f,  2f,   0.5f, 0.5f,  0.5f,  2f},
       /*Omnivore*/      new float[] {1f,  2f,   2f,   0.5f,  1f,    0.5f}
    };

    public static float GetEffectiveness(fableType attackType, fableType defenseType)
    {
        if (attackType == fableType.None || defenseType == fableType.None)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}