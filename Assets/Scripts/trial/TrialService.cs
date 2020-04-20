﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using contingency;
using data;
using UnityEngine;
using value;

namespace trial
{
    public class TrialService : ITrialService
    {
        private readonly Data _data;

        private TrialService(Data data)
        {
            _data = data;
        }

        public static TrialService Create()
        {
            return new TrialService(DataSingleton.GetData());
        }

        private Dictionary<TrialId, Data.Contingency> ConstructContingencyByTrialMap(BlockId blockId)
        {
            var contingencies = _data.Blocks[blockId.Value].Contingencies;
            
            var contingencyByTrial = new Dictionary<TrialId, Data.Contingency>();

            foreach (var contingency in contingencies)
            {
                foreach (var trialId in contingency.ForTrials.Select(x => new TrialId(x)))
                {
                    contingencyByTrial[trialId] = contingency;
                }
            }

            return contingencyByTrial;
        }

        public void GenerateAllStartingTrials(AbstractTrial currentTrial)
        {
            foreach (var blockDisplayIndex in _data.BlockOrder)
            {
                var blockId = new BlockId(blockDisplayIndex);
                var block = _data.Blocks[blockId.Value];
                var newBlock = true;
                AbstractTrial currHead = null;
                
                var trialCount = 0;
                var trialContingenciesForBlock =
                    ConstructContingencyByTrialMap(blockId);
                foreach (var trialDisplayIndex in block.TrialOrder)
                {
                    AbstractTrial newTrial;
                    var trialId = new TrialId(trialDisplayIndex);
                    switch (trialId.Value)
                    {
                        // Here we decide what each trial is, I guess we could do this with a function map, but later. 
                        // here we have a picture as a trial.
                        case -1:
                            newTrial = new RandomTrial(blockId);
                            break;
                        default:
                            var newTrialData = _data.Trials[trialId.Value];
                            newTrial = GenerateBasicTrialFromConfig(blockId, trialId, newTrialData);
                            break;
                    }

                    if (trialContingenciesForBlock.ContainsKey(trialId))
                    {
                        // allows the trial to repeat itself even if it doesn't generate a new trial.
                        newTrial.StartOfContingency = newTrial;
                        newTrial
                            .SetContingency(ContingencyService.Create(trialContingenciesForBlock[trialId], newTrial));
                    }
                    

                    if (newBlock) currHead = newTrial;

                    newTrial.isTail = trialCount == block.TrialOrder.Count - 1;
                    newTrial.head = currHead;

                    currentTrial.next = newTrial;
                    currentTrial = currentTrial.next;

                    newBlock = false;
                    trialCount++;
                }

                currentTrial.next = new CloseTrial();
            }
        }

        /**
         * A basic trial is a trial with a trialIdx and trialLocation
         */
        public AbstractTrial GenerateBasicTrialFromConfig(BlockId blockId, TrialId trialId,
            Data.Trial trialDataFromIndex)
        {
            // Control flow here is for deciding what Trial gets spat out from the config
            AbstractTrial currTrial;
            if (trialDataFromIndex.FileLocation != null)
            {
                Debug.Log("Creating new Instructional Trial");
                currTrial = new InstructionalTrial(blockId, trialId);
            }
            else if (trialDataFromIndex.TwoDimensional == 1)
            {
                Debug.Log("Creating new 2D Screen Trial");
                currTrial = new TwoDTrial(blockId, trialId);
            }
            else
            {
                Debug.Log("Creating new 3D Screen Trial");
                currTrial = new ThreeDTrial(blockId, trialId);
            }
            
            return currTrial;
        }
    }
}