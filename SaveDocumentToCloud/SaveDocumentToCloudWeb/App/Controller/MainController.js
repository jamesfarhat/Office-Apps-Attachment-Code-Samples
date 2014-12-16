actApp.controller("mainController", function ($scope, OAuthService) {
    var self = $scope;
    
    
    self.ShowExtension = true;
    self.changeShowExtension = function () {
        if (self.selectedDocumentType == "PDF")
            self.ShowExtension = false;
        else
            self.ShowExtension = true;
    }


  

    self.selectedDocumentType = 'Office';

    self.DocumentTypes = [{ value: 'Office', displayName: 'Office Documents' },
        { value: 'PDF', displayName: 'PDF' }];


    self.selectedExtension = '.docx';

    self.Extensions = [{ value: '.docx', displayName: 'Word' },
        { value: '.xlsx', displayName: 'Excel' },
        { value: '.pptx', displayName: 'PowerPoint' }];


    self.getFileName = function()
    {
        var name = self.docNameWithOutExtension;
        if (self.selectedDocumentType == "Office")
        {
            name += self.selectedExtension;
        }
        else
        {
            name += ".pdf";
        }
        return name;
    }
    

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
    self.SaveToOneDrive = function () {
        OAuthService.SaveToOneDrive(self.getFileName(),self.selectedDocumentType,self.selectedExtension);
    }

    self.SaveToDropbox = function () {
        OAuthService.SaveToDropbox(self.getFileName(), self.selectedDocumentType, self.selectedExtension);
    }
    self.SaveToGoogleDrive = function () {
        OAuthService.SaveToGoogleDrive(self.getFileName(), self.selectedDocumentType, self.selectedExtension);
    }


});
