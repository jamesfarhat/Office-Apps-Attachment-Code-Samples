actApp.service("OAuthService", function ($http) {

    var fileName;
    var documentType;
    var extension

    // ---------- Authorize Methods  --------------------------//

    

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
        callAuthorizeServiceWithData("OneDrive", "GetAuthorizationUrl", dataToSend);

    }

    // ---------- Save Methods --------------------------//

    this.SaveToOneDrive = function (fName, dType, ext) {
        $('.disable-while-sending').prop('disabled', true);

        fileName = fName;
        documentType = dType;
        extension = ext;

        SendDocument("OneDrive", "SaveToOneDrive");

    }
    this.SaveToDropbox = function (fName, dType, ext) {
        $('.disable-while-sending').prop('disabled', true);

        fileName = fName;
        documentType = dType;
        extension = ext;

        SendDocument("Dropbox", "SaveAttachment");
    }
    this.SaveToGoogleDrive = function (fName,dType,ext) {
        $('.disable-while-sending').prop('disabled', true);

        fileName = fName;
        documentType = dType;
        extension = ext;
        
        SendDocument("GoogleDrive", "SaveAttachment");


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
                  app.showNotification("Success", JSON.stringify(data));
                  $('.disable-while-sending').prop('disabled', false);
              }).
              error(function (data, status, headers, config) {
                  app.showNotification('Error', JSON.stringify(status));
                  $('.disable-while-sending').prop('disabled', false);
              });
    }










    //--------- File Service ------------------------//


    var i = 0;
    var slices = 0;
    var fileByte = [];
    

    function SendDocument(controller, action) {
        fileByte = [];


        Office.context.document.getFileAsync(documentType == "Office" ? "compressed" : "pdf", { sliceSize: 1048576 }, function (result) {
            if (result.status == "succeeded") {
                // If the getFileAsync call succeeded, then
                // result.value will return a valid File Object.
                var myFile = result.value;
                slices = myFile.sliceCount;


                // Iterate over the file slices.
                for (i = 0; i < slices; i++) {
                    var slice = myFile.getSliceAsync(i, function (result) {
                        if (result.status == "succeeded") {
                            combineChunk(result.value.data);
                            if (slices == i) // Means it's done traversing...
                            {
                                SendFileComplete(controller,action);
                            }
                        }
                        else {
                            //document.getElementById("result").innerText = result.error.message;
                        }
                    });
                }
                myFile.closeAsync();
            }
            else {
                //document.getElementById("result2").innerText = result.error.message;
            }
        });
    }

    function combineChunk(currentByte) {
        fileByte.push.apply(fileByte, currentByte);

    }
    function SendFileComplete(controller,action) {
        var i = 0;

        var newFile = {
            AttachmentName: fileName,
            AttachmentBytes: fileByte
        };

        

        saveService(controller, action, newFile);



    }

});