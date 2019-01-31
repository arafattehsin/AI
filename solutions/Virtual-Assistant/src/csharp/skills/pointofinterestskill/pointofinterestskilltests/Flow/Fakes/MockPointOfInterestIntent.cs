﻿using Luis;
using Microsoft.Bot.Builder;
using PointOfInterestSkillTests.Flow.Utterances;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PointOfInterestSkillTests.Flow.Fakes
{
    public class MockPointOfInterestIntent : PointOfInterest
    {
        public string userInput;
        private Intent intent;
        private double score;

        public MockPointOfInterestIntent(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                throw new ArgumentNullException(nameof(userInput));
            }

            this.Entities = new PointOfInterest._Entities();
            this.Intents = new Dictionary<Intent, IntentScore>();

            this.userInput = userInput;

            (intent, score) = ProcessUserInput();
        }

        private (Intent intent, double score) ProcessUserInput()
        {
            var intentScore = new Microsoft.Bot.Builder.IntentScore();
            intentScore.Score = 0.9909704;
            intentScore.Properties = new Dictionary<string, object>();

            switch (userInput.ToLower())
            {
                case "what's nearby?":
                    this.Intents.Add(PointOfInterest.Intent.NAVIGATION_FIND_POINTOFINTEREST, intentScore);
                    break;
                case "cancel my route":
                    this.Intents.Add(PointOfInterest.Intent.NAVIGATION_CANCEL_ROUTE, intentScore);
                    break;
                case "get directions to the pharmacy":
                    this.Entities.KEYWORD = new string[] { "pharmacy" };
                    this.Intents.Add(PointOfInterest.Intent.NAVIGATION_ROUTE_FROM_X_TO_Y, intentScore);
                    break;
                case "option 1":
                    this.Entities.number = new double[] { 1 };
                    break;
                default:
                    return (PointOfInterest.Intent.None, 0.0);
            }

            return this.TopIntent();
        }
    }
}
