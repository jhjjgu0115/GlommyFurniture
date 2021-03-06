﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;
using Verse.AI;
using Harmony;
using System.Reflection;

namespace Gloomylynx
{
    public class PlaceWorker_WallAttachmentNearby : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
        {
            /*
             * 맵 이내여야함
             * 조리대 위치쪽도 맵이내여야함
             * 
             */
            //GenThing.TrueCenter(loc, rot, checkingDef.Size, checkingDef.Altitude)

            AcceptanceReport result = false;

            CellRect cellRect = GenAdj.OccupiedRect(loc, rot, checkingDef.Size);
            CellRect.CellRectIterator iterator = cellRect.GetIterator();
            bool needWallFlag = false;
            bool isInBound = true;
            while (!iterator.Done())
            {
                IntVec3 c = iterator.Current;
                IntVec3 addonTarget = c - rot.FacingCell;

                if(!GenGrid.Impassable(c, map))
                {
                    result = Translator.Translate("WallAttachment_WarningNeedWall");
                    needWallFlag = true;
                }
                if (!c.InBounds(map) || !addonTarget.InBounds(map))
                {
                    result = "OutOfBounds".Translate();
                    isInBound = false;
                }
                if ((c.InNoBuildEdgeArea(map) || addonTarget.InNoBuildEdgeArea(map)) && !DebugSettings.godMode)
                {
                    result = "TooCloseToMapEdge".Translate();
                    isInBound = false;
                }
                iterator.MoveNext();
            }
            if (!needWallFlag && isInBound)
            {
                result = true;
            }  
            return result;
        }
    }
}
