using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntTargetSensor : MonoBehaviour
{

    [SerializeField] float detectionDistance = 5;
    public float DetectionDistance
    {
        get
        {
            return detectionDistance;
        }
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, detectionDistance);
    }

    #endregion


    public GameObject CheckForAvailableTargets()
    {
        foreach (Transform civ in Map.Instance.ActiveCivs)
        {
            float distanceToCiv = Vector2.Distance(this.transform.position, civ.transform.position);
            CivStats civStates = civ.GetComponent<CivStats>();
            if (distanceToCiv <= detectionDistance)
            {
                // Set this as the target
                return civ.gameObject;
            }
        }
        return null;
    }
}
