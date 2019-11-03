// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly PizzaOrderRecognizer _luisRecognizer;
        protected readonly ILogger Logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(PizzaOrderRecognizer luisRecognizer, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with today?\nSay something like \"I'd like to order one large pepperoni pizza with extra mushrooms!\"";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                // LUIS is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }


            // Call LUIS and gather any potential booking details. (Note the TurnContext has the response to the prompt.)

            var luisResult = await _luisRecognizer.Predict(stepContext.Context.Activity.Text);
            
            switch (luisResult.Prediction.TopIntent)
            {
                case "ModifyOrder":
                    var pizzaOrder = PizzaOrder.Convert(luisResult.Prediction);
                    var pizzaResponseText = PizzaOrder.GetPizzaOrderString(pizzaOrder);
                    var responseCardAttachment = new HeroCard("Your order has been placed! ", null, pizzaResponseText, new List<CardImage>() { new CardImage(PizzaOrder.GetPizzaImage())}).ToAttachment();
                    
                    var chatActivity = Activity.CreateMessageActivity();
                    chatActivity.Attachments.Add(responseCardAttachment);
                    await stepContext.Context.SendActivityAsync(chatActivity);
                    break;

                case "Confirmation":
                    var confirmationMessage = MessageFactory.Text("That's great! We'll get right on it", null, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(confirmationMessage, cancellationToken);
                    break;

                case "CancelOrder":
                    var cancelMessage = MessageFactory.Text("Your order has been cancelled!", null, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                    break;

                case "Greetings":
                    var greetingsMessage = MessageFactory.Text("Hello!", null, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(greetingsMessage, cancellationToken);
                    break;

                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.Prediction.TopIntent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            //if (stepContext.Result is BookingDetails result)
            //{
            //    // Now we have all the booking details call the booking service.

            //    // If the call to the booking service was successful tell the user.

            //    var timeProperty = new TimexProperty(result.TravelDate);
            //    var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
            //    var messageText = $"I have you booked to {result.Destination} from {result.Origin} on {travelDateMsg}";
            //    var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
            //    await stepContext.Context.SendActivityAsync(message, cancellationToken);
            //}

            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
