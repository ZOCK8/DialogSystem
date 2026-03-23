using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
using System.Collections;
using UnityEngine.UI; 

public class HighestNumberAgent : Agent
{
    // Konfiguration
    public TextMeshProUGUI TextTask;
    public Button YesButton;
    public Button NoButton;
    private const int NUMBER_COUNT = 4; // Aktions-Größe: 0, 1, 2, 3 (4 Aktionen)

    private bool decisionRequested = false;
    private bool userInputReceived = false; 
    
    // Beobachtungen
    private int SendinWithNoYes; // Zähler für aufeinanderfolgendes "Nein" (Input 1)
    public float Agresation;     // Der aktuelle Aggressionslevel (Input 2, im Inspector sichtbar)

    // --- 1. Episodenstart (Reset) ---
    public override void OnEpisodeBegin()
    {
        TextTask.color = Color.white;
        userInputReceived = false;
        decisionRequested = false;
        
        // 🛑 KORREKTUR 1: Beobachtungen initialisieren.
        SendinWithNoYes = 0;
        
        // Aggressionslevel auf einen neutralen Startwert (0.0 oder 0.5) setzen. 
        // WICHTIG: Sollte im kontrollierten Bereich bleiben (z.B. 0.0 bis 1.0)
        Agresation = 0.5f; 
        
        // Listener bereinigen
        YesButton.onClick.RemoveAllListeners();
        NoButton.onClick.RemoveAllListeners();
    }

    // --- 2. Entscheidungs-Timing ---
    public void FixedUpdate()
    {
        if (!decisionRequested)
        {
            RequestDecision();
            decisionRequested = true;
        }
    }

    // --- 3. Beobachtungen senden (Input) ---
    public override void CollectObservations(VectorSensor sensor)
    {
        // Beobachtung 1: Wie oft wurde in Folge "Nein" geklickt (Ganzer Zahl)
        sensor.AddObservation(SendinWithNoYes);
        
        // Beobachtung 2: Aggressionslevel (Float, auf 0.0 bis 1.0 begrenzen)
        sensor.AddObservation(Agresation); 
    }

    // --- 4. Aktionen empfangen und Belohnung geben (Output & Reward) ---
    public override void OnActionReceived(ActionBuffers actions)
    {
        int chosenActionIndex = actions.DiscreteActions[0];
        string recommendation = "";

        // 🛑 KORREKTUR 2: Logik für die Aggression (Agresation) anpassen.
        // Der Agent lernt, dass aggressives Handeln die Aggression steigert.
        
        switch (chosenActionIndex)
        {
            case 0:
                recommendation = "Go maybe to the left";
                Agresation -= 0.15f; // Reduziert Aggression (weniger aggressiv)
                break;
            case 1:
                recommendation = "I would recommend going to the left";
                Agresation -= 0.05f; // Reduziert Aggression leicht
                break;
            case 2:
                recommendation = "Go to the Left";
                Agresation += 0.1f; // Steigert Aggression leicht
                break;
            case 3:
                recommendation = "GO TO THE LEFT NOW!!!";
                Agresation += 0.3f; // Steigert Aggression stark
                break;
            default:
                recommendation = "Ungültige Aktion";
                SetReward(-0.5f);
                StartCoroutine(WaitForUserInput(chosenActionIndex));
                return;
        }
        
        // 🛑 KORREKTUR 3: Aggressionslevel begrenzen, damit er nicht außer Kontrolle gerät.
        Agresation = Mathf.Clamp(Agresation, 0.0f, 1.0f);

        TextTask.text = recommendation;
        TextTask.color = Color.yellow; 

        // Starte Warte-Routine
        StartCoroutine(WaitForUserInput(chosenActionIndex));
    }

    // --- 5. Korrigierte Warte-Routine für Benutzereingabe ---
    IEnumerator WaitForUserInput(int chosenActionIndex)
    {
        // 🛑 KORREKTUR 4: Setzen des Zählers für aufeinanderfolgendes "Nein".
        // Wenn "Yes" geklickt wird (positive Belohnung), wird SendinWithNoYes auf 0 zurückgesetzt.
        // Wenn "No" geklickt wird (negative Belohnung), wird SendinWithNoYes inkrementiert.
        YesButton.onClick.AddListener(() => { OnFeedbackReceived(1.0f); SendinWithNoYes = 0; });
        NoButton.onClick.AddListener(() => { OnFeedbackReceived(-0.1f); SendinWithNoYes += 1; });

        yield return new WaitUntil(() => userInputReceived);

        // 🛑 KORREKTUR 5: Kürzere Wartezeit für schnelleres Training.
        yield return new WaitForSeconds(2f); 

        EndEpisode();
    }

    // --- 6. Feedback-Verarbeitung ---
    private void OnFeedbackReceived(float rewardValue)
    {
        rewardValue += Agresation;
        SetReward(rewardValue);
        TextTask.color = (rewardValue > 0) ? Color.green : Color.red;

        userInputReceived = true; 
        YesButton.onClick.RemoveAllListeners();
        NoButton.onClick.RemoveAllListeners();
    }
}