using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] Transform SpawnPoint;

    [Space]

    [SerializeField] GameObject EnemyObject;

    GameObject CurrentEnemy;

    // - Data
    GameData GameData;
    PlayerData PlayerData;

    public bool IsEnemyAvailable {  get { return CurrentEnemy != null; } }

    void Awake()
    {
        Instance = this;

        GameData = new GameData();

        PlayerData = new PlayerData();

        EventManager.OnEnemyDeathFinished.AddListener(OnEnemyDeathFinished);
    }

    void Start()
    {
        CreateNewEnemy();

        // - OnReady.Invoke()
        EventManager.OnStageUpdate.Invoke(GameData.CurrentStage, GameData.CurrentEnemy);
    }

    public bool TryDealDamageToEnemy(float amount)
    {
        if (IsEnemyAvailable)
        {
            DealDamageToEnemy(amount);

            return true;
        }

        return false;
    }

    void DealDamageToEnemy(float amount)
    {
        EnemyController controller = CurrentEnemy.GetComponent<EnemyController>();

        bool enemyKilled = GameData.Health.TakeDamage(amount);

        DamageNumbers.Instance.Add(amount);

        if (enemyKilled)
        {
            controller.OnDeathStart();

            CurrentEnemy = null;
        }

        else
        {
            controller.OnDamageTaken();
        }
    }

    void CreateNewEnemy()
    {
        CurrentEnemy = Instantiate(EnemyObject, SpawnPoint.position, Quaternion.identity);

        GameData.Health.Reset();
    }

    void OnEnemyDeathFinished()
    {
        CreateNewEnemy();

        GameData.AddKills(1);

        EventManager.OnStageUpdate.Invoke(GameData.CurrentStage, GameData.CurrentEnemy);
    }
}
