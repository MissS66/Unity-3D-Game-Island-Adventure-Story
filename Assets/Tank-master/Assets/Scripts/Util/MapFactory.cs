using Constant;
using UnityEngine;

namespace Util
{
    public class MapFactory : MonoBehaviour
    {
        /// <summary>
        /// 清空地图静态字典，防止重复进入 Game 场景时报相同 key
        /// </summary>
        public static void ResetMap()
        {
            if (GameContext.GameObjectMap != null)
            {
                GameContext.GameObjectMap.Clear();
            }
        }

        /// <summary>
        /// 创建地图对象
        /// </summary>
        public static void CreateMapItem(string goName, Vector3 vector3, Transform parent)
        {
            string key = $"{vector3.x}-{vector3.y}";

            if (GameContext.GameObjectMap.ContainsKey(key))
            {
                return;
            }

            var prefab = Resources.Load<GameObject>(goName);

            if (prefab == null)
            {
                Debug.LogError("Resources 里找不到预制体：" + goName);
                return;
            }

            GameObject go = Instantiate(prefab, vector3, Quaternion.identity, parent);
            GameContext.GameObjectMap.Add(key, go);
        }

        /// <summary>
        /// 判断该位置是否有物体
        /// </summary>
        public static bool IsEmpty(Vector3 vector3)
        {
            string key = $"{vector3.x}-{vector3.y}";
            GameContext.GameObjectMap.TryGetValue(key, out var go);
            return go == null;
        }
    }
}