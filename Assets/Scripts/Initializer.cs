using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Initializer : Singleton<Initializer>
{
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private RectTransform content;
    [SerializeField] private ContentLine prefabLine;
    [SerializeField] private List<ContentLine> contentLines;

    private const int allCountUsers         = 10000;
    private const int countContentLines     = 100;
    private const float inertiaCoef         = -0.025f;
    private const int countNewUsersWave     = 25;
    private const int maxModifierWaveValue  = (allCountUsers - countNewUsersWave) / countNewUsersWave;
    private const string letters            = "abcdefghijklmnopqrstuvwxyz";

    private float scrollHeight;
    private float scrollHeightHalf;

    private int playerContentLinesIndex;
    private int middleLineDown              = 60;
    private int middleLineUp                = 35;
    private int modifierWave                = 0;

    private SortState sortState;

    public enum SortState
    {
        Empty   = 0,
        Score   = 1,
        Lvl     = 2,
    }


    private UserData[] userDatas = new UserData[allCountUsers];
   //private List<UserData> userDatasList = new List<UserData>(allCountUsers);

    private ContentLine lineDown;
    private ContentLine lineUp;
    private UserData localUser;

    public UserData LocalUser => localUser;

    private void Start()
    {
        scrollHeight = scroll.viewport.rect.height;
        scrollHeightHalf = scrollHeight / 2.0f;
        sortState = SortState.Empty;

        InstantiateUsers();
        localUser = userDatas[RandomValue(allCountUsers)];
        localUser.PlayerName("Player");

        modifierWave = (localUser.Position - 50) / countNewUsersWave;
        Dbg.Log("localUser: " + localUser.Position + "mod: " + modifierWave, Color.green);

        InitLines(modifierWave * countNewUsersWave - 1);//localUser.Position - 50);

        playerContentLinesIndex = localUser.Position - (modifierWave * countNewUsersWave - 1);

        middleLineDown      = playerContentLinesIndex       + 5;
        middleLineUp        = playerContentLinesIndex       - 5;
        lineDown            = contentLines[middleLineDown];
        lineUp              = contentLines[middleLineUp];

        StartCoroutine(CanvasUpd());
    }

    private IEnumerator CanvasUpd()
    {
        yield return new WaitForSeconds(0.05f);

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, (contentLines[playerContentLinesIndex].Transform.anchoredPosition.y * -1) + 50.0f - scrollHeightHalf);

        if (lineDown.UserData == null)        lineDown = contentLines[55];
        if (lineUp.UserData == null)          lineUp = contentLines[25];
    }

    #region Init
    private void InitLines(int userIndex = 0)
    {
        for (int i = 0; i < contentLines.Count; i++)
        {
            if (i + userIndex < userDatas.Length && i + userIndex >= 0)
                contentLines[i].Init(userDatas[i + userIndex]);
            else
                contentLines[i].gameObject.SetActive(false);
        }
    }
    private void InstantiateUsers()
    {
        for (int i = 0; i < allCountUsers; i++)
        {
            InitUser(i + 1);
        }
    }
    private void InitUser(int position)
    {
        UserData user = new UserData(position, GenarateName(), RandomValue(999) + 1, RandomValue(int.MaxValue), position);
        userDatas[position - 1] = user;
        //userDatasList.Add(user);
    }
    private int RandomValue(int Max)
    {
        return Random.Range(0, Max);
    }

    private string GenarateName()
    {
        string st = "";
        int countLetterOneName = RandomValue(3) + 3;
        for (int i = 0; i < countLetterOneName; i++)
        {
            st += letters[Random.Range(0, letters.Length)];
        }
        return st;
    }

#endregion

#region scroll
    public void ValueChange()
    {
        LoadNewWave();
    }

    private void LoadNewWave()
    {
        if (scroll.velocity.y > 0 && (content.anchoredPosition.y * -1) < lineDown.Transform.anchoredPosition.y && modifierWave < maxModifierWaveValue)// && !change)
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + contentLines[countNewUsersWave].Transform.anchoredPosition.y + 50.0f
                + (scroll.velocity.y * inertiaCoef));

            if (maxModifierWaveValue > modifierWave) modifierWave++;

            InitLines(countNewUsersWave * modifierWave - 1);

            //Dbg.Log("down: " + lineDown.UserData.Position + " up: " +lineUp.UserData.Position + " en: " + (scroll.velocity.y * 0.01f) /*+ " el: " + contentLines[0].UserData.Position*/, Color.red);
        }
        else if (scroll.velocity.y < 0 && ((content.anchoredPosition.y + scrollHeight) * -1) > lineUp.Transform.anchoredPosition.y && modifierWave > 0)// && !change)
        {        
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y - contentLines[countNewUsersWave].Transform.anchoredPosition.y - 50.0f
                + (scroll.velocity.y * inertiaCoef));

            if (0 <= modifierWave) modifierWave--;

            InitLines(countNewUsersWave * modifierWave - 1);

           // Dbg.Log("up: " + lineUp.UserData.Position + " down: " + lineDown.UserData.Position + " en: " + (scroll.velocity.y * 0.01f) /*+ " el: " + contentLines[0].UserData.Position*/, Color.green);
        }
    }
    #endregion

    #region Sort
    public void SortedScore()
    {
        if (sortState == SortState.Score)  
            return;

        scroll.velocity = Vector2.zero;
        QuickSort<int>.Sort(userDatas, 0, userDatas.Length - 1, true);
        //userDatasList.Sort((x1, x2) => x1.CompareTo(x2, true));

        ReInitUserPlace();
        modifierWave = (localUser.Position - 50) / countNewUsersWave;
        InitLines(countNewUsersWave * modifierWave - 1);

        playerContentLinesIndex = localUser.Position - (modifierWave * countNewUsersWave - 1);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, (contentLines[playerContentLinesIndex].Transform.anchoredPosition.y * -1) + 50.0f - scrollHeightHalf);

        sortState = SortState.Score;
    }
    public void SortedLevel()
    {
        if (sortState == SortState.Lvl) 
            return;

        scroll.velocity = Vector2.zero;
        QuickSort<int>.Sort(userDatas, 0, userDatas.Length - 1, false);
        //userDatas.Sort((x1, x2) => x1.CompareTo(x2, false));

        ReInitUserPlace();
        modifierWave = (localUser.Position - 50) / countNewUsersWave;
        InitLines(countNewUsersWave * modifierWave - 1);

        playerContentLinesIndex = localUser.Position - (modifierWave * countNewUsersWave - 1);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, (contentLines[playerContentLinesIndex].Transform.anchoredPosition.y * -1) + 50.0f - scrollHeightHalf);

        sortState = SortState.Lvl;
    }

    private void ReInitUserPlace()
    {
        for (int i = 0; i < userDatas.Length; i++)
        {
            userDatas[i].ReInitPosition(i + 1);
        }
    }
    #endregion

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}