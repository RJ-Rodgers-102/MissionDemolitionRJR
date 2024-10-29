using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S;
    [Header("Inscribed")]
    public TMPro.TMP_Text uitLevel;
    public TMPro.TMP_Text uitShots;
    public Vector3 castlepos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotstaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    void Start()
    {
        S = this;

        level = 0;
        shotstaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel(){
        mode = GameMode.playing;
        if (castle != null){
            Destroy(castle);
        }
        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlepos;
        Goal.goalMet = false;

        UpdateGUI();
        
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }
    void UpdateGUI(){
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotstaken;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet){
            mode = GameMode.levelEnd;
            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel(){
        level++;
        if (level == levelMax){
            level = 0;
            shotstaken = 0;
        }
        StartLevel();
    }
    static public void SHOT_FIRED(){
        S.shotstaken++;
    }
    static public GameObject GET_CASTLE(){
        return S.castle;
    }
}
