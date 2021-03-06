﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CalendarSkillTest.Flow.Utterances;
using Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Solutions.Telemetry;

namespace CalendarSkillTest.Flow.Fakes
{
    public class MockLuisRecognizer : ITelemetryLuisRecognizer
    {
        private BaseTestUtterances utterancesManager;
        private GeneralTestUtterances generalUtterancesManager;

        public MockLuisRecognizer(BaseTestUtterances utterancesManager)
        {
            this.utterancesManager = utterancesManager;
        }

        public MockLuisRecognizer()
        {
            this.generalUtterancesManager = new GeneralTestUtterances();
        }

        public bool LogPersonalInformation => throw new NotImplementedException();

        public Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken)
            where T : IRecognizerConvert, new()
        {
            T mockResult = new T();

            Type t = typeof(T);
            var text = turnContext.Activity.Text;
            if (t.Name.Equals(typeof(CalendarLU).Name))
            {
                CalendarLU mockCalendar = utterancesManager.GetValueOrDefault(text, utterancesManager.GetBaseNoneIntent());

                var test = mockCalendar as object;
                mockResult = (T)test;
            }
            else if (t.Name.Equals(typeof(General).Name))
            {
                General mockGeneralIntent = generalUtterancesManager.GetValueOrDefault(text, generalUtterancesManager.GetBaseNoneIntent());

                var test = mockGeneralIntent as object;
                mockResult = (T)test;
            }

            return await Task.FromResult(mockResult);
        }

        public Task<T> RecognizeAsync<T>(DialogContext dialogContext, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new()
        {
            throw new NotImplementedException();
        }
    }
}