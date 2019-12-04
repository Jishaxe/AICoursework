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

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Negate");
                scorer.negate = EditorGUILayout.Toggle(scorer.negate);
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Delete " + scorer.scorer.GetType().Name)) scorerToDelete = scorer;

                // if the user selected a new action
                if (chosenScorerPickerIndex != currentScorerIndex)
                {

                    string typeName = scorerPickerOptions[chosenScorerPickerIndex];

                    DestroyImmediate(scorer.scorer, true);
                    // dynamically instance the type chosen
                    scorer.scorer = (Scorer)ScriptableObject.CreateInstance(typeName);// (UtilityAction)Activator.CreateInstance(newActionType);
                    AssetDatabase.AddObjectToAsset(scorer.scorer, model);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }


                EditorGUILayout.Space();
            }

            if (scorerToDelete != null)
            {
                DestroyImmediate(scorerToDelete.scorer, true);
                action.scorers.Remove(scorerToDelete);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Add new scorer to " + action.action.GetType().Name))
            {
                ScorerAndTransformer newScorer = new ScorerAndTransformer();
                newScorer.scorer = CreateInstance<HasFlag>();
                action.scorers.Add(newScorer);

                //AssetDatabase.AddObjectToAsset(newScorer, model);
                AssetDatabase.AddObjectToAsset(newScorer.scorer, model);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Delete " + action.action.GetType().Name)) actionToDelete = action;
            EditorGUILayout.Space();

            // if the user selected a new action
            if (chosenActionPickerIndex != currentActionIndex)
            {

                string typeName = actionPickerOptions[chosenActionPickerIndex];
                DestroyImmediate(action.action, true);
                // dynamically instance the type chosen
                action.action = (UtilityAction)ScriptableObject.CreateInstance(typeName);// (UtilityAction)Activator.CreateInstance(newActionType);
               
                AssetDatabase.AddObjectToAsset(action.action, model);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        if (actionToDelete != null)
        {
            DestroyImmediate(actionToDelete.action, true);
            model.actions.Remove(actionToDelete);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("Add new action"))
        {
            ActionAndScorers newAction = new ActionAndScorers();
            newAction.action = CreateInstance<DoNothing>();

            model.actions.Add(newAction);
            

            //AssetDatabase.AddObjectToAsset(newAction, model);
            AssetDatabase.AddObjectToAsset(newAction.action, model);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        EditorUtility.SetDirty(model); //
        //serializedObject.ApplyModifiedProperties();
    }
}
