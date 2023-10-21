using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Serialization;
/* 
* class to manage game levels, win/lose conditions, and core game mechanics
*/
public class GameManager : MonoBehaviour
{
    private LevelManager levelManager;
    public MonoBehaviour playerMove;

    // The list of game objects that the player needs to capture
    public List<GameObject> animalsToCapture;

    public TextMeshProUGUI checklistUI;
    public TextMeshProUGUI timerUI;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject hudUI;
    
    public AudioSource backgroundMusicSource; 
    public AudioClip[] levelMusicTracks;

    // savannah animals
    public GameObject zebraPrefab;
    public GameObject zebraFezPrefab;
    public GameObject zebraBabyPrefab;
    public GameObject elephantPrefab;
    public GameObject elephantBabyPrefab;
    public GameObject elephantBlackTopHatPrefab;
    public GameObject elephantWhiteTopHatPrefab;
    public GameObject elephantCowboyPrefab;
    public GameObject elephantFezPrefab;
    public GameObject hippoConePrefab;
    public GameObject hippoSmokingPrefab;
    public GameObject ostrichBabyPrefab;
    public GameObject ostrichHardHatPrefab;
    public GameObject ostrichYellowBowTiePrefab;


    // arctic animals
    public GameObject penguinPrefab;
    public GameObject penguinBowTiePrefab;
    public GameObject reindeerPrefab;
    public GameObject reindeerBabyPrefab;
    public GameObject polarbearPrefab;
    public GameObject babyWalrusPrefab;
    public GameObject walrusCowboyPrefab;
    public GameObject walrusSmokingPrefab;

    // forest animals
    public GameObject alphaWolfPrefab;
    public GameObject beaverBlueHatPrefab;
    public GameObject beaverWhiteHatPrefab;
    public GameObject beaverYellowHatPrefab;
    public GameObject omegaWolfPrefab;
    public GameObject snakeFezPrefab;
    public GameObject snakeStetsonPrefab;
    public GameObject eagleGoldenPrefab;

    public MonoBehaviour fellTree;

    float timeRemaining;


    void Start()
    {
        levelManager = GetComponent<LevelManager>();

    
        InitializeObjectsToCapture(levelManager.currentLevel);
        PlayLevelMusic();

        // Add a switch case statement to increase difficulty as player progresses
        switch (levelManager.currentLevel)
        {
            case Level.Savannah:
                timeRemaining = 150f;
                break;
            case Level.Arctic:
                timeRemaining = 120f;
                break;
            case Level.Forest:
                timeRemaining = 90f;
                break;
            case Level.MainMenu:
                timeRemaining = 999f;
                break;
            default:
                timeRemaining = 120f;
                break;
        }


    }


    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            ShowGameOver();
        }

        //trigger the treefalling event for a dynamic forest level
        //random range for some variance
        if (levelManager.currentLevel == Level.Forest && timeRemaining < Random.Range(40.0f, 60.0f))
        {
            fellTree.enabled = true;
        }

        string timeString = ((int)timeRemaining).ToString();
        timerUI.text = timeString;
        
        levelManager.requiredAnimals = animalsToCapture;

        
        int counter = 0; //count for no. of animals to be captured 
        string checklist = "";
        foreach (GameObject animal in animalsToCapture)
        {
            checklist += animal.name + "\n";
        }

        if(levelManager.currentScene != Level.MainMenu.ToString())
        {
            // set the checklistUI text to the checklist string
            checklistUI.text = checklist;
        }


        // WIN CONDITION
        if (counter == animalsToCapture.Count)
        {
            //Debug.Log("Level Complete");
            ShowWin();
        }
    }
    
    void PlayLevelMusic()
    {
        if (levelManager.currentLevel == Level.Savannah)
        {
            backgroundMusicSource.clip = levelMusicTracks[0];
        }
        else if (levelManager.currentLevel == Level.Arctic)
        {
            backgroundMusicSource.clip = levelMusicTracks[1];
        }
        else if (levelManager.currentLevel == Level.Forest)
        {
            backgroundMusicSource.clip = levelMusicTracks[2];
        }
        backgroundMusicSource.Play();
    }
    
    public void MarkAnimalCaptured(GameObject animal)
    {
        animalsToCapture.Remove(animal);
        Debug.Log("Captured: " + animal.name);

        if (animalsToCapture.Count == 0)
        {
            // all animals captured, show win condition
            ShowWin();
        }
    }

    void InitializeObjectsToCapture(Level level)
    {
        List<GameObject> savannahList = new List<GameObject>
        {
            zebraPrefab,
            zebraFezPrefab,
            zebraBabyPrefab,
            elephantPrefab,
            elephantBabyPrefab,
            elephantBlackTopHatPrefab,
            elephantWhiteTopHatPrefab,
            elephantFezPrefab,
            elephantCowboyPrefab,
            hippoConePrefab,
            hippoSmokingPrefab,
            ostrichBabyPrefab,
            ostrichHardHatPrefab,
            ostrichYellowBowTiePrefab,
        };

        List<GameObject> arcticList = new List<GameObject>
        {
            penguinPrefab,
            penguinBowTiePrefab,
            reindeerPrefab,
            reindeerBabyPrefab,
            polarbearPrefab,
            babyWalrusPrefab,
            walrusCowboyPrefab,
            walrusSmokingPrefab,
        };

        List<GameObject> forestList = new List<GameObject>
        {
            alphaWolfPrefab,
            eagleGoldenPrefab,
            omegaWolfPrefab,
            snakeFezPrefab,
            snakeStetsonPrefab,
        };

        if (level == Level.Savannah)
        {
            // shuffle the list of animal prefabs
            savannahList = savannahList.OrderBy(x => Random.value).ToList();

            // select the first x animal prefabs from the shuffled list
            animalsToCapture = savannahList.Take(3).ToList();
        }
        else if (level == Level.Arctic)
        {
            arcticList = arcticList.OrderBy(x => Random.value).ToList();
            animalsToCapture = arcticList.Take(3).ToList();
        }
        else if (level == Level.Forest)
        { 
            forestList = forestList.OrderBy(x => Random.value).ToList();
            animalsToCapture = forestList.Take(4).ToList();
        }
    }
    
    void ShowWin()
    {
        winUI.SetActive(true);
        hudUI.SetActive(false);
        playerMove.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void ShowGameOver()
    {
        loseUI.SetActive(true);
        hudUI.SetActive(false);
        playerMove.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
