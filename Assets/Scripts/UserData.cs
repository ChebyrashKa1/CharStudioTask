using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData //: System.IComparable
{
    private int         position;
    private string      name;
    private int         lvl;
    private Sprite      icon;
    private int         score;
    private int         id;

    public int Position     => position; 
    public string Name      => name; 
    public int Lvl          => lvl; 
    public int Score        => score;
    public Sprite Icon      => icon;
    public int Id           => id;

    public UserData(int newPosition, string newName, int newLvl, int newScore, int newID)
    {
        position    = newPosition;
        name        = newName;
        lvl         = newLvl;
        icon        = GameCore.icons.GetUserIcon();
        score       = newScore;
        id          = newID;
    }

    public int CompareTo(object obj, bool scoreCheck)
    {
        if (scoreCheck)
        {
            if (score == (obj as UserData).score)
                return (obj as UserData).lvl.CompareTo(lvl);
            return (obj as UserData).score.CompareTo(score);
        }
        else
        {
            if (lvl == (obj as UserData).lvl)
                return (obj as UserData).score.CompareTo(score);
            return (obj as UserData).lvl.CompareTo(lvl);
        }
    }

    public void ReInitPosition(int newPos)
    {
        position = newPos;
    }
    public void PlayerName(string newName)
    {
        name = newName;
    }
}