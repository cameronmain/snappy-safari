using UnityEngine;

/*
 * used for the tree falling over in the forest level to make the level dynamic
 */
public class FellTree : MonoBehaviour
{
    // target rotation of the tree
    private Quaternion targetRotation = Quaternion.Euler(288.0f, 251.8f, 93.0f);

    // rotation speed of the tree
    public float rotationSpeed = 10.0f;

    void Update()
    {
        trigger();
    }

    public void trigger()
    {
        // rotate the tree towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}