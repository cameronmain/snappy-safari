using System.Collections.Generic;


/*
    - WorldState:
        - Represents an individual state of the world with a key-value pair.
    
    - WorldStates:
        - Manages a collection of WorldState objects in a dictionary format.
        - Provides functionality to:
            1) Check if a state exists.
            2) Add a new state.
            3) Modify the value of an existing state.
            4) Remove a state.
            5) Set a state's value directly.
            6) Retrieve the current states as a dictionary.
*/


[System.Serializable]
public class WorldState {

    public string key;
    public int value;
}

public class WorldStates {

    public Dictionary<string, int> states;

    public WorldStates() {

        states = new Dictionary<string, int>();
    }


    public bool HasState(string key) {

        return states.ContainsKey(key);
    }

    private void AddState(string key, int value) {

        states.Add(key, value);
    }

    public void ModifyState(string key, int value) {

        if (HasState(key)) {
            states[key] += value;
            if (states[key] <= 0) {
                RemoveState(key);
            }
        } else {

            AddState(key, value);
        }
    }

    public void RemoveState(string key) {

        if (HasState(key)) {

            states.Remove(key);
        }
    }

    public void SetState(string key, int value) {

        if (HasState(key)) {

            states[key] = value;
        } else {

            AddState(key, value);
        }
    }

    public Dictionary<string, int> GetStates() {

        return states;
    }
}
