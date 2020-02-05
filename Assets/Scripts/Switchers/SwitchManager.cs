///<summary>
/// Switch Manager that allows to control number of switches to activate doors
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] switches; // Array of switches

    [SerializeField]
    GameObject exit; // Door or anything else that needs to change state

    [SerializeField]
    Text switchCount; // Counting number of switches

    int nbOfSwitches = 0; // number of switches used to activate something

    void Start()
    {
        GetNbOfSwitches();
    }

    ///<summary>
    /// Allows to calculate the number of switchers to activate the door
    ///</summary>
    public int GetNbOfSwitches()
    {
        int x = 0;

        for(int i = 0; i < switches.Length; i++)
        {
            if (switches[i].GetComponent<Switch>().isOn == false)
                x++;
            else if (switches[i].GetComponent<Switch>().isOn == true)
                nbOfSwitches--;
        }

        nbOfSwitches = x;

        return nbOfSwitches;
    }

    ///<summary>
    /// Change the state of the door (Open the door)
    ///</summary>
    public void ExitState()
    {
        if (nbOfSwitches <= 0)
            exit.GetComponent<Door>().OpenDoor();
    }

    void Update()
    {
        switchCount.text = GetNbOfSwitches().ToString(); // Display the number of switchers

        ExitState(); // Call the fonction of door state
    }
}
