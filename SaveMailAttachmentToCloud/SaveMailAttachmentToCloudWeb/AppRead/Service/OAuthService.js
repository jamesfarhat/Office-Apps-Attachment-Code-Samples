actApp.service("OAuthService", function ($http) {



    // ---------- Authorize Methods --------------------------//

   

    this.AuthorizeDropbox = function () {

        callAuthorizeService("Dropbox", "GetAuthorizationUrl");


    }
    this.AuthorizeGoogleDrive = function () {
        callAuthorizeService("GoogleDrive", "GetAuthorizationUrl");

    }

    this.AuthorizeOneDrive = function () {


        var dataToSend = {

            ResourceId: ""
        }
        callAuthorizeServiceWithData("OAuth", "GetAuthorizationUrl", dataToSend);

    }


    // ---------- Save Methods --------------------------//

    this.SaveToOneDrive = function (id) {
        $('.disable-while-sending').prop('disabled', true);

        var attachmentId = id;
        var ewsUrl = Office.context.mailbox.ewsUrl;
        Office.context.mailbox.getCallbackTokenAsync(function (ar) {
            var attachmentData = {
                AuthToken: ar.value,
                AttachmentId: attachmentId,
                EwsUrl: ewsUrl
            };

            saveService("GetAttachment", "SaveAttachment", attachmentData);
        });

    }
    this.SaveToDropbox = function (id) {
        $('.disable-while-sending').prop('disabled', true);

        var attachmentId = id;
        var ewsUrl = Office.context.mailbox.ewsUrl;
        Office.context.mailbox.getCallbackTokenAsync(function (ar) {
            var attachmentData = {
                AuthToken: ar.value,
                AttachmentId: attachmentId,
                EwsUrl: ewsUrl
            };

            saveService("Dropbox", "SaveAttachment", attachmentData);
        });
    }
    this.SaveToGoogleDrive = function (id) {
        $('.disable-while-sending').prop('disabled', true);

        var attachmentId = id;
        var ewsUrl = Office.context.mailbox.ewsUrl;
        Office.context.mailbox.getCallbackTokenAsync(function (ar) {
            var attachmentData = {
                AuthToken: ar.value,
                AttachmentId: attachmentId,
                EwsUrl: ewsUrl
            };

            saveService("GoogleDrive", "SaveAttachment", attachmentData);
        });


    }


    // ---------- Private Helper Methods --------------------------//

    // this method is used for Dropbox and Google Drive
    var callAuthorizeService = function (Controller, Action) {
        $http.post('../../api/' + Controller + "/" + Action).
              success(function (data, status, headers, config) {
                  window.open(data);
                  $('.disable-while-sending').prop('disabled', false);
              }).
              error(function (data, status, headers, config) {
                  app.showNotification('Error', JSON.stringify(status));
                  $('.disable-while-sending').prop('disabled', false);
              });
    }

    // this method is used for OneDrive
    var callAuthorizeServiceWithData = function (Controller, Action, jsonData) {
        $http.post('../../api/' + Controller + "/" + Action, jsonData).
              success(function (data, status, headers, config) {
                  window.open(data);
                  $('.disable-while-sending').prop('disabled', false);
              }).
              error(function (data, status, headers, config) {
                  app.showNotification('Error', JSON.stringify(status));
                  $('.disable-while-sending').prop('disabled', false);
              });
    }

    // This method is used to save the file
    var saveService = function (Controller, Action, jsonData) {
        $http.post('../../api/' + Controller + "/" + Action, jsonData).
              success(function (data, status, headers, config) {
                  app.showNotification("Success", "");//, JSON.stringify(data));
                  $('.disable-while-sending').prop('disabled', false);
              }).
              error(function (data, status, headers, config) {
                  app.showNotification('Error', JSON.stringify(status));
                  $('.disable-while-sending').prop('disabled', false);
              });
    }



});