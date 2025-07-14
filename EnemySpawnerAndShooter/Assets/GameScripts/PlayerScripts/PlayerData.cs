using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/Character Data")]
public class PlayerData : ScriptableObject
{
    public string characterName;
    public Sprite icon;
    public GameObject prefab; // Buraya fbx dosyasından oluşturduğun prefab'ı atayacaksın

    public int maxHealth;
    public string description;
}
