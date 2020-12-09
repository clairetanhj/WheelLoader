#if UNITY_EDITOR || UNITY_EDITOR_BETA
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using u040.prespective.utility.editor;
using u040.prespective.referenceobjects.materialhandling.beltsystem;

public class ReferenceObjectsIconDefinitions : HierarchyWindowIconRuleAdditions
{
    [ObfuscationAttribute(Exclude = true, StripAfterObfuscation = false)]
    public override List<HierarchyWindowIconClassRules> AddedIconRules
    {
        get
        {
            return new List<HierarchyWindowIconClassRules>()
                {
                new HierarchyWindowIconClassRules(typeof(BeltSystem),
                new Vector2Int(101, 1),
                new HierarchyWindowIconRule[]
                {
                new HierarchyWindowIconRule(
                    "Assets/Plugins/PREspective/Icons/BeltSystem_Icon_20x20.png",
                    new Vector2Int(20, 20),
                    new Func<UnityEngine.Object, bool>((_script)=>{
                        return true;
                        }))

                }),
            };
        }
    }
}
#endif
