# Custom UP Banking .NET Api
 
This is a project I have been using to pull my transactions from UP and categorize/track these transactions.

This is a clone of the repo and the one I use is still private as it has custom CI/CD that is only relevant to my personal infra.

To get this project running on your local requires a few steps:
1. Create a SQL Server Express database(OR migrate the code to a SQL DB of your choice).
2. Create the relevant tables/stored procs from the SQLScripts folder in that database.
3. Fill in your app settings with the relevant tokens:
    a. UpBankingApiToken: This is your UP Bank Personal Api Token.

    b. ConnectionStrings:MegaDb: This is the connection string for your SQL database.

    c. OpenAIServiceOptions: This shouldn't be necessary for base functionality, it's an API Key for OpenAI to assist with Categorizing as well as the AI advice(Which kind of sucks).

    d. CustyUpApiKey: This is an api key, this is what you will use to auth yourself to this api. Put any secure string in there and use it as your API Key when authorizing.


The daily_budget_insert.sh shows an example curl using that token:
#!/bin/bash

curl "https://$SERVER_URL/Budget/InsertYesterdaysTransactions" \
  -H "Authorization: $CUSTYUP_API_KEY"


If you follow the above steps you should now be able to run the api and get transactions back from UP as well as have it run the categorization logic etc.


A few additional nuances:
1. You can alter the categories however you like the Categories.cs enum these categories must be duplicated to the DB table "Categories" as the budget SP relies on it.

2. The cron job to run the daily budget insert is inside the .github - DISABLED/workflows folder. If you rename that folder to .github/workflows github actions will automatically run it each day.
You must actually host this application on a server or local machine to get this correctly working. It pulls the api key and server url from the github secrets which you will have to put in.

3. I have CI/CD in my personal fork of this repo but I removed it as it is all custom with my personal architecture. If you need help with it, please feel free to contact me at budget@connormdk.xyz.
