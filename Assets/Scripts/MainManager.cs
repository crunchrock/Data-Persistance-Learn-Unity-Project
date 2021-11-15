using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text NameText;
    
    public int HighScore;
    public string HighName;
    public Text HighScoreName;
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public string playerName; 

    
    // Start is called before the first frame update
    void Start()
    {   LoadHighScore();
        playerName = TitleManager.Instance.username;
        Debug.Log("The game has started and the users name is " + playerName);
        NameText.text = playerName;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }
[System.Serializable]
class High
{
    public int newHighScore;
    public string newHighName;
}
    public void GameOver()
    {
        SaveHighScore();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    public void SaveHighScore()
    {
        if(m_Points > HighScore){
            HighScore = m_Points;
            High data = new High();
            data.newHighScore = m_Points;
            data.newHighName = playerName;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
            Debug.Log("this was written" + json );
        } 
    }
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {   
            string json = File.ReadAllText(path);
            High data = JsonUtility.FromJson<High>(json);

            HighScore = data.newHighScore;
            HighName = data.newHighName;
            HighScoreName.text = "High Score: "+ HighName + " - " + HighScore;
        }
    }
}
