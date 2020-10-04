using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();    
        }
        if (Debug.isDebugBuild) {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || collisionsDisabled ){return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke(nameof(LoadNextLevel), levelLoadDelay); // parameterise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke(nameof(LoadFirstLevel), levelLoadDelay); // parameterise time
    }

    private void LoadNextLevel()
    {
        state = State.Alive;

        SceneManager();

    }

    private static void SceneManager()
    {
        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        if (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void LoadFirstLevel()
    {
        state = State.Alive;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.W)) // can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            ReleaseThrust();
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation

    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying) // so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
        }

        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    private void ReleaseThrust()
    {
        audioSource.Stop();
        if (mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Stop();
        }
    }

    private void NextLevelDebugKey()
    {
        if (Input.GetKey(KeyCode.L)) // can thrust while rotating
        {
            SceneManager();
        }
    }

}
