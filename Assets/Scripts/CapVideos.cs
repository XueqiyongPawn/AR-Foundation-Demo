using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;
using UnityEngine;
using UnityEditor;
using UnityEngine.Analytics;

public class CapVideos : MonoBehaviour
{
    public bool recordMicrophoneAudio;
    public Camera[] _camere;
    public AudioSource audioSource;

    public bool isRecording;

    public static CapVideos Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        //Mvc.MvcTool.addNoticeListener(MessageKeys.RecVideo,RecVideo);
    }

    private void OnDestroy()
    {
        //Mvc.MvcTool.removeNoticeListener(MessageKeys.RecVideo,RecVideo);
    }

    public void RecVideo()
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    // Update is called once per frame
    [Header("Recording")]
        private int videoWidth = Screen.width;
        private int videoHeight = Screen.height;

        [Header("Microphone")]
        public bool recordMicrophone;
        public AudioSource microphoneSource;

        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;
        private AudioInput audioInput;

        public void StartRecording () {            
            Debug.Log("StartRecording------");
            // Start recording
            recordingClock = new RealtimeClock();
            videoRecorder = new MP4Recorder(
                videoWidth,
                videoHeight,
                30,
                recordMicrophone ? AudioSettings.outputSampleRate : 0,
                recordMicrophone ? (int)AudioSettings.speakerMode : 0,
                OnReplay
            );
            
            // Create recording inputs
            cameraInput = new CameraInput(videoRecorder, recordingClock, _camere);
            if (recordMicrophone) {
                StartMicrophone();
                audioInput = new AudioInput(videoRecorder, recordingClock, microphoneSource, true);
            }

            isRecording = true;
            //Mvc.MvcTool.sendNotice(MessageKeys.ChangeRecState,isRecording);
        }

        private void StartMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
            // Create a microphone clip
            microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
            while (Microphone.GetPosition(null) <= 0) ;
            // Play through audio source
            microphoneSource.timeSamples = Microphone.GetPosition(null);
            microphoneSource.loop = true;
            microphoneSource.Play();
            #endif
        }

        public void StopRecording () {
            Debug.Log("StopRecording------");

            // Stop the recording inputs
            if (recordMicrophone) {
                StopMicrophone();
                audioInput.Dispose();
            }
            cameraInput.Dispose();
            // Stop recording
            videoRecorder.Dispose();
            isRecording = false;
            //Mvc.MvcTool.sendNotice(MessageKeys.ChangeRecState,isRecording);

        }

        private void StopMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR
            Microphone.End(null);
            microphoneSource.Stop();
            #endif
        }

        private void OnReplay (string path) {
            Debug.Log("Saved recording to: "+path);

            // Playback the video
            #if UNITY_EDITOR
			EditorUtility.OpenWithDefaultApp(path);
            #elif UNITY_IOS
            Handheld.PlayFullScreenMovie("file://" + path);
            #elif UNITY_ANDROID
            //OriginBridge.CallJaveMethod(OriginBridge.md_shareImageTo,(int)SharePlatform.system,path);
            Handheld.PlayFullScreenMovie(path);
            #endif
        }
    
    string GetCurTime()
    {
        return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
               + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
    }
    
    public static void CopyFile(string soucre, string target, System.Action<String> _OnCopyComplete)
    {
        using (FileStream fsRead = new FileStream(soucre, FileMode.Open, FileAccess.Read))
        {
            using (FileStream fsWrite = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] buffer = new byte[1024 * 1024 * 5];
                while (true)
                {
                    int r = fsRead.Read(buffer, 0, buffer.Length);
                    if (r == 0) break;                            
                    fsWrite.Write(buffer, 0, r);
                }
            }
        }

        if (null != _OnCopyComplete) _OnCopyComplete(soucre);
    }
    
}
