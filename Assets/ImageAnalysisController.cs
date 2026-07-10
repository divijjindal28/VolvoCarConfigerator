using System.Collections;
using Meta.XR.BuildingBlocks.AIBlocks;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnalysisController : MonoBehaviour
{
    [Header("Agents")]
    [SerializeField] private LlmAgent llmAgent;
    [SerializeField] private SpeechToTextAgent sstAgent;
    [SerializeField] private TextToSpeechAgent ttsAgent;

    [Header("UI")]
    [SerializeField] private Button STTButton;
    [SerializeField] private TMP_InputField promptInputField;

    [Header("System Prompt")]
    [TextArea(3, 6)]
    [SerializeField]
    private string systemPrompt =
        "You are a helpful AI assistant. Answer clearly, accurately, and briefly.";

    private bool isListening = false;
    private bool micWarmedUp = false;

    [SerializeField]private TextMeshProUGUI sttButtonText;
    [SerializeField] private PointableUnityEventWrapper pointableWrapper;


    private void Awake()
    {
        //sttButtonText = STTButton.GetComponentInChildren<TextMeshProUGUI>();

        if (sttButtonText != null)
            sttButtonText.text = "AI";

        if (sstAgent != null)
        {
            sstAgent.onTranscript.RemoveListener(OnTranscriptReceived);
            sstAgent.onTranscript.AddListener(OnTranscriptReceived);
        }

        if (llmAgent != null)
        {
            llmAgent.onResponseReceived.RemoveAllListeners();
            llmAgent.onResponseReceived.AddListener(OnLlmResponseReceived);
        }

        if (ttsAgent != null)
        {
            ttsAgent.onClipReady.RemoveAllListeners();
            ttsAgent.onSpeakStarting.RemoveAllListeners();
            ttsAgent.onSpeakFinished.RemoveAllListeners();

            ttsAgent.onClipReady.AddListener(
                clip => Debug.Log("[TTS] Clip Ready"));

            ttsAgent.onSpeakStarting.AddListener(
                clip => Debug.Log("[TTS] Speaking..."));

            ttsAgent.onSpeakFinished.AddListener(
                () => Debug.Log("[TTS] Finished"));
        }

        pointableWrapper.WhenSelect.RemoveAllListeners();
        pointableWrapper.WhenSelect.AddListener((args) => OnMicButtonPressed());
    }

    private void Start()
    {
        StartCoroutine(WarmupMic());
    }

    private IEnumerator WarmupMic()
    {
        if (sstAgent == null || micWarmedUp)
            yield break;

        yield return new WaitForSecondsRealtime(0.5f);

        try
        {
            sstAgent.StartListening();
        }
        catch
        {
            yield break;
        }

        yield return new WaitForSecondsRealtime(0.15f);

        try
        {
            sstAgent.StopNow();
            micWarmedUp = true;
            Debug.Log("[STT] Warmup complete.");
        }
        catch
        {
        }
    }

    private void OnMicButtonPressed()
    {
        if (!isListening)
            StartCoroutine(StartListening());
        else
            StopListening();
    }

    private IEnumerator StartListening()
    {

        if (promptInputField != null)
            promptInputField.text = "";

        yield return null;
        yield return new WaitForSecondsRealtime(0.02f);



        isListening = true;

        if (sttButtonText != null)
            sttButtonText.text = "...";

        sstAgent.StartListening();
    }


    private void StopListening()
    {
        sstAgent.StopNow();

        isListening = false;

        if (sttButtonText != null)
            sttButtonText.text = "AI";
    }


    private IEnumerator WaitForTranscript()
    {
        float timeout = 0.7f;
        float startTime = Time.realtimeSinceStartup;

        string transcript = string.Empty;

        while (Time.realtimeSinceStartup - startTime < timeout)
        {
            if (promptInputField != null &&
                !string.IsNullOrWhiteSpace(promptInputField.text))
            {
                transcript = promptInputField.text.Trim();
                break;
            }

            yield return new WaitForSecondsRealtime(0.05f);
        }

        if (string.IsNullOrWhiteSpace(transcript))
        {
            Debug.LogWarning("[STT] No transcript received.");
            yield break;
        }

        SendToLLM(transcript);
    }

    private async void SendToLLM(string userSpeech)
    {
        string finalPrompt =
            $"{systemPrompt}\n\n" +
            $"User: {userSpeech}";

        Debug.Log("==================================");
        Debug.Log("[LLM] Sending Prompt:");
        Debug.Log(finalPrompt);
        Debug.Log("==================================");

        try
        {
            await llmAgent.SendPromptAsync(finalPrompt);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[LLM] " + ex.Message);
        }
    }

    private void OnTranscriptReceived(string transcript)
    {
        Debug.Log("[STT] " + transcript);

        if (promptInputField != null)
            promptInputField.text = transcript;

        if (!string.IsNullOrWhiteSpace(transcript))
            SendToLLM(transcript);
    }

    private void OnLlmResponseReceived(string response)
    {
        Debug.Log("[LLM] " + response);

        if (string.IsNullOrWhiteSpace(response))
        {
            Debug.LogWarning("[LLM] Empty response.");
            ResetUI();
            return;
        }

        if (ttsAgent != null)
            ttsAgent.SpeakText(response);

        ResetUI();
    }

    private void ResetUI()
    {
        if (promptInputField != null)
            promptInputField.text = "";

        if (sttButtonText != null)
            sttButtonText.text = "AI";

        isListening = false;
    }


    private void OnDestroy()
    {
        if (sstAgent != null)
            sstAgent.onTranscript.RemoveListener(OnTranscriptReceived);

        if (llmAgent != null)
            llmAgent.onResponseReceived.RemoveListener(OnLlmResponseReceived);

        if (ttsAgent != null)
        {
            ttsAgent.onClipReady.RemoveAllListeners();
            ttsAgent.onSpeakStarting.RemoveAllListeners();
            ttsAgent.onSpeakFinished.RemoveAllListeners();
        }

        if (pointableWrapper != null)
            pointableWrapper.WhenSelect.RemoveAllListeners();
    }
}