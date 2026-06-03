using Constant;
using UnityEngine;
using Util;

namespace Manager
{
    public class GameSceneManager : MonoBehaviour
    {
        private Object[] enemyList;

        private void Awake()
        {
            ResetTankSceneData();

            enemyList = Resources.LoadAll(GameConst.EnemyPrefab, typeof(GameObject));

            if (GameContext.IsSingle)
            {
                GameContext.Player2Hp = 0;
            }
        }

        private void Start()
        {
            InitMap();
        }

        private void ResetTankSceneData()
        {
            MapFactory.ResetMap();

            GameContext.CurrentEnemyCount = 0;

            CancelInvoke();
        }

        private void InitMap()
        {
            CreateHome();
            CreatePlayer1();

            if (!GameContext.IsSingle)
            {
                CreatePlayer2();
            }

            InvokeRepeating("CreateEnemy", 2f, 5f);
            CreateRandomMap();
        }

        private void CreatePlayer1()
        {
            MapFactory.CreateMapItem(GameConst.BornPrefab1, GameConst.Player1BornVector3, transform);
        }

        private void CreatePlayer2()
        {
            MapFactory.CreateMapItem(GameConst.BornPrefab2, GameConst.Player2BornVector3, transform);
        }

        private void CreateEnemy()
        {
            if (GameContext.CurrentEnemyCount < GameConst.MAXEnemyCount)
            {
                int index = Random.Range(0, enemyList.Length);
                Vector3 pos = GameConst.EnemyBornPosList[Random.Range(0, GameConst.EnemyBornPosList.Length)];
                Instantiate(enemyList[index], pos, Quaternion.identity);
                GameContext.CurrentEnemyCount++;
            }
        }

        private void CreateHome()
        {
            MapFactory.CreateMapItem(GameConst.HomePrefab, GameConst.HomeVector3, transform);
            MapFactory.CreateMapItem(GameConst.WallPrefab, GameConst.HomeVector3 - new Vector3(-1, 0, 0), transform);
            MapFactory.CreateMapItem(GameConst.WallPrefab, GameConst.HomeVector3 - new Vector3(1, 0, 0), transform);
            MapFactory.CreateMapItem(GameConst.WallPrefab, GameConst.HomeVector3 + new Vector3(0, 1, 0), transform);
            MapFactory.CreateMapItem(GameConst.WallPrefab, GameConst.HomeVector3 + new Vector3(1, 1, 0), transform);
            MapFactory.CreateMapItem(GameConst.WallPrefab, GameConst.HomeVector3 + new Vector3(-1, 1, 0), transform);
        }

        private void CreateRandomMap()
        {
            int grassCount = Random.Range(15, 20);
            int wallCount = Random.Range(40, 60);
            int barrierCount = Random.Range(15, 30);
            int riverCount = Random.Range(15, 20);

            for (int i = 0; i < barrierCount; i++)
            {
                Vector3 pos = CreateRandomPosition();
                MapFactory.CreateMapItem(GameConst.BarrierPrefab, pos, transform);
            }

            for (int i = 0; i < riverCount; i++)
            {
                Vector3 pos = CreateRandomPosition();
                MapFactory.CreateMapItem(GameConst.RiverPrefab, pos, transform);
            }

            for (int i = 0; i < grassCount; i++)
            {
                Vector3 pos = CreateRandomPosition();
                MapFactory.CreateMapItem(GameConst.GrassPrefab, pos, transform);
            }

            for (int i = 0; i < wallCount; i++)
            {
                Vector3 pos = CreateRandomPosition();
                MapFactory.CreateMapItem(GameConst.WallPrefab, pos, transform);
            }
        }

        private Vector3 CreateRandomPosition()
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(-8, 8), 0);
            return MapFactory.IsEmpty(pos) ? pos : CreateRandomPosition();
        }
    }
}