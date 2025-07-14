using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int cost;
    public Sprite icon;
    public GameObject prefab;
    public TowerType towerType;
    public string description;
}
