angular.module('streamSubscriptionApp', [])
    .controller('streamSubscriptionCtrl', ["$scope", "$http", "$timeout", function($scope, $http, $timeout) {
        
        // All subscriptions
        $scope.subscriptions = [];
        
        // Stream rippers
        $scope.streamRippers = {};
        
        // New Stream subscription
        $scope.streamSubscription = {
            id: 0,
            url: "",
            serviceType: "",
            token: ""
        };
        
        // Alert message box
        $scope.alert = {
            show: false,
            message: "",
            invoke: function (message) {
                this.message = message;
                this.show = true;
                var self = this;
                
                $timeout(function () {
                    self.message = "";
                    self.show = false;
                }, 3000);
            }
        };
        
        // Event handler to add new stream subscription
        $scope.addStreamSubscription = function ($event) {
          $event.preventDefault();
          
          $http.post("api/StreamingSubscription", $scope.streamSubscription).then(function (value) { 
             $scope.alert.invoke("Successfully added stream subscription");
             
             // Set to default
             $scope.streamSubscription.url = "";
             $scope.streamSubscription.serviceType = "";
             $scope.streamSubscription.token = "";
          });
        };
        
        // Loads all subscriptions
        $scope.loadSubscriptions = function () {
            $http.get("api/StreamingSubscription").then(function (value) { 
               var data = value.data;
               
               $scope.subscriptions = data;
               
               // Load rippers
               $scope.loadStreamRippers();
            });
        };
        
        // Load stream rippers status
        $scope.loadStreamRippers = function () {
            $http.get("api/StreamRipperManagement/status").then(function (value) {
                var data = value.data;

                $scope.streamRippers = data;
            });
        };
        
        // Start stream ripper
        $scope.startStreamRipper = function (id) {
            $http.get("api/StreamRipperManagement/start/" + id).then(function (value) {
                $scope.alert.invoke("Stream successfully started");
                
                $scope.loadStreamRippers();
            });
        };
        
        // Stop stream ripper
        $scope.stopStreamRipper = function (id) {
            $http.get("api/StreamRipperManagement/stop/" + id).then(function (value) {
                $scope.alert.invoke("Stream successfully stopped");

                $scope.loadStreamRippers();
            });
        };

        // Delete stream ripper
        $scope.deleteSubscription = function (id) {
            $http.delete("api/StreamingSubscription/" + id).then(function (value) {
                $scope.alert.invoke("Stream successfully deleted");

                // Load all subscriptions again
                $scope.loadSubscriptions();
            });
        };
        
        // Load all subscriptions
        $scope.loadSubscriptions();
    }]);