# LUIS Pizza Bot

LUIS Pizza Bot demoed at Ignite 2019! 

## Prerequisites

This sample **requires** prerequisites in order to run.

### Overview

This bot uses [LUIS](https://www.luis.ai), an AI based cognitive service, to implement language understanding.

### Install .NET Core CLI

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 2.1

  ```bash
  # determine dotnet version
  dotnet --version
  ```

### Import the Pizza Bot LUIS application in luis.ai

Learn how to import an application in LUIS [here](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-how-to-start-new-app#import-an-app-from-file).

Use the application **MicrosoftPizza.json** path: *CognitiveModels/MicrosoftPizza.json*

**Train** then **Publish** your application to the **Production** Slot. 

*You can alternatively use the LUIS Test pane to directly query the Pizza Application without using the bot. You can see the examples used to train the pizza order extractor in the Intent: Modify Order*

### Change appsettings.json to include your LUISAppId and LUISApiKey 

Go to *appsettings.json*
- Change **LUISAppId** to your LUIS Application Id (Go to luis.ai --> Open the application --> Go to Manage --> Application Settings --> Copy App Id --> Paste it in the value of the LUISAppId key)

- Change **LUISApiKey** to your LUIS Starter Key (Go to luis.ai --> Open the application --> Go to Manage --> Azure Resources --> Copy Primary Key under Starter_Key --> Paste it in the value of the LUISAPIKey)


## To try this sample

- Clone the repository

    ```bash
    git clone https://github.com/Azure/pizza_bot_luis.git
    ```

- In a terminal, navigate to the project directory.
- Run the bot from a terminal or from Visual Studio, choose option A or B.

  A) From a terminal

  ```bash
  # run the bot
  dotnet run
  ```

  B) Or from Visual Studio

  - Launch Visual Studio
  - File -> Open -> Project/Solution
  - Select `CoreBot.csproj` file
  - Press `F5` to run the project

## Testing the bot using Bot Framework Emulator

[Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) is a desktop application that allows bot developers to test and debug their bots on localhost or running remotely through a tunnel.

- Install the Bot Framework Emulator version 4.5.2 or greater from [here][https://github.com/Microsoft/BotFramework-Emulator/releases]
- Scroll to the bottom and download the .exe, .AppImage, or .dmg file based on your OS.

### Connect to the bot using Bot Framework Emulator

- Launch Bot Framework Emulator
- File -> Open Bot
- Enter a Bot URL of `http://localhost:3978/api/messages` *OR* Use **PizzaBot.bot** found in the project folder.
- Connect
- Start using the Pizza Bot!

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot Basics](https://docs.microsoft.com/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Dialogs](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-dialog?view=azure-bot-service-4.0)
- [Gathering Input Using Prompts](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-prompts?view=azure-bot-service-4.0&tabs=csharp)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Azure Bot Service Documentation](https://docs.microsoft.com/azure/bot-service/?view=azure-bot-service-4.0)
- [.NET Core CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- [Azure CLI](https://docs.microsoft.com/cli/azure/?view=azure-cli-latest)
- [Azure Portal](https://portal.azure.com)
- [Language Understanding using LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
