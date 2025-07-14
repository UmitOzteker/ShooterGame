using UnityEngine;

public class CharacterDisplayManager : MonoBehaviour
{
    public GameObject[] characterModels;
    public PlayerData[] characterDatas; // Inspector'dan atayacaksın

    private GameObject currentActiveCharacter = null;

    public static int selectedCharacterIndex = 0;
    public static PlayerData selectedCharacterData = null; // Seçili karakter datası

    void Start()
    {
        foreach (GameObject character in characterModels)
        {
            if (character != null)
            {
                character.SetActive(false);
                Debug.Log($"Karakter pasif yapıldı: {character.name}");
            }
        }

        if (characterModels.Length > 0 && characterModels[0] != null)
        {
            DisplayCharacter(0);
        }
    }

    public void DisplayCharacter(int characterIndex)
    {
        if (characterIndex < 0 || characterIndex >= characterModels.Length)
        {
            return;
        }

        if (
            currentActiveCharacter != null
            && currentActiveCharacter != characterModels[characterIndex]
        )
        {
            currentActiveCharacter.SetActive(false);
        }

        currentActiveCharacter = characterModels[characterIndex];
        if (currentActiveCharacter != null)
        {
            currentActiveCharacter.SetActive(true);
        }
        selectedCharacterIndex = characterIndex;

        // --- CharacterData'yı da ata ---
        if (characterDatas != null && characterIndex < characterDatas.Length)
        {
            selectedCharacterData = characterDatas[characterIndex];
        }
    }
}
