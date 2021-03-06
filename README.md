# streaming-subscription-api

By providing an Url and Access Token, this website rips the IceCast stream or partitions the stream into .mp3 file and subsequently uploads the song to a service (ftp, dropbox or etc.)

This service is free, we do not own the ripped `.mp3` files and we do not store them, only store them in your file sharing service.

- [API](https://stream-subscription-api.herokuapp.com/) URL
- [UI](https://stream-subscription-ui.herokuapp.com/) URL

Libraries:
  - [StreamRipper](https://github.com/amir734jj/Stream-ripper): used to rip online radios
  - SignalR: used to send live log and song infos to the front-end
  - ReactiveX: used to manage concurrency
  - EntityFramework.Core: database ORM
  - Microsoft.AspNet.Identity: for authentication

Notes:
- Make sure you have the .NET Core SDK installed ([Download](https://www.microsoft.com/net/learn/get-started))
- To view environment variables make sure to install `heroku cli` and then
  - `heroku config --json --app="stream-subscription-api"`

Acknowledgment:
- Thanks to [@nabster](https://stackoverflow.com/a/61706419/1834787) for help with last.fm integration

Screenshot:

![screenshot](stream-ripper-screenshot.png "Screenshot")
