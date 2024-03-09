using HuggingFace.API;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SpeechRecognition : MonoBehaviour
{

    //public static bool IsYesSpeech(string command)
    //{
    //    if (command.Contains("yes") || command.Contains("yeah"))
    //        return true;
    //    else return false;
    //}

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private VoiceCommandController voiceCommandController;


    private AudioClip clip;
    private byte[] bytes;
    private bool recording;

    private void Update()
    {
        // Start recording when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space) && !recording)
        {
            StartRecording();
        }

        // Stop recording when the spacebar is released
        if (Input.GetKeyUp(KeyCode.Space) && recording)
        {
            StopRecording();
        }

        // Check for recording completion
        if (recording && Microphone.GetPosition(null) >= clip.samples)
        {
            StopRecording();
        }
    }

    private void StartRecording()
    {
        text.color = Color.white;
        text.text = "Recording...";
        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    private void StopRecording()
    {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    private void SendRecording()
    {
        text.color = Color.yellow;
        text.text = "Sending...";

        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response =>
        {
            text.color = Color.white;
            text.text = response;

            // Pass the recognized command to the VoiceCommandController
            voiceCommandController.ProcessVoiceCommand(response);
        }, error =>
        {
            text.color = Color.red;
            text.text = error;
        });
    }


    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}
