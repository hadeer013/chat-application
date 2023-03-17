var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub", {
        accessTokenFactory:()=>"testing"
        })
    .build();