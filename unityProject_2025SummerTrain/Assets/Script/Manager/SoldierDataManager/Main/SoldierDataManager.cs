using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDataManager : Singleton<SoldierDataManager>
{
    public SoldierData_SO playerSoldierData_SO;
    private List<SoldierDetail> soldierDetailsList => playerSoldierData_SO.soldierDetailsList;
    public List<SoldierDetail> GetAllSoldiers()
    {
        return soldierDetailsList;
    }
    public SoldierDetail GetSoldierDetailByID(int soldierID)
    {
        foreach (var soldier in soldierDetailsList)
        {
            if (soldier.soldierID == soldierID)
            {
                return soldier;
            }
        }
        return null;
    }

    public void AddSoldier(SoldierDetail newSoldier)
    {
        soldierDetailsList.Add(newSoldier);
    }

    public void RemoveSoldier(int soldierID)
    {
        SoldierDetail soldierToRemove = GetSoldierDetailByID(soldierID);
        if (soldierToRemove != null)
        {
            soldierDetailsList.Remove(soldierToRemove);
        }
    }
}
