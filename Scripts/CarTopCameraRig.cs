using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTopCameraRig : MonoBehaviour
{
  public void LookAtTarget(){

    Quaternion q = Quaternion.FromToRotation(transform.up, Vector3.up) * Quaternion.FromToRotation(transform.forward, objectToLookAt.forward) * Quaternion.FromToRotation(transform.right, Vector3.zero) * transform.rotation;
    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * lookSpeed);
  }
  private void FixedUpdate(){
    LookAtTarget();
  }
  public float lookSpeed = 10;
  public Transform objectToLookAt;
}
