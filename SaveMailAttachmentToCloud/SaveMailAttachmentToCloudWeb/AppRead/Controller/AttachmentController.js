actApp.controller("attachmentController", function ($scope, OAuthService) {
    var self = $scope;
    
    // In Binding we use one of the Math Function.
    self.Math = window.Math;

    //setting attachments from global Variable Office
    self.attachments = Office.context.mailbox.item.attachments;


    // ---------- Authorize Methods --------------------------//
    self.AuthorizeOneDrive = function () {
        OAuthService.AuthorizeOneDrive();
    }

    self.AuthorizeDropbox = function () {
        OAuthService.AuthorizeDropbox();
    }

    self.AuthorizeGoogleDrive = function () {
        OAuthService.AuthorizeGoogleDrive();
    }





    // ---------- Save Methods --------------------------//
    self.SaveToOneDrive = function (id) {
        OAuthService.SaveToOneDrive(id);
    }

    self.SaveToDropbox = function (id) {
        OAuthService.SaveToDropbox(id);
    }
    self.SaveToGoogleDrive = function (id) {
        OAuthService.SaveToGoogleDrive(id);
    }


});
