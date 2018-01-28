using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Golgi limitini ayarlamayı unuttun. Golgi hem limit tutacak hem de protein ve enerji sayılarını kontrol edecek.

public class CellScript : MonoBehaviour {
    private int nextUpdate = 1;
    private int organelleCount;

    int mitochondriaCount;
    int mitochondriaEnergyConsumption;
    int mitochondriaEnergyProduction;
    int mitochondriaGlucoseConsumption;
    int mitochondriaNextEnergyRequired;
    int mitochondriaNextProteinRequired;

    int ribosomeCount;
    int ribosomeEnergyConsumption;
    int ribosomeProteinProduction;
    int ribosomeNextEnergyRequired;
    int ribosomeNextProteinRequired;

    int endoplazmikRetikulumCount;
    int endoplazmikRetikulumEnergyConsumption;
    int endoplazmikRetikulumNextEnergyRequired;
    int endoplazmikRetikulumNextProteinRequired;

    int golgiCount;
    int golgiEnergyConsumpiton;
    int golgiProteinStorage;
    int golgiCurrentProteinHold;
    int golgiEnergyStorage;
    int golgiCurrentEnergyHold;
    int golgiNextEnergyRequired;
    int golgiNextProteinRequired;

    int centrosomeLevel;
    int centrosomeNextEnergyRequired;
    int centrosomeNextProteinRequired;

    int cytoplasmCapacity;
    int cytoplasmNextEnergyRequired;
    int cytoplasmNextProteinRequired;

    int coreLevel;
    int coreEnergyConsumption;
    int coreProteinConsumption;
    int coreNextEnergyRequired;
    int coreNextProteinRequired;

