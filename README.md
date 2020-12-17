# ManicTime API Client

ManicTime API client enables querying ManicTime servers using command line. It supports cloud or on-premises servers. It also serves as a sample app for using [ManicTime Cloud API](https://manictime.uservoice.com/knowledgebase/articles/1961233-cloud-api).

## Download

You can [download](https://github.com/manictime/manictime-api-client/releases/latest) the latest version of ManicTime API Client for Windows, macOS and Linux.

## Usage

### Display help

    mtapi -?

### Display help for command

    mtapi auth login -?

### Login to to get access token

Cloud (default callback URL http://127.0.0.1:4040)

    mtapi auth login --client-id <client ID> --client-secret <client secret>

Cloud (custom callback URL)

    mtapi auth login --client-id <client ID> --client-secret <client secret> --callback-url <callback URL>

Server with ManicTime users

    mtapi auth login --server-url <server URL> --username <username> --password <password>

Server with ManicTime users (interactive password)

    mtapi auth login --server-url <server URL> --username <username>

### Use API

Cloud

    mtapi get timelines --access-token <access token> 

Server with ManicTime users
    
    mtapi get timelines --server-url <server URL> --access-token <access token>     

Server with Windows users (as current user)

    mtapi get timelines --server-url <server URL>

Server with Windows users (as different user)

    mtapi get timelines --server-url <server URL> --username <username> --password <password>

Server with Windows users (as different user, interactive password)

    mtapi get timelines --server-url <server URL> --username <username>
