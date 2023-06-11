using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData : MonoBehaviour
{
    [SerializeField]
    private List<Animal> animals; // List to hold animal data

    // Manually assign values for each animal species
    private void Awake()
    {
        animals = new List<Animal>();

        // Tiger
        Animal tiger = new Animal();
        tiger.Species = "Tiger";
        tiger.HabitatTerrain = "Grassland";
        tiger.HabitatVegetation = "Sparse";
        tiger.RequiresWater = true;
        animals.Add(tiger);

        // Bear
        Animal bear = new Animal();
        bear.Species = "Bear";
        bear.HabitatTerrain = "Forest";
        bear.HabitatVegetation = "Dense";
        bear.RequiresWater = true;
        animals.Add(bear);

        // Penguin
        Animal penguin = new Animal();
        penguin.Species = "Penguin";
        penguin.HabitatTerrain = "Ice";
        penguin.HabitatVegetation = "Sparse";
        penguin.RequiresWater = true;
        animals.Add(penguin);

        // Wolf
        Animal wolf = new Animal();
        wolf.Species = "Wolf";
        wolf.HabitatTerrain = "Forest";
        wolf.HabitatVegetation = "Dense";
        wolf.RequiresWater = false;
        animals.Add(wolf);
    }

    // Retrieve animal data by species name
    public Animal GetAnimalData(string species)
    {
        return animals.Find(animal => animal.Species == species);
    }
}
