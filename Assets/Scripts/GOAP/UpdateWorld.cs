using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateWorld : MonoBehaviour {

    // store states
    public TextMeshProUGUI states;

    void LateUpdate() {

        // states dic
        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();

        states.text = "";
        // cycle thru and store in states.text
        foreach (KeyValuePair<string, int> s in worldStates) {

            states.text += s.Key + ", " + s.Value + "\n";
        }
    }
}
