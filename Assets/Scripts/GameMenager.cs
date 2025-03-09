using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameMenager : MonoBehaviour
{
    #region Turrets
    [SerializeField] GameObject[] turretsPrefab;
    [SerializeField] int numberOfTurrets = 5;
    private GameObject[] _turrets;

    [SerializeField][Range(0.1f, 50f)] private float minDistanceX = 10;
    [SerializeField][Range(0.1f, 50f)] private float minDistanceZ = 10;

    [SerializeField][Range(0.1f, 100f)] private float deltaX = 30;
    [SerializeField][Range(0.1f, 100f)] private float deltaZ = 30;

    [SerializeField][Range(0.05f, 5f)] private float minFireRate = 0.5f;
    [SerializeField][Range(0.05f, 5f)] private float maxFireRate = 2f;

    [SerializeField][Range(1f, 50f)] private float minFireDistance = 10f;
    [SerializeField][Range(1f, 100f)] private float maxFireDistance = 30f;
    #endregion

    #region Walls
    private int _wallsAvaible = 0;

    [SerializeField] private GameObject[] wallsPrefab;

    [SerializeField][Range(0.1f, 50f)] private float minDistanceWallX = 1;
    [SerializeField][Range(0.1f, 50f)] private float maxDistanceWallZ = 5;

    [SerializeField][Range(0.1f, 100f)] private float deltaWallX = 1;
    [SerializeField][Range(0.1f, 100f)] private float deltaWallZ = 1;
    #endregion 

    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject youWin;
    [SerializeField] TextMeshProUGUI timePlayed;



    void Start()
    {
        if (numberOfTurrets == 0)
        {
            Debug.LogWarning("No number of turrets detected");
            return;
        }

        _wallsAvaible = numberOfTurrets;

        _turrets = new GameObject[numberOfTurrets];

        for (int i = 0; i < numberOfTurrets; i++)
        {
            GameObject turret = Instantiate(turretsPrefab[Random.Range(0, turretsPrefab.Length)]);
            _turrets[i] = turret;

            int tries = 5;

            bool intersect = false;

            do
            {
                turret.transform.position = new Vector3(minDistanceX + Random.Range(-1f, 1f) * deltaX, 0,
                    minDistanceZ + Random.Range(-1f, 1f) * deltaZ);
                turret.transform.Rotate(Vector3.up, Random.Range(0, 360f), Space.World);

                foreach (var addedTurrent in _turrets)
                {

                    if (addedTurrent == turret || addedTurrent == null) continue;

                    if (addedTurrent.GetComponent<Collider>().bounds.Intersects(turret.GetComponent<Collider>().bounds))
                    {
                        intersect = true;
                        break;
                    }
                }
                tries--;

            } while (intersect && tries > 0);

            FireBulletsAtTarget turretScripts = turret.GetComponent<FireBulletsAtTarget>();
            turretScripts.Configure(Random.Range(minFireRate, maxFireRate), Random.Range(minFireDistance, maxFireDistance), transform);

            GameObject wall = Instantiate(wallsPrefab[Random.Range(0, wallsPrefab.Length)]);

            wall.transform.position = turret.transform.position + new Vector3(
                minDistanceWallX + Random.Range(-1f, 1f) * deltaWallX,
                wall.transform.localScale.y * 0.5f,
                maxDistanceWallZ + Random.Range(-1f, 1f * deltaWallZ));

            wall.transform.RotateAround(turret.transform.position, Vector3.up, Random.Range(0, 360f));

            wall.transform.Rotate(Vector3.up, Random.Range(-45f, -45f), Space.Self);

            Destroy_On_Multiple_Hit destroy_On_Multiple_ = wall.GetComponent<Destroy_On_Multiple_Hit>();
            destroy_On_Multiple_.GameMenager = this;
        }
    }

    public void GameOver()
    {
        DestroyAllTurrets();

        Debug.Log($"GameOver: Play Time :{Time.time}");

        gameOver.SetActive(true);

        TimePlayed();
    }

    public void DidDestroyWall()
    {
        _wallsAvaible--;

        if (_wallsAvaible <= 0)
        {
            DestroyAllTurrets();

            Debug.Log($"GameOver: Play Time: {Time.time}");

            youWin.SetActive(true);

            TimePlayed();
        }
    }

    private void DestroyAllTurrets()
    {
        foreach (var turret in _turrets)
        {
            Destroy(turret);
        }
    }

    private void TimePlayed()
    {
        timePlayed.text = "Time Played: " + Time.time.ToString("F2") + "s";

        Time.timeScale = 0;
    }
}