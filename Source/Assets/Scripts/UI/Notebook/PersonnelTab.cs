using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnelTab : NotebookTab
{
    [SerializeField] private GameObject personnelPrefab;
    // Start is called before the first frame update
    public void Add(PersonnelData personnel)
    {
        if(dataManager.CheckIfPersonnelAlreadyExists(personnel.EvidenceKey))
            return;
        dataManager.personnelListInNotebook.Add(personnel);
        InstantiatePersonnel(personnel);
    }

    public void InstantiatePersonnel(PersonnelData personnel)
    {
        GameObject personnelObject = Instantiate(personnelPrefab, content.gameObject.transform);
        personnelObject.GetComponent<PersonnelObject>().InitializeEvidence(personnel);
    }
}
