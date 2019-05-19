using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
  void start(){
    rb = GetComponent<Rigidbody>();
    audioData = GetComponent<AudioSource>();
  }
  public void Reset(){
    audioData.Play();
    Vector3 _targetPos = new Vector3(0,0.5f,0);
    transform.position = Vector3.Lerp(transform.position, _targetPos, 100 * Time.deltaTime);
    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
  }
  public void Goal(){
    if(transform.position.z > 20f){
      ScoreOrange++;
      Reset();
    }
    else if(transform.position.z < -20f){
      ScoreBlue ++;
      Reset();
    }
  }
  private void FixedUpdate(){
    Goal();
  }
  public Rigidbody rb;
  public AudioSource audioData;
  public float ScoreOrange = 0;
  public float ScoreBlue = 0;
}
