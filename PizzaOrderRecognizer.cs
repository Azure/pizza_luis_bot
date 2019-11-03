// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
//using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime; //This sample uses the LUIS Runtime v3 SDK which includes the new features, not currently available in the Bot Builder LUIS SDK as of 11/4/2019
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.BotBuilderSamples
{
    public class PizzaOrderRecognizer
    {
        private readonly LUISRuntimeClient _recognizer;
        private readonly Guid appId;

        public PizzaOrderRecognizer(IConfiguration configuration)
        {
            var luisIsConfigured = !string.IsNullOrEmpty(configuration["LuisAppId"]) && !string.IsNullOrEmpty(configuration["LuisAPIKey"]) && !string.IsNullOrEmpty(configuration["LuisAPIHostName"]);
            if (luisIsConfigured)
            {
                 appId = new Guid(configuration["LuisAppId"]);
                _recognizer = new LUISRuntimeClient(new ApiKeyServiceClientCredentials(configuration["LuisAPIKey"]));
                _recognizer.Endpoint = "https://" + configuration["LuisAPIHostName"];
            }
        }

        // Returns true if luis is configured in the appsettings.json and initialized.
        public virtual bool IsConfigured => _recognizer != null;

        public async Task<PredictionResponse> Predict(string query)
        {
            PredictionRequest predictionRequest = new PredictionRequest(query);
            return await _recognizer.Prediction.GetSlotPredictionAsync(appId, "Production", predictionRequest, true, true);
        }
    }
}
