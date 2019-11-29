using Assets.Scripts;
using Assets.Scripts.Actions;
using Assets.Scripts.Scorers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UtilityAIModel))]
public class UtilityAiModelEditor : Editor
{
    UtilityAIModel model;
    List<string> actionPickerOptions = new List<string>();
    List<string> scorerPickerOptions = new List<string>();

    public void OnEnable()
    {
        model = (UtilityAIModel)target;

        // Convert the possible Action Types to strings to display in dropdown box
   
        foreach (Type actionType in UtilityAIModel.actionTypes)
        {
            actionPickerOptions.Add(actionType.Name);
        }

       
        foreach (Type scorerType in UtilityAIModel.scorerTypes)
        {
            scorerPickerOptions.Add(scorerType.Name);
        }
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        ActionAndScorers actionToDelete = null;

        // list all the actions
        foreach (ActionAndScorers action in model.actions)
        {
            if (action.action != null) { 
                EditorGUILayout.LabelField(action.action.GetType().Name + " with " + action.scorers.Count + " scorers", EditorStyles.boldLabel);
            } else {
                EditorGUILayout.LabelField("Undefined action");
                continue;//
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Action");
            int currentActionIndex = actionPickerOptions.IndexOf(action.action.GetType().Name);
            int chosenActionPickerIndex = EditorGUILayout.Popup(currentActionIndex, actionPickerOptions.ToArray());
            EditorGUILayout.EndHorizontal();

            ScorerAndTransformer scorerToDelete = null;

            foreach (ScorerAndTransformer scorer in action.scorers)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scorer");//

                int currentScorerIndex = scorerPickerOptions.IndexOf(scorer.scorer.GetType().Name);
                int chosenScorerPickerIndex = EditorGUILayout.Popup(currentScorerIndex, scorerPickerOptions.ToArray());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Multiplier");
                scorer.multiplier = EditorGUILayout.FloatField(scorer.multiplier);
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Delete " + scorer.scorer.GetType().Name)) scorerToDelete = scorer;

                // if the user selected a new action
                if (chosenScorerPickerIndex != currentScorerIndex)
                {

                    string typeName = scorerPickerOptions[chosenScorerPickerIndex];

                    // dynamically instance the type chosen
                    scorer.scorer = (Scorer)ScriptableObject.CreateInstance(typeName);// (UtilityAction)Activator.CreateInstance(newActionType);
                }


                EditorGUILayout.Space();
            }

            if (scorerToDelete != null) action.scorers.Remove(scorerToDelete);

            if (GUILayout.Button("Add new scorer to " + action.action.GetType().Name))
            {
                ScorerAndTransformer newScorer = CreateInstance<ScorerAndTransformer>(); ;
                newScorer.scorer = new HasFlag();
                action.scorers.Add(newScorer);
            }

            if (GUILayout.Button("Delete " + action.action.GetType().Name)) actionToDelete = action;
            EditorGUILayout.Space();

            // if the user selected a new action
            if (chosenActionPickerIndex != currentActionIndex)
            {

                string typeName = actionPickerOptions[chosenActionPickerIndex];

                // dynamically instance the type chosen
                action.action = (UtilityAction)ScriptableObject.CreateInstance(typeName);// (UtilityAction)Activator.CreateInstance(newActionType);
            }
        }

        if (actionToDelete != null) model.actions.Remove(actionToDelete);

        if (GUILayout.Button("Add new action"))
        {
            ActionAndScorers newAction = CreateInstance<ActionAndScorers>();
            newAction.action = new DoNothing();
            model.actions.Add(newAction);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(model);//
            AssetDatabase.SaveAssets();
        }
        //serializedObject.ApplyModifiedProperties();
    }
}
