using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{

  void start(){
    rb = GetComponent<Rigidbody>();
    audioData = GetComponent<AudioSource>();
    engine = GetComponent<AudioSource>();
  }
  bool isGrounded(){
    return FR_Wheel.isGrounded && FL_Wheel.isGrounded && BR_Wheel.isGrounded && BL_Wheel.isGrounded;
  }
  public void GetInput(){
    m_horizontalInput = Input.GetAxis("Horizontal");
    m_verticalInput = Input.GetAxis("Vertical");
    m_boost = Input.GetAxis("Fire1");
    m_jump = Input.GetAxis("Fire2");
    m_rotation = Input.GetAxis("Rotate");
  }
  private void Steer(){
    m_steerAngle = maxSteerAngle * m_horizontalInput;
    FL_Wheel.steerAngle = m_steerAngle;
    FR_Wheel.steerAngle = m_steerAngle;
  }
  private void Turn(){
    if(m_horizontalInput > 0.2 || m_horizontalInput < -0.2){
      if(Input.GetKey("joystick button 4") || Input.GetKey("left shift")){
        m_horizontalInput = Input.GetAxis("Horizontal");
        rb.AddTorque(transform.forward * m_horizontalInput * -5, ForceMode.Impulse);
      }
      else{
        rb.AddTorque(transform.up * m_horizontalInput * 5, ForceMode.Impulse);
      }
    }
  }
  private void Accelerate(){
    FL_Wheel.motorTorque = engineForce * m_verticalInput;
    FR_Wheel.motorTorque = engineForce * m_verticalInput;
    BL_Wheel.motorTorque = engineForce * m_verticalInput;
    BR_Wheel.motorTorque = engineForce * m_verticalInput;
    //engine.Play();
  }
  private void Rotate(){
    if(m_verticalInput > 0.2 || m_verticalInput < -0.2){
      rb.AddTorque(transform.right * m_rotation * -7, ForceMode.Impulse);
    }
  }
  private void Boost(){
    if(m_boost > 0 && rb.velocity.magnitude < maxSpeed){
      rb.AddForce(transform.forward * boostForce, ForceMode.Impulse );
    }
  }

  void OnCollisionEnter(Collision col){
    if(col.gameObject.name == "Ground"){
      lastStepG = Time.time;
    }
    else if(col.gameObject.name == "Soccer Ball"){
      audioData.Play();
    }
  }
  void OnCollisionStay(Collision col){
    if(col.gameObject.name == "Ground" && Time.time - lastStepG > timeBetweenStepsG){
      if(!isGrounded()){
        Vector3 _changePos = new Vector3(transform.position.x, 2, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, _changePos, 15 * Time.deltaTime);
        Quaternion q = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 20);

      }
    }
  }
  private void Jump(){
    if(m_jump > 0){
     if(isGrounded()){
        rb.AddForce(transform.up * jumpForce, ForceMode.Acceleration);
        lastStepJ = Time.time;
      }
      else if(Time.time - lastStepJ < timeBetweenStepsJ && !m_hasJumped){
        Debug.Log("Jump");
        rb.AddForce(transform.up * jumpForce, ForceMode.Acceleration);
        m_hasJumped = true;
      }
    }
  }
  private void UpdateWheelPoses(){
    UpdateWheelPose(FL_Wheel, FL_Wheel_T);
    UpdateWheelPose(FR_Wheel, FR_Wheel_T);
    UpdateWheelPose(BL_Wheel, BL_Wheel_T);
    UpdateWheelPose(BR_Wheel, BR_Wheel_T);
  }
  private void UpdateWheelPose(WheelCollider _collider, Transform _transform){
    Vector3 _pos = _transform.position;
    Quaternion _quat = _transform.rotation;
    _collider.GetWorldPose(out _pos, out _quat);
    _transform.position = _pos;
    _transform.rotation = _quat;
  }

  private void FixedUpdate(){
    GetInput();
    if(isGrounded()){
      m_hasJumped = false;
      Steer();
      Accelerate();
    }
    else{
      Turn();
      Rotate();
    }
    Jump();
    Boost();
    UpdateWheelPoses();

  }

  private float m_horizontalInput;
  private float m_verticalInput;
  private float m_steerAngle;
  private float m_boost;
  private float m_jump;
  private float m_rotation;
  private float lastStepG = 0, timeBetweenStepsG = 1.5f;
  private float lastStepJ = 0, timeBetweenStepsJ = 2f;
  private bool m_hasJumped = false;

  public Rigidbody rb;
  public AudioSource audioData, engine;
  public WheelCollider FL_Wheel, FR_Wheel;
  public WheelCollider BL_Wheel, BR_Wheel;
  public Transform FL_Wheel_T, FR_Wheel_T;
  public Transform BL_Wheel_T, BR_Wheel_T;

  public float maxSteerAngle = 30;
  public float engineForce = 100;
  public float boostForce = 3;
  public float maxSpeed = 10;
  public float jumpForce = 4000;

}
