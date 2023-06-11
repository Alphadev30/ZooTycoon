
using UnityEngine;
using UnityEngine.UI;

public class AnimalDataManager : MonoBehaviour
{
    public AnimalData animalData;

    public Text speciesText;
    public Text terrainText;
    public Text vegetationText;
    public Text waterText;

    public void setUIFieldValues(string species)
    {
        // Retrieve animal data for a specific species
        Animal data = animalData.GetAnimalData(species);

        if (data != null)
        {
            speciesText.text = "Species : " + data.HabitatTerrain;
            terrainText.text = "Terrain : " + data.HabitatTerrain;
            vegetationText.text = "Vegetation : " + data.HabitatVegetation;
            waterText.text = "RequiresWater : " + data.RequiresWater.ToString();

        }
        else
        {
            Debug.Log(species + " : data not found.");
        }
    }
}
