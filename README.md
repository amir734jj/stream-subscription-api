# streaming-subscription-api

By providing an Url and Access Token, this website rips the stream or partitions the stream into `.mp3` file and subsequently uploads the to a service. 

This service is free, we do not own the ripped `.mp3` files and we do not store them, only store them in your file sharing service.

[API Url](https://streaming-subscription.herokuapp.com/)
[UI Url](https://stream-subscription-ui.herokuapp.com/)

Notes:
- Make sure you have the .NET Core SDK installed ([Download](https://www.microsoft.com/net/learn/get-started))
- To view environment variables make sure to install `heroku cli` and then
  - `heroku config --json --app="streaming-subscription"`
