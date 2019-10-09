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

    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;
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
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //parameterise time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
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
