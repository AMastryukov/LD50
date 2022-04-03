using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnelTab : NotebookTab
{
    public List<PersonnelData> personnelList { get; private set; }

    [SerializeField] private GameObject personnelPrefab;
    // Start is called before the first frame update
    public void Add(PersonnelData personnel)
    {
        personnelList.Add(personnel);
        foreach (var personnelData in personnelList )
        {
            if (personnelData.EvidenceKey == personnel.EvidenceKey)
            {
                return;
            }
        }
        personnelList.Add(personnel);
        GameObject personnelObject = Instantiate(personnelPrefab, content.gameObject.transform);
        personnelObject.GetComponent<PersonnelObject>().InitializeEvidence(personnel);
    }
}
