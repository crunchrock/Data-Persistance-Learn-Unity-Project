using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
[System.Serializable]
public class TitleManager : MonoBehaviour
{   
    public static TitleManager Instance;
     public GameObject usernameInput;
     public string username;
     public static int BestScore;
     public Text BestName;
     public GameObject enterNamePrompt;
    
    private void Awake() 
    {       
       if (Instance != null)
       {
           Destroy(gameObject);
           return;
       }

       Instance = this;
       DontDestroyOnLoad(gameObject);
       LoadUserName();
    }

    public void StartGame()
    {   
         
        if(username == null)
        {
            enterNamePrompt.SetActive(true);
        }
        else
        {
            SaveUserName();
            
            SceneManager.LoadScene(1);

        }
    }

    class SaveData
   {
       public string newHighName;
       public int newHighScore;
       public int highScore;
       public string username;
    
   }

    public void SaveUserName()
   {
       
       SaveData data = new SaveData();
       data.username = username;


   }

   public void LoadUserName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {   
            Debug.Log("Your save file exists and the name is " + username);
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            username = data.newHighName;
            BestScore = data.newHighScore;

            BestName.text = username + "'s high score of " + BestScore;
            
        }
    }

    public void ReadUsername(string input) 
    {
        username = input;
        
        StartGame();
    }

    
}
