using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
using SoftCircuits.HtmlMonkey;

[CustomEditor(typeof(DialogueStage))]
public class DialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueStage dialogueEntry = (DialogueStage)target;

        DrawDefaultInspector();
        EditorGUILayout.LabelField("Load From Twine:");

        EditorGUILayout.BeginHorizontal();
        dialogueEntry.linkedFilename = EditorGUILayout.TextField("Twine file", dialogueEntry.linkedFilename);
        if (GUILayout.Button("Open File"))
        {
            dialogueEntry.linkedFilename = EditorUtility.OpenFilePanel("Update from Twine", "", "html");
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Update"))
        {
            UpdateTarget();
        }
        this.Repaint();
    } 

    class DialogueTreeNode
    {
        public string name;
        public int pid;
        public string text;
        public bool passed;
        public DialogueStage dialogueStage;
        public DialogueStage parent;
    }  


    // Goal: O(n)
    private void UpdateTarget()
    {
        DialogueStage dialogueEntry = (DialogueStage)target;

        string oldAssetName = dialogueEntry.gameObject.name;

        NotebookMethods notebookWrapper = dialogueEntry.gameObject.GetComponent<NotebookMethods>();
        if (notebookWrapper == null)
        {
            notebookWrapper = dialogueEntry.gameObject.AddComponent<NotebookMethods>();
        }

        HtmlDocument document = HtmlDocument.FromFile(dialogueEntry.linkedFilename);

        // Extract filename from filepath
        string[] splitPath = dialogueEntry.linkedFilename.Split('/', '\\');
        string filename = splitPath[splitPath.Length - 1].Split('.')[0];

        // Yeah I couldn't figure out how to get the first one so this will have to do
        int startNode = 0;
        foreach(HtmlElementNode n in document.Find("tw-storydata"))
        {
            startNode = System.Int32.Parse(n.Attributes["startnode"].Value);
            break;
        }


        // Catalog all the nodes that are in the tree by name
        List<DialogueTreeNode> nodeQueue = new List<DialogueTreeNode>();
        Hashtable nodeTable = new Hashtable();
        foreach (HtmlElementNode n in document.Find("tw-passagedata"))
        {
            DialogueTreeNode thisNode = new DialogueTreeNode();
            thisNode.name = n.Attributes["name"].Value.Replace("&#39;", "'");
            thisNode.pid = System.Int32.Parse(n.Attributes["pid"].Value);
            thisNode.text = n.InnerHtml.Replace("&#39;", "'"); // Replace apostrophes
            thisNode.passed = false;
            thisNode.dialogueStage = null;
            nodeTable.Add(thisNode.name, thisNode);
            if (thisNode.pid == startNode)
            {
                nodeQueue.Add(thisNode);
            }
        }

        // Collect all the existing DialogueEntries and link them to their tree nodes
        // Destroy any that are unused, DON'T DESTROY THE TOP LEVEL NODE!!
        foreach (DialogueStage stage in dialogueEntry.GetComponentsInChildren<DialogueStage>())
        {
            if (stage == dialogueEntry)
            {
                DialogueTreeNode firstNode = nodeQueue[0];
                firstNode.dialogueStage = stage;
                //nodeQueue[0].passed = true;
                continue;
            }

            //if (thisNode == null)
            if(!nodeTable.Contains(stage.gameObject.name))
            {
                // TODO: handle children
                while (stage.transform.childCount > 0)
                {
                    stage.transform.GetChild(0).parent = stage.transform.parent;
                }

                GameObject.DestroyImmediate(stage.gameObject);
                continue;
            }
            DialogueTreeNode thisNode = (DialogueTreeNode)nodeTable[stage.gameObject.name];
            thisNode.dialogueStage = stage;
        }

        // Traverse the tree and create it
        //DialogueTreeNode lastNode = nodeQueue[0];
        bool firstDone = false;
        while (nodeQueue.Count > 0)
        {
            DialogueTreeNode thisNode = nodeQueue[0];
            nodeQueue.RemoveAt(0);


            if (!firstDone)
            {
                //AssetDatabase.RenameAsset("Assets/Prefabs/Dialogue/" + thisNode.dialogueStage.gameObject.name + ".prefab", filename);
                //string[] assets = AssetDatabase.FindAssets("Assets/Prefabs/Dialogue/" + thisNode.dialogueStage.gameObject.name);
                thisNode.dialogueStage.gameObject.name = filename;
                firstDone = true;
            }
            else
            {
                // TODO: Moved this down. Do I need this here anymore?
                if(thisNode.dialogueStage == null)
                {
                    //thisNode.dialogueStage = new DialogueStage();
                    GameObject go = new GameObject();
                    thisNode.dialogueStage = go.AddComponent<DialogueStage>();
                }
                //Debug.Log(thisNode.dialogueStage);
                thisNode.dialogueStage.gameObject.name = thisNode.name;
                //thisNode.dialogueStage.transform.parent = lastNode.dialogueStage.transform;
                //Debug.Log(thisNode.parent.gameObject.name);
                //thisNode.dialogueStage.transform.parent = thisNode.parent.transform.parent;
                GameObjectUtility.SetParentAndAlign(thisNode.dialogueStage.gameObject, thisNode.parent.gameObject);
            }

            // Parse text
            int responseIndex = thisNode.text.IndexOf('[');
            string[] responseStrings;
            if (responseIndex != -1)
            {
                thisNode.dialogueStage.prompt = thisNode.text.Substring(0, responseIndex);
                responseStrings = thisNode.text.Substring(responseIndex).Split('\n');
            }
            else
            {
                thisNode.dialogueStage.prompt = thisNode.text;
                responseStrings = new string[0];
            }
            if (responseStrings.Length == 0)
            {
                DialogueStage.DialogueResponse response = new DialogueStage.DialogueResponse();
                response.response = "*Hang up*";
                thisNode.dialogueStage.responses = new DialogueStage.DialogueResponse[1];
                thisNode.dialogueStage.responses[0] = response;
            }
            else
            {
                List<DialogueStage.DialogueResponse> responseList = new List<DialogueStage.DialogueResponse>();
                foreach (string r in responseStrings)
                {
                    DialogueStage.DialogueResponse response = new DialogueStage.DialogueResponse();
                    string[] split = r.Split("]]");
                    string text = split[0].Replace("[", "");
                    if (text.Length == 0)
                    {
                        continue;
                    }
                    response.response = text;
                    response.onContinue = new UnityEvent();

                    if (split.Length > 1)
                    {
                        string flags = split[1];
                        while (flags.Length > 0)
                        //while (false)
                        {
                            if (flags[0] == ' ')
                            {
                                flags = flags.Substring(1);
                                continue;
                            }

                            string f = flags.Substring(0, 2);
                            string[] splitData = flags.Split('(', ')');
                            flags = flags.Substring(flags.IndexOf(')')+1);
                            string data = "";
                            if (splitData.Length > 1)
                            {
                                data = splitData[1];
                            }

                            if (f == "cr")
                            {
                                response.requiredClue = data;
                            }
                            else if (f == "cs")
                            {
                                // TODO: make this differnt from cr
                                response.requiredClue = data;
                            }
                            else if (f == "c+")
                            {
                                UnityEventTools.AddStringPersistentListener(response.onContinue, notebookWrapper.DiscoverClue, data);
                            }
                            else if (f == "c-")
                            {
                                UnityEventTools.AddStringPersistentListener(response.onContinue, notebookWrapper.UndiscoverClue, data);
                            }
                            else if (f == "p+")
                            {
                                UnityEventTools.AddStringPersistentListener(response.onContinue, notebookWrapper.DiscoverPhoneNumber, data);
                            }
                            else if (f == "p-")
                            {
                                UnityEventTools.AddStringPersistentListener(response.onContinue, notebookWrapper.UndiscoverPhoneNumber, data);
                            }
                            else if (f == "pr")
                            {
                                UnityEventTools.AddStringPersistentListener(response.onContinue, notebookWrapper.RevealPhoneName, data);
                                Debug.Log("This dialogue can reveal " + data + "'s phone number. Please make sure that the name is hidden in the scene to begin with.");
                            }
                            else if (f == "pc")
                            {
                                UnityEventTools.AddStringPersistentListener(response.onContinue, notebookWrapper.Call, data);
                            }
                            else if (f == "tc")
                            {
                                UnityEventTools.AddStringPersistentListener(response.onContinue, notebookWrapper.ChangeDialogueTree, data);
                            }
                        }
                    }
                    // Had some errors but the tree was correct
                    // I believe this guards against extra newlines
                    /*
                    if (text.Length == 0)
                    {
                        continue;
                    }
                    */
                    //Debug.Log(text);
                    DialogueTreeNode nextNode = (DialogueTreeNode)nodeTable[text];
                    /*
                    if (nextNode == null)
                    {
                        continue;
                    }
                    */
                    //Debug.Log(nextNode);
                    //if (nextNode != null)
                    //{

                    if(nextNode.dialogueStage == null)
                    {
                        //thisNode.dialogueStage = new DialogueStage();
                        GameObject go = new GameObject();
                        nextNode.dialogueStage = go.AddComponent<DialogueStage>();
                    }
                    response.nextStage = nextNode.dialogueStage;
                    if (!nextNode.passed)
                    {
                        nextNode.parent = thisNode.dialogueStage;
                        nodeQueue.Add(nextNode);
                    }
                    //}
                    
                    responseList.Add(response);
                }
                thisNode.dialogueStage.responses = responseList.ToArray();
            }

            thisNode.passed = true;
            //lastNode = thisNode;
        }


        // TODO: need to do this but it breaks everything else somehow
        //AssetDatabase.RenameAsset("Assets/Prefabs/Dialogue/" + oldAssetName + ".prefab", filename);
    }
}