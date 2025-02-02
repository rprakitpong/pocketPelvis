﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ParserForAll {

    //Parse all relevant .txt from Resources folder into public variables for other scripts to get from.

    public Dictionary<string, List<string>> SortStructureNamesByGroups { get; set; }
    public Dictionary<string, List<string>> SortGroupsByStructureNames { get; set; }
    public OrderedDictionary SortGroupsByInteractionTypes_ordered { get; set; }
    public Dictionary<string, List<string>> SortInteractionTypesByGroups { get; set; }
    public Dictionary<string, AudioClip> SortAudioNamesByStructureNames { get; set; }
    public Dictionary<string, AudioClip> SortSFXByNames { get; set; }
    public OrderedDictionary SortOnOffStatesByRoomTypes { get; set; }
    public OrderedDictionary SortGuideStepsByGuideRooms_ordered { get; set; }
    public OrderedDictionary SortStructuresByGuideSteps_ordered { get; set; }
    public Dictionary<string, List<string>> SortStructuresByGuideSteps { get; set; }
    public Dictionary<string, string> SortTextInfoByStructure { get; set; }

    private static readonly ParserForAll INSTANCE = new ParserForAll();

    private ParserForAll()
    {
        SortStructureNamesByGroups = ParseStructureNamesByGroups();
        SortGroupsByStructureNames = ParseGroupsByStructureNames();
        SortInteractionTypesByGroups = ParseInteractionTypesByGroups();
        SortGroupsByInteractionTypes_ordered = ParseGroupsByInteractionTypes_ordered();
        SortAudioNamesByStructureNames = ParseAudioNamesByStructureNames();
        SortSFXByNames = ParseSFXByNames();
        SortOnOffStatesByRoomTypes = ParseRoomOnOff();
        SortGuideStepsByGuideRooms_ordered = ParseGuideSteps_linkedlist();
        SortStructuresByGuideSteps_ordered = ParseGuidedStructures_ordered();
        SortStructuresByGuideSteps = ParseGuidedStructures();
        SortTextInfoByStructure = ParseTextInfo();

    }

    public static ParserForAll Instance
    {
        get
        {
            return INSTANCE;
        }
    }

    private Dictionary<string, List<string>> ParseStructureNamesByGroups()
    {
        Dictionary<string, List<string>> parsedGroups = new Dictionary<string, List<string>>();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("SortStructureByGroups");
        String structureGroupsRaw = structureGroupsTextAsset.text;
        string[] structures = structureGroupsRaw.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            parsedGroups.Add(stringTrimmer(structureGroups[0]), LOSTrimmer(structureGroups[1].Split(',').ToList()));
        }
        return parsedGroups;
    }

    private Dictionary<string, List<string>> ParseGroupsByStructureNames()
    {
        Dictionary<string, List<string>> structureGroup = new Dictionary<string, List<string>>();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("SortStructureByGroups");
        string[] structures = structureGroupsTextAsset.text.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            foreach (string group in structureGroups[1].Split(','))
            {
                string groupTrimmed = stringTrimmer(group);
                if (structureGroup.ContainsKey(groupTrimmed))
                {
                    structureGroup[groupTrimmed].Add(stringTrimmer(structureGroups[0]));
                }
                else
                {
                    List<string> structuresInGroup = new List<string>();
                    structuresInGroup.Add(stringTrimmer(structureGroups[0]));
                    structureGroup.Add(groupTrimmed, structuresInGroup);
                }
            }
        }
        return structureGroup;
    }

    private Dictionary<string, List<string>> ParseInteractionTypesByGroups()
    {
        Dictionary<string, List<string>> structureGroup = new Dictionary<string, List<string>>();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("SortGroupsByInteractionTypes");
        string[] structures = structureGroupsTextAsset.text.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            foreach (string group in structureGroups[1].Split(','))
            {
                string groupTrimmed = stringTrimmer(group);
                if (structureGroup.ContainsKey(groupTrimmed))
                {
                    structureGroup[groupTrimmed].Add(stringTrimmer(structureGroups[0]));
                }
                else
                {
                    List<string> structuresInGroup = new List<string>();
                    structuresInGroup.Add(stringTrimmer(structureGroups[0]));
                    structureGroup.Add(groupTrimmed, structuresInGroup);
                }
            }
        }

        return structureGroup;
    }

    private OrderedDictionary ParseGroupsByInteractionTypes_ordered()
    {
        OrderedDictionary interactionAndGroupings = new OrderedDictionary();

        TextAsset structureGroupsSortingTextAsset = Resources.Load<TextAsset>("SortGroupsByInteractionTypes");
        string[] groupsSortings = structureGroupsSortingTextAsset.text.Split('\n');
        foreach (string groupsSorting in groupsSortings)
        {
            string[] sortingAndCorrespondingGroups = groupsSorting.Split(':');
            interactionAndGroupings.Add(stringTrimmer(sortingAndCorrespondingGroups[0]), LOSTrimmer(sortingAndCorrespondingGroups[1].Split(',').ToList()));
        }

        //interactionAndGroupings.Add("GAZELIGHT", new List<string>()); //NOTE gazelight is disabled for now
        interactionAndGroupings.Add("SEARCH", new List<string>()); //NOTE search function is hardcoded in
        interactionAndGroupings.Add("GUIDE", new List<string>()); //NOTE guide mode is hardcoded, its functionality is handled differently than structure groups

        return interactionAndGroupings;
    }
    
    private Dictionary<string, AudioClip> ParseAudioNamesByStructureNames()
    {
        Dictionary<string, AudioClip> parsedGroups = new Dictionary<string, AudioClip>();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("StructureNameAudio/StructureNameAndAudio");
        String structureGroupsRaw = structureGroupsTextAsset.text;
        string[] structures = structureGroupsRaw.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            if (stringTrimmer(structureGroups[1]) != "None")
            {
                string fileLocation = "StructureNameAudio/" + stringTrimmer(structureGroups[1]);
                AudioClip audio = Resources.Load<AudioClip>(fileLocation);
                parsedGroups.Add(stringTrimmer(structureGroups[0]), audio);
            }
        }
        return parsedGroups;
    }


    private Dictionary<string, AudioClip> ParseSFXByNames()
    {
        Dictionary<string, AudioClip> parsedGroups = new Dictionary<string, AudioClip>();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("SFXAudio/SortSFXByNames");
        String structureGroupsRaw = structureGroupsTextAsset.text;
        string[] structures = structureGroupsRaw.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            if (stringTrimmer(structureGroups[1]) != "None")
            {
                string fileLocation = "SFXAudio/" + stringTrimmer(structureGroups[1]);
                AudioClip audio = Resources.Load<AudioClip>(fileLocation);
                parsedGroups.Add(stringTrimmer(structureGroups[0]), audio);
            }
        }
        return parsedGroups;
    }

    private OrderedDictionary ParseRoomOnOff()
    {
        OrderedDictionary parsedGroups = new OrderedDictionary();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("SortOnOffStatesByRoomTypes");
        String structureGroupsRaw = structureGroupsTextAsset.text;
        string[] structures = structureGroupsRaw.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            parsedGroups.Add(stringTrimmer(structureGroups[0]), Convert.ToBoolean(stringTrimmer(structureGroups[1])));
        }
        return parsedGroups;
    }

    private OrderedDictionary ParseGuideSteps_linkedlist()
    {
        OrderedDictionary interactionAndGroupings = new OrderedDictionary();

        TextAsset structureGroupsSortingTextAsset = Resources.Load<TextAsset>("SortGuideStepsByGuideRooms");
        string[] groupsSortings = structureGroupsSortingTextAsset.text.Split('\n');
        foreach (string groupsSorting in groupsSortings)
        {
            string[] sortingAndCorrespondingGroups = groupsSorting.Split(':');
            interactionAndGroupings.Add(stringTrimmer(sortingAndCorrespondingGroups[0]), new LinkedList<string>(LOSTrimmer(sortingAndCorrespondingGroups[1].Split(',').ToList())));
        }

        return interactionAndGroupings;
    }

    private OrderedDictionary ParseGuidedStructures_ordered()
    {
        OrderedDictionary interactionAndGroupings = new OrderedDictionary();

        TextAsset structureGroupsSortingTextAsset = Resources.Load<TextAsset>("SortStructuresByGuideSteps");
        string[] groupsSortings = structureGroupsSortingTextAsset.text.Split('\n');
        foreach (string groupsSorting in groupsSortings)
        {
            string[] sortingAndCorrespondingGroups = groupsSorting.Split(':');
            interactionAndGroupings.Add(stringTrimmer(sortingAndCorrespondingGroups[0]), LOSTrimmer(sortingAndCorrespondingGroups[1].Split(',').ToList()));
        }

        return interactionAndGroupings;
    }

    private Dictionary<string, List<string>> ParseGuidedStructures()
    {
        Dictionary<string, List<string>> parsedGroups = new Dictionary<string, List<string>>();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("SortStructuresByGuideSteps");
        String structureGroupsRaw = structureGroupsTextAsset.text;
        string[] structures = structureGroupsRaw.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            parsedGroups.Add(stringTrimmer(structureGroups[0]), LOSTrimmer(structureGroups[1].Split(',').ToList()));
        }
        return parsedGroups;
    }


    private Dictionary<string, string> ParseTextInfo()
    {
        Dictionary<string, string> parsedGroups = new Dictionary<string, string>();
        TextAsset structureGroupsTextAsset = Resources.Load<TextAsset>("SortTextInfoByStructure");
        String structureGroupsRaw = structureGroupsTextAsset.text;
        string[] structures = structureGroupsRaw.Split('\n');
        foreach (string structure in structures)
        {
            string[] structureGroups = structure.Split(':');
            parsedGroups.Add(stringTrimmer(structureGroups[0]), stringTrimmer(structureGroups[1]));
        }
        return parsedGroups;
    }

    // helpers
    public string stringTrimmer(string str)
    {
        str = new string(str.Where(c => !char.IsControl(c)).ToArray());
        return str;
    }

    public List<string> LOSTrimmer(List<string> los)
    {
        List<string> trimmed_los = new List<string>();
        foreach (string str in los)
        {
            string trimmed_str = stringTrimmer(str);
            trimmed_los.Add(trimmed_str);
        }
        return trimmed_los;
    }
}
