using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
public class StartButton : MonoBehaviour
{
    GameObject EmitterGameObject;
    FMODUnity.StudioEventEmitter FMODEmitter;
    FMODUnity.StudioParameterTrigger FMODTrigger;
    FMOD.Studio.EventInstance EventInstance;
    FMOD.Studio.ParameterInstance ParameterInstance;

//musicEvent = target.GetComponent<FMODUnity.StudioEventEmitter>();
//FMOD.Studio.ParameterInstance myParameter;
//myParameter = musicEvent.GetParameter (“War”);
//myParameter.getValue(out float);
//myParameter.setValue(float);
    void Start()
    {
        FMODEmitter = GameObject.FindGameObjectWithTag("FMODEmitter").GetComponent<FMODUnity.StudioEventEmitter>();
    }
    void Update()
    {
        //Debug.Log(FMODEmitter.Params[0].Name + FMODEmitter.Params[0].Value);
    }

    public void ChangeScene()
    {
        FMODEmitter.SetParameter("Intro", 1f);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