    // Use this for initialization
    void Start () {
        //Initialization counts and levels of organelles
        mitochondriaCount = 1;
        ribosomeCount = 1;
        endoplazmikRetikulumCount = 1;
        golgiCount = 1;
        centrosomeLevel = 1;
        coreLevel = 1;
        cytoplasmCapacity = 8; //değiştirmeyi unutma
        organelleCount = 5;

        //Initialization of productions

        mitochondriaEnergyProduction = 431;
        ribosomeProteinProduction = 101;
        golgiEnergyStorage = 700;
        golgiProteinStorage = 300;

        InitializeConsumptions();
        InitializeRequirements();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            UpdateEverySecond();
        }
        
    }

    void UpdateEverySecond()
    {
        calculateEnergyProduction();
        EnergyBalance();
        ProteinBalance();
    }

    void EnergyBalance() {
        int result = calculateEnergyConsumption();

        if (result + getGolgiCurrentEnergy() < golgiEnergyStorage)
            addEnergy(result);
        else
            golgiCurrentEnergyHold = golgiEnergyStorage;
    }

    int calculateEnergyConsumption()
    {
        int result = mitochondriaEnergyProduction -
            (mitochondriaEnergyConsumption + ribosomeEnergyConsumption + endoplazmikRetikulumEnergyConsumption
            + golgiEnergyConsumpiton + coreEnergyConsumption);
        return result;
    }

    void calculateEnergyProduction()
    {
        mitochondriaGlucoseConsumption = endoplazmikRetikulumCount;
        mitochondriaEnergyProduction = 380 + 3 * mitochondriaGlucoseConsumption;
    }

    void ProteinBalance() {
        int result = golgiCurrentProteinHold + ribosomeProteinProduction - coreProteinConsumption;

        if (result < golgiProteinStorage)
            golgiCurrentProteinHold = result;
        else
            golgiCurrentProteinHold = golgiProteinStorage;
    }

    public void UpgradeMitochondria() {
        if (getGolgiCurrentEnergy() >= mitochondriaNextEnergyRequired && golgiCurrentProteinHold >= mitochondriaNextProteinRequired
            && organelleCount < cytoplasmCapacity) {

            golgiCurrentEnergyHold -= mitochondriaNextEnergyRequired;
            golgiCurrentProteinHold -= mitochondriaNextProteinRequired;

            mitochondriaCount++;
            mitochondriaEnergyProduction += mitochondriaEnergyProduction / 60;
            mitochondriaEnergyConsumption += mitochondriaEnergyConsumption / 36;
            mitochondriaNextProteinRequired += mitochondriaNextProteinRequired / 10;
            mitochondriaNextEnergyRequired += mitochondriaNextEnergyRequired / 10;

            organelleCount++;
            
            GameObject.FindObjectOfType<mitoValueDisplay>().setText(mitochondriaCount);
        }
    }

    public void DowngradeMitochondria() {
        if (mitochondriaCount > 1)
        {
            mitochondriaCount--;
            mitochondriaEnergyProduction = mitochondriaEnergyProduction * 10 / 16;
            mitochondriaGlucoseConsumption = mitochondriaGlucoseConsumption * 10 / 11;
            mitochondriaEnergyConsumption = mitochondriaEnergyConsumption * 10 / 11;
            mitochondriaNextProteinRequired = mitochondriaNextProteinRequired * 10 / 11;
            mitochondriaNextEnergyRequired = mitochondriaNextEnergyRequired * 10 / 11;

            organelleCount--;

            GameObject.FindObjectOfType<mitoValueDisplay>().setText(mitochondriaCount);
        }
    }

    public void UpgradeRibosome() {
        if (golgiCurrentEnergyHold >= ribosomeNextEnergyRequired && golgiCurrentProteinHold >= ribosomeNextProteinRequired
            && organelleCount < cytoplasmCapacity) {

            golgiCurrentEnergyHold -= ribosomeNextEnergyRequired;
            golgiCurrentProteinHold -= ribosomeNextProteinRequired;

            ribosomeCount++;
            ribosomeEnergyConsumption += ribosomeEnergyConsumption / 100 * 5;
            ribosomeProteinProduction += ribosomeProteinProduction / 100 * 1;
            ribosomeNextEnergyRequired += ribosomeNextEnergyRequired / 100;
            ribosomeNextProteinRequired += ribosomeNextProteinRequired / 10;

            organelleCount++;

            GameObject.FindObjectOfType<riboValueDisplay>().setText(ribosomeCount);
        }
    }

    public void DowngradeRibosome() {
        if (ribosomeCount > 1)
        {
            ribosomeCount--;
            ribosomeEnergyConsumption = ribosomeEnergyConsumption / 20 * 10;
            ribosomeProteinProduction = ribosomeProteinProduction / 11 * 10;
            ribosomeNextEnergyRequired = ribosomeNextEnergyRequired / 11 * 10;
            ribosomeNextProteinRequired = ribosomeNextProteinRequired / 11 * 10;

            organelleCount--;

            GameObject.FindObjectOfType<riboValueDisplay>().setText(ribosomeCount);
        }
    }

    public void UpgradeEndoplazmikRetikulum(){
        if(golgiCurrentEnergyHold >= endoplazmikRetikulumNextEnergyRequired 
            && golgiCurrentProteinHold >= endoplazmikRetikulumNextProteinRequired
            && organelleCount < cytoplasmCapacity) {

            golgiCurrentEnergyHold -= endoplazmikRetikulumNextEnergyRequired;
            golgiCurrentProteinHold -= endoplazmikRetikulumNextProteinRequired;

            endoplazmikRetikulumCount++;
            endoplazmikRetikulumEnergyConsumption += endoplazmikRetikulumEnergyConsumption / 10;
            endoplazmikRetikulumNextEnergyRequired += endoplazmikRetikulumNextEnergyRequired / 10;
            endoplazmikRetikulumNextProteinRequired += endoplazmikRetikulumNextProteinRequired / 10;

            organelleCount++;

            GameObject.FindObjectOfType<ERValueDisplay>().setText(endoplazmikRetikulumCount);
        }
    }

    public void DowngradeEndoplazmikRetikulum() {
        if (endoplazmikRetikulumCount > 1)
        {
            endoplazmikRetikulumCount--;
            endoplazmikRetikulumEnergyConsumption = endoplazmikRetikulumEnergyConsumption / 11 * 10;
            endoplazmikRetikulumNextEnergyRequired = endoplazmikRetikulumNextEnergyRequired / 11 * 10;
            endoplazmikRetikulumNextProteinRequired = endoplazmikRetikulumNextProteinRequired / 11 * 10;

            organelleCount--;

            GameObject.FindObjectOfType<ERValueDisplay>().setText(endoplazmikRetikulumCount);
        }
    }

    public void UpgradeGolgi() {
        if (golgiCurrentEnergyHold >= golgiNextEnergyRequired && golgiCurrentProteinHold >= golgiNextProteinRequired
            && organelleCount < cytoplasmCapacity) {

            golgiCurrentEnergyHold -= golgiNextEnergyRequired;
            golgiCurrentProteinHold -= golgiNextProteinRequired;

            golgiCount++;
            golgiEnergyStorage += golgiEnergyStorage / 10;
            golgiProteinStorage += golgiProteinStorage / 10;
            golgiEnergyConsumpiton += golgiEnergyConsumpiton / 10;
            golgiNextEnergyRequired += golgiNextEnergyRequired / 10;
            golgiNextProteinRequired += golgiNextProteinRequired / 10;

            organelleCount++;

            GameObject.FindObjectOfType<golgiValueDisplay>().setText(golgiCount);
        }
    }

    public void DowngradeGolgi() {
        if (golgiCount > 1)
        {
            golgiCount--;
            golgiEnergyConsumpiton = golgiEnergyConsumpiton / 11 * 10;
            golgiEnergyStorage = golgiEnergyStorage / 11 * 10;
            golgiProteinStorage = golgiProteinStorage / 11 * 10;
            golgiNextEnergyRequired = golgiNextEnergyRequired / 11 * 10;
            golgiNextProteinRequired = golgiNextProteinRequired / 11 * 10;

            organelleCount--;

            GameObject.FindObjectOfType<golgiValueDisplay>().setText(golgiCount);
        }
    }

    public void UpgradeCentrosome() {
        if (golgiCurrentEnergyHold >= centrosomeNextEnergyRequired && golgiCurrentProteinHold >= centrosomeNextProteinRequired
            && organelleCount < cytoplasmCapacity) {

            golgiCurrentEnergyHold -= centrosomeNextEnergyRequired;
            golgiCurrentProteinHold -= centrosomeNextProteinRequired;

            centrosomeLevel++;
            centrosomeNextEnergyRequired += centrosomeNextEnergyRequired / 10;
            centrosomeNextProteinRequired += centrosomeNextProteinRequired / 10;

            organelleCount++;

            GameObject.FindObjectOfType<centrosomValueDisplay>().setText(centrosomeLevel);
        }
    }

    public void DowngradeCentrosome() {
        if (centrosomeLevel > 1)
        {
            centrosomeLevel--;
            centrosomeNextEnergyRequired = centrosomeNextEnergyRequired / 11 * 10;
            centrosomeNextProteinRequired = centrosomeNextProteinRequired / 11 * 10;

            organelleCount--;
            
            GameObject.FindObjectOfType<centrosomValueDisplay>().setText(centrosomeLevel);
        }
    }

    public void UpgradeCytoplasm() {
        if (golgiCurrentEnergyHold >= cytoplasmNextEnergyRequired && golgiCurrentProteinHold >= cytoplasmNextProteinRequired) {

            golgiCurrentEnergyHold -= cytoplasmNextEnergyRequired;
            golgiCurrentProteinHold -= cytoplasmNextProteinRequired;

            cytoplasmCapacity += 4;
            cytoplasmNextEnergyRequired += cytoplasmNextEnergyRequired / 10;
            cytoplasmNextProteinRequired += cytoplasmNextProteinRequired / 10;

            GameObject.FindObjectOfType<cytoplasmValueDisplay>().setText(cytoplasmCapacity);
        }
    }

    public void DowngradeCytoplasm(){
        if (cytoplasmCapacity > 1)
        {
            cytoplasmCapacity = cytoplasmCapacity / 2;
            cytoplasmNextEnergyRequired = cytoplasmNextEnergyRequired / 11 * 10;
            cytoplasmNextProteinRequired = cytoplasmNextProteinRequired / 11 * 10;

            GameObject.FindObjectOfType<cytoplasmValueDisplay>().setText(cytoplasmCapacity);
        }
    }

    public void UpgradeCore()
    {
        if(golgiCurrentEnergyHold >= coreNextEnergyRequired && golgiCurrentProteinHold >= coreNextProteinRequired
            && centrosomeLevel > coreLevel) {

            golgiCurrentEnergyHold -= coreNextEnergyRequired;
            golgiCurrentProteinHold -= coreNextProteinRequired;

            coreLevel++;
            coreNextEnergyRequired += coreNextEnergyRequired / 10;
            coreNextProteinRequired += coreNextProteinRequired / 10;

            GameObject.FindObjectOfType<coreValueDisplay>().setText(coreLevel);
        }
    }

    public int getGolgiCurrentEnergy()
    {
        return golgiCurrentEnergyHold;
    }

    void addEnergy(int amount)
    {
        golgiCurrentEnergyHold += amount;
    }

    void consumeEnergy(int amount)
    {
        golgiCurrentEnergyHold -= amount;
    }

    public int getGolgiCurrentProtein()
    {
        return golgiCurrentProteinHold;
    }

    void InitializeRequirements() {
        //Initialization of next level requirements
        mitochondriaNextEnergyRequired = 150;
        mitochondriaNextProteinRequired = 100;

        ribosomeNextEnergyRequired = 70;
        ribosomeNextProteinRequired = 130;

        endoplazmikRetikulumNextEnergyRequired = 100;
        endoplazmikRetikulumNextProteinRequired = 55;

        golgiNextEnergyRequired = 150;
        golgiNextProteinRequired = 80;
        golgiCurrentEnergyHold = 0;
        golgiCurrentProteinHold = 0;

        centrosomeNextEnergyRequired = 110;
        centrosomeNextProteinRequired = 120;

        coreNextEnergyRequired = 900;
        coreNextProteinRequired = 700;

        cytoplasmNextEnergyRequired = 120;
        cytoplasmNextProteinRequired = 45;
    }

    void InitializeConsumptions() {
        //Initialization of consumptions
        mitochondriaEnergyConsumption = 80;

        ribosomeEnergyConsumption = 75;

        endoplazmikRetikulumEnergyConsumption = 35;

        golgiEnergyConsumpiton = 40;

        coreEnergyConsumption = 150;
        coreProteinConsumption = 100;
    }
}
