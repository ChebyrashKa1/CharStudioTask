using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContentLine : MonoBehaviour
{
    [SerializeField] private TMP_Text       txtPosition;
    [SerializeField] private Image          icon;
    [SerializeField] private TMP_Text       txtLvl;
    [SerializeField] private RectTransform  rectTr;
    [SerializeField] private Image          line;
    [SerializeField] private Color          defaultColor;

    private UserData userData;

    public UserData UserData => userData;
    public RectTransform Transform => rectTr;

    public void Init(UserData user)
    {
        gameObject.SetActive(true);
        userData = user;
       // string positionString = string.Format("{0,-5:D}", user.Name);
       // string scoreString = string.Format("{0,50:D}", user.Score);

        txtPosition.SetText(user.Position.ToString().PadRight(6) + user.Name);
        icon.sprite = user.Icon;
        txtLvl.SetText(user.Lvl.ToString().PadRight(5) + user.Score.ToString());

        if (GameCore.initializer.LocalUser.Id == user.Id)
            ColorUserLine();
        else
            line.color = defaultColor;
    }

    public void ColorUserLine()
    {
        line.color = Color.red;
    }
}