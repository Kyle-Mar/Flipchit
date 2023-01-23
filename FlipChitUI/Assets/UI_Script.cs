using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour
{
    public GameObject modeScreen, difficultyScreen, profileScreen;
    

    enum MODE { LOCAL, ONLINE, CPU}
    enum SCREENS { MAIN, DIFFICULTY, PROFILE}
    enum DIFFICULTY { EASY, NORMAL, EXPERT}

    SCREENS curScreen;   // utility to quickly check what screen we're on

    // Start is called before the first frame update
    void Start()
    {
        // more just a sanity check then anything
        modeScreen.SetActive(true);
        difficultyScreen.SetActive(false);
        profileScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (curScreen == SCREENS.MAIN)
                {
                    Application.Quit();
                }

                else
                {
                    change_Screen(SCREENS.DIFFICULTY);
                }
            }
        }
    }

    public void open_Website()
    {
        Application.OpenURL("https://www.playflipchit.com/");
    }

    // function that will eventually be used to handle which mode to load into
    public void Play_Game(string gameMode)
    {
        change_Screen(SCREENS.DIFFICULTY);
    }

    public void difficulty_select(string difficulty)
    {
        ;
    }

    public void view_profile()
    {
        change_Screen(SCREENS.PROFILE);
    }

    public void back_button()
    {
        change_Screen(SCREENS.MAIN);
    }

    //helper function to handle changing view
    void change_Screen(SCREENS screen)
    {
        switch(screen)
        {
            case SCREENS.MAIN:
                modeScreen.SetActive(true);
                difficultyScreen.SetActive(false);
                profileScreen.SetActive(false);
                curScreen = screen;
                break;

            case SCREENS.DIFFICULTY:
                modeScreen.SetActive(false);
                difficultyScreen.SetActive(true);
                profileScreen.SetActive(false);
                curScreen = screen;
                break;

            case SCREENS.PROFILE:
                modeScreen.SetActive(false);
                difficultyScreen.SetActive(false);
                profileScreen.SetActive(false);
                curScreen = screen;
                break;
        }
            
    }
}
