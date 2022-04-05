using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    public static Action<Suspect, EvidenceData> OnKeyEvidenceShown;
    public static Action<Suspect> OnGenericEvidenceShown;
    public static Action<Suspect> OnConfess;

    public SuspectData Data;

    [SerializeField] private AudioSource mouthAudioSource;

    /// <summary>
    /// Every time the player brings up the matching key evidence, we remove a string from the list.
    /// When the list is empty, the suspect confesses
    /// </summary>
    [HideInInspector] public List<string> RemainingKeyEvidence { get; private set; }

    private VoicelineSubtitles voicelineSubtitles;
    private PlayerVoice playerVoice;

    private bool isTalking = false;
    private bool hasConfessed = false;

    private void Awake()
    {
        voicelineSubtitles = FindObjectOfType<VoicelineSubtitles>();
        playerVoice = FindObjectOfType<PlayerVoice>();
    }

    public void Start()
    {
        RemainingKeyEvidence = new List<string>();

        foreach (EvidenceData ev in Data.KeyEvidence)
        {
            RemainingKeyEvidence.Add(ev.Name);
        }
    }

    public IEnumerator AskAboutEvidence(EvidenceData evidenceData)
    {
        if (hasConfessed || isTalking) { yield break; }

        EvidenceData evidence = Data.KeyEvidence.FirstOrDefault(ev => ev.Name == evidenceData.Name);

        isTalking = true;

        if (evidence)
        {
            // Remove from the remaining key evidence list
            RemainingKeyEvidence.Remove(evidenceData.Name);
            OnKeyEvidenceShown?.Invoke(this, evidenceData);

            int i = Data.KeyEvidence.IndexOf(evidence);

            #region Shitty String Manipulation for Player Voice Prompt
            var voiceline = Data.KeyEvidenceVoicelines[i].name;
            voiceline = voiceline.Substring(0, voiceline.LastIndexOf("_RESPONSE"));
            voiceline = "PLAYER_" + voiceline;
            var playerVoiceline = DataManager.Instance.GetVoiceLineDataFromKey(voiceline);

            Debug.Log(playerVoiceline.name);

            yield return playerVoice.PlayAudio(playerVoiceline);
            #endregion

            VoiceLineData vld = Data.KeyEvidenceVoicelines[i];

            yield return PlayAudio(vld);

            if (RemainingKeyEvidence.Count == 0)
            {
                OnConfess?.Invoke(this);

                yield return PlayAudio(Data.ConfessionVoiceline);

                hasConfessed = true;
            }
        }
        else
        {
            yield return PlayAudio(DataManager.Instance.GetRandomGenericPlayerEvidenceVoiceline());
            yield return PlayAudio(Data.GenericEvidenceVoicelines[UnityEngine.Random.Range(0, Data.GenericEvidenceVoicelines.Count)]);

            OnGenericEvidenceShown?.Invoke(this);
        }

        isTalking = false;
    }

    public IEnumerator PlayAudio(VoiceLineData voicelineData)
    {
        StartCoroutine(voicelineSubtitles.ShowSubtitle(voicelineData));

        mouthAudioSource.clip = voicelineData.AudioClip;
        mouthAudioSource.Play();

        yield return new WaitForSeconds(mouthAudioSource.clip.length);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator PlayAudio(AudioClip clip)
    {
        mouthAudioSource.clip = clip;
        mouthAudioSource.Play();

        yield return new WaitForSeconds(mouthAudioSource.clip.length);
        yield return new WaitForSeconds(0.5f);
    }
}
