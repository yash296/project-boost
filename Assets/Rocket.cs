/*using System;
using System.Collections;
using System.Collections.Generic;*/
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending };
    State state = State.Alive;
    // Start is called before the first frame update

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //stop sound on death
        if(state == State.Alive) { 
            RepondToThrust();
            RespondToRotate();
        }
    }
     void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) return;
       
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", 1f);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        Invoke("LoadNextLevel", 1f); //parameterise time
    }

    private  void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RepondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying) { audioSource.PlayOneShot(mainEngine); }

        }
        else
        {
            audioSource.Stop();
        }
    }
    private void RespondToRotate()
    {
        rigidBody.freezeRotation = true; //take manual control of roation
        float roatationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * roatationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * roatationThisFrame);
        }
        rigidBody.freezeRotation = false; //resume pysicys control

    }


}
