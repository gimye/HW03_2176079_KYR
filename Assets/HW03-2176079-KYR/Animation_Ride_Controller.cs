using UnityEngine;

public class Animation_Ride_Controller : MonoBehaviour
{
    private Vehicle_Controller currentVehicle;
    private bool isRiding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isRiding) return;
        if (other.CompareTag("Vehicle"))
        {
            currentVehicle = other.GetComponent<Vehicle_Controller>();
            if (currentVehicle != null)
            {
                isRiding = true;
                currentVehicle.BoardVehicle(transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vehicle") && !currentVehicle.justArrived)
        {
            if (currentVehicle != null)
            {
                currentVehicle.ExitVehicle(true); // 강제 하차
            }
            isRiding = false;
        }
    }

}
