using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{

  public void GetInput(){
    m_camToggle = Input.GetAxis("Jump");
    if(m_camToggle > 0){
      if(Time.time - lastStep > timeBetweenSteps){
        lastStep = Time.time;
        BallCam = !BallCam;
      }
    }
  }

//FreeCam
  public void LookAtTargetFreeCam(){
    Vector3 _lookDirection = objectToLookAt.position - transform.position;
    Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
    transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
  }
  public void MoveToTargetFreeCam(){
    Vector3 _targetPos =  objectToFollow.position
    + objectToFollow.forward * offsetFreeCam.z
    + objectToFollow.right * offsetFreeCam.x
    + objectToFollow.up * offsetFreeCam.y;
    transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
  }

//BallCam
  public void LookAtTargetBallCam(){
    Vector3 _lookDirection = objectToLookAtBallCam.position - transform.position;
    Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
    transform.rotation = Quaternion.Lerp(transform.rotation, _rot, 100 * Time.deltaTime);
  }
  public void MoveToTargetBallCam(){
    Vector3 _targetPos =  objectToFollowBallCam.position
    + objectToFollowBallCam.forward * offsetBallCam.z
    + objectToFollowBallCam.right * offsetBallCam.x
    + objectToFollowBallCam.up * offsetBallCam.y;
    transform.position = Vector3.Lerp(transform.position, _targetPos, 100 * Time.deltaTime);
  }


  private void FixedUpdate(){
    GetInput();
    if(!BallCam){
      LookAtTargetFreeCam();
      MoveToTargetFreeCam();
    }
    else{
      LookAtTargetBallCam();
      MoveToTargetBallCam();
    }
  }

  private float lastStep, timeBetweenSteps = 0.5f;
  private float m_camToggle;
  public bool BallCam = false;

  //FreeCam
  public Transform objectToFollow;
  public Transform objectToLookAt;
  public Vector3 offsetFreeCam;
  public float followSpeed = 10;
  public float lookSpeed = 75;

  //BallCam
  public Transform objectToFollowBallCam;
  public Transform objectToLookAtBallCam;
  public Vector3 offsetBallCam;



}
