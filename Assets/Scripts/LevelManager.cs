using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level
{
    MainMenu,
    Savannah,
    Arctic,
    Forest
}

public class LevelManager : MonoBehaviour
{

    public Level currentLevel;
    public List<GameObject> requiredAnimals { get; set; }
    public string currentScene;



public void SetLevel(Level level)
    {
        currentLevel = level;

        // Load the appropriate scene based on the selected level
        switch (currentLevel)
        {
            case Level.MainMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case Level.Savannah:
                SceneManager.LoadScene("Savannah");
                break;
            case Level.Arctic:

                SceneManager.LoadScene("Arctic");
                break;
            case Level.Forest:
                SceneManager.LoadScene("Forest");
                break;
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Savannah")
        {
            currentLevel = Level.Savannah;
        }
        else if (currentScene == "Arctic")
        {
            currentLevel = Level.Arctic;
        }
    }

}
