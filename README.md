# streaming-subscription-api

By providing an Url and Access Token, this website rips the stream or partitions the stream into `.mp3` file and subsequently uploads the to a service. 

This service is free, we do not own the ripped `.mp3` files and we do not store them, only store them in your file sharing service.

- [API](https://streaming-subscription.herokuapp.com/) URL
- [UI](https://stream-subscription-ui.herokuapp.com/) URL

Libraries:
  - SignalR: used to send live log and song infos to the front-end
  - ReactiveX: used to manage concurrency
  - EntityFramework.Core: database ORM
  - Microsoft.AspNet.Identity: for authentication

Notes:
- Make sure you have the .NET Core SDK installed ([Download](https://www.microsoft.com/net/learn/get-started))
- To view environment variables make sure to install `heroku cli` and then
  - `heroku config --json --app="stream-subscription-api"`
